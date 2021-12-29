using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CefNet
{
	public class BeforeBrowseEventArgs: CancelEventArgs
	{
		public BeforeBrowseEventArgs(CefBrowser browser, CefFrame frame, CefRequest request, bool userGesture, bool isRedirect)
		{
			this.Browser = browser;
			this.Frame = frame;
			this.Request = request;
			this.UserGesture = userGesture;
			this.IsRedirect = isRedirect;
		}

		/// <summary>
		/// The browser object.
		/// </summary>
		public CefBrowser Browser { get; }

		/// <summary>
		/// The frame the request is coming from.
		/// </summary>
		public CefFrame Frame { get; }

		/// <summary>
		/// The request object. Cannot be modified.
		/// </summary>
		public CefRequest Request { get; }

		/// <summary>
		/// 
		/// </summary>
		public bool UserGesture { get; }

		/// <summary>
		/// Has the request been redirected.
		/// </summary>
		public bool IsRedirect { get; }

		/// <summary>
		/// A Uri representing the location of the document to which the WebView control is navigating.
		/// </summary>
		public Uri Url
		{
			get { return new Uri(Request.Url, UriKind.Absolute); }
		}

	}
}
