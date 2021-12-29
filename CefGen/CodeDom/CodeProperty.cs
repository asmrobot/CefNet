// --------------------------------------------------------------------------------------------
// Copyright (c) 2019 The CefNet Authors. All rights reserved.
// Licensed under the MIT license.
// See the licence file in the project root for full license information.
// --------------------------------------------------------------------------------------------

namespace CefGen.CodeDom
{
	public class CodeProperty : CodeTypeMember
	{
		public CodeProperty(string name)
			: base(name)
		{

		}

		public CodeMethodParameter Type { get; set; }

		public string GetterBody { get; set; }

		public string SetterBody { get; set; }

	}
}
