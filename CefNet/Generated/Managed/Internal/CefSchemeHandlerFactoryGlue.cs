// --------------------------------------------------------------------------------------------
// Copyright (c) 2019 The CefNet Authors. All rights reserved.
// Licensed under the MIT license.
// See the licence file in the project root for full license information.
// --------------------------------------------------------------------------------------------
// Generated by CefGen
// Source: Generated/Native/Types/cef_scheme_handler_factory_t.cs
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
	sealed partial class CefSchemeHandlerFactoryGlue: CefSchemeHandlerFactory, ICefSchemeHandlerFactoryPrivate
	{
		private WebViewGlue _implementation;

		public CefSchemeHandlerFactoryGlue(WebViewGlue impl)
		{
			_implementation = impl;
		}

		bool ICefSchemeHandlerFactoryPrivate.AvoidCreate()
		{
			return _implementation.AvoidCreate();
		}

		protected internal unsafe override CefResourceHandler Create(CefBrowser browser, CefFrame frame, string schemeName, CefRequest request)
		{
			return _implementation.Create(browser, frame, schemeName, request);
		}

	}
}
