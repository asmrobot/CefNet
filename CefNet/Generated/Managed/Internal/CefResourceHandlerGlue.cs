// --------------------------------------------------------------------------------------------
// Copyright (c) 2019 The CefNet Authors. All rights reserved.
// Licensed under the MIT license.
// See the licence file in the project root for full license information.
// --------------------------------------------------------------------------------------------
// Generated by CefGen
// Source: Generated/Native/Types/cef_resource_handler_t.cs
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
	sealed partial class CefResourceHandlerGlue: CefResourceHandler, ICefResourceHandlerPrivate
	{
		private WebViewGlue _implementation;

		public CefResourceHandlerGlue(WebViewGlue impl)
		{
			_implementation = impl;
		}

		bool ICefResourceHandlerPrivate.AvoidOpen()
		{
			return _implementation.AvoidOpen();
		}

		protected internal unsafe override bool Open(CefRequest request, ref int handleRequest, CefCallback callback)
		{
			return _implementation.Open(request, ref handleRequest, callback);
		}

		bool ICefResourceHandlerPrivate.AvoidProcessRequest()
		{
			return _implementation.AvoidProcessRequest();
		}

		protected internal unsafe override bool ProcessRequest(CefRequest request, CefCallback callback)
		{
			return _implementation.ProcessRequest(request, callback);
		}

		bool ICefResourceHandlerPrivate.AvoidGetResponseHeaders()
		{
			return _implementation.AvoidGetResponseHeaders();
		}

		protected internal unsafe override void GetResponseHeaders(CefResponse response, ref long responseLength, ref string redirectUrl)
		{
			_implementation.GetResponseHeaders(response, ref responseLength, ref redirectUrl);
		}

		bool ICefResourceHandlerPrivate.AvoidSkip()
		{
			return _implementation.AvoidSkip();
		}

		protected internal unsafe override bool Skip(long bytesToSkip, ref long bytesSkipped, CefResourceSkipCallback callback)
		{
			return _implementation.Skip(bytesToSkip, ref bytesSkipped, callback);
		}

		bool ICefResourceHandlerPrivate.AvoidRead()
		{
			return _implementation.AvoidRead();
		}

		protected internal unsafe override bool Read(IntPtr dataOut, int bytesToRead, ref int bytesRead, CefResourceReadCallback callback)
		{
			return _implementation.Read(dataOut, bytesToRead, ref bytesRead, callback);
		}

		bool ICefResourceHandlerPrivate.AvoidReadResponse()
		{
			return _implementation.AvoidReadResponse();
		}

		protected internal unsafe override bool ReadResponse(IntPtr dataOut, int bytesToRead, ref int bytesRead, CefCallback callback)
		{
			return _implementation.ReadResponse(dataOut, bytesToRead, ref bytesRead, callback);
		}

		protected internal unsafe override void Cancel()
		{
			_implementation.Cancel();
		}

	}
}
