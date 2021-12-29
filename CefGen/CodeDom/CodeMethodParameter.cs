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
	public class CodeMethodParameter
	{
		public CodeMethodParameter(string name)
		{
			this.Name = name;
		}

		public string Type { get; set; }

		public string Name { get; set; }

		public CodeMethodParameterDirection Direction { get; set; }

		public List<CustomCodeAttribute> CustomAttributes { get; } = new List<CustomCodeAttribute>(0);
	}
}
