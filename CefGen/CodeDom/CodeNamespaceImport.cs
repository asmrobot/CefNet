// --------------------------------------------------------------------------------------------
// Copyright (c) 2019 The CefNet Authors. All rights reserved.
// Licensed under the MIT license.
// See the licence file in the project root for full license information.
// --------------------------------------------------------------------------------------------

namespace CefGen.CodeDom
{
	class CodeNamespaceImport
	{

		public CodeNamespaceImport(string @namespace)
		{
			this.Namespace = @namespace;
		}

		public string Namespace { get; set; }
	}
}
