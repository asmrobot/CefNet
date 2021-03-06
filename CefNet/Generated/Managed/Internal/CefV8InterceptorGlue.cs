// --------------------------------------------------------------------------------------------
// Copyright (c) 2019 The CefNet Authors. All rights reserved.
// Licensed under the MIT license.
// See the licence file in the project root for full license information.
// --------------------------------------------------------------------------------------------
// Generated by CefGen
// Source: Generated/Native/Types/cef_v8interceptor_t.cs
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
	sealed partial class CefV8InterceptorGlue: CefV8Interceptor, ICefV8InterceptorPrivate
	{
		private WebViewGlue _implementation;

		public CefV8InterceptorGlue(WebViewGlue impl)
		{
			_implementation = impl;
		}

		bool ICefV8InterceptorPrivate.AvoidGetByName()
		{
			return _implementation.AvoidGetByName();
		}

		protected internal unsafe override bool GetByName(string name, CefV8Value @object, ref CefV8Value retval, ref string exception)
		{
			return _implementation.GetByName(name, @object, ref retval, ref exception);
		}

		bool ICefV8InterceptorPrivate.AvoidGetByIndex()
		{
			return _implementation.AvoidGetByIndex();
		}

		protected internal unsafe override bool GetByIndex(int index, CefV8Value @object, ref CefV8Value retval, ref string exception)
		{
			return _implementation.GetByIndex(index, @object, ref retval, ref exception);
		}

		bool ICefV8InterceptorPrivate.AvoidSetByName()
		{
			return _implementation.AvoidSetByName();
		}

		protected internal unsafe override bool SetByName(string name, CefV8Value @object, CefV8Value value, ref string exception)
		{
			return _implementation.SetByName(name, @object, value, ref exception);
		}

		bool ICefV8InterceptorPrivate.AvoidSetByIndex()
		{
			return _implementation.AvoidSetByIndex();
		}

		protected internal unsafe override bool SetByIndex(int index, CefV8Value @object, CefV8Value value, ref string exception)
		{
			return _implementation.SetByIndex(index, @object, value, ref exception);
		}

	}
}
