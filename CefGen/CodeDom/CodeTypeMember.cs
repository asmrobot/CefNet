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
	public abstract class CodeTypeMember
	{
		public CodeTypeMember(string name)
		{
			this.Name = name;
		}

		public string Name { get; }

		public CodeAttributes Attributes { get; set; }

		public List<CodeComment> Comments { get; } = new List<CodeComment>(0);

		public List<CustomCodeAttribute> CustomAttributes { get; } = new List<CustomCodeAttribute>(0);

		public string LegacyDefine { get; set; }
	}
}
