using CefNet.CApi;
using System;
using System.Runtime.InteropServices;

namespace CefNet
{
	public unsafe partial class CefListValue
	{
		/// <summary>
		/// Creates a new object that is not owned by any other object.
		/// </summary>
		public CefListValue()
			: this(CefNativeApi.cef_list_value_create())
		{

		}

		public bool SetBinary(long index, byte[] buffer)
		{
			using (var v = new CefBinaryValue(buffer))
			{
				return SetBinary(index, v);
			}
		}

		internal unsafe long GetInt64(long index)
		{
			double value = this.GetDouble(index);
			return *((long*)&value);
		}

		internal unsafe bool SetInt64(long index, long value)
		{
			double f64 = *((double*)&value);
			return SetDouble(index, f64);
		}

		//internal GCHandle GetGCHandle(long index)
		//{
		//	return GCHandle.FromIntPtr(new IntPtr(GetInt64(index)));
		//}

		//internal bool SetGCHandle(long index, GCHandle handle)
		//{
		//	return SetInt64(index, GCHandle.ToIntPtr(handle).ToInt64());
		//}
	}
}
