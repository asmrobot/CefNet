// --------------------------------------------------------------------------------------------
// Copyright (c) 2019 The CefNet Authors. All rights reserved.
// Licensed under the MIT license.
// See the licence file in the project root for full license information.
// --------------------------------------------------------------------------------------------

using CefNet.CApi;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace CefNet
{
	/// <summary>
	/// Providers static methods for working with CEF strings.
	/// </summary>
	public static unsafe class CefString
	{
		[UnmanagedFunctionPointer(CallingConvention.Winapi)]
		private delegate void CefStringDestructorDelegate(IntPtr str);

		private static readonly CefStringDestructorDelegate fnDtor = Marshal.FreeHGlobal;

		internal static readonly void* DestructorAddress = (void*)Marshal.GetFunctionPointerForDelegate(fnDtor);

		/// <summary>
		/// Creates a <see cref="cef_string_userfree_t"/> from the specified <see cref="string"/>.
		/// </summary>
		/// <param name="s">The source string.</param>
		/// <returns>A new <see cref="cef_string_userfree_t"/> that this method creates.</returns>
		public static cef_string_userfree_t Create(string s)
		{
			cef_string_userfree_t str = default;
			str.Base = CefNativeApi.cef_string_userfree_utf16_alloc();
			if (s != null)
			{
				str.Base.Base->str = (char*)Marshal.StringToHGlobalUni(s);
				str.Base.Base->length = (UIntPtr)s.Length;
				str.Base.Base->dtor = DestructorAddress;
			}
			return str;
		}

		/// <summary>
		/// Allocates a managed <see cref="string"/> and copies a CEF string into it.
		/// </summary>
		/// <param name="str">The pointer to the CEF string.</param>
		/// <returns>A managed string that holds a copy of the CEF string.</returns>
		public static string Read(cef_string_t* str)
		{
			unchecked
			{
				cef_string_utf16_t* s = (cef_string_utf16_t*)str;
				if (s == default || s->str == default)
					return null;
				return Marshal.PtrToStringUni((IntPtr)s->str, (int)s->length);
			}
		}

		/// <summary>
		/// Allocates a managed <see cref="string"/>, copies <see cref="cef_string_t"/>&apos;s value
		/// into it and frees memory allocated for the <see cref="cef_string_t"/>&apos;s value.
		/// </summary>
		/// <param name="str">The pointer to the CEF string.</param>
		/// <returns>A managed string that holds a copy of the CEF string.</returns>
		public static string ReadAndFree(cef_string_t* str)
		{
			cef_string_utf16_t* s = (cef_string_utf16_t*)str;
			if (s == null || s->str == null)
				return null;

			string rv = Marshal.PtrToStringUni((IntPtr)s->str, (int)s->length);
			if (s->dtor != default)
			{
				s->Dtor(s->str);
				s->dtor = default;
			}
			s->str = default;
			s->length = UIntPtr.Zero;
			return rv;
		}

		/// <summary>
		/// Allocates a managed <see cref="string"/>, copies <see cref="cef_string_userfree_t"/>&apos;s
		/// value into it and frees memory allocated for the <see cref="cef_string_userfree_t"/>&apos;s
		/// value.
		/// </summary>
		/// <param name="str">The pointer to the CEF string.</param>
		/// <returns>A managed string that holds a copy of the CEF string.</returns>
		public static string ReadAndFree(cef_string_userfree_t str)
		{
			cef_string_utf16_t* s = str.Base.Base;
			if (s == null)
				return null;
			string rv = Marshal.PtrToStringUni((IntPtr)s->str, (int)s->length);
			CefNativeApi.cef_string_userfree_utf16_free(str.Base);
			return rv;
		}

		/// <summary>
		/// Frees memory allocated for the <see cref="cef_string_t"/>&apos;s value.
		/// </summary>
		/// <param name="str">The pointer to the CEF string.</param>
		public static void Free(cef_string_t* str)
		{
			if (str == null)
				return;

			cef_string_utf16_t* s = (cef_string_utf16_t*)str;
			if (s->dtor != null)
			{
				s->Dtor(s->str);
				s->dtor = default;
			}
			s->str = default;
			s->length = UIntPtr.Zero;
		}

		/// <summary>
		/// Frees memory allocated for the <see cref="cef_string_userfree_t"/>&apos;s value.
		/// </summary>
		/// <param name="str">The pointer to the CEF string.</param>
		public static void Free(cef_string_userfree_t str)
		{
			if (str.Base.Base == null)
				return;

			CefNativeApi.cef_string_userfree_utf16_free(str.Base);
		}

		/// <summary>
		/// Replaces a value of a CEF string to a new value.
		/// </summary>
		/// <param name="str">The pointer to a CEF string.</param>
		/// <param name="value">The string to replace.</param>
		/// <remarks>This method frees memory allocated for the old string.</remarks>
		public static void Replace(cef_string_t* str, string value)
		{
			if (str == null)
				throw new ArgumentNullException(nameof(str));

			unchecked
			{
				cef_string_utf16_t* s = (cef_string_utf16_t*)str;
				if (s->str != default && s->dtor != default)
				{
					s->Dtor(s->str);
				}
				if (value == null)
				{
					s->str = null;
					s->length = default;
					s->dtor = null;
				}
				else
				{
					s->str = (char*)Marshal.StringToHGlobalUni(value);
					s->length = unchecked((UIntPtr)value.Length);
					s->dtor = CefString.DestructorAddress;
				}
			}
		}

	}


}
