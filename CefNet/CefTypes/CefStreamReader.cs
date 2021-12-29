using CefNet.CApi;
using System;
using System.IO;

namespace CefNet
{
	public unsafe partial class CefStreamReader
	{
		/// <summary>
		/// Create a new CefStreamReader object from a file.
		/// </summary>
		public CefStreamReader(string filename)
			: this(CreateFromFile(filename))
		{

		}

		/// <summary>
		/// Create a new CefStreamReader object from data.
		/// </summary>
		public CefStreamReader(IntPtr data, int length)
			: this(CefNativeApi.cef_stream_reader_create_for_data((void*)data, length >= 0 ? unchecked((UIntPtr)length) : throw new ArgumentOutOfRangeException(nameof(length))))
		{

		}

		/// <summary>
		/// Create a new CefStreamReader object from a custom handler.
		/// </summary>
		public CefStreamReader(CefReadHandler handler)
			: this(CefNativeApi.cef_stream_reader_create_for_handler((handler ?? throw new ArgumentNullException(nameof(handler))).GetNativeInstance()))
		{

		}

		/// <summary>
		/// Create a new cef_stream_reader_t object from buffer.
		/// </summary>
		public CefStreamReader(byte[] buffer)
			: this(CreateFromBuffer(buffer))
		{
			
		}

		private static cef_stream_reader_t* CreateFromFile(string filename)
		{
			if (filename == null)
				throw new ArgumentNullException(nameof(filename));
			if (!File.Exists(filename))
				throw new FileNotFoundException(null, filename);

			fixed (char* s = filename)
			{
				cef_string_t cstr = new cef_string_t();
				cstr.Base.str = s;
				cstr.Base.length = unchecked((UIntPtr)filename.Length);
				return CefNativeApi.cef_stream_reader_create_for_file(&cstr);
			}
		}

		private static cef_stream_reader_t* CreateFromBuffer(byte[] buffer)
		{
			if (buffer == null)
				throw new ArgumentNullException(nameof(buffer));
			fixed (void* data = buffer)
			{
				return CefNativeApi.cef_stream_reader_create_for_data(data, unchecked((UIntPtr)buffer.Length));
			}
		}

	}
}
