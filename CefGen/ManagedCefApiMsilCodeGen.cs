// --------------------------------------------------------------------------------------------
// Copyright (c) 2019 The CefNet Authors. All rights reserved.
// Licensed under the MIT license.
// See the licence file in the project root for full license information.
// --------------------------------------------------------------------------------------------

using CefGen.CodeDom;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text;

namespace CefGen
{
	sealed class ManagedCefApiMsilCodeGen : CodeGenBase
	{
		public ManagedCefApiMsilCodeGen(string basePath)
		{
			InputPath = Path.Combine(basePath, "Managed", "Types");
			OutputPath = Path.Combine(basePath, "Managed", "MSIL");
		}

		public string InputPath { get; private set; }
		public string OutputPath { get; private set; }

		public void GenerateFrom(ManagedCefApiTypes managedTypes)
		{
			var license = string.Join("\r\n", File.ReadAllText(Path.Combine("Settings", "LicenseTemplate.txt"), Encoding.UTF8).Trim().Split('\n').Select(s => "// " + s.Trim()));
			foreach (INamedTypeSymbol symbol in managedTypes.RefCountedClasses)
			{
				if (!symbol.AllInterfaces.Any(t => t.TypeKind == TypeKind.Interface && t.Name.EndsWith("Private")))
					continue;

				IMethodSymbol[] avoids = symbol.GetMembers().OfType<IMethodSymbol>().Where(m => m.Name.Split('.').Last().StartsWith("Avoid")).ToArray();
				if (avoids.Length == 0)
					continue;

				string filePath = symbol.Locations[0].SourceTree.FilePath;

				Output = new StreamWriter(Path.Combine(OutputPath, symbol.Name + ".il"), false, Encoding.UTF8);
				try
				{
					Output.WriteLine(license, nameof(CefGen), filePath.Substring(filePath.IndexOf("Generated")).Replace('\\', '/'));
					Output.WriteLine();
					Output.Write(".namespace CefNet");
					WriteBlockStart(CodeGenBlockType.Namespace);
					WriteIndent();
					Output.Write(".class public " + symbol.Name);
					WriteBlockStart(CodeGenBlockType.Type);
					Output.WriteLine();
					foreach (IMethodSymbol method in avoids)
					{
						WriteMsilMethod(method, FindTarget(method, symbol));
						Output.WriteLine();
					}
					WriteBlockEnd(CodeGenBlockType.Type);
					WriteBlockEnd(CodeGenBlockType.Namespace);
					Output.Flush();
				}
				finally
				{
					Output.Close();
					Output = null;
				}

			}


		}

		private void WriteMsilMethod(IMethodSymbol method, IMethodSymbol target)
		{
			string token = target.ToDisplayString();
			int pos = token.IndexOf('(');
			token = TypeToMsil(target.ReturnType) + " " + NameToMsil(token.Remove(pos)) + "(" + string.Join(", ", target.Parameters.Select(ParamToMsil)) + ")";

			WriteIndent();
			Output.Write(".method private final hidebysig newslot virtual instance bool " + method.Name + "()");
			WriteBlockStart(CodeGenBlockType.Method);
			WriteIndent();
			Output.WriteLine(".override method instance bool " + NameToMsil(method.Name) + "()");
			WriteIndent();
			Output.WriteLine(".maxstack 2");
			WriteIndent();
			Output.WriteLine("ldarg.0");
			WriteIndent();
			Output.WriteLine("ldvirtftn instance " + token);
			WriteIndent();
			Output.WriteLine("ldftn instance " + token);
			WriteIndent();
			Output.WriteLine("ceq");
			WriteIndent();
			Output.WriteLine("ret");
			WriteBlockEnd(CodeGenBlockType.Method);
		}

		private static string NameToMsil(string name)
		{
			int dotpos = name.LastIndexOf('.');
			return name.Remove(dotpos) + "::" +name.Substring(dotpos + 1);
		}

		private static string ParamToMsil(IParameterSymbol symbol)
		{
			if (symbol.RefKind == RefKind.Ref || symbol.RefKind == RefKind.Out)
				return TypeToMsil(symbol.Type) + "&";
			return TypeToMsil(symbol.Type);
		}

		private static string TypeToMsil(ITypeSymbol symbol)
		{
			switch (symbol.SpecialType)
			{
				case SpecialType.System_Boolean:
					return "bool";
				case SpecialType.System_Char:
					return "char";
				case SpecialType.System_SByte:
					return "int8";
				case SpecialType.System_Byte:
					return "uint8";
				case SpecialType.System_Int16:
					return "int16";
				case SpecialType.System_UInt16:
					return "uint16";
				case SpecialType.System_Int32:
					return "int32";
				case SpecialType.System_UInt32:
					return "uint32";
				case SpecialType.System_Int64:
					return "int64";
				case SpecialType.System_UInt64:
					return "uint64";
				case SpecialType.System_Single:
					return "float32";
				case SpecialType.System_Double:
					return "float64";
				case SpecialType.System_String:
					return "string";
				case SpecialType.System_Void:
					return "void";
				case SpecialType.System_IntPtr:
					return "native int";
				case SpecialType.System_UIntPtr:
					return "native uint";
			}

			if (symbol.IsValueType)
			{
				return "valuetype " + symbol.ToString();
			}
			return "class " + symbol.ToString();
		}

		private IMethodSymbol FindTarget(IMethodSymbol func, INamedTypeSymbol type)
		{
			string fnName = func.Name.Substring(func.Name.LastIndexOf('.') + 1);
			if (!fnName.StartsWith("Avoid"))
				throw new InvalidOperationException();
			fnName = fnName.Substring(5);
			foreach (IMethodSymbol method in type.GetMembers().OfType<IMethodSymbol>())
			{
				if (method.Name == fnName && method.IsVirtual)
					return method;
			}
			return null;
		}
	}

}
