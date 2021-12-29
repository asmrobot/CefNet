// --------------------------------------------------------------------------------------------
// Copyright (c) 2019 The CefNet Authors. All rights reserved.
// Licensed under the MIT license.
// See the licence file in the project root for full license information.
// --------------------------------------------------------------------------------------------

using System;

namespace CefGen
{
	public sealed class ResolveTypeNameEventArgs : EventArgs
	{
		public ResolveTypeNameEventArgs(string type)
		{
			this.Type = type;
			this.Result = type;
		}

		public string Type { get; set; }

		public string Result { get; set; }
	}
}