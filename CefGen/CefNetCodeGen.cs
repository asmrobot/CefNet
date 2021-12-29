// --------------------------------------------------------------------------------------------
// Copyright (c) 2019 The CefNet Authors. All rights reserved.
// Licensed under the MIT license.
// See the licence file in the project root for full license information.
// --------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using CefGen.CodeDom;
using CppAst;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CefGen
{
	sealed class CefNetCodeGen : CodeGenBase
	{
		private delegate void CodeGenFunc(CompilationUnitSyntax unit);

		public CefNetCodeGen(string basePath)
		{
			InputPath = Path.Combine(basePath, "Managed", "Types");
			OutputPath = Path.Combine(basePath, "Managed", "Internal");
		}

		public string InputPath { get; private set; }
		public string OutputPath { get; private set; }

		public void Generate()
		{
			foreach (string filename in Directory.EnumerateFiles(InputPath))
			{
				string source = File.ReadAllText(filename, Encoding.UTF8);
				if (!source.Contains("Role: Handler"))
					continue;

				SyntaxTree tree = CSharpSyntaxTree.ParseText(source, path: filename, encoding: Encoding.UTF8);
				CompilationUnitSyntax unit = (CompilationUnitSyntax)tree.GetRoot();

				string handlerClassName = Path.GetFileNameWithoutExtension(filename);
				GenerateFileCode(GeneratePrivateInterfaceFileCode, Path.Combine(OutputPath, "I" + handlerClassName + "Private.cs"), unit);

				if (handlerClassName == "CefApp")
					continue;

				GenerateFileCode(GenerateHandlerGlueFileCode, Path.Combine(OutputPath, handlerClassName + "Glue.cs"), unit);
			}
		}

		private void GenerateFileCode(CodeGenFunc codegen, string filename, CompilationUnitSyntax unit)
		{
			StreamWriter file = null;
			try
			{
				file = new StreamWriter(filename, false, Encoding.UTF8);
				Output = file;
				codegen(unit);
				file.Flush();
			}
			finally
			{
				Output = null;
				file.Close();
			}
		}

		private void GeneratePrivateInterfaceFileCode(CompilationUnitSyntax unit)
		{
			Output.WriteLine(unit.Usings.ToFullString());
			GenerateNodePrivateInterfaceCode(unit);
		}

		private void GenerateNodePrivateInterfaceCode(SyntaxNode node)
		{
			foreach (SyntaxNode childNode in node.ChildNodes())
			{

				if (childNode is NamespaceDeclarationSyntax namespaceNode)
				{
					WriteIndent();
					Output.Write("namespace ");
					Output.Write(namespaceNode.Name.ToString() + ".Internal");
					WriteBlockStart(CodeGenBlockType.Namespace);
					GenerateNodePrivateInterfaceCode(namespaceNode);
					WriteBlockEnd(CodeGenBlockType.Namespace);
				}
				else if (childNode is ClassDeclarationSyntax classNode)
				{
					WriteIndent();
					Output.Write("public interface I");
					Output.Write(classNode.Identifier.ToString());
					Output.Write("Private");
					WriteBlockStart(CodeGenBlockType.Type);
					GenerateNodePrivateInterfaceCode(classNode);
					WriteBlockEnd(CodeGenBlockType.Type);
				}
				else if (childNode is MethodDeclarationSyntax methodNode)
				{
					if (methodNode.Modifiers.ToString().Contains("static"))
						continue;

					string name = methodNode.Identifier.ToString();
					if (!name.StartsWith("Avoid"))
						continue;

					WriteIndent();
					Output.Write(methodNode.ReturnType.ToString());
					Output.Write(" ");
					Output.Write(name);
					Output.Write(methodNode.ParameterList.ToString());
					Output.WriteLine(";");
				}
			}
		}

		private void GenerateHandlerGlueFileCode(CompilationUnitSyntax unit)
		{
			Output.WriteLine(unit.Usings.ToFullString());
			GenerateNodeHandlerGlueCode(unit);
		}

		private void GenerateNodeHandlerGlueCode(SyntaxNode node)
		{
			foreach (SyntaxNode childNode in node.ChildNodes())
			{
				if (childNode is NamespaceDeclarationSyntax namespaceNode)
				{
					WriteIndent();
					Output.Write("namespace ");
					Output.Write(namespaceNode.Name.ToString() + ".Internal");
					WriteBlockStart(CodeGenBlockType.Namespace);
					GenerateNodeHandlerGlueCode(namespaceNode);
					WriteBlockEnd(CodeGenBlockType.Namespace);
				}
				else if (childNode is ClassDeclarationSyntax classNode)
				{
					string glueClassName = GetGlueClass(classNode.Identifier.ToString());
					string privateInterfaceName = "I" + classNode.Identifier.ToString() + "Private";
					WriteIndent();
					Output.Write("sealed partial class ");
					Output.Write(classNode.Identifier.ToString());
					Output.Write("Glue: ");
					Output.Write(classNode.Identifier.ToString());
					Output.Write(", ");
					Output.Write(privateInterfaceName);
					WriteBlockStart(CodeGenBlockType.Type);
					WriteIndent();
					Output.Write("private ");
					Output.Write(glueClassName);
					Output.WriteLine(" _implementation;");
					Output.WriteLine();
					WriteIndent();
					Output.Write("public ");
					Output.Write(classNode.Identifier.ToString());
					Output.Write("Glue(");
					Output.Write(glueClassName);
					Output.Write(" impl)");
					WriteBlockStart(CodeGenBlockType.Method);
					WriteIndent();
					Output.WriteLine("_implementation = impl;");
					WriteBlockEnd(CodeGenBlockType.Method);
					Output.WriteLine();
					GenerateNodeHandlerGlueMedhodCode(privateInterfaceName, classNode);
					WriteBlockEnd(CodeGenBlockType.Type);
				}
			}
		}

		private void GenerateNodeHandlerGlueMedhodCode(string privateInterfaceName, SyntaxNode node)
		{
			foreach (SyntaxNode childNode in node.ChildNodes())
			{
				if (childNode is MethodDeclarationSyntax methodNode)
				{
					if (methodNode.Modifiers.ToString().Contains("static"))
						continue;

					string name = methodNode.Identifier.ToString();

					bool isAvoidMethod = name.StartsWith("Avoid");

					WriteIndent();
					if (!isAvoidMethod)
					{
						Output.Write(methodNode.Modifiers.ToString().Replace("virtual", "override"));
						Output.Write(" ");
					}
					else
					{
						name = privateInterfaceName + "." + name;
					}
					Output.Write(methodNode.ReturnType.ToString());
					Output.Write(" ");
					Output.Write(name);
					Output.Write(methodNode.ParameterList.ToString());

					WriteBlockStart(CodeGenBlockType.Method);
					if (isAvoidMethod)
					{
						WriteIndent();
						Output.Write("return _implementation.");
						Output.Write(methodNode.Identifier.ToString());
						Output.WriteLine("();");
					}
					else
					{
						WriteIndent();
						if (methodNode.ReturnType.ToString() != "void")
						{
							Output.Write("return ");
						}
						Output.Write("_implementation.");
						Output.Write(methodNode.Identifier.ToString());
						Output.Write("(");
						Output.Write(GetCallArgs(methodNode.ParameterList.Parameters));
						Output.WriteLine(");");
					}
					WriteBlockEnd(CodeGenBlockType.Method);
					Output.WriteLine();
				}
			}
		}

		private static string GetCallArgs(SeparatedSyntaxList<ParameterSyntax> args)
		{
			var list = new List<string>(args.Count);
			foreach(var arg in args)
			{
				string mods = arg.Modifiers.ToString();
				if (string.IsNullOrWhiteSpace(mods))
					list.Add(arg.Identifier.ToString());
				else
					list.Add(arg.Modifiers.ToString() + " " + arg.Identifier.ToString());
			}
			return string.Join(", ", list);
		}

		private string GetGlueClass(string handlerTypeName)
		{
			switch(handlerTypeName)
			{
				case "CefBrowserProcessHandler":
				case "CefRenderProcessHandler":
				case "CefResourceBundleHandler":
					return "CefAppGlue";
			}
			return "WebViewGlue";
		}

	}
}
