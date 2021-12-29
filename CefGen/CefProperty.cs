// --------------------------------------------------------------------------------------------
// Copyright (c) 2019 The CefNet Authors. All rights reserved.
// Licensed under the MIT license.
// See the licence file in the project root for full license information.
// --------------------------------------------------------------------------------------------

using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace CefGen
{
	[DebuggerDisplay("{Name}")]
	sealed class CefProperty
	{
		public IMethodSymbol Getter { get; set; }
		public IMethodSymbol Setter { get; set; }

		public ITypeSymbol SetterType
		{
			get
			{
				if (Setter == null)
					return null;
				return Setter.Parameters[0].Type;
			}
		}

		public ITypeSymbol GetterType
		{
			get
			{
				if (Getter == null)
					return null;
				return Getter.ReturnType;
			}
		}

		public string Name
		{
			get
			{
				string name;
				if (Getter == null)
				{
					name = Setter.Name.Substring(Math.Max(Setter.Name.IndexOf(char.IsUpper, 1), 0));
				}
				else if (Getter.Name.StartsWith("Get"))
				{
					name = Getter.Name.Substring(3);
				}
				else if (Setter == null)
				{
					name = Getter.Name;
				}
				else
				{
					name = Getter.Name.Substring(Math.Max(Getter.Name.IndexOf(char.IsUpper, 1), 0));
				}
				return (name == "CefType" ? "Type" : name);
			}
		}
	}
}
