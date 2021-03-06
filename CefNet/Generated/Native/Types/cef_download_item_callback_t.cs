// --------------------------------------------------------------------------------------------
// Copyright (c) 2019 The CefNet Authors. All rights reserved.
// Licensed under the MIT license.
// See the licence file in the project root for full license information.
// --------------------------------------------------------------------------------------------
// Generated by CefGen
// Source: include/capi/cef_download_handler_capi.h
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
	/// Callback structure used to asynchronously cancel a download.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public unsafe partial struct cef_download_item_callback_t
	{
		/// <summary>
		/// Base structure.
		/// </summary>
		public cef_base_ref_counted_t @base;

		/// <summary>
		/// void (*)(_cef_download_item_callback_t* self)*
		/// </summary>
		public void* cancel;

		/// <summary>
		/// Call to cancel the download.
		/// </summary>
		[NativeName("cancel")]
		public unsafe void Cancel()
		{
			fixed (cef_download_item_callback_t* self = &this)
			{
				((delegate* unmanaged[Stdcall]<cef_download_item_callback_t*, void>)cancel)(self);
			}
		}

		/// <summary>
		/// void (*)(_cef_download_item_callback_t* self)*
		/// </summary>
		public void* pause;

		/// <summary>
		/// Call to pause the download.
		/// </summary>
		[NativeName("pause")]
		public unsafe void Pause()
		{
			fixed (cef_download_item_callback_t* self = &this)
			{
				((delegate* unmanaged[Stdcall]<cef_download_item_callback_t*, void>)pause)(self);
			}
		}

		/// <summary>
		/// void (*)(_cef_download_item_callback_t* self)*
		/// </summary>
		public void* resume;

		/// <summary>
		/// Call to resume the download.
		/// </summary>
		[NativeName("resume")]
		public unsafe void Resume()
		{
			fixed (cef_download_item_callback_t* self = &this)
			{
				((delegate* unmanaged[Stdcall]<cef_download_item_callback_t*, void>)resume)(self);
			}
		}
	}
}

