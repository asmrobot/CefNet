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
	public abstract class CodeType : CodeTypeMember
	{
		public CodeType(string name)
			: base(name)
		{

		}

		public string BaseType { get; set; }

		public List<CodeTypeMember> Members { get; } = new List<CodeTypeMember>(0);

	}
}
