// --------------------------------------------------------------------------------------------
// Copyright (c) 2019 The CefNet Authors. All rights reserved.
// Licensed under the MIT license.
// See the licence file in the project root for full license information.
// --------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

namespace CefGen.CodeDom
{
	public sealed class CodeDelegate : CodeTypeMember
	{
		public CodeDelegate(string returnTypeName, string name)
			: base(name)
		{
			this.ReturnTypeName = returnTypeName;
		}

		public List<CodeMethodParameter> Parameters { get; } = new List<CodeMethodParameter>(0);

		public string ReturnTypeName { get; set; }
	}
}
