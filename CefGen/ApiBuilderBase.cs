// --------------------------------------------------------------------------------------------
// Copyright (c) 2019 The CefNet Authors. All rights reserved.
// Licensed under the MIT license.
// See the licence file in the project root for full license information.
// --------------------------------------------------------------------------------------------

using CefGen.CodeDom;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CefGen
{
	internal abstract class ApiBuilderBase
	{
		public List<string> Imports { get; } = new List<string> {
			"System",
			"System.Runtime.InteropServices",
			"System.Runtime.CompilerServices",
			"CefNet.WinApi"
		};

		private static Lazy<string> License = new Lazy<string>(() => File.ReadAllText(Path.Combine("Settings", "LicenseTemplate.txt"), Encoding.UTF8));

		protected virtual CodeFile CreateCodeFile(string sources)
		{
			var f = new CodeFile();
			f.Comments.AddComment(string.Format(License.Value, nameof(CefGen), sources));

			foreach (string import in this.Imports)
			{
				f.Imports.Add(new CodeNamespaceImport(import));
			}

			return f;
		}

	}
}
