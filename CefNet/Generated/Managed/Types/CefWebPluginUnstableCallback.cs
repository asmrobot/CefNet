// --------------------------------------------------------------------------------------------
// Copyright (c) 2019 The CefNet Authors. All rights reserved.
// Licensed under the MIT license.
// See the licence file in the project root for full license information.
// --------------------------------------------------------------------------------------------
// Generated by CefGen
// Source: Generated/Native/Types/cef_web_plugin_unstable_callback_t.cs
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
	/// Structure to implement for receiving unstable plugin information. The
	/// functions of this structure will be called on the browser process IO thread.
	/// </summary>
	/// <remarks>
	/// Role: Handler
	/// </remarks>
	public unsafe partial class CefWebPluginUnstableCallback : CefBaseRefCounted<cef_web_plugin_unstable_callback_t>, ICefWebPluginUnstableCallbackPrivate
	{
#if NET_LESS_5_0
		private static readonly IsUnstableDelegate fnIsUnstable = IsUnstableImpl;

#endif // NET_LESS_5_0
		internal static unsafe CefWebPluginUnstableCallback Create(IntPtr instance)
		{
			return new CefWebPluginUnstableCallback((cef_web_plugin_unstable_callback_t*)instance);
		}

		public CefWebPluginUnstableCallback()
		{
			cef_web_plugin_unstable_callback_t* self = this.NativeInstance;
			#if NET_LESS_5_0
			self->is_unstable = (void*)Marshal.GetFunctionPointerForDelegate(fnIsUnstable);
			#else
			self->is_unstable = (delegate* unmanaged[Stdcall]<cef_web_plugin_unstable_callback_t*, cef_string_t*, int, void>)&IsUnstableImpl;
			#endif
		}

		public CefWebPluginUnstableCallback(cef_web_plugin_unstable_callback_t* instance)
			: base((cef_base_ref_counted_t*)instance)
		{
		}

		[MethodImpl(MethodImplOptions.ForwardRef)]
		extern bool ICefWebPluginUnstableCallbackPrivate.AvoidIsUnstable();

		/// <summary>
		/// Method that will be called for the requested plugin. |unstable| will be
		/// true (1) if the plugin has reached the crash count threshold of 3 times in
		/// 120 seconds.
		/// </summary>
		protected internal unsafe virtual void IsUnstable(string path, bool unstable)
		{
		}

#if NET_LESS_5_0
		[UnmanagedFunctionPointer(CallingConvention.Winapi)]
		private unsafe delegate void IsUnstableDelegate(cef_web_plugin_unstable_callback_t* self, cef_string_t* path, int unstable);

#endif // NET_LESS_5_0
		// void (*)(_cef_web_plugin_unstable_callback_t* self, const cef_string_t* path, int unstable)*
#if !NET_LESS_5_0
		[UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvStdcall) })]
#endif
		private static unsafe void IsUnstableImpl(cef_web_plugin_unstable_callback_t* self, cef_string_t* path, int unstable)
		{
			var instance = GetInstance((IntPtr)self) as CefWebPluginUnstableCallback;
			if (instance == null || ((ICefWebPluginUnstableCallbackPrivate)instance).AvoidIsUnstable())
			{
				return;
			}
			instance.IsUnstable(CefString.Read(path), unstable != 0);
		}
	}
}
