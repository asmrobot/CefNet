// --------------------------------------------------------------------------------------------
// Copyright (c) 2019 The CefNet Authors. All rights reserved.
// Licensed under the MIT license.
// See the licence file in the project root for full license information.
// --------------------------------------------------------------------------------------------

using System;

namespace CefGen.CodeDom
{
	[Flags]
	public enum CodeAttributes
	{
		None = 0,
		Private = 1 << 0,
		Public = 1 << 1,
		Internal = 1 << 2,
		Sealed = 1 << 3,
		Unsafe = 1 << 4,
		Abstract = 1 << 5,
		External = 1 << 6,
		ReadOnly = 1 << 7,
		Static = 1 << 8,
		Virtual = 1 << 9,
		Overrided = 1 << 10,
		Protected = 1 << 11,
		Partial = 1 << 12,
	}
}
