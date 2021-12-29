// --------------------------------------------------------------------------------------------
// Copyright (c) 2019 The CefNet Authors. All rights reserved.
// Licensed under the MIT license.
// See the licence file in the project root for full license information.
// --------------------------------------------------------------------------------------------
// Source: libcef\renderer\v8_impl.h
// --------------------------------------------------------------------------------------------

namespace CefNet
{
	/// <summary>
	/// V8 value types.
	/// </summary>
	public enum CefV8ValueType
	{
		/// <summary>
		/// The invalid value.
		/// </summary>
		Invalid = 0,
		/// <summary>
		/// The value is the undefined value.
		/// </summary>
		Undefined = 1,
		/// <summary>
		/// The value is the null value.
		/// </summary>
		Null = 2,
		/// <summary>
		/// The value is an JavaScript boolean value.
		/// </summary>
		Bool = 3,
		/// <summary>
		/// The value is an integer value.
		/// </summary>
		Int = 4,
		/// <summary>
		/// The value is an unsigned integer value.
		/// </summary>
		UInt = 5,
		/// <summary>
		/// The value is a double value.
		/// </summary>
		Double = 6,
		/// <summary>
		/// The value is a Date value.
		/// </summary>
		Date = 7,
		/// <summary>
		/// The value is an JavaScript string value.
		/// </summary>
		String = 8,
		/// <summary>
		/// The value is an JavaScript object value.
		/// </summary>
		Object = 9,

	}
}
