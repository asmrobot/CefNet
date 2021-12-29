using CefNet;
using CefNet.Wpf;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;

namespace WpfCoreApp
{
	public class WebViewTab : TabItem
	{
		private class WebViewTabTitle : Control
		{
			private WebViewTab _tab;
			private FormattedText _xButton;
			private Brush _xbuttonBrush;
			public WebViewTabTitle(WebViewTab tab)
			{
				_tab = tab;
			}

			public string Text
			{
				get
				{
					return FormattedText?.Text;
				}
				set
				{
					if (string.IsNullOrWhiteSpace(value))
					{
						this.FormattedText = null;
						this.InvalidateMeasure();
						return;
					}
					this.FormattedText = new FormattedText(value, CultureInfo.CurrentUICulture, FlowDirection.LeftToRight, new Typeface(FontFamily, FontStyle, FontWeight, FontStretch), FontSize, Brushes.Black, VisualTreeHelper.GetDpi(this).PixelsPerDip);
					this.InvalidateMeasure();
				}
			}

			private FormattedText FormattedText { get; set; }

			private FormattedText XButton
			{
				get
				{
					if (_xButton == null)
					{
						_xButton = new FormattedText("x", CultureInfo.CurrentUICulture, FlowDirection.LeftToRight, new Typeface(FontFamily, FontStyle, FontWeights.Bold, FontStretch), FontSize, Brushes.Gray, VisualTreeHelper.GetDpi(this).PixelsPerDip);
					}
					return _xButton;
				}
			}

			protected override Size MeasureOverride(Size constraint)
			{
				var ft = this.FormattedText;
				if (ft == null)
					return base.MeasureOverride(constraint);
				return new Size(ft.Width + XButton.Width + 4, ft.Height);
			}

			protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
			{
				if (GetXButtonRect().Contains(e.GetPosition(this)))
				{
					_tab.Close();
				}
			}

			private Rect GetXButtonRect()
			{
				return new Rect(ActualWidth - XButton.Width, 0, XButton.Width, XButton.Height);
			}

			protected override void OnMouseMove(MouseEventArgs e)
			{
				SetXButtonBrush(GetXButtonRect().Contains(e.GetPosition(this)) ? Brushes.Black : Brushes.Gray);
				base.OnMouseMove(e);
			}

			protected override void OnMouseLeave(MouseEventArgs e)
			{
				SetXButtonBrush(Brushes.Gray);
				base.OnMouseLeave(e);
			}

			private void SetXButtonBrush(Brush brush)
			{
				if (brush != _xbuttonBrush)
				{
					_xbuttonBrush = brush;
					XButton.SetForegroundBrush(brush);
					this.InvalidateVisual();
				}
			}

			protected override void OnRender(DrawingContext drawingContext)
			{
				FormattedText formattedText = this.FormattedText;
				if (formattedText == null)
					return;
				drawingContext.DrawText(formattedText, new Point());
				drawingContext.DrawText(XButton, new Point(ActualWidth - XButton.Width, 0));
			}
		}

		public WebViewTab()
			: this(new CustomWebView())
		{

		}

		//public WebViewTab(CefBrowserSettings settings, CefRequestContext requestContext)
		//	: this(new WebView(settings, requestContext))
		//{

		//}

		private WebViewTab(WebView webview)
		{
			//webview.Dock = DockStyle.Fill;
			webview.CreateWindow += Webview_CreateWindow;
			webview.DocumentTitleChanged += HandleDocumentTitleChanged;
			this.WebView = webview;
			this.Header = new WebViewTabTitle(this);
			//this.Controls.Add(webview);
		}

		public string Title
		{
			get
			{
				return ((WebViewTabTitle)this.Header).Text;
			}
			set
			{
				((WebViewTabTitle)this.Header).Text = value;
			}
		}

		protected override void OnInitialized(EventArgs e)
		{
			base.OnInitialized(e);
			this.Content = this.WebView;
		}

		public void Close()
		{
			this.WebView.Close();

			var tabs = this.Parent as TabControl;
			if (tabs == null)
				return;
			tabs.Items.Remove(this);
		}

		private void HandleDocumentTitleChanged(object sender, DocumentTitleChangedEventArgs e)
		{
			this.Title = e.Title;
			//this.ToolTipText = e.Title;
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

			var wpfwindow = System.Windows.Window.GetWindow(this);
			if (wpfwindow == null)
				throw new InvalidOperationException("Window not found!");

			var webview = new CustomWebView((WebView)this.WebView);
			e.WindowInfo.SetAsWindowless(new WindowInteropHelper(wpfwindow).Handle);
			e.Client = webview.Client;
			OnCreateWindow(webview);
		}


		protected void OnCreateWindow(WebView webview)
		{
			var tab = new WebViewTab(webview);
			TabControl tabs = this.FindTabControl();
			tabs.Items.Add(tab);
			tabs.SelectedItem = tab;
		}
	}
}
