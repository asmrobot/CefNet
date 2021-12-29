// --------------------------------------------------------------------------------------------
// Copyright (c) 2019 The CefNet Authors. All rights reserved.
// Licensed under the MIT license.
// See the licence file in the project root for full license information.
// --------------------------------------------------------------------------------------------

using CefGen.CodeDom;
using System;
using System.Collections.Generic;
using System.Text;

namespace CefGen
{
	abstract class MsilCodeGenBase : CefCodeGenBase
	{
		private Stack<string> _namespaces = new Stack<string>();
		private Stack<string> _classes = new Stack<string>();

		public MsilCodeGenBase()
		{

		}


		protected virtual string Namespace
		{
			get
			{
				return string.Join(".", _namespaces);
			}
		}

		protected virtual string ClassName
		{
			get
			{
				return string.Join(".", _classes);
			}
		}

		protected override void GenerateCommentCode(CodeComment commentDecl)
		{
			if (commentDecl.IsXml)
				return;

			GenerateCommentCode(commentDecl.Text);
		}

		protected virtual void GenerateCommentCode(string commentText)
		{
			foreach (string line in commentText.Split('\n'))
			{
				WriteIndent();
				Output.Write("// ");
				Output.WriteLine(line.TrimEnd('\r'));
			}
		}

		protected virtual void WriteAttributes(CodeAttributes attributes)
		{
			if (attributes.HasFlag(CodeAttributes.Public))
			{
				Output.Write("public ");
			}
			else if (attributes.HasFlag(CodeAttributes.Private))
			{
				Output.Write("private ");
			}
			else if (attributes.HasFlag(CodeAttributes.Internal))
			{
				Output.Write("internal ");
			}
		}

		protected override void GenerateNamespaceCode(CodeNamespace namespaceDecl)
		{
			WriteIndent();
			Output.Write(".namespace ");
			Output.Write(namespaceDecl.Name);
			WriteBlockStart(CodeGenBlockType.Namespace);
			_namespaces.Push(namespaceDecl.Name);

			bool insertLine = false;
			foreach (CodeType typeDecl in namespaceDecl.Types)
			{
				if (insertLine)
					Output.WriteLine();

				GenerateTypeCode(typeDecl, namespaceDecl.Name);
				insertLine = true;
			}

			_namespaces.Pop();
			WriteBlockEnd(CodeGenBlockType.Namespace);
		}

		protected virtual void GenerateTypeCode(CodeType typeDecl, string typeRef)
		{
			typeRef = typeRef + "." + typeDecl.Name;

			foreach (CodeComment commentDecl in typeDecl.Comments)
			{
				GenerateCommentCode(commentDecl);
			}

			WriteIndent();
			Output.Write(".class ");
			WriteAttributes(typeDecl.Attributes);
			Output.Write(typeDecl.Name);
			WriteBlockStart(CodeGenBlockType.Type);
			_classes.Push(typeDecl.Name);

			bool insertLine = false;
			foreach (CodeTypeMember memberDecl in typeDecl.Members)
			{
				if (insertLine)
					Output.WriteLine();

				if (memberDecl is CodeType classDecl)
				{
					GenerateTypeCode(classDecl, typeRef);
				}
				else if (memberDecl is CodeMethod methodDecl)
				{
					if (methodDecl.Callee == null)
						continue;
					if (!GenerateMethodCode(methodDecl, typeRef))
						continue;
				}
				else
				{
					insertLine = false;
					continue;
				}
				insertLine = true;
			}
			_classes.Pop();
			WriteBlockEnd(CodeGenBlockType.Type);

		}

		protected abstract bool GenerateMethodCode(CodeMethod methodDecl, string typeRef);

		protected static bool IsNotXmlTagComment(CodeComment c)
		{
			if (!c.IsXml)
				return true;
			string s = c.Text;
			return !(s.StartsWith('<') && s.EndsWith('>'));
		}

		protected static string GetName(string name)
		{
			int lastDot = name.LastIndexOf('.');
			int lastSemi = name.LastIndexOf("::");
			if (lastSemi > lastDot)
			{
				return name.Substring(lastSemi + 2);
			}
			return name.Substring(lastDot + 1);
		}

	}
}
