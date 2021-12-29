using CefNet.CApi;
using System;
using System.Runtime.CompilerServices;

namespace CefNet
{
	public unsafe partial class CefBinaryValue
	{
		/// <summary>
		/// Creates a new object that is not owned by any other object. The specified |data| will be copied.
		/// </summary>
		/// <param name="data">The memory pointer to data.</param>
		/// <param name="size">The size of data.</param>
		public CefBinaryValue(IntPtr data, int size)
			: this(CefNativeApi.cef_binary_value_create((void*)data, size >= 0 ? unchecked((UIntPtr)size) : throw new ArgumentOutOfRangeException(nameof(size))))
		{

		}

		/// <summary>
		/// Creates a new object that is not owned by any other object. The specified |buffer| will be copied.
		/// </summary>
		public CefBinaryValue(byte[] buffer)
			: this(CreateFromBuffer(buffer))
		{

		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static cef_binary_value_t* CreateFromBuffer(byte[] buffer)
		{
			if (buffer == null)
				throw new ArgumentNullException(nameof(buffer));
			fixed (void* data = buffer)
			{
				return CefNativeApi.cef_binary_value_create(data, unchecked((UIntPtr)buffer.Length));
			}
		}

		public unsafe byte[] ToArray()
		{
			var buffer = new byte[(int)Size];
			fixed (byte* buf = buffer)
			{
				if (GetData(new IntPtr(buf), buffer.Length, 0) != buffer.Length)
					throw new InvalidOperationException();
			}
			return buffer;
		}
	}
}
