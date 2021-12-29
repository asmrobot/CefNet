// --------------------------------------------------------------------------------------------
// Copyright (c) 2019 The CefNet Authors. All rights reserved.
// Licensed under the MIT license.
// See the licence file in the project root for full license information.
// --------------------------------------------------------------------------------------------

using CefGen.CodeDom;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CefGen
{
	sealed class NativeCefApiMsilCodeGen : MsilCodeGenBase
	{
		private readonly bool _onlyStdCall;

		public NativeCefApiMsilCodeGen(bool onlyStdCall)
		{
			_onlyStdCall = onlyStdCall;
		}

		protected override bool GenerateMethodCode(CodeMethod methodDecl, string typeRef)
		{
			foreach (CodeComment commentDecl in methodDecl.Callee.Comments
				.Where(IsNotXmlTagComment))
			{
				GenerateCommentCode(commentDecl.Text);
			}

			string retTypeName = GetILTypeName(methodDecl.RetVal.Type);

			WriteIndent();
			Output.Write(".method ");
			WriteAttributes(methodDecl.Attributes);
			Output.Write("hidebysig ");
			Output.Write(retTypeName);
			Output.Write(" ");
			Output.Write(methodDecl.Name);
			Output.Write("(");
			Output.Write(string.Join(", ", methodDecl.Parameters.Select(arg => GetILTypeName(arg.Type) + " " + arg.Name.EscapeILName())));
			Output.Write(")");

			WriteBlockStart(CodeGenBlockType.Method);

			int argc = methodDecl.Parameters.Count;

			WriteIndent();
			Output.Write(".maxstack ");
			Output.WriteLine(argc + 2);

			WriteIndent();
			if (methodDecl.HasThisArg)
			{
				Output.WriteLine("ldarg.0");
			}
			for (int i = 0; i < argc; i++)
			{
				WriteIndent();
				if (i < 3)
					Output.WriteLine("ldarg." + (i + 1));
				else if (i < 255)
					Output.WriteLine("ldarg.s " + (i + 1).ToString());
				else
					throw new NotImplementedException();
			}
			WriteIndent();
			Output.WriteLine("ldarg.0");
			WriteIndent();
			Output.Write("ldfld void* ");
			Output.Write(typeRef);
			Output.Write("::");
			Output.WriteLine(methodDecl.Callee.Name.EscapeILName());

			WriteIndent();
			Output.Write("calli explicit unmanaged");
			if (_onlyStdCall)
			{
				Output.Write(" stdcall ");
			}
			else
			{
				Output.WriteLine();
				Output.WriteLine("#ifdef WINDOWS");
				WriteIndent();
				Output.WriteLine("\tstdcall");
				Output.WriteLine("#else");
				WriteIndent();
				Output.WriteLine("\tcdecl");
				Output.WriteLine("#endif");
				WriteIndent();
				Output.Write("\t");
			}
			Output.Write(retTypeName);
			Output.Write("(");
			Output.Write(string.Join(", ", GetCalliMethodTypes(methodDecl.HasThisArg ? GetName(typeRef) + "*" : null, methodDecl.Parameters)));
			Output.WriteLine(")");

			WriteIndent();
			Output.WriteLine("ret");

			WriteBlockEnd(CodeGenBlockType.Method);

			return true;
		}

		private string GetILTypeName(string type)
		{
			switch (type)
			{
				case "int":
					return "int32";
				case "size_t":
					return "native uint";
				case "HWND":
				case "HINSTANCE":
				case "HCURSOR":
				case "time_t":
					return "native int";
				case "MSG":
					return "valuetype CefNet.WinApi.MSG";
				case "double":
					return "float64";
				case "float":
					return "float32";
				case "DWORD":
				case "pid_t":
					return "uint32";
				case "void":
				case "uint8":
				case "uint16":
				case "uint32":
				case "uint64":
				case "int8":
				case "int16":
				case "int32":
				case "int64":
					return type;
				case "long long":
					return "int64";
				case "char":
					return "uint8";
				case "wchar":
				case "char16":
					return "char";
			}
			if (type.EndsWith("*"))
			{
				return GetILTypeName(type.Remove(type.Length - 1)) + "*";
			}
			return "valuetype " + ResolveType(type);
		}

		private IEnumerable<string> GetCalliMethodTypes(string thisArgTypeName, IEnumerable<CodeMethodParameter> @params)
		{
			if (thisArgTypeName != null)
				yield return GetILTypeName(thisArgTypeName);
			foreach (CodeMethodParameter param in @params)
			{
				if (param.Direction == CodeMethodParameterDirection.Ref)
					yield return GetILTypeName(param.Type) + "&";
				else
					yield return GetILTypeName(param.Type);
			}
		}

	}
}
