// --------------------------------------------------------------------------------------------
// Copyright (c) 2019 The CefNet Authors. All rights reserved.
// Licensed under the MIT license.
// See the licence file in the project root for full license information.
// --------------------------------------------------------------------------------------------
// Generated by CefGen
// Source: Generated/Native/Types/cef_string_visitor_t.cs
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
	/// Implement this structure to receive string values asynchronously.
	/// </summary>
	/// <remarks>
	/// Role: Handler
	/// </remarks>
	public unsafe partial class CefStringVisitor : CefBaseRefCounted<cef_string_visitor_t>, ICefStringVisitorPrivate
	{
#if NET_LESS_5_0
		private static readonly VisitDelegate fnVisit = VisitImpl;

#endif // NET_LESS_5_0
		internal static unsafe CefStringVisitor Create(IntPtr instance)
		{
			return new CefStringVisitor((cef_string_visitor_t*)instance);
		}

		public CefStringVisitor()
		{
			cef_string_visitor_t* self = this.NativeInstance;
			#if NET_LESS_5_0
			self->visit = (void*)Marshal.GetFunctionPointerForDelegate(fnVisit);
			#else
			self->visit = (delegate* unmanaged[Stdcall]<cef_string_visitor_t*, cef_string_t*, void>)&VisitImpl;
			#endif
		}

		public CefStringVisitor(cef_string_visitor_t* instance)
			: base((cef_base_ref_counted_t*)instance)
		{
		}

		[MethodImpl(MethodImplOptions.ForwardRef)]
		extern bool ICefStringVisitorPrivate.AvoidVisit();

		/// <summary>
		/// Method that will be executed.
		/// </summary>
		protected internal unsafe virtual void Visit(string @string)
		{
		}

#if NET_LESS_5_0
		[UnmanagedFunctionPointer(CallingConvention.Winapi)]
		private unsafe delegate void VisitDelegate(cef_string_visitor_t* self, cef_string_t* @string);

#endif // NET_LESS_5_0
		// void (*)(_cef_string_visitor_t* self, const cef_string_t* string)*
#if !NET_LESS_5_0
		[UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvStdcall) })]
#endif
		private static unsafe void VisitImpl(cef_string_visitor_t* self, cef_string_t* @string)
		{
			var instance = GetInstance((IntPtr)self) as CefStringVisitor;
			if (instance == null || ((ICefStringVisitorPrivate)instance).AvoidVisit())
			{
				return;
			}
			instance.Visit(CefString.Read(@string));
		}
	}
}
