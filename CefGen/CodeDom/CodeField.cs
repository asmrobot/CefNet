// --------------------------------------------------------------------------------------------
// Copyright (c) 2019 The CefNet Authors. All rights reserved.
// Licensed under the MIT license.
// See the licence file in the project root for full license information.
// --------------------------------------------------------------------------------------------

namespace CefGen.CodeDom
{
	public sealed class CodeField : CodeTypeMember
	{
		public CodeField(string typeName, string name)
			: base(name)
		{
			this.TypeName = typeName;
		}

		public string TypeName { get; set; }

		public string Value { get; set; }
	}
}
