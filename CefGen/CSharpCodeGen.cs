// --------------------------------------------------------------------------------------------
// Copyright (c) 2019 The CefNet Authors. All rights reserved.
// Licensed under the MIT license.
// See the licence file in the project root for full license information.
// --------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using CefGen.CodeDom;

namespace CefGen
{
	sealed class CSharpCodeGen : CefCodeGenBase
	{
		public CSharpCodeGen()
		{

		}

		public IList<string> Directives { get; } = new List<string>();

		protected override void GenerateGlobalDirectivesCode()
		{
			bool insertline = true;
			foreach(string s in Directives)
			{
				if (insertline) Output.WriteLine();
				Output.WriteLine(s);
				insertline = false;
			}
		}

		protected override void GenerateNamespaceImportCode(CodeNamespaceImport importDecl)
		{
			WriteIndent();
			Output.Write("using ");
			Output.Write(importDecl.Namespace);
			Output.WriteLine(";");
		}

		protected override void GenerateNamespaceCode(CodeNamespace namespaceDecl)
		{
			WriteIndent();
			Output.Write("namespace ");
			Output.Write(namespaceDecl.Name);
			WriteBlockStart(CodeGenBlockType.Namespace);

			foreach (CodeNamespaceImport importDecl in namespaceDecl.Imports)
			{
				GenerateNamespaceImportCode(importDecl);
			}

			bool insertLine = false;
			foreach (CodeType typeDecl in namespaceDecl.Types)
			{
				if (insertLine)
					Output.WriteLine();

				GenerateTypeCode(typeDecl);
				insertLine = true;
			}

			WriteBlockEnd(CodeGenBlockType.Namespace);

		}

		private void WriteAttributes(CodeAttributes attributes)
		{
			WriteIndent();
			if (attributes.HasFlag(CodeAttributes.Public))
			{
				Output.Write("public ");
			}
			else if (attributes.HasFlag(CodeAttributes.Protected))
			{
				Output.Write("protected ");
				if (attributes.HasFlag(CodeAttributes.Internal))
					Output.Write("internal ");
				else if (attributes.HasFlag(CodeAttributes.Private))
					Output.Write("private ");
			}
			else if (attributes.HasFlag(CodeAttributes.Internal))
			{
				Output.Write("internal ");
			}
			else if (attributes.HasFlag(CodeAttributes.Private))
			{
				Output.Write("private ");
			}

			if (attributes.HasFlag(CodeAttributes.Static))
			{
				Output.Write("static ");
			}

			if (attributes.HasFlag(CodeAttributes.ReadOnly))
			{
				Output.Write("readonly ");
			}

			if (attributes.HasFlag(CodeAttributes.Unsafe))
			{
				Output.Write("unsafe ");
			}

			if (attributes.HasFlag(CodeAttributes.Abstract))
			{
				Output.Write("abstract ");
			} 
			else if (attributes.HasFlag(CodeAttributes.Virtual))
			{
				Output.Write("virtual ");
			}
			else if (attributes.HasFlag(CodeAttributes.Overrided))
			{
				Output.Write("override ");
			}

			if (!attributes.HasFlag(CodeAttributes.Abstract)
				&& attributes.HasFlag(CodeAttributes.External))
			{
				Output.Write("extern ");
			}

			if(attributes.HasFlag(CodeAttributes.Partial))
			{
				Output.Write("partial ");
			}
		}

		private void GenerateTypeCode(CodeType typeDecl)
		{
			foreach (CodeComment commentDecl in typeDecl.Comments)
			{
				GenerateCommentCode(commentDecl);
			}

			WriteCustomAttributes(typeDecl.CustomAttributes);

			WriteAttributes(typeDecl.Attributes);
			if (typeDecl is CodeStruct)
			{
				Output.Write("struct ");
			}
			else if (typeDecl is CodeEnum)
			{
				Output.Write("enum ");
			}
			else if (typeDecl is CodeClass)
			{
				Output.Write("class ");
			}

			Output.Write(typeDecl.Name);

			if (typeDecl.BaseType != null)
			{
				Output.Write(" : ");
				Output.Write(typeDecl.BaseType);
			}

			WriteBlockStart(CodeGenBlockType.Type);

			var defines = new Stack<string>();

			bool insertLine = false;
			foreach (CodeTypeMember memberDecl in typeDecl.Members)
			{
				if (insertLine)
					Output.WriteLine();

				if (memberDecl.LegacyDefine is not null)
				{
					if (defines.Count == 0 || defines.Peek() != memberDecl.LegacyDefine)
					{
						if (defines.Count > 0)
						{
							Output.WriteLine("#endif // " + defines.Pop());
						}

						defines.Push(memberDecl.LegacyDefine);
						Output.Write("#if ");
						Output.WriteLine(memberDecl.LegacyDefine);
					}
				}
				else if (defines.Count > 0)
				{
					Output.WriteLine("#endif // " + defines.Pop());
				}

				if (memberDecl is CodeType classDecl)
				{
					GenerateTypeCode(classDecl);
				}
				else if (memberDecl is CodeField fieldDecl)
				{
					GenerateFieldCode(fieldDecl);
				}
				else if (memberDecl is CodeMethod methodDecl)
				{
					GenerateMethodCode(methodDecl);
				}
				else if (memberDecl is CodeEnumItem itemDecl)
				{
					GenerateEnumItemCode(itemDecl);
				}
				else if (memberDecl is CodeDelegate delegateDecl)
				{
					GenerateDelegateCode(delegateDecl);
				}
				else if (memberDecl is CodeConstructor ctorDecl)
				{
					GenerateConstructorCode(ctorDecl);
				}
				else if (memberDecl is CodeProperty propDecl)
				{
					GeneratePropertyCode(propDecl);
				}
				else if (memberDecl is CodeOperator operatorDecl)
				{
					GenerateOperatorCode(operatorDecl);
				}
				else if (memberDecl is CodeFinalizer dtorDecl)
				{
					GenerateFinalizerCode(dtorDecl);
				}
				insertLine = true;
			}

			while (defines.Count > 0)
			{
				Output.WriteLine("#endif // " + defines.Pop());
			}

			WriteBlockEnd(CodeGenBlockType.Type);

		}

		private void GenerateEnumItemCode(CodeEnumItem itemDecl)
		{
			foreach (CodeComment commentDecl in itemDecl.Comments)
			{
				GenerateCommentCode(commentDecl);
			}

			string value = itemDecl.Value;
			if (value.StartsWith("0x")
					&& uint.TryParse(value.Substring(2), NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture, out uint num)
					&& num > int.MaxValue)
			{
				value = "unchecked((int)" + value + ")";
			}

			WriteIndent();
			Output.Write(itemDecl.Name);
			Output.Write(" = ");
			Output.Write(value);
			Output.WriteLine(",");
		}

		private void GeneratePropertyCode(CodeProperty propDecl)
		{
			foreach (CodeComment commentDecl in propDecl.Comments)
			{
				GenerateCommentCode(commentDecl);
			}

			WriteAttributes(propDecl.Attributes);
			Output.Write(GetClrTypeName(propDecl.Type.Type));
			Output.Write(" ");
			Output.Write(propDecl.Name);
			WriteBlockStart(CodeGenBlockType.Method);
			if (propDecl.GetterBody != null)
			{
				WriteIndent();
				Output.Write("get");
				WriteBlockStart(CodeGenBlockType.Method);
				GenerateMethodBodyCode(propDecl.GetterBody);
				WriteBlockEnd(CodeGenBlockType.Method);
			}
			if (propDecl.SetterBody != null)
			{
				WriteIndent();
				Output.Write("set");
				WriteBlockStart(CodeGenBlockType.Method);
				GenerateMethodBodyCode(propDecl.SetterBody);
				WriteBlockEnd(CodeGenBlockType.Method);
			}
			WriteBlockEnd(CodeGenBlockType.Method);
		}

		private void GenerateFieldCode(CodeField fieldDecl)
		{
			foreach (CodeComment commentDecl in fieldDecl.Comments)
			{
				GenerateCommentCode(commentDecl);
			}

			WriteCustomAttributes(fieldDecl.CustomAttributes);

			WriteAttributes(fieldDecl.Attributes);
			Output.Write(GetClrTypeName(fieldDecl.TypeName));
			Output.Write(" ");
			Output.Write(fieldDecl.Name);
			if (fieldDecl.Value != null)
			{
				Output.Write(" = ");
				Output.Write(fieldDecl.Value);
			}
			Output.WriteLine(";");
		}

		private void GenerateDelegateCode(CodeDelegate delegateDecl)
		{
			foreach (CodeComment commentDecl in delegateDecl.Comments)
			{
				GenerateCommentCode(commentDecl);
			}

			WriteCustomAttributes(delegateDecl.CustomAttributes);
			WriteAttributes(delegateDecl.Attributes);
			Output.Write("delegate ");
			Output.Write(GetClrTypeName(delegateDecl.ReturnTypeName));
			Output.Write(" ");
			Output.Write(delegateDecl.Name);
			Output.Write("(");
			Output.Write(string.Join(", ", delegateDecl.Parameters.Select(arg => GetClrTypeName(arg.Type) + " " + arg.Name)));
			Output.WriteLine(");");

		}

		private void GenerateConstructorCode(CodeConstructor ctorDecl)
		{
			foreach (CodeComment commentDecl in ctorDecl.Comments)
			{
				GenerateCommentCode(commentDecl);
			}

			WriteCustomAttributes(ctorDecl.CustomAttributes);
			WriteAttributes(ctorDecl.Attributes);
			Output.Write(ctorDecl.Name);
			Output.Write("(");
			Output.Write(string.Join(", ", ctorDecl.Parameters.Select(arg => GetClrTypeName(arg.Type) + " " + arg.Name)));
			Output.Write(")");
			if (ctorDecl.BaseArgs.Count > 0)
			{
				Output.WriteLine();
				WriteIndent(1);
				Output.Write(": base(");
				Output.Write(string.Join(", ", ctorDecl.BaseArgs));
				Output.Write(")");
			}
			WriteBlockStart(CodeGenBlockType.Method);
			GenerateMethodBodyCode(ctorDecl.Body);
			WriteBlockEnd(CodeGenBlockType.Method);

		}

		private void GenerateFinalizerCode(CodeFinalizer dtorDecl)
		{
			WriteIndent();
			Output.Write("~");
			Output.Write(dtorDecl.Name);
			Output.Write("()");
			WriteBlockStart(CodeGenBlockType.Method);
			GenerateMethodBodyCode(dtorDecl.Body);
			WriteBlockEnd(CodeGenBlockType.Method);
		}

		private void GenerateOperatorCode(CodeOperator opdDecl)
		{
			foreach (CodeComment commentDecl in opdDecl.Comments)
			{
				GenerateCommentCode(commentDecl);
			}

			WriteCustomAttributes(opdDecl.CustomAttributes);
			WriteAttributes(opdDecl.Attributes);
			Output.Write(GetClrTypeName(opdDecl.RetVal.Type));
			Output.Write(" operator ");
			Output.Write(opdDecl.Name);
			Output.Write("(");

			var args = new List<string>(opdDecl.Parameters.Count);
			foreach (CodeMethodParameter param in opdDecl.Parameters)
			{
				string modifier = string.Empty;
				if (param.Direction == CodeMethodParameterDirection.Ref)
				{
					modifier = "ref ";
				}
				else if (param.Direction == CodeMethodParameterDirection.Out)
				{
					modifier = "out ";
				}
				args.Add(string.Format("{0}{1} {2}", modifier, param.Type, param.Name));
			}
			Output.Write(string.Join(", ", args));


			Output.Write(")");
			WriteBlockStart(CodeGenBlockType.Method);
			GenerateMethodBodyCode(opdDecl.Body);
			WriteBlockEnd(CodeGenBlockType.Method);

		}

		private void GenerateMethodCode(CodeMethod methodDecl)
		{
			foreach (CodeComment commentDecl in methodDecl.Comments)
			{
				GenerateCommentCode(commentDecl);
			}

			WriteCustomAttributes(methodDecl.CustomAttributes);
			WriteAttributes(methodDecl.Attributes);
			Output.Write(GetClrTypeName(methodDecl.RetVal.Type));
			Output.Write(" ");
			Output.Write(methodDecl.Name);
			Output.Write("(");

			var args = new List<string>(methodDecl.Parameters.Count);
			foreach(CodeMethodParameter param in methodDecl.Parameters)
			{
				string modifier = string.Empty;
				if (param.Direction == CodeMethodParameterDirection.Ref)
				{
					modifier = "ref ";
				}
				else if (param.Direction == CodeMethodParameterDirection.Out)
				{
					modifier = "out ";
				}
				string customAttributes = string.Join(", ", param.CustomAttributes.Select(a => a.ToString()));
				if (customAttributes != null && customAttributes.Length > 0)
				{
					customAttributes = "[" + customAttributes + "]";
				}

				args.Add(string.Format("{3}{0}{1} {2}", modifier, GetClrTypeName(param.Type), param.Name, customAttributes));
			}
			Output.Write(string.Join(", ", args));
			//Output.Write(string.Join(", ", methodDecl.Parameters.Select(arg => GetClrTypeName(arg.Type) + " " + arg.Name)));


			Output.Write(")");
			if (methodDecl.NoBody)
			{
				Output.WriteLine(";");
				return;
			}
			WriteBlockStart(CodeGenBlockType.Method);
			GenerateMethodBodyCode(methodDecl.Body);
			WriteBlockEnd(CodeGenBlockType.Method);
			
		}

		private void GenerateMethodBodyCode(string body)
		{
			if (body == null)
				return;
			foreach (string line in body.Split('\n'))
			{
				WriteIndent();
				Output.WriteLine(line.TrimEnd('\r'));
			}
		}

		private void WriteCustomAttributes(IList<CustomCodeAttribute> customAttributes)
		{
			foreach (CustomCodeAttribute attribute in customAttributes.OrderBy(a => a.ToString().Length))
			{
				if (attribute.Condition != null)
					Output.WriteLine("#if " + attribute.Condition);
				WriteIndent();
				Output.Write("[");
				Output.Write(attribute.ToString());
				Output.WriteLine("]");
				if (attribute.Condition != null)
					Output.WriteLine("#endif");
			}
		}

		protected override void GenerateCommentCode(CodeComment commentDecl)
		{
			string commentPrefix;
			if (commentDecl.IsXml)
			{
				commentPrefix = "/// ";
			}
			else
			{
				commentPrefix = "// ";
			}
			foreach (string line in commentDecl.Text.Split('\n'))
			{
				WriteIndent();
				Output.Write(commentPrefix);
				Output.WriteLine(line.TrimEnd('\r'));
			}
		}

		internal static string GetClrTypeName(string type)
		{
			switch (type)
			{
				case "size_t":
					return "UIntPtr";
				case "HWND":
				case "HMENU":
				case "HINSTANCE":
				case "HCURSOR":
				case "time_t":
				case "pthread_t":
					return "IntPtr";
				case "void*":
					return type;
				case "uint16":
					return "ushort";
				case "int32":
					return "int";
				case "uint32":
				case "DWORD":
				case "pid_t":
					return "uint";
				case "int64":
				case "long long":
					return "long";
				case "uint64":
					return "ulong";
				case "char":
					return "byte";
				case "wchar":
				case "char16":
					return "char";
				case "const char*":
				case "XDisplay*":
					return "IntPtr";

			}
			if (type.EndsWith('*'))
			{
				string t = type.Remove(type.Length - 1);
				return GetClrTypeName(t) + "*";
			}
			return type;
		}

	}
}
