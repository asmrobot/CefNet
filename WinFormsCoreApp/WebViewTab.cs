using CefNet;
using CefNet.Windows.Forms;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using WinFormsCoreApp;

namespace WinFormsCoreApp
{
	public class WebViewTab : TabPage, IWebViewTab
	{
		public WebViewTab()
			: this(new CustomWebView())
		{

		}

		public WebViewTab(CefBrowserSettings settings, CefRequestContext requestContext)
			: this(new CustomWebView { RequestContext = requestContext, BrowserSettings = settings })
		{

		}

		private WebViewTab(CustomWebView webview)
		{
			webview.Dock = DockStyle.Fill;
			webview.CreateWindow += Webview_CreateWindow;
			webview.DocumentTitleChanged += HandleDocumentTitleChanged;
			webview.Closing += Webview_Closing;
			webview.Closed += Webview_Closed;
			this.WebView = webview;
			this.Controls.Add(webview);
		}

		private void Webview_Closed(object sender, EventArgs e)
		{
			this.FindTabControl()?.TabPages.Remove(this);
		}

		private void Webview_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (!IsDisposed) // ignore if disposed
			{
				e.Cancel = (MessageBox.Show(this, "Do you want to close this tab?", this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes);
			}
		}

		private void HandleDocumentTitleChanged(object sender, DocumentTitleChangedEventArgs e)
		{
			this.Text = e.Title + "    ";
			this.ToolTipText = e.Title;
		}

		public IChromiumWebView WebView { get; protected set; }

		private void Webview_CreateWindow(object sender, CreateWindowEventArgs e)
		{
			TabControl tabs = this.FindTabControl();
			if (tabs == null)
			{
				e.Cancel = true;
				return;
			}

			var webview = new CustomWebView((CustomWebView)this.WebView);
			if (webview.WindowlessRenderingEnabled)
			{
				e.WindowInfo.SetAsWindowless(webview.Handle);
			}
			else
			{
				e.WindowInfo.SetAsDisabledChild(webview.Handle);
			}
			e.Client = webview.Client;
			OnCreateWindow(webview);
		}


		protected virtual void OnCreateWindow(CustomWebView webview)
		{
			this.FindTabControl().Controls.Add(new WebViewTab(webview));
		}

		protected override void Dispose(bool disposing)
		{
			WebView?.Close();
			base.Dispose(disposing);
		}
	}
}
