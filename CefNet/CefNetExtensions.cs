using System;
using System.Collections.Generic;
using System.Text;
using CefNet.Internal;

namespace CefNet
{
	/// <summary>
	/// Provides extension methods for common scenarios.
	/// </summary>
	public static class CefNetExtensions
	{
		/// <summary>
		/// Returns a WebView control for this <see cref="CefClient"/>.
		/// </summary>
		/// <param name="client">The <see cref="CefClient"/> object.</param>
		/// <returns>An <see cref="IChromiumWebView"/> object associated with this client or null.</returns>
		public static IChromiumWebView GetWebView(this CefClient client)
		{
			if (client is null)
				throw new ArgumentNullException(nameof(client));
			var glue = client as CefClientGlue;
			if (glue is null)
				return null;
			return glue.Implementation.WebView as IChromiumWebView;
		}

		/// <summary>
		/// Returns a WebView control for this <see cref="CefBrowser"/>.
		/// </summary>
		/// <param name="browser">The <see cref="CefBrowser"/> object.</param>
		/// <returns>An <see cref="IChromiumWebView"/> object associated with this client or null.</returns>
		public static IChromiumWebView GetWebView(this CefBrowser browser)
		{
			if (browser is null)
				throw new ArgumentNullException(nameof(browser));

			CefBrowserHost host = browser.Host;
			if (host is null)
				return null;
			CefClient client = host.Client;
			if (client is null)
				return null;
			return GetWebView(client);
		}

		/// <summary>
		/// Returns a WebView control for this frame.
		/// </summary>
		/// <param name="client">The <see cref="CefFrame"/> object.</param>
		/// <returns>An <see cref="IChromiumWebView"/> object associated with this client or null.</returns>
		public static IChromiumWebView GetWebView(this CefFrame frame)
		{
			if (frame is null)
				throw new ArgumentNullException(nameof(frame));

			if (!frame.IsValid)
				return null;

			CefBrowser browser = frame.Browser;
			if (browser is null)
				return null;
			return GetWebView(browser);
		}

	}
}
