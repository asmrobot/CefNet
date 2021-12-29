// --------------------------------------------------------------------------------------------
// Copyright (c) 2019 The CefNet Authors. All rights reserved.
// Licensed under the MIT license.
// See the licence file in the project root for full license information.
// --------------------------------------------------------------------------------------------

using System;

namespace CefGen.CodeDom
{
	[Flags]
	public enum CodeMethodParameterDirection
	{
		Default = 0,
		In = 1 << 0,
		Out = 1 << 1,
		Ref = In | Out,
	}
}
