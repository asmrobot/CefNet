﻿// --------------------------------------------------------------------------------------------
// Copyright (c) 2019 The CefNet Authors. All rights reserved.
// Licensed under the MIT license.
// See the licence file in the project root for full license information.
// --------------------------------------------------------------------------------------------
// Generated by CefGen
// Source: Generated/Native/Types/cef_browser_process_handler_t.cs
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
	sealed partial class CefBrowserProcessHandlerGlue: CefBrowserProcessHandler, ICefBrowserProcessHandlerPrivate
	{
		private CefAppGlue _implementation;

		public CefBrowserProcessHandlerGlue(CefAppGlue impl)
		{
			_implementation = impl;
		}

		protected internal unsafe override void OnContextInitialized()
		{
			_implementation.OnContextInitialized();
		}

		bool ICefBrowserProcessHandlerPrivate.AvoidOnBeforeChildProcessLaunch()
		{
			return _implementation.AvoidOnBeforeChildProcessLaunch();
		}

		protected internal unsafe override void OnBeforeChildProcessLaunch(CefCommandLine commandLine)
		{
			_implementation.OnBeforeChildProcessLaunch(commandLine);
		}

		bool ICefBrowserProcessHandlerPrivate.AvoidOnScheduleMessagePumpWork()
		{
			return _implementation.AvoidOnScheduleMessagePumpWork();
		}

		protected internal unsafe override void OnScheduleMessagePumpWork(long delayMs)
		{
			_implementation.OnScheduleMessagePumpWork(delayMs);
		}

		protected internal unsafe override CefClient GetDefaultClient()
		{
			return _implementation.GetDefaultClient();
		}

	}
}