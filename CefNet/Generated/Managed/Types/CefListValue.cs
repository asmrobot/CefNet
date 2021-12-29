﻿// --------------------------------------------------------------------------------------------
// Copyright (c) 2019 The CefNet Authors. All rights reserved.
// Licensed under the MIT license.
// See the licence file in the project root for full license information.
// --------------------------------------------------------------------------------------------
// Generated by CefGen
// Source: Generated/Native/Types/cef_list_value_t.cs
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
	/// Structure representing a list value. Can be used on any process and thread.
	/// </summary>
	/// <remarks>
	/// Role: Proxy
	/// </remarks>
	public unsafe partial class CefListValue : CefBaseRefCounted<cef_list_value_t>
	{
		internal static unsafe CefListValue Create(IntPtr instance)
		{
			return new CefListValue((cef_list_value_t*)instance);
		}

		public CefListValue(cef_list_value_t* instance)
			: base((cef_base_ref_counted_t*)instance)
		{
		}

		/// <summary>
		/// Gets a value indicating whether this object is valid. This object may become invalid if
		/// the underlying data is owned by another object (e.g. list or dictionary)
		/// and that other object is then modified or destroyed. Do not call any other
		/// functions if this property returns false.
		/// </summary>
		public unsafe virtual bool IsValid
		{
			get
			{
				return SafeCall(NativeInstance->IsValid() != 0);
			}
		}

		/// <summary>
		/// Gets a value indicating whether this object is currently owned by another object.
		/// </summary>
		public unsafe virtual bool IsOwned
		{
			get
			{
				return SafeCall(NativeInstance->IsOwned() != 0);
			}
		}

		/// <summary>
		/// Gets a value indicating whether the values of this object are read-only. Some APIs may
		/// expose read-only objects.
		/// </summary>
		public unsafe virtual bool IsReadOnly
		{
			get
			{
				return SafeCall(NativeInstance->IsReadOnly() != 0);
			}
		}

		/// <summary>
		/// Returns true (1) if this object and |that| object have the same underlying
		/// data. If true (1) modifications to this object will also affect |that|
		/// object and vice-versa.
		/// </summary>
		public unsafe virtual bool IsSame(CefListValue that)
		{
			return SafeCall(NativeInstance->IsSame((that != null) ? that.GetNativeInstance() : null) != 0);
		}

		/// <summary>
		/// Returns true (1) if this object and |that| object have an equivalent
		/// underlying value but are not necessarily the same object.
		/// </summary>
		public unsafe virtual bool IsEqual(CefListValue that)
		{
			return SafeCall(NativeInstance->IsEqual((that != null) ? that.GetNativeInstance() : null) != 0);
		}

		/// <summary>
		/// Returns a writable copy of this object.
		/// </summary>
		public unsafe virtual CefListValue Copy()
		{
			return SafeCall(CefListValue.Wrap(CefListValue.Create, NativeInstance->Copy()));
		}

		/// <summary>
		/// Sets the number of values. If the number of values is expanded all new
		/// value slots will default to type null. Returns true (1) on success.
		/// </summary>
		public unsafe virtual bool SetSize(long size)
		{
			return SafeCall(NativeInstance->SetSize(new UIntPtr((ulong)size)) != 0);
		}

		/// <summary>
		/// Removes all values. Returns true (1) on success.
		/// </summary>
		public unsafe virtual bool Clear()
		{
			return SafeCall(NativeInstance->Clear() != 0);
		}

		/// <summary>
		/// Removes the value at the specified index.
		/// </summary>
		public unsafe virtual int Remove(long index)
		{
			return SafeCall(NativeInstance->Remove(new UIntPtr((ulong)index)));
		}

		/// <summary>
		/// Returns the value type at the specified index.
		/// </summary>
		public unsafe virtual CefValueType GetType(long index)
		{
			return SafeCall(NativeInstance->GetType(new UIntPtr((ulong)index)));
		}

		/// <summary>
		/// Returns the value at the specified index. For simple types the returned
		/// value will copy existing data and modifications to the value will not
		/// modify this object. For complex types (binary, dictionary and list) the
		/// returned value will reference existing data and modifications to the value
		/// will modify this object.
		/// </summary>
		public unsafe virtual CefValue GetValue(long index)
		{
			return SafeCall(CefValue.Wrap(CefValue.Create, NativeInstance->GetValue(new UIntPtr((ulong)index))));
		}

		/// <summary>
		/// Returns the value at the specified index as type bool.
		/// </summary>
		public unsafe virtual int GetBool(long index)
		{
			return SafeCall(NativeInstance->GetBool(new UIntPtr((ulong)index)));
		}

		/// <summary>
		/// Returns the value at the specified index as type int.
		/// </summary>
		public unsafe virtual int GetInt(long index)
		{
			return SafeCall(NativeInstance->GetInt(new UIntPtr((ulong)index)));
		}

		/// <summary>
		/// Returns the value at the specified index as type double.
		/// </summary>
		public unsafe virtual double GetDouble(long index)
		{
			return SafeCall(NativeInstance->GetDouble(new UIntPtr((ulong)index)));
		}

		/// <summary>
		/// Returns the value at the specified index as type string.
		/// The resulting string must be freed by calling cef_string_userfree_free().
		/// </summary>
		public unsafe virtual string GetString(long index)
		{
			return SafeCall(CefString.ReadAndFree(NativeInstance->GetString(new UIntPtr((ulong)index))));
		}

		/// <summary>
		/// Returns the value at the specified index as type binary. The returned value
		/// will reference existing data.
		/// </summary>
		public unsafe virtual CefBinaryValue GetBinary(long index)
		{
			return SafeCall(CefBinaryValue.Wrap(CefBinaryValue.Create, NativeInstance->GetBinary(new UIntPtr((ulong)index))));
		}

		/// <summary>
		/// Returns the value at the specified index as type dictionary. The returned
		/// value will reference existing data and modifications to the value will
		/// modify this object.
		/// </summary>
		public unsafe virtual CefDictionaryValue GetDictionary(long index)
		{
			return SafeCall(CefDictionaryValue.Wrap(CefDictionaryValue.Create, NativeInstance->GetDictionary(new UIntPtr((ulong)index))));
		}

		/// <summary>
		/// Returns the value at the specified index as type list. The returned value
		/// will reference existing data and modifications to the value will modify
		/// this object.
		/// </summary>
		public unsafe virtual CefListValue GetList(long index)
		{
			return SafeCall(CefListValue.Wrap(CefListValue.Create, NativeInstance->GetList(new UIntPtr((ulong)index))));
		}

		/// <summary>
		/// Sets the value at the specified index. Returns true (1) if the value was
		/// set successfully. If |value| represents simple data then the underlying
		/// data will be copied and modifications to |value| will not modify this
		/// object. If |value| represents complex data (binary, dictionary or list)
		/// then the underlying data will be referenced and modifications to |value|
		/// will modify this object.
		/// </summary>
		public unsafe virtual bool SetValue(long index, CefValue value)
		{
			return SafeCall(NativeInstance->SetValue(new UIntPtr((ulong)index), (value != null) ? value.GetNativeInstance() : null) != 0);
		}

		/// <summary>
		/// Sets the value at the specified index as type null. Returns true (1) if the
		/// value was set successfully.
		/// </summary>
		public unsafe virtual bool SetNull(long index)
		{
			return SafeCall(NativeInstance->SetNull(new UIntPtr((ulong)index)) != 0);
		}

		/// <summary>
		/// Sets the value at the specified index as type bool. Returns true (1) if the
		/// value was set successfully.
		/// </summary>
		public unsafe virtual bool SetBool(long index, bool value)
		{
			return SafeCall(NativeInstance->SetBool(new UIntPtr((ulong)index), value ? 1 : 0) != 0);
		}

		/// <summary>
		/// Sets the value at the specified index as type int. Returns true (1) if the
		/// value was set successfully.
		/// </summary>
		public unsafe virtual bool SetInt(long index, int value)
		{
			return SafeCall(NativeInstance->SetInt(new UIntPtr((ulong)index), value) != 0);
		}

		/// <summary>
		/// Sets the value at the specified index as type double. Returns true (1) if
		/// the value was set successfully.
		/// </summary>
		public unsafe virtual bool SetDouble(long index, double value)
		{
			return SafeCall(NativeInstance->SetDouble(new UIntPtr((ulong)index), value) != 0);
		}

		/// <summary>
		/// Sets the value at the specified index as type string. Returns true (1) if
		/// the value was set successfully.
		/// </summary>
		public unsafe virtual bool SetString(long index, string value)
		{
			fixed (char* s1 = value)
			{
				var cstr1 = new cef_string_t { Str = s1, Length = value != null ? value.Length : 0 };
				return SafeCall(NativeInstance->SetString(new UIntPtr((ulong)index), &cstr1) != 0);
			}
		}

		/// <summary>
		/// Sets the value at the specified index as type binary. Returns true (1) if
		/// the value was set successfully. If |value| is currently owned by another
		/// object then the value will be copied and the |value| reference will not
		/// change. Otherwise, ownership will be transferred to this object and the
		/// |value| reference will be invalidated.
		/// </summary>
		public unsafe virtual bool SetBinary(long index, CefBinaryValue value)
		{
			return SafeCall(NativeInstance->SetBinary(new UIntPtr((ulong)index), (value != null) ? value.GetNativeInstance() : null) != 0);
		}

		/// <summary>
		/// Sets the value at the specified index as type dict. Returns true (1) if the
		/// value was set successfully. If |value| is currently owned by another object
		/// then the value will be copied and the |value| reference will not change.
		/// Otherwise, ownership will be transferred to this object and the |value|
		/// reference will be invalidated.
		/// </summary>
		public unsafe virtual bool SetDictionary(long index, CefDictionaryValue value)
		{
			return SafeCall(NativeInstance->SetDictionary(new UIntPtr((ulong)index), (value != null) ? value.GetNativeInstance() : null) != 0);
		}

		/// <summary>
		/// Sets the value at the specified index as type list. Returns true (1) if the
		/// value was set successfully. If |value| is currently owned by another object
		/// then the value will be copied and the |value| reference will not change.
		/// Otherwise, ownership will be transferred to this object and the |value|
		/// reference will be invalidated.
		/// </summary>
		public unsafe virtual bool SetList(long index, CefListValue value)
		{
			return SafeCall(NativeInstance->SetList(new UIntPtr((ulong)index), (value != null) ? value.GetNativeInstance() : null) != 0);
		}

		/// <summary>
		/// Returns the number of values.
		/// </summary>
		public unsafe virtual long GetSize()
		{
			return SafeCall((long)NativeInstance->GetSize());
		}
	}
}
