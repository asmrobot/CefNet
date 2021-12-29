using CefNet.CApi;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace CefNet.Unsafe
{
	[StructLayout(LayoutKind.Sequential)]
#if DEBUG
	public
#endif
	unsafe struct RefCountedWrapperStruct
	{
		public IntPtr type;
		public IntPtr cppObject;
		public IntPtr wrapper;
		public cef_base_ref_counted_t _refcounted;

		public CefWrapperType Type
		{
			get { return (CefWrapperType)(type.ToInt64() & 0xFFFFFFFF); }
		}

		private static readonly unsafe int RefCountedFieldOffset = GetRefCountetFieldOffset();

		private unsafe static int GetRefCountetFieldOffset()
		{
			//return (int)Marshal.OffsetOf<RefCountedWrapperStruct>("_refcounted");
			var ws = new RefCountedWrapperStruct();
			return (int)((byte*)&(ws._refcounted) - (byte*)&ws);
		}

		public static RefCountedWrapperStruct* FromRefCounted(void* instance)
		{
			return (RefCountedWrapperStruct*)((byte*)instance - RefCountedFieldOffset);
		}
	}



}
