// --------------------------------------------------------------------------------------------
// Copyright (c) 2019 The CefNet Authors. All rights reserved.
// Licensed under the MIT license.
// See the licence file in the project root for full license information.
// --------------------------------------------------------------------------------------------
// Generated by CefGen
// Source: Generated/Native/Types/cef_life_span_handler_t.cs
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
	sealed partial class CefLifeSpanHandlerGlue: CefLifeSpanHandler, ICefLifeSpanHandlerPrivate
	{
		private WebViewGlue _implementation;

		public CefLifeSpanHandlerGlue(WebViewGlue impl)
		{
			_implementation = impl;
		}

		bool ICefLifeSpanHandlerPrivate.AvoidOnBeforePopup()
		{
			return _implementation.AvoidOnBeforePopup();
		}

		protected internal unsafe override bool OnBeforePopup(CefBrowser browser, CefFrame frame, string targetUrl, string targetFrameName, CefWindowOpenDisposition targetDisposition, bool userGesture, CefPopupFeatures popupFeatures, CefWindowInfo windowInfo, ref CefClient client, CefBrowserSettings settings, ref CefDictionaryValue extraInfo, ref int noJavascriptAccess)
		{
			return _implementation.OnBeforePopup(browser, frame, targetUrl, targetFrameName, targetDisposition, userGesture, popupFeatures, windowInfo, ref client, settings, ref extraInfo, ref noJavascriptAccess);
		}

		bool ICefLifeSpanHandlerPrivate.AvoidOnAfterCreated()
		{
			return _implementation.AvoidOnAfterCreated();
		}

		protected internal unsafe override void OnAfterCreated(CefBrowser browser)
		{
			_implementation.OnAfterCreated(browser);
		}

		bool ICefLifeSpanHandlerPrivate.AvoidDoClose()
		{
			return _implementation.AvoidDoClose();
		}

		protected internal unsafe override bool DoClose(CefBrowser browser)
		{
			return _implementation.DoClose(browser);
		}

		bool ICefLifeSpanHandlerPrivate.AvoidOnBeforeClose()
		{
			return _implementation.AvoidOnBeforeClose();
		}

		protected internal unsafe override void OnBeforeClose(CefBrowser browser)
		{
			_implementation.OnBeforeClose(browser);
		}

	}
}
