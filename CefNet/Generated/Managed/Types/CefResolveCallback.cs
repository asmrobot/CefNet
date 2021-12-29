﻿// --------------------------------------------------------------------------------------------
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

namespace CefNet
{
	/// <summary>
	/// Callback structure for cef_request_context_t::ResolveHost.
	/// </summary>
	/// <remarks>
	/// Role: Handler
	/// </remarks>
	public unsafe partial class CefResolveCallback : CefBaseRefCounted<cef_resolve_callback_t>, ICefResolveCallbackPrivate
	{
#if NET_LESS_5_0
		private static readonly OnResolveCompletedDelegate fnOnResolveCompleted = OnResolveCompletedImpl;

#endif // NET_LESS_5_0
		internal static unsafe CefResolveCallback Create(IntPtr instance)
		{
			return new CefResolveCallback((cef_resolve_callback_t*)instance);
		}

		public CefResolveCallback()
		{
			cef_resolve_callback_t* self = this.NativeInstance;
			#if NET_LESS_5_0
			self->on_resolve_completed = (void*)Marshal.GetFunctionPointerForDelegate(fnOnResolveCompleted);
			#else
			self->on_resolve_completed = (delegate* unmanaged[Stdcall]<cef_resolve_callback_t*, CefErrorCode, cef_string_list_t, void>)&OnResolveCompletedImpl;
			#endif
		}

		public CefResolveCallback(cef_resolve_callback_t* instance)
			: base((cef_base_ref_counted_t*)instance)
		{
		}

		[MethodImpl(MethodImplOptions.ForwardRef)]
		extern bool ICefResolveCallbackPrivate.AvoidOnResolveCompleted();

		/// <summary>
		/// Called on the UI thread after the ResolveHost request has completed.
		/// |result| will be the result code. |resolved_ips| will be the list of
		/// resolved IP addresses or NULL if the resolution failed.
		/// </summary>
		protected internal unsafe virtual void OnResolveCompleted(CefErrorCode result, CefStringList resolvedIps)
		{
		}

#if NET_LESS_5_0
		[UnmanagedFunctionPointer(CallingConvention.Winapi)]
		private unsafe delegate void OnResolveCompletedDelegate(cef_resolve_callback_t* self, CefErrorCode result, cef_string_list_t resolved_ips);

#endif // NET_LESS_5_0
		// void (*)(_cef_resolve_callback_t* self, cef_errorcode_t result, cef_string_list_t resolved_ips)*
#if !NET_LESS_5_0
		[UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvStdcall) })]
#endif
		private static unsafe void OnResolveCompletedImpl(cef_resolve_callback_t* self, CefErrorCode result, cef_string_list_t resolved_ips)
		{
			var instance = GetInstance((IntPtr)self) as CefResolveCallback;
			if (instance == null || ((ICefResolveCallbackPrivate)instance).AvoidOnResolveCompleted())
			{
				return;
			}
			instance.OnResolveCompleted(result, CefStringList.Wrap(resolved_ips));
		}
	}
}
