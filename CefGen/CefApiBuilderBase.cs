// --------------------------------------------------------------------------------------------
// Copyright (c) 2019 The CefNet Authors. All rights reserved.
// Licensed under the MIT license.
// See the licence file in the project root for full license information.
// --------------------------------------------------------------------------------------------

using CefGen.CodeDom;
using CppAst;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CefGen
{
	abstract class CefApiBuilderBase : ApiBuilderBase
	{
		public event EventHandler<ResolveTypeNameEventArgs> ResolveCefTypeDef;

		public CefApiBuilderBase()
		{

		}

		public string BaseDirectory { get; set; }

		public string Namespace { get; set; }


		public void Format(CppTypeDeclaration typeDecl, TextWriter stream)
		{
			var codefile = CreateCodeFile(this.Namespace, typeDecl);

			var sb = new StringBuilder();
			using (var w = new StringWriter(sb))
			{
				var codeGen = new CSharpCodeGen();
				codeGen.Directives.Add("#pragma warning disable 0169, 1591, 1573");
				codeGen.GenerateCode(codefile, w);
				w.Flush();
			}
			stream.WriteLine(sb.ToString());


		}

		public void Format(CppClass @class, TextWriter langStream, TextWriter msilStream)
		{
			var codefile = CreateCodeFile(this.Namespace, @class);

			var sb = new StringBuilder();
			using (var w = new StringWriter(sb))
			{
				var codeGen = new CSharpCodeGen();
				codeGen.Directives.Add("#pragma warning disable 0169, 1591, 1573");
				codeGen.GenerateCode(codefile, w);
				w.Flush();
			}
			langStream.WriteLine(sb.ToString());

			//sb.Clear();

			//using (var w = new StringWriter(sb))
			//{
			//	var codeGen = CreateMsilCodeGen();
			//	codeGen.ResolveTypeName += CodeGen_ResolveTypeName;
			//	codeGen.GenerateCode(codefile, w);
			//	codeGen.ResolveTypeName -= CodeGen_ResolveTypeName;

			//	w.Flush();
			//}
			//msilStream.WriteLine(sb.ToString());

		}

		protected abstract MsilCodeGenBase CreateMsilCodeGen();

		private void CodeGen_ResolveTypeName(object sender, ResolveTypeNameEventArgs e)
		{
			if (e.Type.StartsWith("cef_") || e.Type.StartsWith("_cef_"))
			{
				e.Result = "CefNet.CApi." + e.Type;
			}
			if (e.Type.StartsWith("Cef"))
			{
				e.Result = "CefNet." + e.Type;
			}
		}

		protected string ResolveCefType(string type)
		{
			if (type.EndsWith('*'))
			{
				return ResolveCefType(type.Remove(type.Length - 1)) + "*";
			}
			else
			{
				var ea = new ResolveTypeNameEventArgs(type);
				ResolveCefTypeDef?.Invoke(this, ea);
				return ea.Result;
			}
		}

		protected string GetClassName(string name)
		{
			return ResolveCefType(name);
		}

		protected TypeDesc GetTypeDesc(CppType type)
		{
			var t = new TypeDesc(type.GetDisplayName(), type);
			if (type is CppPointerType pointerType)
			{
				t.FunctionTypeRef = pointerType.ElementType as CppFunctionType;
			}
			return t;
		}

		protected virtual CodeFile CreateCodeFile(string @namespace, CppTypeDeclaration decl)
		{
			var decls = new CppTypeDeclaration[1];
			decls[0] = decl;
			return CreateCodeFile(@namespace, decls);
		}

		protected virtual CodeFile CreateCodeFile(string @namespace, IList<CppTypeDeclaration> decls)
		{
			string sources;
			if (decls.Count == 1)
			{
				sources = decls[0].GetSourceFile();
				if (sources != null)
					sources = Path.GetRelativePath(BaseDirectory, sources).Replace('\\', '/');
			}
			else
			{
				var files = new HashSet<string>(decls.Count);
				foreach (CppTypeDeclaration decl in decls)
				{
					string filename = decl.GetSourceFile();
					filename = Path.GetRelativePath(BaseDirectory, filename);
					files.Add(filename.Replace('\\', '/'));
				}
				sources = "\n" + string.Join('\n', files);
			}

			CodeFile f = CreateCodeFile(sources);
			var ns = new CodeNamespace(@namespace);
			foreach (CppTypeDeclaration decl in decls)
			{
				if (decl is CppClass @class)
				{
					BuildClass(ns, @class);
				}
				else if (decl is CppEnum @enum)
				{
					BuildEnum(ns, @enum);
				}
				else if (decl is CppTypedef typedef)
				{
					BuildTypedef(ns, typedef);
				}
				else if (decl is CefApiClass cefapi)
				{
					BuildCefApi(ns, cefapi);
				}
			}
			f.Namespaces.Add(ns);
			return f;
		}

		protected abstract void BuildTypedef(CodeNamespace ns, CppTypedef typedef);

		protected abstract void BuildEnum(CodeNamespace ns, CppEnum @enum);

		protected abstract void BuildClass(CodeNamespace ns, CppClass @class);

		protected abstract void BuildCefApi(CodeNamespace ns, CefApiClass @class);

		
	}
}
