// --------------------------------------------------------------------------------------------
// Copyright (c) 2019 The CefNet Authors. All rights reserved.
// Licensed under the MIT license.
// See the licence file in the project root for full license information.
// --------------------------------------------------------------------------------------------
// Generated by CefGen
// Source: Generated/Native/Types/cef_render_process_handler_t.cs
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
	sealed partial class CefRenderProcessHandlerGlue: CefRenderProcessHandler, ICefRenderProcessHandlerPrivate
	{
		private CefAppGlue _implementation;

		public CefRenderProcessHandlerGlue(CefAppGlue impl)
		{
			_implementation = impl;
		}

		protected internal unsafe override void OnWebKitInitialized()
		{
			_implementation.OnWebKitInitialized();
		}

		bool ICefRenderProcessHandlerPrivate.AvoidOnBrowserCreated()
		{
			return _implementation.AvoidOnBrowserCreated();
		}

		protected internal unsafe override void OnBrowserCreated(CefBrowser browser, CefDictionaryValue extraInfo)
		{
			_implementation.OnBrowserCreated(browser, extraInfo);
		}

		bool ICefRenderProcessHandlerPrivate.AvoidOnBrowserDestroyed()
		{
			return _implementation.AvoidOnBrowserDestroyed();
		}

		protected internal unsafe override void OnBrowserDestroyed(CefBrowser browser)
		{
			_implementation.OnBrowserDestroyed(browser);
		}

		protected internal unsafe override CefLoadHandler GetLoadHandler()
		{
			return _implementation.GetLoadHandler();
		}

		bool ICefRenderProcessHandlerPrivate.AvoidOnContextCreated()
		{
			return _implementation.AvoidOnContextCreated();
		}

		protected internal unsafe override void OnContextCreated(CefBrowser browser, CefFrame frame, CefV8Context context)
		{
			_implementation.OnContextCreated(browser, frame, context);
		}

		bool ICefRenderProcessHandlerPrivate.AvoidOnContextReleased()
		{
			return _implementation.AvoidOnContextReleased();
		}

		protected internal unsafe override void OnContextReleased(CefBrowser browser, CefFrame frame, CefV8Context context)
		{
			_implementation.OnContextReleased(browser, frame, context);
		}

		bool ICefRenderProcessHandlerPrivate.AvoidOnUncaughtException()
		{
			return _implementation.AvoidOnUncaughtException();
		}

		protected internal unsafe override void OnUncaughtException(CefBrowser browser, CefFrame frame, CefV8Context context, CefV8Exception exception, CefV8StackTrace stackTrace)
		{
			_implementation.OnUncaughtException(browser, frame, context, exception, stackTrace);
		}

		bool ICefRenderProcessHandlerPrivate.AvoidOnFocusedNodeChanged()
		{
			return _implementation.AvoidOnFocusedNodeChanged();
		}

		protected internal unsafe override void OnFocusedNodeChanged(CefBrowser browser, CefFrame frame, CefDOMNode node)
		{
			_implementation.OnFocusedNodeChanged(browser, frame, node);
		}

		bool ICefRenderProcessHandlerPrivate.AvoidOnProcessMessageReceived()
		{
			return _implementation.AvoidOnProcessMessageReceived();
		}

		protected internal unsafe override bool OnProcessMessageReceived(CefBrowser browser, CefFrame frame, CefProcessId sourceProcess, CefProcessMessage message)
		{
			return _implementation.OnProcessMessageReceived(browser, frame, sourceProcess, message);
		}

	}
}
