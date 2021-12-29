// --------------------------------------------------------------------------------------------
// Copyright (c) 2019 The CefNet Authors. All rights reserved.
// Licensed under the MIT license.
// See the licence file in the project root for full license information.
// --------------------------------------------------------------------------------------------

namespace CefGen.CodeDom
{
	public class CodeEnumItem : CodeTypeMember
	{
		public CodeEnumItem(string name, string value)
			: base(name)
		{
			this.Value = value;
		}

		public string Value { get; }
	}
}
