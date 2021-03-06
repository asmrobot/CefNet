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
	/// Return values for CefResponseFilter::Filter().
	/// </summary>
	public enum CefResponseFilterStatus
	{
		/// <summary>
		/// Some or all of the pre-filter data was read successfully but more data is
		/// needed in order to continue filtering (filtered output is pending).
		/// </summary>
		NeedMoreData = 0,

		/// <summary>
		/// Some or all of the pre-filter data was read successfully and all available
		/// filtered output has been written.
		/// </summary>
		Done = 1,

		/// <summary>
		/// An error occurred during filtering.
		/// </summary>
		Error = 2,
	}
}

