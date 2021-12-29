﻿// --------------------------------------------------------------------------------------------
// Copyright (c) 2019 The CefNet Authors. All rights reserved.
// Licensed under the MIT license.
// See the licence file in the project root for full license information.
// --------------------------------------------------------------------------------------------
// Generated by CefGen
// Source: Generated/Native/Types/cef_thread_t.cs
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

namespace CefNet
{
	/// <summary>
	/// A simple thread abstraction that establishes a message loop on a new thread.
	/// The consumer uses cef_task_runner_t to execute code on the thread&apos;s message
	/// loop. The thread is terminated when the cef_thread_t object is destroyed or
	/// stop() is called. All pending tasks queued on the thread&apos;s message loop will
	/// run to completion before the thread is terminated. cef_thread_create() can be
	/// called on any valid CEF thread in either the browser or render process. This
	/// structure should only be used for tasks that require a dedicated thread. In
	/// most cases you can post tasks to an existing CEF thread instead of creating a
	/// new one; see cef_task.h for details.
	/// </summary>
	/// <remarks>
	/// Role: Proxy
	/// </remarks>
	public unsafe partial class CefThread : CefBaseRefCounted<cef_thread_t>
	{
		internal static unsafe CefThread Create(IntPtr instance)
		{
			return new CefThread((cef_thread_t*)instance);
		}

		public CefThread(cef_thread_t* instance)
			: base((cef_base_ref_counted_t*)instance)
		{
		}

		/// <summary>
		/// Gets the cef_task_runner_t that will execute code on this thread&apos;s
		/// message loop. This property is safe to call from any thread.
		/// </summary>
		public unsafe virtual CefTaskRunner TaskRunner
		{
			get
			{
				return SafeCall(CefTaskRunner.Wrap(CefTaskRunner.Create, NativeInstance->GetTaskRunner()));
			}
		}

		/// <summary>
		/// Gets the platform thread ID. It will return the same value after stop()
		/// is called. This property is safe to call from any thread.
		/// </summary>
		public unsafe virtual uint PlatformThreadId
		{
			get
			{
				return SafeCall(NativeInstance->GetPlatformThreadId());
			}
		}

		/// <summary>
		/// Gets a value indicating whether the thread is currently running. This property must be
		/// called from the same thread that called cef_thread_create().
		/// </summary>
		public unsafe virtual bool IsRunning
		{
			get
			{
				return SafeCall(NativeInstance->IsRunning() != 0);
			}
		}

		/// <summary>
		/// Stop and join the thread. This function must be called from the same thread
		/// that called cef_thread_create(). Do not call this function if
		/// cef_thread_create() was called with a |stoppable| value of false (0).
		/// </summary>
		public unsafe virtual void Stop()
		{
			NativeInstance->Stop();
			GC.KeepAlive(this);
		}
	}
}