using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Text;
using CefNet.CApi;

namespace CefNet
{
	public unsafe partial class CefPostData
	{
		/// <summary>
		/// No content.
		/// </summary>
		public const CefPostData Null = null;

		/// <summary>
		/// Create a new CefPostData object.
		/// </summary>
		public CefPostData()
			: this(CefNativeApi.cef_post_data_create())
		{

		}

		/// <summary>
		/// Create a new <see cref="CefPostData"/> object with the application/x-www-form-urlencoded data.
		/// </summary>
		/// <param name="content">The post data.</param>
		public CefPostData(NameValueCollection content)
			: this(CefNativeApi.cef_post_data_create())
		{
			if (content != null)
				AddUrlEncoded(content);
		}

		/// <summary>
		/// Create a new <see cref="CefPostData"/> object with the application/x-www-form-urlencoded data.
		/// </summary>
		/// <param name="content">The post data.</param>
		public CefPostData(IEnumerable<KeyValuePair<string, string>> content)
			: this(CefNativeApi.cef_post_data_create())
		{
			if (content != null)
				AddUrlEncoded(content);
		}

		/// <summary>
		/// Adds the elements of the specified <see cref="NameValueCollection"/> as an application/x-www-form-urlencoded string.
		/// </summary>
		/// <param name="content">The collection whose elements should be added.</param>
		/// <exception cref="ArgumentNullException">The <paramref name="content"/> is null.</exception>
		public void AddUrlEncoded(NameValueCollection content)
		{
			if (content is null)
				throw new ArgumentNullException(nameof(content));

			var ms = new MemoryStream();
			using (var w = new StreamWriter(ms, Encoding.ASCII, 1024, true))
			{
				foreach (string key in content.Keys)
				{
					string safeKey = key is null ? null : Uri.EscapeDataString(key);
					foreach (string value in content.GetValues(key))
					{
						w.Write(safeKey);
						if (value != null)
						{
							w.Write('=');
							w.Write(Uri.EscapeDataString(value));
						}
						w.Write('&');
					}
				}
				w.Flush();
			}
			using (var dataElt = new CefPostDataElement())
			{
				fixed (byte* buffer = ms.GetBuffer())
				{
					// copy (ms.Length - 1) bytes to ignore last ampersand
					dataElt.SetToBytes(ms.Length > 0 ? ms.Length - 1 : 0, new IntPtr(buffer));
				}
				AddElement(dataElt);
			}
		}

		/// <summary>
		/// Adds the elements of the specified collection as an application/x-www-form-urlencoded string.
		/// </summary>
		/// <param name="content">The collection whose elements should be added.</param>
		/// <exception cref="ArgumentNullException">The <paramref name="content"/> is null.</exception>
		public void AddUrlEncoded(IEnumerable<KeyValuePair<string, string>> content)
		{
			if (content is null)
				throw new ArgumentNullException(nameof(content));

			var ms = new MemoryStream();
			using (var w = new StreamWriter(ms, Encoding.ASCII, 1024, true))
			{
				foreach (KeyValuePair<string, string> kvp in content)
				{
					if (kvp.Key is null)
					{
						if (kvp.Value is null)
							continue;
					}
					else
					{
						w.Write(Uri.EscapeDataString(kvp.Key));
					}
					if (kvp.Value != null)
					{
						w.Write('=');
						w.Write(Uri.EscapeDataString(kvp.Value));
					}
					w.Write('&');
				}
				w.Flush();
			}
			using (var dataElt = new CefPostDataElement())
			{
				fixed (byte* buffer = ms.GetBuffer())
				{
					// copy (ms.Length - 1) bytes to ignore last ampersand
					dataElt.SetToBytes(ms.Length > 0 ? ms.Length - 1 : 0, new IntPtr(buffer));
				}
				AddElement(dataElt);
			}
		}

	}

}
