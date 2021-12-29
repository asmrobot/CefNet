using CefNet;
using CefNet.Windows.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace WinFormsCoreApp
{
	public partial class DeviceEmulatorForm : Form
	{
		private WebView view;

		public DeviceEmulatorForm()
		{
			InitializeComponent();

			view = new CustomWebView()
			{
				RequestContext = new CefRequestContext(new CefRequestContextSettings()),
				WindowlessRenderingEnabled = true,
				InitialUrl = "https://www.mydevice.io/"
			};
			view.SimulateDevice(IPhoneDevice.Create(IPhone.Model7));
			view.Top = txtAddress.Bottom + 2;
			view.Left = 0;
			view.Width = ClientSize.Width;
			view.Height = ClientSize.Height - view.Top;
			view.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;
			view.Navigated += View_Navigated;
			view.CreateWindow += View_CreateWindow;
			view.BrowserCreated += View_BrowserCreated;
			this.Controls.Add(view);
		}

		protected override void OnResizeBegin(EventArgs e)
		{
			this.SuspendLayout();
			base.OnResizeBegin(e);
		}

		protected override void OnResizeEnd(EventArgs e)
		{
			this.ResumeLayout(true);
			base.OnResizeEnd(e);
		}

		private async void View_BrowserCreated(object sender, EventArgs e)
		{
			await view.SetUserAgentOverrideAsync("Mozilla/5.0 (iPhone; CPU iPhone OS 12_2 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Mobile/15E148 Safari/604.1", null, "iPhone", CancellationToken.None);
			await view.SetTouchEmulationEnabledAsync(true, null, CancellationToken.None);
			await view.SetEmulatedMediaAsync(null, new Dictionary<string, string> { ["any-pointer"] = "coarse" }, CancellationToken.None);
		}

		private void View_CreateWindow(object sender, CefNet.CreateWindowEventArgs e)
		{
			e.Cancel = true;
		}

		private void View_Navigated(object sender, CefNet.NavigatedEventArgs e)
		{
			txtAddress.Text = e.Url;
			txtAddress.Select(txtAddress.Text.Length, 0);
		}

		private void txtAddress_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
			{
				if (Uri.TryCreate(txtAddress.Text, UriKind.Absolute, out Uri url))
				{
					view.Navigate(url.AbsoluteUri);
				}
			}
		}

		protected override void OnClosing(CancelEventArgs e)
		{
			view.Dispose();
			base.OnClosing(e);
		}

	}
}
