using CefNet;
using Modern.Forms;
using System;
using System.Collections.Generic;
using System.Text;
using WinFormsCoreApp;

namespace ModernFormsApp
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
			//VirtualDevice device = IPhoneDevice.Create(IPhone.Model5);
			//device.Rotate();
			//webview.SimulateDevice(device);

			this.Text = "about:blank";
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

		}

		private void HandleDocumentTitleChanged(object sender, DocumentTitleChangedEventArgs e)
		{
			this.Text = e.Title + "    ";
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
			e.WindowInfo.SetAsWindowless(IntPtr.Zero);
			e.Client = webview.Client;
			OnCreateWindow(webview);
		}


		protected virtual void OnCreateWindow(CustomWebView webview)
		{
			this.FindTabControl().TabPages.Add(new WebViewTab(webview));
		}

		protected override void Dispose(bool disposing)
		{
			WebView?.Close();
			base.Dispose(disposing);
		}
	}
}
