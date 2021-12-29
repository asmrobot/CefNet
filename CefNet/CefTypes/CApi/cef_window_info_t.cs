// --------------------------------------------------------------------------------------------
// Copyright (c) 2019 The CefNet Authors. All rights reserved.
// Licensed under the MIT license.
// See the licence file in the project root for full license information.
// --------------------------------------------------------------------------------------------
// Source: include\internal\cef_types_win.h
//         include\internal\cef_types_linux.h
//         include\internal\cef_types_mac.h
// --------------------------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;

namespace CefNet.CApi
{
	/// <summary>
	/// Structure representing window information.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct cef_window_info_t
	{

	}

	/// <summary>
	/// Structure representing window information.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct cef_window_info_windows_t
	{
		/// <summary>
		/// The extended window style of the window being created.<para/>
		/// Standard parameter required by <see href="https://docs.microsoft.com/windows/win32/api/winuser/nf-winuser-createwindowexw">CreateWindowEx()</see>.
		/// See the Microsoft Docs for more information.
		/// </summary>
		public uint ex_style;

		/// <summary>
		/// The initial title of the window, to be set when the window is created.<para/>
		/// Standard parameter required by <see href="https://docs.microsoft.com/windows/win32/api/winuser/nf-winuser-createwindowexw">CreateWindowEx()</see>.
		/// See the Microsoft Docs for more information.
		/// </summary>
		public cef_string_t window_name;

		/// <summary>
		/// The style of the window being created.<para/>
		/// Standard parameter required by <see href="https://docs.microsoft.com/windows/win32/api/winuser/nf-winuser-createwindowexw">CreateWindowEx()</see>.
		/// See the Microsoft Docs for more information.
		/// </summary>
		public uint style;

		/// <summary>
		/// The initial horizontal position of the window.<para/>
		/// Standard parameter required by <see href="https://docs.microsoft.com/windows/win32/api/winuser/nf-winuser-createwindowexw">CreateWindowEx()</see>.
		/// See the Microsoft Docs for more information.		/// </summary>
		public int x;

		/// <summary>
		/// The initial vertical position of the window.<para/>
		/// Standard parameter required by <see href="https://docs.microsoft.com/windows/win32/api/winuser/nf-winuser-createwindowexw">CreateWindowEx()</see>.
		/// See the Microsoft Docs for more information.
		/// </summary>
		public int y;

		/// <summary>
		/// The width, in device units, of the window.<para/>
		/// Standard parameter required by <see href="https://docs.microsoft.com/windows/win32/api/winuser/nf-winuser-createwindowexw">CreateWindowEx()</see>.
		/// See the Microsoft Docs for more information.
		/// </summary>
		public int width;

		/// <summary>
		/// The height, in device units, of the window.<para/>
		/// Standard parameter required by <see href="https://docs.microsoft.com/windows/win32/api/winuser/nf-winuser-createwindowexw">CreateWindowEx()</see>.
		/// See the Microsoft Docs for more information.
		/// </summary>
		public int height;

		/// <summary>
		/// A handle to the parent or owner window of the window being created.
		/// To create a child window or an owned window, supply a valid window
		/// handle. This parameter is optional for pop-up windows.
		/// </summary>
		public IntPtr parent_window;

		/// <summary>
		/// A handle to a menu, or specifies a child-window identifier,
		/// depending on the window style.
		/// See <see href="https://docs.microsoft.com/windows/win32/api/winuser/nf-winuser-createwindowexw">Microsoft Docs</see> for more information.
		/// </summary>
		public IntPtr menu;

		/// <summary>
		/// Set to true to create the browser using windowless (off-screen)
		/// rendering.<para/>No window will be created for the browser and all rendering will
		/// occur via the <see cref="CefRenderHandler"/> interface. The <see cref="parent_window"/>
		/// value will be used to identify monitor info and to act as the parent window for dialogs,
		/// context menus, etc. If <see cref="parent_window"/> is not provided then the main screen
		/// monitor will be used and some functionality that requires a parent window
		/// may not function correctly. In order to create windowless browsers the
		/// <see cref="cef_settings_t.windowless_rendering_enabled"/> value must be set to true.
		/// Transparent painting is enabled by default but can be disabled by setting
		/// <see cref="cef_browser_settings_t.background_color"/> to an opaque value.
		/// </summary>
		public int windowless_rendering_enabled;

		/// <summary>
		/// Set to true to enable shared textures for windowless rendering. Only
		/// valid if <see cref="windowless_rendering_enabled"/> above is also set to true.<para/>
		/// Currently only supported on Windows (D3D11).
		/// </summary>
		public int shared_texture_enabled;

		/// <summary>
		/// Set to true to enable the ability to issue BeginFrame requests from the
		/// client application by calling <see cref="CefBrowserHost.SendExternalBeginFrame"/>.
		/// </summary>
		public int external_begin_frame_enabled;

		/// <summary>
		/// Handle for the new browser window. Only used with windowed rendering.
		/// </summary>
		public IntPtr window;
	}

	/// <summary>
	/// Structure representing window information.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct cef_window_info_linux_t
	{
		/// <summary>
		/// The initial title of the window, to be set when the window is created.<para/>
		/// Some layout managers (e.g., Compiz) can look at the window title
		/// in order to decide where to place the window when it is
		/// created. When this attribute is not empty, the window title will
		/// be set before the window is mapped to the dispay. Otherwise the
		/// title will be initially empty.
		/// </summary>
		public cef_string_t window_name;

		/// <summary>
		/// The initial horizontal position of the window.
		/// </summary>
		public int x;

		/// <summary>
		/// The initial vertical position of the window.
		/// </summary>
		public int y;

		/// <summary>
		/// The width of the window.
		/// </summary>
		public int width;

		/// <summary>
		/// The height of the window.
		/// </summary>
		public int height;

		/// <summary>
		/// Pointer for the parent window.
		/// </summary>
		public IntPtr parent_window;

		/// <summary>
		/// Set to true to create the browser using windowless (off-screen)
		/// rendering.<para/>No window will be created for the browser and all rendering will
		/// occur via the <see cref="CefRenderHandler"/> interface. The <see cref="parent_window"/>
		/// value will be used to identify monitor info and to act as the parent window for dialogs,
		/// context menus, etc. If <see cref="parent_window"/> is not provided then the main screen
		/// monitor will be used and some functionality that requires a parent window
		/// may not function correctly. In order to create windowless browsers the
		/// <see cref="cef_settings_t.windowless_rendering_enabled"/> value must be set to true.
		/// Transparent painting is enabled by default but can be disabled by setting
		/// <see cref="cef_browser_settings_t.background_color"/> to an opaque value.
		/// </summary>
		public int windowless_rendering_enabled;

		/// <summary>
		/// Set to true to enable shared textures for windowless rendering. Only
		/// valid if <see cref="windowless_rendering_enabled"/> above is also set to true.<para/>
		/// Currently only supported on Windows (D3D11).
		/// </summary>
		public int shared_texture_enabled;

		/// <summary>
		/// Set to true to enable the ability to issue BeginFrame requests from the
		/// client application by calling <see cref="CefBrowserHost.SendExternalBeginFrame"/>.
		/// </summary>
		public int external_begin_frame_enabled;

		/// <summary>
		/// Pointer for the new browser window. Only used with windowed rendering.
		/// </summary>
		public IntPtr window;
	}


	/// <summary>
	/// Structure representing window information.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct cef_window_info_mac_t
	{
		/// <summary>
		/// The initial title of the view, to be set when the view is created.
		/// </summary>
		public cef_string_t window_name;

		/// <summary>
		/// The initial horizontal position of the view.
		/// </summary>
		public int x;

		/// <summary>
		/// The initial vertical position of the view.
		/// </summary>
		public int y;

		/// <summary>
		/// The width of the view.
		/// </summary>
		public int width;

		/// <summary>
		/// The height of the view.
		/// </summary>
		public int height;

		/// <summary>
		/// Set to true to create the view initially hidden.
		/// </summary>
		public int hidden;

		/// <summary>
		/// NSView pointer for the parent view.
		/// </summary>
		public IntPtr parent_view;

		/// <summary>
		/// Set to true to create the browser using windowless (off-screen)
		/// rendering.<para/>No view will be created for the browser and all rendering will
		/// occur via the <see cref="CefRenderHandler"/> interface. The <see cref="parent_view"/>
		/// value will be used to identify monitor info and to act as the parent window for dialogs,
		/// context menus, etc. If <see cref="parent_view"/> is not provided then the main screen
		/// monitor will be used and some functionality that requires a parent view
		/// may not function correctly. In order to create windowless browsers the
		/// <see cref="cef_settings_t.windowless_rendering_enabled"/> value must be set to true.
		/// Transparent painting is enabled by default but can be disabled by setting
		/// <see cref="cef_browser_settings_t.background_color"/> to an opaque value.
		/// </summary>
		public int windowless_rendering_enabled;

		/// <summary>
		/// Set to true to enable shared textures for windowless rendering. Only
		/// valid if <see cref="windowless_rendering_enabled"/> above is also set to true.<para/>
		/// Currently only supported on Windows (D3D11).
		/// </summary>
		public int shared_texture_enabled;

		/// <summary>
		/// Set to true to enable the ability to issue BeginFrame requests from the
		/// client application by calling <see cref="CefBrowserHost.SendExternalBeginFrame"/>.
		/// </summary>
		public int external_begin_frame_enabled;

		/// <summary>
		/// NSView pointer for the new browser view. Only used with windowed rendering.
		/// </summary>
		public IntPtr view;
	}


}

