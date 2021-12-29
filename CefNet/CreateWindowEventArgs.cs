

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CefNet
{
	public class CreateWindowEventArgs : CancelEventArgs
	{
		public CreateWindowEventArgs(
			CefFrame frame,
			string targetUrl,
			string targetFrameName,
			CefWindowOpenDisposition targetDisposition,
			bool userGesture,
			CefPopupFeatures popupFeatures,
			CefWindowInfo windowInfo,
			CefClient client,
			CefBrowserSettings settings,
			CefDictionaryValue extraInfo,
			bool noJavascriptAccess)
		{
			this.Frame = frame;
			this.TargetUrl = targetUrl;
			this.TargetFrameName = targetFrameName;
			this.TargetDisposition = targetDisposition;
			this.UserGesture = userGesture;
			this.PopupFeatures = popupFeatures;
			this.Settings = settings;

			this.WindowInfo = windowInfo;
			this.Client = client;
			this.ExtraInfo = extraInfo;
			this.NoJavaScriptAccess = noJavascriptAccess;
		}

		public CefFrame Frame { get; }

		public string TargetUrl { get; }

		public string TargetFrameName { get; }

		public CefWindowOpenDisposition TargetDisposition { get; }

		public bool UserGesture { get; }

		public CefPopupFeatures PopupFeatures { get; }

		public CefBrowserSettings Settings { get; }

		public CefWindowInfo WindowInfo { get; }

		public CefClient Client { get; set; }

		public CefDictionaryValue ExtraInfo { get; set; }

		public bool NoJavaScriptAccess { get; set; }

	}
}
