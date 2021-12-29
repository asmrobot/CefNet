// --------------------------------------------------------------------------------------------
// Copyright (c) 2019 The CefNet Authors. All rights reserved.
// Licensed under the MIT license.
// See the licence file in the project root for full license information.
// --------------------------------------------------------------------------------------------

using System.IO;

namespace CefGen
{
	abstract class CodeGenBase
	{
		private int _indent;

		public CodeGenBase()
		{

		}

		protected TextWriter Output { get; set; }

		protected virtual void GenerateGlobalDirectivesCode() { }

		protected void ResetIndent()
		{
			_indent = 0;
		}

		public void WriteBlockStart(CodeGenBlockType blockType)
		{
			Output.WriteLine();
			WriteIndent();
			Output.WriteLine("{");
			_indent += 4;
		}

		public void WriteBlockEnd(CodeGenBlockType blockType)
		{
			_indent -= 4;
			WriteIndent();
			Output.WriteLine("}");
		}

		public void WriteIndent()
		{
			WriteIndent(0);
		}

		public void WriteIndent(int plus)
		{
			int tabs = _indent >> 2;
			tabs += plus;
			while (tabs-- > 0)
			{
				Output.Write('\t');
			}
		}

	}
}
