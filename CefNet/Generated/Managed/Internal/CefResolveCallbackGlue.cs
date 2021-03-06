// --------------------------------------------------------------------------------------------
// Copyright (c) 2019 The CefNet Authors. All rights reserved.
// Licensed under the MIT license.
// See the licence file in the project root for full license information.
// --------------------------------------------------------------------------------------------
// Generated by CefGen
// Source: Generated/Native/Types/cef_resolve_callback_t.cs
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
	sealed partial class CefResolveCallbackGlue: CefResolveCallback, ICefResolveCallbackPrivate
	{
		private WebViewGlue _implementation;

		public CefResolveCallbackGlue(WebViewGlue impl)
		{
			_implementation = impl;
		}

		bool ICefResolveCallbackPrivate.AvoidOnResolveCompleted()
		{
			return _implementation.AvoidOnResolveCompleted();
		}

		protected internal unsafe override void OnResolveCompleted(CefErrorCode result, CefStringList resolvedIps)
		{
			_implementation.OnResolveCompleted(result, resolvedIps);
		}

	}
}
