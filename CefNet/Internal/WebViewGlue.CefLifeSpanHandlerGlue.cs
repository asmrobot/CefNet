using System;
using System.Collections.Generic;
using System.Text;

namespace CefNet.Internal
{
	public partial class WebViewGlue
	{
		internal bool AvoidOnBeforePopup()
		{
			return false;
		}

		/// <summary>
		/// Called on the UI thread before a new popup browser is created. To allow creation of the popup
		/// browser optionally modify <paramref name="windowInfo"/>, <paramref name="client"/>,
		/// <paramref name="settings"/> and <paramref name="noJavascriptAccess"/> and return false.
		/// To cancel creation of the popup browser return true. Popup browser creation will be canceled
		/// if the parent browser is destroyed before the popup browser creation completes (indicated by a
		/// call to <see cref="OnAfterCreated"/> for the popup browser).
		/// </summary>
		/// <param name="browser">A value represent the source browser of the popup request.</param>
		/// <param name="frame">A value represent the source frame of the popup request.</param>
		/// <param name="targetUrl">
		/// A value indicate where the popup browser should navigate and may be empty if not specified
		/// with the request.
		/// </param>
		/// <param name="targetFrameName">
		/// A value indicate where the popup browser should navigate and may be empty if not specified
		/// with the request.
		/// </param>
		/// <param name="targetDisposition">
		/// A value indicates where the user intended to open the popup (e.g. current tab, new tab, etc).
		/// </param>
		/// <param name="userGesture">
		/// A value will be true if the popup was opened via explicit user gesture (e.g. clicking a link)
		/// or false if the popup opened automatically (e.g. via the DomContentLoaded event).
		/// </param>
		/// <param name="popupFeatures">Additional information about the requested popup window.</param>
		/// <param name="windowInfo">The window information.</param>
		/// <param name="client"></param>
		/// <param name="settings">The browser settings, defaults to source browsers.</param>
		/// <param name="extraInfo">
		/// Provides an opportunity to specify extra information specific to the created popup browser that
		/// will be passed to <see cref="CefNetApplication.OnBrowserCreated"/> in the render process.
		/// </param>
		/// <param name="noJavascriptAccess">
		/// If the value is set to false the new browser will not be scriptable and may not be hosted in
		/// the same renderer process as the source browser.
		/// </param>
		/// <returns></returns>
		internal protected virtual bool OnBeforePopup(CefBrowser browser, CefFrame frame, string targetUrl, string targetFrameName, CefWindowOpenDisposition targetDisposition,
			bool userGesture, CefPopupFeatures popupFeatures, CefWindowInfo windowInfo, ref CefClient client, CefBrowserSettings settings, ref CefDictionaryValue extraInfo, ref int noJavascriptAccess)
		{
#if DEBUG
			if (!BrowserObject.IsSame(browser))
				throw new InvalidOperationException();
#endif
			var ea = new CreateWindowEventArgs(frame, targetUrl, targetFrameName, targetDisposition, userGesture, popupFeatures, windowInfo, null, settings, extraInfo, noJavascriptAccess != 0);
			WebView.RaiseCefCreateWindow(ea);
			extraInfo = ea.ExtraInfo;
			noJavascriptAccess = ea.NoJavaScriptAccess ? 1 : 0;
			client = ea.Client;
			if (!ea.Cancel)
			{
				(client as CefClientGlue)?.NotifyPopupBrowserCreating();
			}
			return ea.Cancel;
		}

		internal bool AvoidOnAfterCreated()
		{
			return false;
		}

		/// <summary>
		/// Called after a new browser is created. This callback will be the first
		/// notification that references <paramref name="browser"/>.
		/// </summary>
		/// <param name="browser">The browser instance.</param>
		internal protected virtual void OnAfterCreated(CefBrowser browser)
		{
#if DEBUG
			if (this.BrowserObject != null)
				throw new InvalidOperationException();
#endif
			this.BrowserObject = browser;
			CefNetApplication.Instance.OnBrowserCreated(browser, null);
			WebView.RaiseCefBrowserCreated();
		}

		internal bool AvoidDoClose()
		{
			return false;
		}

		/// <summary>
		/// Called when a browser has recieved a request to close. The <see cref="DoClose"/>
		/// method  will be called after the JavaScript &apos;onunload&apos; event has been
		/// fired. If the browser&apos;s top-level owner window requires a non-standard close
		/// notification then send that notification from <see cref="DoClose"/> and return true.
		/// </summary>
		/// <param name="browser">The browser instance.</param>
		internal protected virtual bool DoClose(CefBrowser browser)
		{
#if DEBUG
			if (!BrowserObject.IsSame(browser))
				throw new InvalidOperationException();
#endif
			return WebView.RaiseClosing();
		}

		internal bool AvoidOnBeforeClose()
		{
			return false;
		}

		/// <summary>
		/// Called just before a browser is destroyed. This callback will be the last notification that references 
		/// <paramref name="browser"/> on the UI thread.
		/// </summary>
		/// <param name="browser"></param>
		/// <remarks>
		/// Release all references to the browser object and do not attempt to execute any methods on the browser
		/// object (other than <see cref="CefBrowser.Identifier"/> or <see cref="CefBrowser.IsSame"/>) after this
		/// callback returns. Any in-progress network requests associated with <paramref name="browser"/> will be
		/// aborted when the browser is destroyed, and <see cref="CefResourceRequestHandler"/> callbacks related
		/// to those requests may still arrive on the IO thread after this method is called.
		///</remarks>
		internal protected virtual void OnBeforeClose(CefBrowser browser)
		{
#if DEBUG
			if (!BrowserObject.IsSame(browser))
				throw new InvalidOperationException();
#endif
			try
			{
				WebView.RaiseClosed();
			}
			finally
			{
				this.BrowserObject = null;
				DevToolsExtensions.ReleaseProtocolClient(browser.Identifier);
				CefNetApplication.Instance.OnBrowserDestroyed(browser);
			}
		}
	}
}
