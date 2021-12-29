using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace CefNet
{
	internal unsafe sealed class ArrayBuffer : CefV8ArrayBufferReleaseCallback
	{
		private static Dictionary<IntPtr, ArrayBuffer> Root = new Dictionary<IntPtr, ArrayBuffer>();

		private GCHandle? _handle;

		public ArrayBuffer(byte[] buffer)
		{
			if (buffer == null)
				throw new ArgumentNullException(nameof(buffer));

			Length = new UIntPtr((ulong)buffer.LongLength);
			_handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
			lock (Root)
			{
				Root.Add(new IntPtr(this.GetBuffer()), this);
			}
		}

		public void* GetBuffer()
		{
			return (void*)_handle?.AddrOfPinnedObject();
		}

		public UIntPtr Length { get; }

		protected internal override void ReleaseBuffer(IntPtr buffer)
		{
			ArrayBuffer instance;
			lock (Root)
			{
				Root.Remove(buffer, out instance);
			}

			if (instance is null)
				return;

			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected override void Dispose(bool disposing)
		{
			_handle?.Free();
			_handle = null;
		}
	}
}
