// --------------------------------------------------------------------------------------------
// Copyright (c) 2019 The CefNet Authors. All rights reserved.
// Licensed under the MIT license.
// See the licence file in the project root for full license information.
// --------------------------------------------------------------------------------------------
// Generated by CefGen
// Source: Generated/Native/Types/cef_auth_callback_t.cs
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
	/// Callback structure used for asynchronous continuation of authentication
	/// requests.
	/// </summary>
	/// <remarks>
	/// Role: Proxy
	/// </remarks>
	public unsafe partial class CefAuthCallback : CefBaseRefCounted<cef_auth_callback_t>
	{
		internal static unsafe CefAuthCallback Create(IntPtr instance)
		{
			return new CefAuthCallback((cef_auth_callback_t*)instance);
		}

		public CefAuthCallback(cef_auth_callback_t* instance)
			: base((cef_base_ref_counted_t*)instance)
		{
		}

		/// <summary>
		/// Continue the authentication request.
		/// </summary>
		public unsafe virtual void Continue(string username, string password)
		{
			fixed (char* s0 = username)
			fixed (char* s1 = password)
			{
				var cstr0 = new cef_string_t { Str = s0, Length = username != null ? username.Length : 0 };
				var cstr1 = new cef_string_t { Str = s1, Length = password != null ? password.Length : 0 };
				NativeInstance->Continue(&cstr0, &cstr1);
			}
			GC.KeepAlive(this);
		}

		/// <summary>
		/// Cancel the authentication request.
		/// </summary>
		public unsafe virtual void Cancel()
		{
			NativeInstance->Cancel();
			GC.KeepAlive(this);
		}
	}
}
