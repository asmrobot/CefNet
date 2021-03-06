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
	/// Structure representing keyboard event information.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public unsafe partial struct cef_key_event_t
	{
		/// <summary>
		/// The type of keyboard event.
		/// </summary>
		public CefKeyEventType type;

		/// <summary>
		/// Bit flags describing any pressed modifier keys. See
		/// cef_event_flags_t for values.
		/// </summary>
		public uint modifiers;

		/// <summary>
		/// The Windows key code for the key event. This value is used by the DOM
		/// specification. Sometimes it comes directly from the event (i.e. on
		/// Windows) and sometimes it&apos;s determined using a mapping function. See
		/// WebCore/platform/chromium/KeyboardCodes.h for the list of values.
		/// </summary>
		public int windows_key_code;

		/// <summary>
		/// The actual key code genenerated by the platform.
		/// </summary>
		public int native_key_code;

		/// <summary>
		/// Indicates whether the event is considered a &quot;system key&quot; event (see
		/// http://msdn.microsoft.com/en-us/library/ms646286(VS.85).aspx for details).
		/// This value will always be false on non-Windows platforms.
		/// </summary>
		public int is_system_key;

		/// <summary>
		/// The character generated by the keystroke.
		/// </summary>
		[MarshalAs(UnmanagedType.U2)]
		public char character;

		/// <summary>
		/// Same as |character| but unmodified by any concurrently-held modifiers
		/// (except shift). This is useful for working out shortcut keys.
		/// </summary>
		[MarshalAs(UnmanagedType.U2)]
		public char unmodified_character;

		/// <summary>
		/// True if the focus is currently on an editable field on the page. This is
		/// useful for determining if standard key events should be intercepted.
		/// </summary>
		public int focus_on_editable_field;
	}
}

