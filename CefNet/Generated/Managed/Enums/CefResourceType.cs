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

namespace CefNet
{
	/// <summary>
	/// Resource type for a request.
	/// </summary>
	public enum CefResourceType
	{
		/// <summary>
		/// Top level page.
		/// </summary>
		MainFrame = 0,

		/// <summary>
		/// Frame or iframe.
		/// </summary>
		SubFrame = 1,

		/// <summary>
		/// CSS stylesheet.
		/// </summary>
		Stylesheet = 2,

		/// <summary>
		/// External script.
		/// </summary>
		Script = 3,

		/// <summary>
		/// Image (jpg/gif/png/etc).
		/// </summary>
		Image = 4,

		/// <summary>
		/// Font.
		/// </summary>
		FontResource = 5,

		/// <summary>
		/// Some other subresource. This is the default type if the actual type is
		/// unknown.
		/// </summary>
		SubResource = 6,

		/// <summary>
		/// Object (or embed) tag for a plugin, or a resource that a plugin requested.
		/// </summary>
		Object = 7,

		/// <summary>
		/// Media resource.
		/// </summary>
		Media = 8,

		/// <summary>
		/// Main resource of a dedicated worker.
		/// </summary>
		Worker = 9,

		/// <summary>
		/// Main resource of a shared worker.
		/// </summary>
		SharedWorker = 10,

		/// <summary>
		/// Explicitly requested prefetch.
		/// </summary>
		Prefetch = 11,

		/// <summary>
		/// Favicon.
		/// </summary>
		Favicon = 12,

		/// <summary>
		/// XMLHttpRequest.
		/// </summary>
		Xhr = 13,

		/// <summary>
		/// A request for a 
		/// &lt;ping
		/// &gt;
		/// </summary>
		Ping = 14,

		/// <summary>
		/// Main resource of a service worker.
		/// </summary>
		ServiceWorker = 15,

		/// <summary>
		/// A report of Content Security Policy violations.
		/// </summary>
		CspReport = 16,

		/// <summary>
		/// A resource that a plugin requested.
		/// </summary>
		PluginResource = 17,

		/// <summary>
		/// A main-frame service worker navigation preload request.
		/// </summary>
		NavigationPreloadMainFrame = 19,

		/// <summary>
		/// A sub-frame service worker navigation preload request.
		/// </summary>
		NavigationPreloadSubFrame = 20,
	}
}

