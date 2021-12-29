using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;

#if WINFORMS
using System.Windows.Forms;
namespace CefNet.Windows.Forms
#elif WPF
namespace CefNet.Wpf
#endif
{
	/// <summary>
	/// Implements a basic data transfer mechanism.
	/// </summary>
	public class CefNetDragData : IDataObject
	{
		internal const string DataFormatUrl = "UniformResourceLocator";
		internal const string DataFormatUnicodeUrl = "UniformResourceLocatorW";
		internal const string DataFormatTextHtml = "text/html";
		public const string DataFormatCefNetDragData = nameof(CefNetDragData);
		public const string DataFormatCefDragData = nameof(CefDragData);

		private WeakReference<WebView> _source;
		private HashSet<string> _formats;

		/// <summary>
		/// Initializes a new instance of the <see cref="CefNetDragData"/> class.
		/// </summary>
		/// <param name="source">The source of the drag event.</param>
		/// <param name="data">The original drag data.</param>
		public CefNetDragData(WebView source, CefDragData data)
		{
			var formats = new HashSet<string>();
			formats.Add(DataFormatCefNetDragData);
			formats.Add(DataFormatCefDragData);
			if (data.IsFile)
			{
				formats.Add(DataFormats.FileDrop);
			}
			if (data.IsLink)
			{
				formats.Add(DataFormatUnicodeUrl);
				formats.Add(DataFormats.UnicodeText);
			}
			if (data.IsFragment)
			{
				formats.Add(DataFormats.UnicodeText);
				formats.Add(DataFormats.Html);
				formats.Add(DataFormatTextHtml);
			}

			_source = new WeakReference<WebView>(source);
			_formats = formats;
			this.Data = data;
		}

		/// <summary>
		/// The original drag data.
		/// </summary>
		public CefDragData Data { get; }

		/// <summary>
		/// The source of the drag event.
		/// </summary>
		public WebView Source
		{
			get
			{
				if (_source.TryGetTarget(out WebView source))
					return source;
				return null;
			}
		}

		public virtual object GetData(string format, bool autoConvert)
		{
			string s;
			if (DataFormatUnicodeUrl.Equals(format, StringComparison.Ordinal))
			{
				s = Data.LinkUrl;
				return s is null ? null : new MemoryStream(Encoding.Unicode.GetBytes(s));
			}
			if (DataFormatUrl.Equals(format, StringComparison.Ordinal))
			{
				s = Data.LinkUrl;
				return s is null ? null : new MemoryStream(Encoding.ASCII.GetBytes(s));
			}
			if (DataFormats.UnicodeText.Equals(format, StringComparison.Ordinal))
			{
				if (Data.IsLink)
					return Data.LinkUrl;
				return Data.FragmentText;
			}
			if (DataFormats.Html.Equals(format, StringComparison.Ordinal))
				return Data.FragmentHtml;
			if (DataFormats.Text.Equals(format, StringComparison.Ordinal))
				return Data.FragmentText;
			if (DataFormatTextHtml.Equals(format, StringComparison.Ordinal))
				return Data.FragmentHtml;
			if (DataFormatCefDragData.Equals(format, StringComparison.Ordinal))
				return Data;
			if (DataFormatCefNetDragData.Equals(format, StringComparison.Ordinal))
				return this;

			return null;
		}

		public object GetData(string format)
		{
			return GetData(format, true);
		}

		public virtual object GetData(Type format)
		{
			return null;
		}

		public virtual bool GetDataPresent(string format, bool autoConvert)
		{
			return _formats.Contains(format);
		}

		public bool GetDataPresent(string format)
		{
			return GetDataPresent(format, true);
		}

		public virtual bool GetDataPresent(Type format)
		{
			return false;
		}

		public virtual string[] GetFormats(bool autoConvert)
		{
			return _formats.ToArray();
		}

		public string[] GetFormats()
		{
			return GetFormats(true);
		}

#if WINFORMS
		public virtual void SetData(string format, bool autoConvert, object data)
#elif WPF
		public virtual void SetData(string format, object data, bool autoConvert)
#endif
		{
			throw new NotSupportedException();
		}

		public void SetData(string format, object data)
		{
#if WINFORMS
			SetData(format, true, data);
#elif WPF
			SetData(format, data, true);
#else
			throw new NotImplementedException();
#endif
		}

		public virtual void SetData(Type format, object data)
		{
			throw new NotSupportedException();
		}

		public virtual void SetData(object data)
		{
			throw new NotSupportedException();
		}
	}
}
