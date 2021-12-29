// --------------------------------------------------------------------------------------------
// Copyright (c) 2019 The CefNet Authors. All rights reserved.
// Licensed under the MIT license.
// See the licence file in the project root for full license information.
// --------------------------------------------------------------------------------------------

namespace CefGen.CodeDom
{
	public sealed class CodeComment
	{
		public CodeComment(string commentText)
			: this(commentText, false)
		{

		}

		public CodeComment(string commentText, bool isXml)
		{
			this.Text = commentText;
			this.IsXml = isXml;
		}

		public string Text { get; }

		public bool IsXml { get; set; }
	}
}
