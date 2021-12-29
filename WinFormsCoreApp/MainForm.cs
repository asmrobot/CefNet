using CefNet;
using CefNet.JSInterop;
using CefNet.Net;
using CefNet.Unsafe;
using CefNet.Windows.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsCoreApp
{
	public partial class MainForm : Form
	{
		private MenuStrip menu;
		private WebViewTabControl tabs;
		private TextBox txtAddress;
		private Button btnBack;
		private Button btnForward;
		private Button btnGo;

		public MainForm()
		{
			InitializeComponent();
			InitalizeComponent2();

			//	this.DoubleBuffered = true;
			//tabs.ItemSize = new Size(120, tabs.ItemSize.Height);
			AddTab(true);

		}

		private void InitalizeComponent2()
		{
			ToolStripMenuItem submenu;

			submenu = new ToolStripMenuItem("File");
			submenu.DropDownItems.AddRange(new ToolStripItem[] {
				new ToolStripMenuItem("Add Tab", null, HandleAddTab) { Tag = true },
				new ToolStripMenuItem("Add Tab (new context)", null, HandleAddTab) { Tag = false },
				new ToolStripMenuItem("Show Device Simulator", null, HandleShowSimulator),
				new ToolStripMenuItem("Print to PDF", null, HandlePrintToPdf),
				new ToolStripMenuItem("Load from String", null, HandleLoadFromString) { Tag = true },
				new ToolStripMenuItem("Load from Stream", null, HandleLoadFromString) { Tag = false },
				new ToolStripMenuItem("Load to file", null, HandleLoadToFile),
				new ToolStripMenuItem("Capture screenshot", null, HandleCaptureScreenshot),
				new ToolStripMenuItem("Test2", null, Button2_Click),
				new ToolStripMenuItem("Main Process", null, new ToolStripItem[] {
					new ToolStripMenuItem("Test ScriptableObject", null, async (s,e) => await ScriptableObjectTests.ScriptableObjectTestAsync(SelectedView.GetMainFrame())),
					new ToolStripMenuItem("Call GC.Collect()", null, (s,e) => GC.Collect()),
				}),
				new ToolStripMenuItem("Renderer process", null, new ToolStripItem[] {
					new ToolStripMenuItem("Test ScriptableObject", null, (s,e) => ScriptableObjectTests.SendTestScriptableObjectToRenderer(SelectedView.GetMainFrame())),
					new ToolStripMenuItem("Call GC.Collect()", null, (s,e) => ScriptableObjectTests.SendGCCollectToRenderer(SelectedView.GetMainFrame())),
				})
			});

			menu = new MenuStrip();
			menu.Items.Add(submenu);
			this.Controls.Add(menu);

			btnBack = new Button();
			btnBack.Text = "<";
			btnBack.UseVisualStyleBackColor = true;
			btnBack.Top = menu.Bottom;
			btnBack.Width = btnBack.Height;
			btnBack.Anchor = AnchorStyles.Left | AnchorStyles.Top;
			btnBack.Click += (s, e) => { SelectedView?.GoBack(); };
			this.Controls.Add(btnBack);

			btnForward = new Button();
			btnForward.Text = ">";
			btnForward.UseVisualStyleBackColor = true;
			btnForward.Left = btnBack.Right;
			btnForward.Top = menu.Bottom;
			btnForward.Width = btnForward.Height;
			btnForward.Anchor = AnchorStyles.Left | AnchorStyles.Top;
			btnForward.Click += (s, e) => { SelectedView?.GoForward(); };
			this.Controls.Add(btnForward);

			btnGo = new Button();
			btnGo.Text = "Go";
			btnGo.UseVisualStyleBackColor = true;
			btnGo.Left = btnForward.Right;
			btnGo.Top = menu.Bottom;
			btnGo.Width = btnGo.Height * 2;
			btnGo.Anchor = AnchorStyles.Left | AnchorStyles.Top;
			btnGo.Click += new System.EventHandler(this.BtnGo_Click);
			this.Controls.Add(btnGo);


			txtAddress = new TextBox();
			txtAddress.KeyDown += HandleAddressKeyDown;
			txtAddress.Top = menu.Bottom;
			txtAddress.Left = btnGo.Right;
			txtAddress.Width = ClientSize.Width - txtAddress.Left;
			txtAddress.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;
			this.Controls.Add(txtAddress);

			tabs = new WebViewTabControl();
			tabs.Top = txtAddress.Bottom;
			tabs.Width = ClientSize.Width;
			tabs.Height = ClientSize.Height - tabs.Top;
			tabs.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;
			tabs.ControlAdded += Tabs_ControlAdded;
			tabs.ControlRemoved += Tabs_ControlRemoved;
			tabs.SelectedIndexChanged += Tabs_SelectedIndexChanged;
			this.Controls.Add(tabs);
		}

		private void HandlePrintToPdf(object sender, EventArgs e)
		{
			using (var dialog = new SaveFileDialog())
			{
				var settings = new CefPdfPrintSettings
				{
					HeaderFooterUrl = SelectedView.GetMainFrame().Url
				};
				try
				{
					dialog.Filter = "PDF file|*.pdf";
					if (dialog.ShowDialog() == DialogResult.OK)
					{
						SelectedView.PrintToPdf(dialog.FileName, settings);
					}
				}
				finally
				{
					settings.Dispose();
				}
			}
		}

		private void HandleLoadFromString(object sender, EventArgs e)
		{
			if (!CefCommandLine.Global.HasSwitch("disable-site-isolation-trials"))
			{
				// info:
				// https://magpcss.org/ceforum/viewtopic.php?f=6&t=17176&p=43706
				// https://bitbucket.org/chromiumembedded/cef/issues/2586
				MessageBox.Show("This test only works with --disable-site-isolation-trials.");
				return;
			}

			var view = SelectedView as CustomWebView;
			if (view is null)
				return;

			Guid sourceKey = Guid.NewGuid();

			if (sender is ToolStripMenuItem m && false.Equals(m.Tag))
				view.AddSource(sourceKey, new StreamSource(new MemoryStream(Encoding.UTF8.GetBytes("Hello, world (stream)!")), "text/html", "utf-8", false));
			else
				view.AddSource(sourceKey, new StringSource("Hello, world (string)!", "text/html"));

			var request = new CefRequest();
			request.Url = "http://example.com";
			request.SetReferrer("https://www.google.com/", CefReferrerPolicy.NeverClearReferrer);
			request.SetHeaderByName("CefNet-Source", sourceKey.ToString(), false); // see CustomWebViewGlue.GetResourceHandler()
			SelectedView?.GetMainFrame().LoadRequest(request);
		}

		private async void HandleLoadToFile(object sender, EventArgs e)
		{
			CefUrlRequestStatus status = CefUrlRequestStatus.Unknown;
			string url = "https://speed.hetzner.de/100MB.bin";
			using (var dlg = new SaveFileDialog())
			{
				dlg.FileName = Path.GetFileName(url);
				dlg.Filter = "All Files (*.*)|*.*";
				if (dlg.ShowDialog() == DialogResult.OK)
				{
					var r = new CustomWebRequest();
					r.IgnoreSize = MessageBox.Show("Download 100MB file?", "Download file...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
					try
					{
						await r.DownloadFileAsync(new CefRequest { Url = url }, null, dlg.FileName, CancellationToken.None);
					}
					catch(Exception ex)
					{
						Program.ShowUnhandledException(ex, "MainForm");
					}
					finally
					{
						status = r.RequestStatus;
					}
				}
			}
			MessageBox.Show($"Complete ({status})!");
		}

		private async void HandleCaptureScreenshot(object sender, EventArgs e)
		{
			using (var dlg = new SaveFileDialog())
			{
				dlg.Filter = "Image (*.png)|*.png";
				if (dlg.ShowDialog() == DialogResult.OK)
				{
					using (var f = File.Open(dlg.FileName, FileMode.Create, FileAccess.Write))
					{
						await SelectedView.CaptureScreenshotAsync(null, f, CancellationToken.None);
					}
				}
			}
		}

		private void HandleShowSimulator(object sender, EventArgs e)
		{
			var f = new DeviceEmulatorForm();
			f.Show();
		}

		private void HandleAddTab(object sender, EventArgs e)
		{
			AddTab((sender as ToolStripItem)?.Tag as bool? ?? true);
		}

		private void AddTab(bool useGlobalContext)
		{
			WebViewTab viewTab;
			if (useGlobalContext)
			{
				viewTab = new WebViewTab();
			}
			else
			{
				viewTab = new WebViewTab(new CefBrowserSettings(), new CefRequestContext(new CefRequestContextSettings()));
			}
			((CustomWebView)viewTab.WebView).WindowlessRenderingEnabled = true;
			tabs.Controls.Add(viewTab);
		}

		private void WebView_Navigated(object sender, NavigatedEventArgs e)
		{
			Control control = sender as Control;
			while (control != null)
			{
				if (control is TabPage tab)
				{
					if (tabs.TabCount > 0 && tabs.SelectedTab == tab)
					{
						txtAddress.Text = e.Url;
					}
					return;
				}
				else
				{
					control = control.Parent;
				}
			}
			
		}

		private IChromiumWebView SelectedView
		{
			get
			{
				if (tabs == null || tabs.TabCount == 0)
					return null;
				return (tabs.SelectedTab as IWebViewTab)?.WebView;
			}
		}

		protected override void OnResizeBegin(EventArgs e)
		{
			tabs?.NotifyRootMovedOrResized();
			this.SuspendLayout();
			base.OnResizeBegin(e);
		}

		protected override void OnResizeEnd(EventArgs e)
		{
			this.ResumeLayout(true);
			base.OnResizeEnd(e);
			tabs?.NotifyRootMovedOrResized();
		}

		protected override void OnLocationChanged(EventArgs e)
		{
			base.OnLocationChanged(e);
			//((WebView)SelectedView)?.NotifyRootMovedOrResized();
		}

		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged(e);
			tabs?.NotifyRootMovedOrResized();
		}

		private void BtnGo_Click(object sender, EventArgs e)
		{
			if (this.SelectedView == null)
				return;

			SelectedView.Navigate("https://cefnet.github.io/winsize.html");
			//SelectedView.Navigate("http://internet.yandex.ru");
			//SelectedView.Navigate("http://cefnet.github.io/crossiframe.html");
			//SelectedView.Navigate("http://cefnet.github.io/iframepage.html");
			//SelectedView.Navigate("http://cefnet.github.io/openpage.html", "http://example.com");
			//SelectedView.Navigate("http://yandex.ru/");

		}

		private void Tabs_ControlAdded(object sender, ControlEventArgs e)
		{
			var tab = e.Control as WebViewTab;
			if (tab != null)
			{
				tab.WebView.Navigated += WebView_Navigated;
				tab.WebView.PdfPrintFinished += WebView_PdfPrintFinished;
				tabs.SelectTab(tab);
			}
		}

		private void Tabs_ControlRemoved(object sender, ControlEventArgs e)
		{
			var tab = e.Control as WebViewTab;
			if (tab != null)
			{
				tab.WebView.Navigated -= WebView_Navigated;
				tab.WebView.PdfPrintFinished -= WebView_PdfPrintFinished;
			}
		}

		private void WebView_PdfPrintFinished(object sender, IPdfPrintFinishedEventArgs e)
		{
			MessageBox.Show(e.Success ? $"Success ({e.Path})." : "Error.", "PDF print", MessageBoxButtons.OK, e.Success ? MessageBoxIcon.Information : MessageBoxIcon.Error);
		}

		private void Tabs_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (tabs.SelectedIndex == -1)
				txtAddress.Text = string.Empty;
			else
				txtAddress.Text = SelectedView?.GetMainFrame()?.Url;
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

		protected override void OnClosing(CancelEventArgs e)
		{
			base.OnClosing(e);
			if(!e.Cancel)
			{
				e.Cancel = !tabs.CloseAllTabs(false);
			}
		}

		CefFrame frame1;
		CefFrame frame2;

		private async void Button2_Click(object sender, EventArgs e)
		{
			WebView view = ((WebView)SelectedView);

			if (frame1 is null)
			{
				frame1 = view.GetMainFrame();
				MessageBox.Show(string.Format("f1: {0}", frame1.Identifier));
			}
			else
			{
				frame2 = view.GetMainFrame();
				MessageBox.Show(string.Format("(f1 = f2): {0}, (f1 eq f2): {1}, f1: {2}, f2: {3}", frame1 == frame2, ReferenceEquals(frame1, frame2), frame1.Identifier, frame2.Identifier));
			}
			//view.BrowserObject.Host.ZoomLevel = 0;
			//view.BrowserObject.Host.WasResized();

			//var framesIds = SelectedView.BrowserObject.FrameIdentifiers;
			//var frame = SelectedView.BrowserObject.GetFrame(framesIds[0]);
			//frame.ExecuteJavaScript("window.addEventListener('message', function(msg){alert('message from ' + origin);}, false);", frame.Url, 0);


			//SelectedView.BrowserObject.MainFrame.LoadString("hellow world", "http://hello.world");
			//SelectedView.SetUserAgentOverride("Mozilla/5.0 (Windows 10.0) CustomAgent/1.0");
			//SelectedView.LoadContent("http://hello.world", "http://example.com", "text/html", "Hello, World!", Encoding.UTF8);
		
		}

		protected override void WndProc(ref Message m)
		{
			const int WM_ENTERMENULOOP = 0x0211;
			const int WM_EXITMENULOOP = 0x0212;

			switch (m.Msg)
			{
				case WM_ENTERMENULOOP:
					if (m.WParam == IntPtr.Zero)
						CefApi.SetOSModalLoop(true);
					break;
				case WM_EXITMENULOOP:
					if (m.WParam == IntPtr.Zero)
						CefApi.SetOSModalLoop(false);
					break;
			}
			base.WndProc(ref m);
		}

	}
}
