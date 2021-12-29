using CefNet.Internal;
using CefNet.WinApi;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace CefNet.Windows.Forms
{
	public partial class WebView : Control
	{
		private object SyncRoot;

		private IntPtr browserWindowHandle;
		private ContextMenuStrip _cefmenu;

		private EventHandler<ITextFoundEventArgs> TextFoundEvent;
		private EventHandler<IPdfPrintFinishedEventArgs> PdfPrintFinishedEvent;
		private EventHandler<EventArgs> StatusTextChangedEvent;
		private EventHandler<IScriptDialogOpeningEventArgs> ScriptDialogOpeningEvent;

		public WebView()
			: this((WebView)null)
		{

		}

		public WebView(WebView opener)
		{
			SyncRoot = new Dictionary<InitialPropertyKeys, object>();

			if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
			{
				BackColor = System.Drawing.Color.White;
				return;
			}
			if (opener != null)
			{
				this.Opener = opener;
				this.WindowlessRenderingEnabled = opener.WindowlessRenderingEnabled;
				this.BrowserSettings = opener.BrowserSettings;
				SimulateDevice(opener.Device);
			}
			Initialize();
		}

		protected override void Dispose(bool disposing)
		{
			lock (SyncRoot)
			{
				ToolTip?.Dispose();	
				base.Dispose(disposing);
			}
		}

		protected SynchronizationContext UIContext { get; private set; }

		protected void VerifyAccess()
		{
			if (InvokeRequired)
				throw new InvalidOperationException("Cross-thread operation not valid. The WebView accessed from a thread other than the thread it was created on.");
		}

		protected override Size DefaultMinimumSize
		{
			get { return new Size(1, 1); }
		}

		protected override void CreateHandle()
		{
			if (GetState(State.Created))
				throw new ObjectDisposedException(GetType().Name);

			UIContext = SynchronizationContext.Current;
			base.CreateHandle();

			if (this.DesignMode)
				return;

			OnCreateBrowser();
		}

		protected override void DestroyHandle()
		{
			if (GetState(State.Created) && !GetState(State.Closing))
			{
				OnDestroyBrowser();
			}
			base.DestroyHandle();
		}

		private void SetInitProperty(InitialPropertyKeys key, object value)
		{
			var propertyBag = SyncRoot as Dictionary<InitialPropertyKeys, object>;
			if (propertyBag != null)
			{
				propertyBag[key] = value;
			}
			else
			{
				throw new InvalidOperationException("This property must be set before the underlying CEF browser is created.");
			}
		}

		private T GetInitProperty<T>(InitialPropertyKeys key)
		{
			var propertyBag = SyncRoot as Dictionary<InitialPropertyKeys, object>;
			if (propertyBag != null && propertyBag.TryGetValue(key, out object value))
			{
				return (T)value;
			}
			return default;
		}

		protected virtual void OnCreateBrowser()
		{
			if (this.Opener != null)
				return;

			if (GetState(State.Creating) || GetState(State.Created))
				throw new InvalidOperationException();

			var propertyBag = SyncRoot as Dictionary<InitialPropertyKeys, object>;
			SetState(State.Creating, true);
			SyncRoot = new object();

			using (var windowInfo = new CefWindowInfo())
			{
				if (WindowlessRenderingEnabled)
				{
					windowInfo.SetAsWindowless(Handle);
				}
				else
				{
					// In order to avoid focus issues when creating browsers offscreen,
					// the browser must be created with a disabled child window.
					windowInfo.SetAsDisabledChild(Handle);
				}

				string initialUrl = null;
				CefDictionaryValue extraInfo = null;
				CefRequestContext requestContext = null;
				CefBrowserSettings browserSettings = null;
				if (propertyBag != null)
				{
					object value;
					if (propertyBag.TryGetValue(InitialPropertyKeys.Url, out value))
						initialUrl = value as string;
					if (propertyBag.TryGetValue(InitialPropertyKeys.BrowserSettings, out value))
						browserSettings = value as CefBrowserSettings;
					if (propertyBag.TryGetValue(InitialPropertyKeys.RequestContext, out value))
						requestContext = value as CefRequestContext;
					if (propertyBag.TryGetValue(InitialPropertyKeys.ExtraInfo, out value))
						extraInfo = value as CefDictionaryValue;
				}

				if (initialUrl == null)
					initialUrl = "about:blank";
				if (browserSettings == null)
					browserSettings = DefaultBrowserSettings;

				if (!CefApi.CreateBrowser(windowInfo, ViewGlue.Client, initialUrl, browserSettings, extraInfo, requestContext))
					throw new InvalidOperationException("Failed to create browser instance.");
			}
		}

		protected virtual void OnDestroyBrowser()
		{
			if (GetState(State.Created) && !GetState(State.Closing))
			{
				SetState(State.Closing, true);
				if (!this.DesignMode)
				{
					ViewGlue.BrowserObject?.Host.CloseBrowser(true);
				}
			}
		}

		protected virtual void Initialize()
		{
			SetStyle(ControlStyles.ContainerControl
				| ControlStyles.ResizeRedraw
				| ControlStyles.FixedWidth
				| ControlStyles.FixedHeight
				| ControlStyles.StandardClick
				| ControlStyles.StandardDoubleClick
				| ControlStyles.SupportsTransparentBackColor
				| ControlStyles.EnableNotifyMessage
				| ControlStyles.DoubleBuffer
				| ControlStyles.OptimizedDoubleBuffer
				| ControlStyles.UseTextForAccessibility
				| ControlStyles.Opaque
				| ControlStyles.CacheText
				| ControlStyles.Selectable
				, false);

			SetStyle(ControlStyles.UserPaint
				| ControlStyles.ResizeRedraw
				| ControlStyles.AllPaintingInWmPaint
				| ControlStyles.Opaque
				| ControlStyles.Selectable
				, true);

			SetStyle(ControlStyles.UserMouse, WindowlessRenderingEnabled);

			this.BackColor = Color.White;
			this.AllowDrop = true;

			this.ViewGlue = CreateWebViewGlue();
			this.ToolTip = new ToolTip { ShowAlways = true };
		}
		
		protected virtual WebViewGlue CreateWebViewGlue()
		{
			return new WinFormsWebViewGlue(this);
		}

		protected void SetDevicePixelRatio(float ratio)
		{
			if (OffscreenGraphics.PixelsPerDip != ratio)
			{
				OffscreenGraphics.PixelsPerDip = ratio;
				RaiseCrossThreadEvent(OnSizeChanged, EventArgs.Empty, false);
			}
		}

		/// <summary>
		/// Gets or sets the tool-tip object that is displayed for this element in the user interface (UI).
		/// </summary>
		public ToolTip ToolTip { get; set; }

		[Browsable(false)]
		public string StatusText { get; protected set; }

		protected virtual void RaiseCrossThreadEvent<TEventArgs>(Action<TEventArgs> raiseEvent, TEventArgs e, bool synchronous)
			where TEventArgs : class
		{
			if (UIContext != null)
			{
				if (synchronous)
					UIContext.Send(new CrossThreadEventMethod<TEventArgs>(raiseEvent, e).Invoke, this);
				else
					UIContext.Post(new CrossThreadEventMethod<TEventArgs>(raiseEvent, e).Invoke, this);
			}
			else
			{
				if (synchronous)
					this.Invoke(new CrossThreadEventMethod<TEventArgs>(raiseEvent, e).Invoke, this);
				else
					this.BeginInvoke(new CrossThreadEventMethod<TEventArgs>(raiseEvent, e).Invoke, this);
			}
		}

		private void AddHandler<TEventHadler>(in TEventHadler eventKey, TEventHadler handler)
			where TEventHadler : Delegate
		{
			TEventHadler current;
			TEventHadler key = eventKey;
			do
			{
				current = key;
				key = CefNetApi.CompareExchange<TEventHadler>(in eventKey, (TEventHadler)Delegate.Combine(current, handler), current);
			}
			while (key != current);
		}

		private void RemoveHandler<TEventHadler>(in TEventHadler eventKey, TEventHadler handler)
			where TEventHadler : Delegate
		{
			TEventHadler current;
			TEventHadler key = eventKey;
			do
			{
				current = key;
				key = CefNetApi.CompareExchange<TEventHadler>(in eventKey, (TEventHadler)Delegate.Remove(current, handler), current);
			}
			while (key != current);
		}

		protected virtual void OnBrowserCreated(EventArgs e)
		{
			if (SyncRoot.GetType() != typeof(object))
			{
#if DEBUG
				throw new InvalidOperationException("WTF?");
#endif
				SyncRoot = new object(); // ugly huck
			}
			browserWindowHandle = BrowserObject.Host.WindowHandle;
			BrowserCreated?.Invoke(this, e);
			ResizeBrowserWindow();
		}

		protected override void OnGotFocus(System.EventArgs e)
		{
			BrowserObject?.Host.SetFocus(true);
			base.OnGotFocus(e);
		}

		protected override void OnLostFocus(EventArgs e)
		{
			BrowserObject?.Host.SetFocus(false);
			base.OnLostFocus(e);
		}

		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			ResizeBrowserWindow();
		}

		protected override void OnVisibleChanged(EventArgs e)
		{
			base.OnVisibleChanged(e);

			if (!WindowlessRenderingEnabled)
			{
				ResizeBrowserWindow();
				if (Visible) Invalidate();
			}
		}

		/// <inheritdoc />
		public CefRect GetBounds()
		{
			OffscreenGraphics offscreenGraphics = this.OffscreenGraphics;
			if (offscreenGraphics is null)
			{
				var r = this.DisplayRectangle;
				return new CefRect(r.X, r.Y, r.Width, r.Height);
			}
			return offscreenGraphics.GetBounds();
		}

		internal void ResizeBrowserWindow()
		{
			const uint SWP_NOSIZE = 0x0001;
			const uint SWP_NOMOVE = 0x0002;
			const uint SWP_NOZORDER = 0x0004;
			const uint SWP_SHOWWINDOW = 0x0040;
			const uint SWP_HIDEWINDOW = 0x0080;
			const uint SWP_NOCOPYBITS = 0x0100;
			const uint SWP_ASYNCWINDOWPOS = 0x4000;
			const int GWL_STYLE = -16;

			if (browserWindowHandle == IntPtr.Zero)
				return;

			if (!IsHandleCreated)
				return;

			WindowStyle style = (WindowStyle)NativeMethods.GetWindowLong(Handle, GWL_STYLE);
			if (WindowlessRenderingEnabled)
			{
				NativeMethods.SetWindowLong(Handle, GWL_STYLE, new IntPtr((int)(style | WindowStyle.WS_VSCROLL | WindowStyle.WS_HSCROLL)));
				return;
			}

			Size size = this.ClientSize;
			if (Visible && size.Height > 0 && size.Width > 0)
			{
				style &= ~WindowStyle.WS_DISABLED;
				NativeMethods.SetWindowLong(browserWindowHandle, GWL_STYLE, new IntPtr((int)(style | WindowStyle.WS_TABSTOP | WindowStyle.WS_VISIBLE)));
				NativeMethods.SetWindowPos(browserWindowHandle, IntPtr.Zero, 0, 0, size.Width, size.Height, SWP_NOZORDER | SWP_SHOWWINDOW | SWP_NOCOPYBITS | SWP_ASYNCWINDOWPOS);
			}
			else
			{
				NativeMethods.SetWindowLong(browserWindowHandle, GWL_STYLE, new IntPtr((int)(style | WindowStyle.WS_DISABLED)));
				NativeMethods.SetWindowPos(browserWindowHandle, IntPtr.Zero, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_HIDEWINDOW | SWP_ASYNCWINDOWPOS);
			}
		}
		
		void IChromiumWebViewPrivate.RaisePopupBrowserCreating()
		{
			SetState(State.Creating, true);
			SyncRoot = new object();
		}

		bool IChromiumWebViewPrivate.RaiseRunContextMenu(CefFrame frame, CefContextMenuParams menuParams, CefMenuModel model, CefRunContextMenuCallback callback)
		{
			if (model.Count == 0
#if OBSOLETED_CONTROLS_3_1
				|| ContextMenu != null
#endif
				|| ContextMenuStrip != null)
			{
				callback.Cancel();
				return true;
			}
			var menuRunner = new WinFormsContextMenuRunner(menuParams, model, callback);
			menuRunner.Build();
			_cefmenu = menuRunner.Menu;
			_cefmenu.Closed += HandleMenuCefMenuClosed;
			var location = new CefRect(menuParams.XCoord, menuParams.YCoord, 0, 0);
			VirtualDevice device = Device;
			if (device != null)
			{
				device.ScaleToViewport(ref location, OffscreenGraphics.PixelsPerDip);
				device.MoveToDevice(ref location, OffscreenGraphics.PixelsPerDip);
			}
			else
			{
				location.Scale(OffscreenGraphics.PixelsPerDip);
			}
			var ea = new ContextMenuEventArgs(menuRunner.Menu, new Point(location.X, location.Y));
			RaiseCrossThreadEvent(OnShowContextMenu, ea, true);
			return ea.Handled;
		}

		private void HandleMenuCefMenuClosed(object sender, ToolStripDropDownClosedEventArgs e)
		{
			ContextMenuStrip menu = _cefmenu;
			if (menu != null)
			{
				_cefmenu = null;
				menu.Closed -= HandleMenuCefMenuClosed;
			}
		}

		private void DismissContextMenu()
		{
			ContextMenuStrip menu = _cefmenu;
			if (menu != null)
			{
				OnHideContextMenu(new ContextMenuEventArgs(menu, new Point()));
			}
		}

		protected virtual void OnShowContextMenu(ContextMenuEventArgs e)
		{
			e.Handled = true;
			e.ContextMenu.Show(this, e.Location);
		}

		protected virtual void OnHideContextMenu(ContextMenuEventArgs e)
		{
			e.ContextMenu.Close();
		}

		protected virtual void OnLoadingStateChange(LoadingStateChangeEventArgs e)
		{
			LoadingStateChange?.Invoke(this, e);
		}

		void IWinFormsWebViewPrivate.CefSetStatusText(string statusText)
		{
			this.StatusText = statusText;
			RaiseCrossThreadEvent(OnStatusTextChanged, EventArgs.Empty, false);
		}

		protected override void WndProc(ref Message m)
		{
			if (WindowlessRenderingEnabled)
			{
				if (ProcessWindowlessMessage(ref m))
					return;
			}
			else if(m.Msg == 0x0210) // WM_PARENTNOTIFY
			{
				DismissContextMenu();
			}

			base.WndProc(ref m);
		}

	}
}
