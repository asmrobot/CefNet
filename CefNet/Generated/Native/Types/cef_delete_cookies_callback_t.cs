// --------------------------------------------------------------------------------------------
// Copyright (c) 2019 The CefNet Authors. All rights reserved.
// Licensed under the MIT license.
// See the licence file in the project root for full license information.
// --------------------------------------------------------------------------------------------
// Generated by CefGen
// Source: include/capi/cef_cookie_capi.h
// --------------------------------------------------------------------------------------------﻿
// DO NOT MODIFY! THIS IS AUTOGENERATED FILE!
// --------------------------------------------------------------------------------------------

#pragma warning disable 0169, 1591, 1573

using System;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using CefNet.WinApi;

namespace CefNet.CApi
{
	/// <summary>
	/// Structure to implement to be notified of asynchronous completion via
	/// cef_cookie_manager_t::delete_cookies().
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public unsafe partial struct cef_delete_cookies_callback_t
	{
		/// <summary>
		/// Base structure.
		/// </summary>
		public cef_base_ref_counted_t @base;

		/// <summary>
		/// void (*)(_cef_delete_cookies_callback_t* self, int num_deleted)*
		/// </summary>
		public void* on_complete;

		/// <summary>
		/// Method that will be called upon completion. |num_deleted| will be the
		/// number of cookies that were deleted.
		/// </summary>
		[NativeName("on_complete")]
		public unsafe void OnComplete(int num_deleted)
		{
			fixed (cef_delete_cookies_callback_t* self = &this)
			{
				((delegate* unmanaged[Stdcall]<cef_delete_cookies_callback_t*, int, void>)on_complete)(self, num_deleted);
			}
		}
	}
}

