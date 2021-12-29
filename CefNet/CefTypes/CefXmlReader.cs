using CefNet.CApi;
using System;

namespace CefNet
{
	public unsafe partial class CefXmlReader
	{
		/// <summary>
		/// Create a new cef_xml_reader_t object. The created instance can
		/// only be used from the thread that created the object.
		/// </summary>
		public CefXmlReader(CefStreamReader reader, CefXmlEncodingType encodingType, string uri)
		: this(Create(reader, encodingType, uri))
		{

		}

		public static cef_xml_reader_t* Create(CefStreamReader reader, CefXmlEncodingType encodingType, string uri)
		{
			if (reader == null)
				throw new ArgumentNullException(nameof(reader));

			fixed (char* s0 = uri)
			{
				var cstr0 = new cef_string_t { Str = s0, Length = (uri != null ? uri.Length : 0) };
				return CefNativeApi.cef_xml_reader_create(reader.GetNativeInstance(), encodingType, &cstr0);
			}
		}

	}
}
