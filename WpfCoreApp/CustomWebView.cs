using CefNet.Internal;
using CefNet.Wpf;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace WpfCoreApp
{
	sealed class CustomWebView : WebView
	{
		public static RoutedEvent FullscreenEvent = EventManager.RegisterRoutedEvent("Fullscreen", RoutingStrategy.Bubble, typeof(EventHandler<FullscreenModeChangeEventArgs>), typeof(WebView));

		public event EventHandler<FullscreenModeChangeEventArgs> Fullscreen
		{
			add { AddHandler(FullscreenEvent, value); }
			remove { RemoveHandler(FullscreenEvent, value); }
		}

		public CustomWebView()
		{

		}

		public CustomWebView(WebView opener)
			: base(opener)
		{

		}

		protected override WebViewGlue CreateWebViewGlue()
		{
			return new CustomWebViewGlue(this);
		}



		internal void RaiseFullscreenModeChange(bool fullscreen)
		{
			RaiseCrossThreadEvent(OnFullScreenModeChange, new FullscreenModeChangeEventArgs(this, fullscreen), false);
		}

		private void OnFullScreenModeChange(FullscreenModeChangeEventArgs e)
		{

			RaiseEvent(e);
		}


	}
}
