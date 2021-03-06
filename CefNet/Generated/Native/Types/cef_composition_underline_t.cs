// --------------------------------------------------------------------------------------------
// Copyright (c) 2019 The CefNet Authors. All rights reserved.
// Licensed under the MIT license.
// See the licence file in the project root for full license information.
// --------------------------------------------------------------------------------------------
// Generated by CefGen
// Source: include/internal/cef_types.h
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
	/// Structure representing IME composition underline information. This is a thin
	/// wrapper around Blink&apos;s WebCompositionUnderline class and should be kept in
	/// sync with that.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public unsafe partial struct cef_composition_underline_t
	{
		/// <summary>
		/// Underline character range.
		/// </summary>
		public cef_range_t range;

		/// <summary>
		/// Text color.
		/// </summary>
		public cef_color_t color;

		/// <summary>
		/// Background color.
		/// </summary>
		public cef_color_t background_color;

		/// <summary>
		/// Set to true (1) for thick underline.
		/// </summary>
		public int thick;

		/// <summary>
		/// Style.
		/// </summary>
		public CefCompositionUnderlineStyle style;
	}
}

