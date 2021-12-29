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
	public sealed class CodeConstructor : CodeTypeMember
	{
		public CodeConstructor(string name)
			: base(name)
		{

		}

		public List<CodeMethodParameter> Parameters { get; } = new List<CodeMethodParameter>(0);

		public string Body { get; internal set; }

		public List<string> BaseArgs { get; set; } = new List<string>(0); 
	}
}
