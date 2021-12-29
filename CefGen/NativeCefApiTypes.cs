// --------------------------------------------------------------------------------------------
// Copyright (c) 2019 The CefNet Authors. All rights reserved.
// Licensed under the MIT license.
// See the licence file in the project root for full license information.
// --------------------------------------------------------------------------------------------

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CefGen
{

	public class NativeCefApiTypes
	{
		private readonly string _basePath;

		public NativeCefApiTypes(string basePath)
		{
			_basePath = basePath;
			Enums = new Dictionary<string, INamedTypeSymbol>();
			RefCounted = new Dictionary<string, INamedTypeSymbol>();
			Scoped = new Dictionary<string, INamedTypeSymbol>();
			Sized = new Dictionary<string, INamedTypeSymbol>();
			Simple = new Dictionary<string, INamedTypeSymbol>();
		}

		public Dictionary<string, INamedTypeSymbol> RefCounted { get; private set; }
		public Dictionary<string, INamedTypeSymbol> Scoped { get; private set; }
		public Dictionary<string, INamedTypeSymbol> Enums { get; private set; }
		public Dictionary<string, INamedTypeSymbol> Sized { get; private set; }
		public Dictionary<string, INamedTypeSymbol> Simple { get; private set; }

		public void Build()
		{
			foreach (INamedTypeSymbol symbol in GetSymbolsForNativeApi())
			{
				if (symbol.TypeKind == TypeKind.Enum)
				{
					Enums.Add(symbol.Name, symbol);
					continue;
				}
				if (IsRefCountedTypeSymbol(symbol))
				{
					RefCounted.Add(symbol.Name, symbol);
					continue;
				}
				if (IsScopedTypeSymbol(symbol))
				{
					Scoped.Add(symbol.Name, symbol);
					continue;
				}

				if (symbol.TypeKind != TypeKind.Struct)
					continue;

				if (IsSizedStruct(symbol))
				{
					Sized.Add(symbol.Name, symbol);
					continue;
				}

				Simple.Add(symbol.Name, symbol);
			}
		}

		private static bool IsRefCountedTypeSymbol(INamedTypeSymbol symbol)
		{
			if (symbol.TypeKind != TypeKind.Struct)
				return false;

			ImmutableArray<ISymbol> members = symbol.GetMembers();
			if (members.Length > 0)
			{
				var field = members[0] as IFieldSymbol;
				if (field != null)
				{
					return field.Type.Name == "cef_base_ref_counted_t";
				}
			}
			return false;
		}

		private static bool IsScopedTypeSymbol(INamedTypeSymbol symbol)
		{
			if (symbol.TypeKind != TypeKind.Struct)
				return false;

			ImmutableArray<ISymbol> members = symbol.GetMembers();
			if (members.Length > 0)
			{
				var field = members[0] as IFieldSymbol;
				if (field != null)
				{
					return field.Type.Name == "cef_base_scoped_t";
				}
			}
			return false;
		}

		private static bool IsSizedStruct(INamedTypeSymbol symbol)
		{
			if (symbol.TypeKind != TypeKind.Struct)
				return false;

			ImmutableArray<ISymbol> members = symbol.GetMembers();
			if (members.Length > 0)
			{
				var field = members[0] as IFieldSymbol;
				if (field != null && field.Name == "size")
				{
					return field.Type.Name == "UIntPtr";
				}
			}
			return false;
		}

		private List<INamedTypeSymbol> GetSymbolsForNativeApi()
		{
			CSharpCompilation compilation = CompileNativeClasses(_basePath);
			GetAllSymbolsVisitor visitor = new GetAllSymbolsVisitor();
			visitor.Visit(compilation.Assembly.GlobalNamespace);
			return visitor.GetSymbols();
		}

		internal static void AddFilesToSyntaxTrees(List<SyntaxTree> syntaxTrees, string baseDir)
		{
			string sourceCode;
			SyntaxTree syntaxTree;
			foreach (string file in Directory.GetFiles(baseDir, "*.cs", SearchOption.AllDirectories))
			{
				sourceCode = File.ReadAllText(file, Encoding.UTF8);
				syntaxTree = SyntaxFactory.ParseSyntaxTree(SourceText.From(sourceCode), path: file);
				syntaxTrees.Add(syntaxTree);
			}
		}

		public static CSharpCompilation CompileNativeClasses(string basePath)
		{
			var syntaxTrees = new List<SyntaxTree>();
			AddFilesToSyntaxTrees(syntaxTrees, Path.Combine(basePath, "Native"));
			AddFilesToSyntaxTrees(syntaxTrees, Path.Combine(basePath, "Managed", "Enums"));
			syntaxTrees.Add(SyntaxFactory.ParseSyntaxTree(SourceText.From("namespace CefNet { public struct CefEventHandle { } }")));
			syntaxTrees.Add(SyntaxFactory.ParseSyntaxTree(SourceText.From("namespace CefNet.CApi { public struct cef_main_args_t { } public struct cef_window_info_t { } }")));
			syntaxTrees.Add(SyntaxFactory.ParseSyntaxTree(SourceText.From("namespace CefNet.WinApi { public struct MSG { } }")));
			syntaxTrees.Add(SyntaxFactory.ParseSyntaxTree(SourceText.From("namespace System { [AttributeUsage(AttributeTargets.Parameter)] sealed class ImmutableAttribute : Attribute { } }")));
			syntaxTrees.Add(SyntaxFactory.ParseSyntaxTree(SourceText.From("namespace System { [AttributeUsage(AttributeTargets.Method)] sealed class NativeNameAttribute : Attribute { public NativeNameAttribute(string name) { } } }")));

			var references = new MetadataReference[]
			{
					MetadataReference.CreateFromFile(typeof(object).GetTypeInfo().Assembly.Location),
					MetadataReference.CreateFromFile(typeof(HashSet<>).GetTypeInfo().Assembly.Location),
			};

			CSharpCompilation compilation = CSharpCompilation.Create(
				"CefNet.dll",
				syntaxTrees,
				references: references,
				options: new CSharpCompilationOptions(
					outputKind: OutputKind.DynamicallyLinkedLibrary,
					optimizationLevel: OptimizationLevel.Release,
					assemblyIdentityComparer: DesktopAssemblyIdentityComparer.Default,
					allowUnsafe: true
				));

			using (var ms = new MemoryStream())
			{
				EmitResult emitResult = compilation.Emit(ms);
				if (!emitResult.Success)
				{
					foreach(var diag in emitResult.Diagnostics)
						Console.WriteLine(diag);
					Debugger.Break();
					Environment.Exit(-1);
				}
			}

			return compilation;
		}

		public CefTypeKind GetTypeKind(ITypeSymbol symbol)
		{
			if (!symbol.Name.StartsWith("cef_"))
				return CefTypeKind.Unknown;

			INamedTypeSymbol value;
			if (RefCounted.TryGetValue(symbol.Name, out value) && SymbolEqualityComparer.Default.Equals(value, symbol))
				return CefTypeKind.RefCounted;
			if (Scoped.TryGetValue(symbol.Name, out value) && SymbolEqualityComparer.Default.Equals(value, symbol))
				return CefTypeKind.Scoped;
			if (Enums.TryGetValue(symbol.Name, out value) && SymbolEqualityComparer.Default.Equals(value, symbol))
				return CefTypeKind.Enum;
			if (Sized.TryGetValue(symbol.Name, out value) && SymbolEqualityComparer.Default.Equals(value, symbol))
				return CefTypeKind.Sized;
			if (Simple.TryGetValue(symbol.Name, out value) && SymbolEqualityComparer.Default.Equals(value, symbol))
				return CefTypeKind.Simple;
			throw new NotImplementedException();
		}
	}


}
