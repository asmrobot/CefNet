// --------------------------------------------------------------------------------------------
// Copyright (c) 2019 The CefNet Authors. All rights reserved.
// Licensed under the MIT license.
// See the licence file in the project root for full license information.
// --------------------------------------------------------------------------------------------
// Generated by CefGen
// Source: Generated/Native/Types/cef_end_tracing_callback_t.cs
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
	sealed partial class CefEndTracingCallbackGlue: CefEndTracingCallback, ICefEndTracingCallbackPrivate
	{
		private WebViewGlue _implementation;

		public CefEndTracingCallbackGlue(WebViewGlue impl)
		{
			_implementation = impl;
		}

		bool ICefEndTracingCallbackPrivate.AvoidOnEndTracingComplete()
		{
			return _implementation.AvoidOnEndTracingComplete();
		}

		protected internal unsafe override void OnEndTracingComplete(string tracingFile)
		{
			_implementation.OnEndTracingComplete(tracingFile);
		}

	}
}
