// --------------------------------------------------------------------------------------------
// Copyright (c) 2019 The CefNet Authors. All rights reserved.
// Licensed under the MIT license.
// See the licence file in the project root for full license information.
// --------------------------------------------------------------------------------------------
// Generated by CefGen
// Source: Generated/Native/Types/cef_composition_underline_t.cs
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
	/// Structure representing IME composition underline information. This is a thin
	/// wrapper around Blink&apos;s WebCompositionUnderline class and should be kept in
	/// sync with that.
	/// </summary>
	/// <remarks>
	/// Role: Proxy
	/// </remarks>
	public unsafe partial struct CefCompositionUnderline
	{
		private cef_composition_underline_t _instance;

		/// <summary>
		/// Underline character range.
		/// </summary>
		public CefRange Range
		{
			get
			{
				return _instance.range;
			}
			set
			{
				_instance.range = value;
			}
		}

		/// <summary>
		/// Text color.
		/// </summary>
		public CefColor Color
		{
			get
			{
				return _instance.color;
			}
			set
			{
				_instance.color = value;
			}
		}

		/// <summary>
		/// Background color.
		/// </summary>
		public CefColor BackgroundColor
		{
			get
			{
				return _instance.background_color;
			}
			set
			{
				_instance.background_color = value;
			}
		}

		/// <summary>
		/// Set to true (1) for thick underline.
		/// </summary>
		public bool Thick
		{
			get
			{
				return _instance.thick != 0;
			}
			set
			{
				_instance.thick = value ? 1 : 0;
			}
		}

		/// <summary>
		/// Style.
		/// </summary>
		public CefCompositionUnderlineStyle Style
		{
			get
			{
				return _instance.style;
			}
			set
			{
				_instance.style = value;
			}
		}

		public static implicit operator CefCompositionUnderline(cef_composition_underline_t instance)
		{
			return new CefCompositionUnderline { _instance = instance };
		}

		public static implicit operator cef_composition_underline_t(CefCompositionUnderline instance)
		{
			return instance._instance;
		}
	}
}
