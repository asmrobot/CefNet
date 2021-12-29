using System;
using System.Collections.Generic;
using System.Text;

namespace CefNet.Internal
{
	public partial class WebViewGlue
	{
		internal bool AvoidOnBeforeBrowse()
		{
			return false;
		}

		internal protected virtual bool OnBeforeBrowse(CefBrowser browser, CefFrame frame, CefRequest request, bool userGesture, bool isRedirect)
		{
			var ea = new BeforeBrowseEventArgs(browser, frame, request, userGesture, isRedirect);
			WebView.RaiseBeforeBrowse(ea);
			return ea.Cancel;
		}

		internal bool AvoidOnOpenUrlFromTab()
		{
			return false;
		}

		internal protected virtual bool OnOpenUrlFromTab(CefBrowser browser, CefFrame frame, string targetUrl, CefWindowOpenDisposition targetDisposition, bool userGesture)
		{
			return false;
		}

		internal bool AvoidGetResourceRequestHandler()
		{
			return false;
		}

		internal protected virtual CefResourceRequestHandler GetResourceRequestHandler(CefBrowser browser, CefFrame frame, CefRequest request, bool isNavigation, bool isDownload, string requestInitiator, ref int disableDefaultHandling)
		{
			return ResourceRequestGlue;
		}

		internal bool AvoidGetAuthCredentials()
		{
			return false;
		}

		internal protected virtual bool GetAuthCredentials(CefBrowser browser, string originUrl, bool isProxy, string host, int port, string realm, string scheme, CefAuthCallback callback)
		{
			return false;
		}

		internal bool AvoidOnQuotaRequest()
		{
			return false;
		}

		internal protected virtual bool OnQuotaRequest(CefBrowser browser, string originUrl, long newSize, CefCallback callback)
		{
			return false;
		}

		internal bool AvoidOnCertificateError()
		{
			return false;
		}

		internal protected virtual bool OnCertificateError(CefBrowser browser, CefErrorCode certError, string requestUrl, CefSSLInfo sslInfo, CefCallback callback)
		{
			return false;
		}

		internal bool AvoidOnSelectClientCertificate()
		{
			return false;
		}

		internal protected virtual bool OnSelectClientCertificate(CefBrowser browser, bool isProxy, string host, int port, CefX509Certificate[] certificates, CefSelectClientCertificateCallback callback)
		{
			return false;
		}

		internal bool AvoidOnPluginCrashed()
		{
			return false;
		}

		internal protected virtual void OnPluginCrashed(CefBrowser browser, string pluginPath)
		{

		}

		internal bool AvoidOnRenderViewReady()
		{
			return false;
		}

		internal protected virtual void OnRenderViewReady(CefBrowser browser)
		{

		}

		internal bool AvoidOnRenderProcessTerminated()
		{
			return false;
		}

		internal protected virtual void OnRenderProcessTerminated(CefBrowser browser, CefTerminationStatus status)
		{

		}

		internal bool AvoidOnDocumentAvailableInMainFrame()
		{
			return false;
		}

		/// <summary>
		/// Called on the browser process UI thread when the window.document object of
		/// the main frame has been created.
		/// </summary>
		internal protected virtual void OnDocumentAvailableInMainFrame(CefBrowser browser)
		{

		}

	}
}
