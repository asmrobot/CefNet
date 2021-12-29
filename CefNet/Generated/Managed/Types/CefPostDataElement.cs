﻿// --------------------------------------------------------------------------------------------
// Copyright (c) 2019 The CefNet Authors. All rights reserved.
// Licensed under the MIT license.
// See the licence file in the project root for full license information.
// --------------------------------------------------------------------------------------------
// Generated by CefGen
// Source: Generated/Native/Types/cef_post_data_element_t.cs
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
	/// Structure used to represent a single element in the request post data. The
	/// functions of this structure may be called on any thread.
	/// </summary>
	/// <remarks>
	/// Role: Proxy
	/// </remarks>
	public unsafe partial class CefPostDataElement : CefBaseRefCounted<cef_post_data_element_t>
	{
		internal static unsafe CefPostDataElement Create(IntPtr instance)
		{
			return new CefPostDataElement((cef_post_data_element_t*)instance);
		}

		public CefPostDataElement(cef_post_data_element_t* instance)
			: base((cef_base_ref_counted_t*)instance)
		{
		}

		/// <summary>
		/// Gets a value indicating whether this object is read-only.
		/// </summary>
		public unsafe virtual bool IsReadOnly
		{
			get
			{
				return SafeCall(NativeInstance->IsReadOnly() != 0);
			}
		}

		/// <summary>
		/// Return the type of this post data element.
		/// </summary>
		public unsafe virtual CefPostDataElementType Type
		{
			get
			{
				return SafeCall(NativeInstance->GetCefType());
			}
		}

		/// <summary>
		/// Return the file name.
		/// The resulting string must be freed by calling cef_string_userfree_free().
		/// </summary>
		public unsafe virtual string File
		{
			get
			{
				return SafeCall(CefString.ReadAndFree(NativeInstance->GetFile()));
			}
		}

		/// <summary>
		/// Return the number of bytes.
		/// </summary>
		public unsafe virtual long BytesCount
		{
			get
			{
				return SafeCall((long)NativeInstance->GetBytesCount());
			}
		}

		/// <summary>
		/// Remove all contents from the post data element.
		/// </summary>
		public unsafe virtual void SetToEmpty()
		{
			NativeInstance->SetToEmpty();
			GC.KeepAlive(this);
		}

		/// <summary>
		/// The post data element will represent bytes.  The bytes passed in will be
		/// copied.
		/// </summary>
		public unsafe virtual void SetToBytes(long size, IntPtr bytes)
		{
			NativeInstance->SetToBytes(new UIntPtr((ulong)size), (void*)bytes);
			GC.KeepAlive(this);
		}

		/// <summary>
		/// Read up to |size| bytes into |bytes| and return the number of bytes
		/// actually read.
		/// </summary>
		public unsafe virtual long GetBytes(long size, IntPtr bytes)
		{
			return SafeCall((long)NativeInstance->GetBytes(new UIntPtr((ulong)size), (void*)bytes));
		}

		/// <summary>
		/// The post data element will represent a file.
		/// </summary>
		public unsafe virtual void SetToFile(string fileName)
		{
			fixed (char* s0 = fileName)
			{
				var cstr0 = new cef_string_t { Str = s0, Length = fileName != null ? fileName.Length : 0 };
				NativeInstance->SetToFile(&cstr0);
			}
			GC.KeepAlive(this);
		}
	}
}
