﻿// --------------------------------------------------------------------------------------------
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
	/// Settings used when initializing a CefBoxLayout.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public unsafe partial struct cef_box_layout_settings_t
	{
		/// <summary>
		/// If true (1) the layout will be horizontal, otherwise the layout will be
		/// vertical.
		/// </summary>
		public int horizontal;

		/// <summary>
		/// Adds additional horizontal space between the child view area and the host
		/// view border.
		/// </summary>
		public int inside_border_horizontal_spacing;

		/// <summary>
		/// Adds additional vertical space between the child view area and the host
		/// view border.
		/// </summary>
		public int inside_border_vertical_spacing;

		/// <summary>
		/// Adds additional space around the child view area.
		/// </summary>
		public cef_insets_t inside_border_insets;

		/// <summary>
		/// Adds additional space between child views.
		/// </summary>
		public int between_child_spacing;

		/// <summary>
		/// Specifies where along the main axis the child views should be laid out.
		/// </summary>
		public CefMainAxisAlignment main_axis_alignment;

		/// <summary>
		/// Specifies where along the cross axis the child views should be laid out.
		/// </summary>
		public CefCrossAxisAlignment cross_axis_alignment;

		/// <summary>
		/// Minimum cross axis size.
		/// </summary>
		public int minimum_cross_axis_size;

		/// <summary>
		/// Default flex for views when none is specified via CefBoxLayout methods.
		/// Using the preferred size as the basis, free space along the main axis is
		/// distributed to views in the ratio of their flex weights. Similarly, if the
		/// views will overflow the parent, space is subtracted in these ratios. A flex
		/// of 0 means this view is not resized. Flex values must not be negative.
		/// </summary>
		public int default_flex;
	}
}
