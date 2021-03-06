// --------------------------------------------------------------------------------------------
// Copyright (c) 2019 The CefNet Authors. All rights reserved.
// Licensed under the MIT license.
// See the licence file in the project root for full license information.
// --------------------------------------------------------------------------------------------
// Generated by CefGen
// Source: include/capi/cef_media_router_capi.h
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
	/// Represents a source from which media can be routed. Instances of this object
	/// are retrieved via cef_media_router_t::GetSource. The functions of this
	/// structure may be called on any browser process thread unless otherwise
	/// indicated.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public unsafe partial struct cef_media_source_t
	{
		/// <summary>
		/// Base structure.
		/// </summary>
		public cef_base_ref_counted_t @base;

		/// <summary>
		/// cef_string_userfree_t (*)(_cef_media_source_t* self)*
		/// </summary>
		public void* get_id;

		/// <summary>
		/// Returns the ID (media source URN or URL) for this source.
		/// The resulting string must be freed by calling cef_string_userfree_free().
		/// </summary>
		[NativeName("get_id")]
		public unsafe cef_string_userfree_t GetId()
		{
			fixed (cef_media_source_t* self = &this)
			{
				return ((delegate* unmanaged[Stdcall]<cef_media_source_t*, cef_string_userfree_t>)get_id)(self);
			}
		}

		/// <summary>
		/// int (*)(_cef_media_source_t* self)*
		/// </summary>
		public void* is_cast_source;

		/// <summary>
		/// Returns true (1) if this source outputs its content via Cast.
		/// </summary>
		[NativeName("is_cast_source")]
		public unsafe int IsCastSource()
		{
			fixed (cef_media_source_t* self = &this)
			{
				return ((delegate* unmanaged[Stdcall]<cef_media_source_t*, int>)is_cast_source)(self);
			}
		}

		/// <summary>
		/// int (*)(_cef_media_source_t* self)*
		/// </summary>
		public void* is_dial_source;

		/// <summary>
		/// Returns true (1) if this source outputs its content via DIAL.
		/// </summary>
		[NativeName("is_dial_source")]
		public unsafe int IsDialSource()
		{
			fixed (cef_media_source_t* self = &this)
			{
				return ((delegate* unmanaged[Stdcall]<cef_media_source_t*, int>)is_dial_source)(self);
			}
		}
	}
}

