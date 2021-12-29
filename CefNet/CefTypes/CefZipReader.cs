using CefNet.CApi;
using System;

namespace CefNet
{
	public unsafe partial class CefZipReader
	{
		/// <summary>
		/// Create a new cef_zip_reader_t object. The created instance can
		/// only be used from the thread that created the object.
		/// </summary>
		public CefZipReader(CefStreamReader reader, CefXmlEncodingType encodingType, string uri)
		: this(CefNativeApi.cef_zip_reader_create((reader ?? throw new ArgumentNullException(nameof(reader))).GetNativeInstance()))
		{

		}

	}
}
