using CefNet.CApi;
using System;
using System.IO;

namespace CefNet
{
	public unsafe partial class CefStreamWriter
	{
		/// <summary>
		/// Create a new CefStreamWriter object for a file.
		/// </summary>
		public CefStreamWriter(string filename)
			: this(CreateForFile(filename))
		{

		}

		/// <summary>
		/// Create a new CefStreamReader object for a custom handler.
		/// </summary>
		public CefStreamWriter(CefWriteHandler handler)
			: this(CefNativeApi.cef_stream_writer_create_for_handler((handler ?? throw new ArgumentNullException(nameof(handler))).GetNativeInstance()))
		{

		}

		private static cef_stream_writer_t* CreateForFile(string filename)
		{
			if (filename == null)
				throw new ArgumentNullException(nameof(filename));

			// TODO: check file?
			//if (!File.Exists(filename))
			//	throw new FileNotFoundException(null, filename);

			fixed (char* s = filename)
			{
				cef_string_t cstr = new cef_string_t();
				cstr.Base.str = s;
				cstr.Base.length = unchecked((UIntPtr)filename.Length);
				return CefNativeApi.cef_stream_writer_create_for_file(&cstr);
			}
		}

	}
}
