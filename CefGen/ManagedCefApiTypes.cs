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
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;

namespace CefGen
{
	class ManagedCefApiTypes
	{
		private readonly string _basePath;

		public ManagedCefApiTypes(string basePath)
		{
			_basePath = basePath;
		}

		public List<INamedTypeSymbol> RefCountedClasses { get; private set; }
		public List<INamedTypeSymbol> PrivateInterfaces { get; private set; }

		public void Build()
		{
			RefCountedClasses = new List<INamedTypeSymbol>();
			PrivateInterfaces = new List<INamedTypeSymbol>();
			foreach (INamedTypeSymbol symbol in GetSymbolsForManagedApi())
			{
				if (symbol.BaseType != null && symbol.BaseType.Name.StartsWith("CefBaseRefCounted"))
				{
					RefCountedClasses.Add(symbol);
				}
				else if (symbol.TypeKind == TypeKind.Interface && symbol.Name.EndsWith("Private"))
				{
					PrivateInterfaces.Add(symbol);
				}
			}
		}

		private CSharpCompilation CompileManagedClasses(CSharpCompilation compilation)
		{
			var syntaxTrees = new List<SyntaxTree>();
			NativeCefApiTypes.AddFilesToSyntaxTrees(syntaxTrees, Path.Combine(_basePath, "Managed", "Types"));
			AddPrivateInterfaceFilesToSyntaxTrees(syntaxTrees, Path.Combine(_basePath, "Managed", "Internal"));
			syntaxTrees.Add(SyntaxFactory.ParseSyntaxTree(SourceText.From(File.ReadAllText(Path.Combine(_basePath, "..", "CefTypes", "CefBaseRefCounted.cs")))));
			syntaxTrees.Add(SyntaxFactory.ParseSyntaxTree(SourceText.From(File.ReadAllText(Path.Combine(_basePath, "..", "CefTypes", "CefBaseScoped.cs")))));
			syntaxTrees.Add(SyntaxFactory.ParseSyntaxTree(SourceText.From(File.ReadAllText(Path.Combine(_basePath, "..", "CefTypes", "CefColor.cs")))));
			syntaxTrees.Add(SyntaxFactory.ParseSyntaxTree(SourceText.From(File.ReadAllText(Path.Combine(_basePath, "..", "CefTypes", "CefStringList.cs")))));
			syntaxTrees.Add(SyntaxFactory.ParseSyntaxTree(SourceText.From(File.ReadAllText(Path.Combine(_basePath, "..", "CefTypes", "CefStringMap.cs")))));
			syntaxTrees.Add(SyntaxFactory.ParseSyntaxTree(SourceText.From(File.ReadAllText(Path.Combine(_basePath, "..", "CefString.cs")))));
			syntaxTrees.Add(SyntaxFactory.ParseSyntaxTree(SourceText.From(File.ReadAllText(Path.Combine(_basePath, "..", "CefStructure.cs")))));
			syntaxTrees.Add(SyntaxFactory.ParseSyntaxTree(SourceText.From(File.ReadAllText(Path.Combine(_basePath, "..", "CefTypes", "CApi", "cef_string_t.cs")))));
			syntaxTrees.Add(SyntaxFactory.ParseSyntaxTree(SourceText.From(File.ReadAllText(Path.Combine(_basePath, "..", "Unsafe", "RefCountedWrapperStruct.cs")))));
			syntaxTrees.Add(SyntaxFactory.ParseSyntaxTree(SourceText.From(File.ReadAllText(Path.Combine(_basePath, "..", "Unsafe", "CefWrapperType.cs")))));
			syntaxTrees.Add(SyntaxFactory.ParseSyntaxTree(SourceText.From("namespace System { static class Ext111 { public static void InitBlock(this IntPtr startAddress, byte value, int size) { } } }")));
			syntaxTrees.Add(SyntaxFactory.ParseSyntaxTree(SourceText.From("using CefNet.CApi; namespace CefNet { public unsafe class CefWindowInfo { public cef_window_info_t* GetNativeInstance() { return null; } public static CefWindowInfo Wrap(cef_window_info_t* p) { return null; } } }")));
			syntaxTrees.Add(SyntaxFactory.ParseSyntaxTree(SourceText.From("namespace CefNet { public class InvalidCefObjectException : System.Exception { } }")));
			syntaxTrees.Add(SyntaxFactory.ParseSyntaxTree(SourceText.From("namespace CefNet { public class CefApi { public static bool UseUnsafeImplementation; } }")));
			syntaxTrees.Add(SyntaxFactory.ParseSyntaxTree(SourceText.From("using CefNet.CApi; namespace CefNet { public class CefStringMultimap { cef_string_multimap_t a; public static implicit operator cef_string_multimap_t(CefStringMultimap a) { return a.a; } } }")));

			compilation = compilation.AddSyntaxTrees(syntaxTrees);

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

		internal static void AddPrivateInterfaceFilesToSyntaxTrees(List<SyntaxTree> syntaxTrees, string baseDir)
		{
			string sourceCode;
			SyntaxTree syntaxTree;
			foreach (string file in Directory.GetFiles(baseDir, "*.cs", SearchOption.AllDirectories))
			{
				sourceCode = File.ReadAllText(file, Encoding.UTF8);
				if (!sourceCode.Contains("interface I"))
					continue;
				syntaxTree = SyntaxFactory.ParseSyntaxTree(SourceText.From(sourceCode), path: file);
				syntaxTrees.Add(syntaxTree);
			}
		}

		private List<INamedTypeSymbol> GetSymbolsForManagedApi()
		{
			CSharpCompilation compilation = NativeCefApiTypes.CompileNativeClasses(_basePath);
			compilation = CompileManagedClasses(compilation);
			GetAllSymbolsVisitor visitor = new GetAllSymbolsVisitor();
			visitor.Visit(compilation.Assembly.GlobalNamespace);
			return visitor.GetSymbols();
		}

	}
}
