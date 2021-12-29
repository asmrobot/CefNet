using CefNet.CApi;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace CefNet.Unsafe
{
	[StructLayout(LayoutKind.Sequential)]
#if DEBUG
	public
#endif
	unsafe struct V8ValueImplLayout
	{
		private static int LayoutOffset = PlatformInfo.IsWindows ? 0 : IntPtr.Size;

		public IntPtr v8value_vtable;
		public IntPtr refcounted_vtable;
		public IntPtr isolate;
		public IntPtr type;
		public V8ValueImpl_ValueUnion value;
		public V8ValueImplHandleLayout* handle;
		public IntPtr last_exception;

		public CefV8ValueType Type
		{
			get { return (CefV8ValueType)(type.ToInt64() & 0xFFFFFFFF); }
		}

		public static V8ValueImplLayout* FromCppObject(IntPtr cppObj)
		{
			// v8value_vtable or refcounted_vtable does not exist on non-Windows platforms.
			return (V8ValueImplLayout*)IntPtr.Subtract(cppObj, LayoutOffset);
		}
	}


	[StructLayout(LayoutKind.Explicit)]
#if DEBUG
	public
#endif
	unsafe struct V8ValueImpl_ValueUnion
	{
		[FieldOffset(0)]
		public byte bool_value_;
		[FieldOffset(0)]
		public int int_value_;
		[FieldOffset(0)]
		public uint uint_value_;
		[FieldOffset(0)]
		public double double_value_;
		[FieldOffset(0)]
		public cef_time_t date_value_;
		[FieldOffset(0)]
		public cef_string_t string_value_;
	}

	[StructLayout(LayoutKind.Sequential)]
#if DEBUG
	public
#endif
	unsafe struct V8ValueImplHandleLayout
	{
		public void* v8value_handle_vtable;
		public void* atomic_refcount;
		public void* isolate;
		public void* taskrunner;
		public void* context_state;
		public IntPtr* handle;
		public void* tracker;
		public byte should_persist;
		public byte is_set_weak;
	}
}
