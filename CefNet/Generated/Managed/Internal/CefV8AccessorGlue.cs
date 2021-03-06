// --------------------------------------------------------------------------------------------
// Copyright (c) 2019 The CefNet Authors. All rights reserved.
// Licensed under the MIT license.
// See the licence file in the project root for full license information.
// --------------------------------------------------------------------------------------------
// Generated by CefGen
// Source: Generated/Native/Types/cef_v8accessor_t.cs
// --------------------------------------------------------------------------------------------﻿
// DO NOT MODIFY! THIS IS AUTOGENERATED FILE!
// --------------------------------------------------------------------------------------------

#pragma warning disable 0169, 1591, 1573

using System;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using CefNet.WinApi;
using CefNet.CApi;
using CefNet.Internal;

namespace CefNet.Internal
{
	sealed partial class CefV8AccessorGlue: CefV8Accessor, ICefV8AccessorPrivate
	{
		private WebViewGlue _implementation;

		public CefV8AccessorGlue(WebViewGlue impl)
		{
			_implementation = impl;
		}

		bool ICefV8AccessorPrivate.AvoidGet()
		{
			return _implementation.AvoidGet();
		}

		protected internal unsafe override bool Get(string name, CefV8Value @object, ref CefV8Value retval, ref string exception)
		{
			return _implementation.Get(name, @object, ref retval, ref exception);
		}

		bool ICefV8AccessorPrivate.AvoidSet()
		{
			return _implementation.AvoidSet();
		}

		protected internal unsafe override bool Set(string name, CefV8Value @object, CefV8Value value, ref string exception)
		{
			return _implementation.Set(name, @object, value, ref exception);
		}

	}
}
