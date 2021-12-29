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
	class CodeFile
	{
		public List<CodeNamespace> Namespaces { get; } = new List<CodeNamespace>();

		public List<CodeComment> Comments { get; } = new List<CodeComment>();

		public List<CodeNamespaceImport> Imports { get; } = new List<CodeNamespaceImport>();

	}
}
