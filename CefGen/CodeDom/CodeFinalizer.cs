// --------------------------------------------------------------------------------------------
// Copyright (c) 2019 The CefNet Authors. All rights reserved.
// Licensed under the MIT license.
// See the licence file in the project root for full license information.
// --------------------------------------------------------------------------------------------

namespace CefGen.CodeDom
{
	public sealed class CodeFinalizer : CodeTypeMember
	{
		public CodeFinalizer(string name)
			: base(name)
		{

		}

		public string Body { get; internal set; }
	}
}
