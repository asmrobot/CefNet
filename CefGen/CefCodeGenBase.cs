// --------------------------------------------------------------------------------------------
// Copyright (c) 2019 The CefNet Authors. All rights reserved.
// Licensed under the MIT license.
// See the licence file in the project root for full license information.
// --------------------------------------------------------------------------------------------

using CefGen.CodeDom;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CefGen
{
	abstract class CefCodeGenBase : CodeGenBase
	{

		public event EventHandler<ResolveTypeNameEventArgs> ResolveTypeName;

		public CefCodeGenBase()
		{

		}

		public void GenerateCode(CodeFile fileDecl, TextWriter output)
		{
			ResetIndent();

			this.Output = output;
			try
			{
				foreach (CodeComment commentDecl in fileDecl.Comments)
				{
					GenerateCommentCode(commentDecl);
				}

				GenerateGlobalDirectivesCode();

				bool insertline = true;
				foreach (CodeNamespaceImport importDecl in fileDecl.Imports)
				{
					if (insertline) Output.WriteLine();
					GenerateNamespaceImportCode(importDecl);
					insertline = false;
				}

				foreach (CodeNamespace nsDecl in fileDecl.Namespaces)
				{
					Output.WriteLine();
					GenerateNamespaceCode(nsDecl);
				}
			}
			finally
			{
				this.Output = null;
			}
		}

		protected abstract void GenerateCommentCode(CodeComment commentDecl);

		protected virtual void GenerateNamespaceImportCode(CodeNamespaceImport importDecl) { }

		protected abstract void GenerateNamespaceCode(CodeNamespace namespaceDecl);

		protected string ResolveType(string type)
		{
			var ea = new ResolveTypeNameEventArgs(type);
			ResolveTypeName?.Invoke(this, ea);
			return ea.Result;
		}
	}
}
