using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using CefNet;
using CefNet.Modern.Forms;
using Modern.Forms;

namespace ModernFormsApp
{
	public class MainForm : Form
	{
		private readonly Button btnGo;
		private readonly Button btnBack;
		private readonly Button btnForward;
		private readonly Menu menu;
		private readonly TextBox txtAddress;
		private readonly TabControl tabs;

		public MainForm()
		{
			MenuItem submenu;

			submenu = new MenuItem("File");
			submenu.Items.Add("Add Tab", null, HandleAddTab);
			submenu.Items.Add("Call GC.Collect()", null, (s, e) => GC.Collect());

			menu = new Menu();
			menu.Items.Add(submenu);
			this.Controls.Add(menu);


			btnBack = new Button();
			btnBack.Text = "<";
			btnBack.Top = menu.Bottom;
			btnBack.Width = btnBack.Height;
			btnBack.Anchor = AnchorStyles.Left | AnchorStyles.Top;
			this.Controls.Add(btnBack);

			btnForward = new Button();
			btnForward.Text = ">";
			btnForward.Left = btnBack.Right;
			btnForward.Top = menu.Bottom;
			btnForward.Width = btnForward.Height;
			btnForward.Anchor = AnchorStyles.Left | AnchorStyles.Top;
			this.Controls.Add(btnForward);

			btnGo = new Button();
			btnGo.Text = "Go";
			btnGo.Left = btnForward.Right;
			btnGo.Top = menu.Bottom;
			btnGo.Width = btnGo.Height * 2;
			btnGo.Anchor = AnchorStyles.Left | AnchorStyles.Top;
			btnGo.Click += BtnGo_Click;
			this.Controls.Add(btnGo);

			txtAddress = new TextBox();
			txtAddress.KeyDown += HandleAddressKeyDown;
			txtAddress.Top = menu.Bottom;
			txtAddress.Left = btnGo.Right;
			txtAddress.Width = ScaledSize.Width - txtAddress.ScaledLeft;
			txtAddress.Height = btnGo.Height;
			txtAddress.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;
			this.Controls.Add(txtAddress);

			tabs = new TabControl();
			tabs.SetBounds(0, txtAddress.Bottom, this.ScaledSize.Width, this.ScaledSize.Height - tabs.ScaledTop);
			tabs.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;
			//tabs.ControlAdded += Tabs_ControlAdded;
			//tabs.ControlRemoved += Tabs_ControlRemoved;
			tabs.SelectedIndexChanged += Tabs_SelectedIndexChanged;
			this.Controls.Add(tabs);

			this.Style.BackgroundColor = SkiaSharp.SKColors.Red;
			this.Text = nameof(MainForm);
		}

		private void HandleAddTab(object sender, EventArgs e)
		{
			AddTab(true);
		}

		private void AddTab(bool useGlobalContext)
		{
			var viewTab = new WebViewTab();
			tabs.TabPages.Add(viewTab);
		}

		private IChromiumWebView SelectedView
		{
			get
			{
				if (tabs == null || tabs.TabPages.Count == 0)
					return null;
				return (tabs.SelectedTabPage as IWebViewTab)?.WebView;
			}
		}

		private void BtnGo_Click(object sender, EventArgs e)
		{
			if (this.SelectedView == null)
				return;

			SelectedView.Navigate("https://cefnet.github.io/winsize.html");
		}

		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);

			btnBack.Top = menu.Bottom;
			btnForward.Top = menu.Bottom;
			btnGo.Top = menu.Bottom;
			txtAddress.SetBounds(txtAddress.Left, menu.Bottom, this.ScaledSize.Width - txtAddress.ScaledLeft, txtAddress.Height);
			tabs.SetBounds(tabs.Left, btnGo.Bottom, this.ScaledSize.Width, this.ScaledSize.Height - btnGo.ScaledBounds.Bottom);

			AddTab(true);
		}

		private void Tabs_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (tabs.SelectedIndex == -1)
				txtAddress.Text = string.Empty;
			else
				txtAddress.Text = SelectedView?.GetMainFrame()?.Url ?? "about:blank";
		}

		private void HandleAddressKeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
			{
				if (Uri.TryCreate(txtAddress.Text, UriKind.Absolute, out Uri url))
				{
					SelectedView?.Navigate(url.AbsoluteUri);
				}
			}
		}


	}
}
