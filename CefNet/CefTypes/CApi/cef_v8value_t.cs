using System;
using CefNet.Unsafe;

namespace CefNet.CApi
{
#pragma warning disable CS1591
	public unsafe partial struct cef_v8value_t
#pragma warning restore CS1591
	{
		/// <summary>
		/// Returns the hash code for this instance.
		/// </summary>
		/// <returns>A hash code for the <see cref="cef_v8value_t"/> object.</returns>
		public override int GetHashCode()
		{
			if (IsValid() == 0)
				return 0;

			fixed (cef_v8value_t* self = &this)
			{
				RefCountedWrapperStruct* ws = RefCountedWrapperStruct.FromRefCounted(self);
				V8ValueImplLayout* v8impl = V8ValueImplLayout.FromCppObject(ws->cppObject);
				switch (v8impl->Type)
				{
					case CefV8ValueType.Object:
						V8ValueImplHandleLayout* v8ValueHandle = v8impl->handle;
						if (v8ValueHandle == null)
							return 0;
						IntPtr* handle = v8ValueHandle->handle;
						return (handle != null) ? (*handle).GetHashCode() : 0;
					case CefV8ValueType.Bool:
						return v8impl->value.bool_value_ | (int)CefV8ValueType.Bool;
					case CefV8ValueType.Double:
						return v8impl->value.double_value_.GetHashCode();
					case CefV8ValueType.Int:
					case CefV8ValueType.UInt:
						return v8impl->value.int_value_;
					case CefV8ValueType.Null:
					case CefV8ValueType.Undefined:
						return (int)v8impl->Type;
					case CefV8ValueType.String:
						return v8impl->value.string_value_.GetHashCode();
					case CefV8ValueType.Date:
						return v8impl->value.date_value_.GetHashCode();
				}
			}
			return 0;
		}
	}
}
