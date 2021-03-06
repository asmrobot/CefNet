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

namespace CefNet
{
	/// <summary>
	/// Message loop types. Indicates the set of asynchronous events that a message
	/// loop can process.
	/// </summary>
	public enum CefMessageLoopType
	{
		/// <summary>
		/// Supports tasks and timers.
		/// </summary>
		Default = 0,

		/// <summary>
		/// Supports tasks, timers and native UI events (e.g. Windows messages).
		/// </summary>
		UI = 1,

		/// <summary>
		/// Supports tasks, timers and asynchronous IO events.
		/// </summary>
		IO = 2,
	}
}

