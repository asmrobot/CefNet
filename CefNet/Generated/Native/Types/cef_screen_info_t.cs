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
	/// Screen information used when window rendering is disabled. This structure is
	/// passed as a parameter to CefRenderHandler::GetScreenInfo and should be filled
	/// in by the client.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public unsafe partial struct cef_screen_info_t
	{
		/// <summary>
		/// Device scale factor. Specifies the ratio between physical and logical
		/// pixels.
		/// </summary>
		public float device_scale_factor;

		/// <summary>
		/// The screen depth in bits per pixel.
		/// </summary>
		public int depth;

		/// <summary>
		/// The bits per color component. This assumes that the colors are balanced
		/// equally.
		/// </summary>
		public int depth_per_component;

		/// <summary>
		/// This can be true for black and white printers.
		/// </summary>
		public int is_monochrome;

		/// <summary>
		/// This is set from the rcMonitor member of MONITORINFOEX, to whit:
		/// &quot;A RECT structure that specifies the display monitor rectangle,
		/// expressed in virtual-screen coordinates. Note that if the monitor
		/// is not the primary display monitor, some of the rectangle&apos;s
		/// coordinates may be negative values.&quot;
		/// The |rect| and |available_rect| properties are used to determine the
		/// available surface for rendering popup views.
		/// </summary>
		public cef_rect_t rect;

		/// <summary>
		/// This is set from the rcWork member of MONITORINFOEX, to whit:
		/// &quot;A RECT structure that specifies the work area rectangle of the
		/// display monitor that can be used by applications, expressed in
		/// virtual-screen coordinates. Windows uses this rectangle to
		/// maximize an application on the monitor. The rest of the area in
		/// rcMonitor contains system windows such as the task bar and side
		/// bars. Note that if the monitor is not the primary display monitor,
		/// some of the rectangle&apos;s coordinates may be negative values&quot;.
		/// The |rect| and |available_rect| properties are used to determine the
		/// available surface for rendering popup views.
		/// </summary>
		public cef_rect_t available_rect;
	}
}

