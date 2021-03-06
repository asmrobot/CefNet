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

namespace CefNet
{
	/// <summary>
	/// Implement this structure to receive notification when tracing has completed.
	/// The functions of this structure will be called on the browser process UI
	/// thread.
	/// </summary>
	/// <remarks>
	/// Role: Handler
	/// </remarks>
	public unsafe partial class CefEndTracingCallback : CefBaseRefCounted<cef_end_tracing_callback_t>, ICefEndTracingCallbackPrivate
	{
#if NET_LESS_5_0
		private static readonly OnEndTracingCompleteDelegate fnOnEndTracingComplete = OnEndTracingCompleteImpl;

#endif // NET_LESS_5_0
		internal static unsafe CefEndTracingCallback Create(IntPtr instance)
		{
			return new CefEndTracingCallback((cef_end_tracing_callback_t*)instance);
		}

		public CefEndTracingCallback()
		{
			cef_end_tracing_callback_t* self = this.NativeInstance;
			#if NET_LESS_5_0
			self->on_end_tracing_complete = (void*)Marshal.GetFunctionPointerForDelegate(fnOnEndTracingComplete);
			#else
			self->on_end_tracing_complete = (delegate* unmanaged[Stdcall]<cef_end_tracing_callback_t*, cef_string_t*, void>)&OnEndTracingCompleteImpl;
			#endif
		}

		public CefEndTracingCallback(cef_end_tracing_callback_t* instance)
			: base((cef_base_ref_counted_t*)instance)
		{
		}

		[MethodImpl(MethodImplOptions.ForwardRef)]
		extern bool ICefEndTracingCallbackPrivate.AvoidOnEndTracingComplete();

		/// <summary>
		/// Called after all processes have sent their trace data. |tracing_file| is
		/// the path at which tracing data was written. The client is responsible for
		/// deleting |tracing_file|.
		/// </summary>
		protected internal unsafe virtual void OnEndTracingComplete(string tracingFile)
		{
		}

#if NET_LESS_5_0
		[UnmanagedFunctionPointer(CallingConvention.Winapi)]
		private unsafe delegate void OnEndTracingCompleteDelegate(cef_end_tracing_callback_t* self, cef_string_t* tracing_file);

#endif // NET_LESS_5_0
		// void (*)(_cef_end_tracing_callback_t* self, const cef_string_t* tracing_file)*
#if !NET_LESS_5_0
		[UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvStdcall) })]
#endif
		private static unsafe void OnEndTracingCompleteImpl(cef_end_tracing_callback_t* self, cef_string_t* tracing_file)
		{
			var instance = GetInstance((IntPtr)self) as CefEndTracingCallback;
			if (instance == null || ((ICefEndTracingCallbackPrivate)instance).AvoidOnEndTracingComplete())
			{
				return;
			}
			instance.OnEndTracingComplete(CefString.Read(tracing_file));
		}
	}
}
