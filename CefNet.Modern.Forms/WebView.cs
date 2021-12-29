using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Threading;
using CefNet.Input;
using CefNet.Internal;
using CefNet.WinApi;
using Modern.Forms;
using SkiaSharp;

namespace CefNet.Modern.Forms
{
	public partial class WebView : Control, IModernFormsWebViewPrivate
	{
		private object SyncRoot;
		private IntPtr _keyboardLayout;

		private EventHandler<ITextFoundEventArgs> TextFoundEvent;
		private EventHandler<IPdfPrintFinishedEventArgs> PdfPrintFinishedEvent;
		private EventHandler<EventArgs> StatusTextChangedEvent;
		private EventHandler<IScriptDialogOpeningEventArgs> ScriptDialogOpeningEvent;

		/// <summary>
		/// Occurs when the user starts dragging content in the web view.
		/// </summary>
		/// <remarks>
		/// OS APIs that run a system message loop may be used within the StartDragging event handler.
		/// Call <see cref="WebView.DragSourceEndedAt"/> and <see cref="WebView.DragSourceSystemDragEnded"/>
		/// either synchronously or asynchronously to inform the web view that the drag operation has ended.
		/// </remarks>
		public event EventHandler<StartDraggingEventArgs> StartDragging;

		public WebView()
			: this((WebView)null)
		{

		}

		public WebView(WebView opener)
		{
			SyncRoot = new Dictionary<InitialPropertyKeys, object>();

			if (opener != null)
			{
				this.Opener = opener;
				this.BrowserSettings = opener.BrowserSettings;
			}
			Initialize();
		}

		/// <inheritdoc/>
		protected override void Dispose(bool disposing)
		{
			OnDestroyBrowser();
			base.Dispose(disposing);
		}

		private IntPtr Handle
		{
			get { return IntPtr.Zero; }
		}

		protected void VerifyAccess()
		{
			if (!CefNetApplication.Instance.CheckAccess())
				throw new InvalidOperationException("Cross-thread operation not valid. The WebView accessed from a thread other than the thread it was created on.");
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

		/// <summary>
		/// Creates new CEF browser.
		/// </summary>
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
				
				windowInfo.SetAsWindowless(Handle);

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
				ViewGlue.BrowserObject?.Host.CloseBrowser(true);
			}
		}

		protected virtual void Initialize()
		{
			SetControlBehavior(ControlBehaviors.Selectable);

			//this.AllowDrop = true;

			this.ViewGlue = CreateWebViewGlue();
			OffscreenGraphics = new OffscreenGraphics();
			this.Style.BackgroundColor = SkiaSharp.SKColors.White;
			//this.ToolTip = new ToolTip { ShowAlways = true };
		}

		protected virtual WebViewGlue CreateWebViewGlue()
		{
			return new ModernFormsWebViewGlue(this);
		}

		protected void SetDevicePixelRatio(float ratio)
		{
			if (OffscreenGraphics.PixelsPerDip != ratio)
			{
				OffscreenGraphics.PixelsPerDip = ratio;
				RaiseCrossThreadEvent(OnSizeChanged, EventArgs.Empty, false);
			}
		}

		[Browsable(false)]
		public string StatusText { get; protected set; }

		/// <summary>
		/// Gets the rectangle that represents the bounds of the WebView control.
		/// </summary>
		/// <returns>
		/// A <see cref="CefRect"/> representing the bounds within which the WebView control is scaled.
		/// </returns>
		public CefRect GetBounds()
		{
			return OffscreenGraphics.GetBounds();
		}

		/// <summary>
		/// Sets the bounds of the control to the specified location and size.
		/// </summary>
		/// <param name="x">The new <see cref="X"/> property value of the control.</param>
		/// <param name="y">The new <see cref="Y"/> property value of the control.</param>
		/// <param name="width">The new <see cref="Width"/> property value of the control.</param>
		/// <param name="height">The new <see cref="Height"/> property value of the control.</param>
		public void SetBounds(int x, int y, int width, int height)
		{
			if (width <= 0)
				throw new ArgumentOutOfRangeException(nameof(width));
			if (height <= 0)
				throw new ArgumentOutOfRangeException(nameof(height));
			base.SetBounds(x, y, width, height, BoundsSpecified.All);
		}

		/// <summary>
		/// Performs the desired delegate on the UI thread.
		/// </summary>
		/// <typeparam name="TEventArgs">The type of the event data generated by the event.</typeparam>
		/// <param name="raiseEvent">
		/// A delegate to a method that takes parameters specified in <paramref name="e"/>
		/// and contains a method to be called in the UI thread context.
		/// </param>
		/// <param name="e">An object that contains the event data.</param>
		/// <param name="synchronous">
		/// A value which indicates that the delegate should be called synchronously or asynchronously.
		/// </param>
		protected virtual void RaiseCrossThreadEvent<TEventArgs>(Action<TEventArgs> raiseEvent, TEventArgs e, bool synchronous)
			where TEventArgs : class
		{
			if (synchronous)
			{
				Invoke(() => raiseEvent(e));
			}
			else
			{
				Application.RunOnUIThread(() => raiseEvent(e));
			}
		}

		/// <summary>
		/// Executes the specified <see cref="Action"/> synchronously on the UI thread.
		/// </summary>
		/// <param name="action">A delegate to invoke on the UI thread.</param>
		protected void Invoke(Action action)
		{
			if (CefNetApplication.Instance.CheckAccess())
			{
				action();
			}
			else
			{
				using (var mev = new ManualResetEvent(false))
				{
					Application.RunOnUIThread(() =>
					{
						try
						{
							action();
						}
						finally
						{
							mev.Set();
						}
					});
					mev.WaitOne();
				}
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

		/// <inheritdoc/>
		protected override void OnLayout(LayoutEventArgs e)
		{
			OffscreenGraphics.WidgetHandle = IntPtr.Zero;
			float devicePixelRatio = this.DeviceDpi / 96f;
			if (OffscreenGraphics.PixelsPerDip != devicePixelRatio)
			{
				SetDevicePixelRatio(devicePixelRatio);
			}

			if (!GetState(State.Creating) && !GetState(State.Created))
			{
				OnCreateBrowser();
			}

			base.OnLayout(e);
		}

		/// <summary>
		/// Raises the <see cref="BrowserCreated"/> event.
		/// </summary>
		/// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
		protected virtual void OnBrowserCreated(EventArgs e)
		{
			if (SyncRoot.GetType() != typeof(object))
			{
#if DEBUG
				throw new InvalidOperationException("WTF?");
#endif
				SyncRoot = new object();
			}
			BrowserCreated?.Invoke(this, e);
		}

		void IChromiumWebViewPrivate.RaisePopupBrowserCreating()
		{
			SetState(State.Creating, true);
			SyncRoot = new object();
		}

		bool IChromiumWebViewPrivate.RaiseRunContextMenu(CefFrame frame, CefContextMenuParams menuParams, CefMenuModel model, CefRunContextMenuCallback callback)
		{
			if (model.Count == 0 || ContextMenu != null)
			{
				callback.Cancel();
				return true;
			}
			var menuRunner = new ModernFormsContextMenuRunner(menuParams, model, callback);
			menuRunner.Build();
			//_cefmenu = menuRunner.Menu;
			//_cefmenu.Closed += HandleMenuCefMenuClosed;
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

		protected virtual void OnShowContextMenu(ContextMenuEventArgs e)
		{
			e.Handled = true;
			e.ContextMenu.Show(PointToScreen(e.Location));
		}

		protected virtual void OnHideContextMenu(ContextMenuEventArgs e)
		{
			e.ContextMenu.Hide();
		}

		protected virtual void OnLoadingStateChange(LoadingStateChangeEventArgs e)
		{
			LoadingStateChange?.Invoke(this, e);
		}

		void IModernFormsWebViewPrivate.CefSetStatusText(string statusText)
		{
			this.StatusText = statusText;
			RaiseCrossThreadEvent(OnStatusTextChanged, EventArgs.Empty, false);
		}

		void IModernFormsWebViewPrivate.RaiseCefCursorChange(CursorChangeEventArgs e)
		{
			RaiseCrossThreadEvent(OnCursorChange, e, false);
		}

		protected virtual void OnCursorChange(CursorChangeEventArgs e)
		{
			this.Cursor = e.Cursor;
		}

		void IModernFormsWebViewPrivate.CefSetToolTip(string text)
		{
			RaiseCrossThreadEvent(OnSetToolTip, new TooltipEventArgs(text), false);
		}

		protected virtual void OnSetToolTip(TooltipEventArgs e)
		{
			//ToolTip toolTip = this.ToolTip;
			//if (toolTip != null && toolTip.GetToolTip(this) != e.Text)
			//{
			//	toolTip.SetToolTip(this, e.Text);
			//}
		}

		void IModernFormsWebViewPrivate.RaiseStartDragging(StartDraggingEventArgs e)
		{
			RaiseCrossThreadEvent(OnStartDragging, e, true);
		}

		/// <summary>
		/// Raises <see cref="WebView.StartDragging"/> event.
		/// </summary>
		/// <param name="e">The event data.</param>
		protected virtual void OnStartDragging(StartDraggingEventArgs e)
		{
			StartDragging?.Invoke(this, e);
			if (e.Handled)
				return;

			//DoDragDrop(new CefNetDragData(this, e.Data), e.AllowedEffects.ToDragDropEffects());
			//DragSourceSystemDragEnded();
			//e.Handled = true;
		}



		/// <summary>
		/// Gets emulated device.
		/// </summary>
		protected VirtualDevice Device
		{
			get { return OffscreenGraphics.Device; }
			private set { OffscreenGraphics.Device = value; }
		}

		/// <summary>
		/// Enable or disable device simulation.
		/// </summary>
		/// <param name="device">The simulated device or null.</param>
		public void SimulateDevice(VirtualDevice device)
		{
			if (Device == device)
				return;

			Device = device;
			OnSizeChanged(EventArgs.Empty);
		}

		protected CefRect GetViewportRect()
		{
			CefRect viewportRect;
			VirtualDevice device = Device;
			if (device == null)
			{
				viewportRect = new CefRect(0, 0, this.Width, this.Height);
				viewportRect.Scale(OffscreenGraphics.PixelsPerDip);
				return viewportRect;
			}
			else
			{
				return device.GetBounds(OffscreenGraphics.PixelsPerDip);
			}
		}

		/// <summary>
		/// Gets a graphics buffer for off-screen rendering.
		/// </summary>
		protected OffscreenGraphics OffscreenGraphics { get; private set; }

		bool IChromiumWebViewPrivate.GetCefScreenInfo(ref CefScreenInfo screenInfo)
		{
			return GetCefScreenInfo(ref screenInfo);
		}

		protected virtual bool GetCefScreenInfo(ref CefScreenInfo screenInfo)
		{
			VirtualDevice device = Device;
			if (device == null)
				return false;
			screenInfo = device.ScreenInfo;
			return true;
		}

		bool IChromiumWebViewPrivate.CefPointToScreen(ref CefPoint point)
		{
			return CefPointToScreen(ref point);
		}

		protected virtual bool CefPointToScreen(ref CefPoint point)
		{
			VirtualDevice device = Device;
			if (device == null)
			{
				point.Scale(OffscreenGraphics.PixelsPerDip);

				Point ppt = new Point(point.X, point.Y);

				if (CefNetApplication.Instance.CheckAccess())
				{
					ppt = PointToScreen(new Point(ppt.X, ppt.Y));
				}
				else
				{
					Thread.MemoryBarrier();
					Invoke(new Action(() =>
					{
						Thread.MemoryBarrier();
						ppt = PointToScreen(new Point(ppt.X, ppt.Y));
						Thread.MemoryBarrier();
					}));
					Thread.MemoryBarrier();
				}

				point.X = ppt.X;
				point.Y = ppt.Y;
				return true;
			}
			else
			{
				return device.PointToScreen(ref point);
			}
		}

		public float GetDevicePixelRatio()
		{
			VirtualDevice device = Device;
			return device != null ? device.DevicePixelRatio : OffscreenGraphics.PixelsPerDip;
		}

		protected virtual CefPoint PointToViewport(CefPoint point)
		{
			float scale = OffscreenGraphics.PixelsPerDip;
			VirtualDevice viewport = Device;
			if (viewport != null) scale = scale * viewport.Scale;
			CefRect viewportRect = GetViewportRect();
			return new CefPoint((int)((point.X - viewportRect.X) / scale), (int)((point.Y - viewportRect.Y) / scale));
		}

		protected virtual bool PointInViewport(int x, int y)
		{
			CefRect viewportRect = GetViewportRect();
			return viewportRect.X <= x && x <= viewportRect.Right && viewportRect.Y <= y && y <= viewportRect.Bottom;
		}

		CefRect IChromiumWebViewPrivate.GetCefRootBounds()
		{
			return GetCefRootBounds();
		}

		protected virtual unsafe CefRect GetCefRootBounds()
		{
			VirtualDevice device = Device;
			if (device == null)
			{
				Form form = this.FindForm();
				if (form != null)
				{
					Rectangle windowBounds = form.ScaledDisplayRectangle;
					windowBounds.Offset(form.Location);
					float ppd = OffscreenGraphics.PixelsPerDip;
					if (ppd == 1.0f)
						return windowBounds.ToCefRect();
					return new CefRect(
						(int)(windowBounds.Left / ppd),
						(int)(windowBounds.Top / ppd),
						(int)((windowBounds.Right - windowBounds.Left) / ppd),
						(int)((windowBounds.Bottom - windowBounds.Top) / ppd)
					);
				}

				//const int GA_ROOT = 2;
				//RECT windowBounds;
				//IntPtr hwnd = NativeMethods.GetAncestor(OffscreenGraphics.WidgetHandle, GA_ROOT);
				//if ((NativeMethods.DwmIsCompositionEnabled() && NativeMethods.DwmGetWindowAttribute(hwnd, DWMWINDOWATTRIBUTE.ExtendedFrameBounds, &windowBounds, sizeof(RECT)) == 0)
				//	|| NativeMethods.GetWindowRect(hwnd, out windowBounds))
				//{
				//	float ppd = OffscreenGraphics.PixelsPerDip;
				//	if (ppd == 1.0f)
				//		return windowBounds.ToCefRect();
				//	return new CefRect(
				//		(int)(windowBounds.Left / ppd),
				//		(int)(windowBounds.Top / ppd),
				//		(int)((windowBounds.Right - windowBounds.Left) / ppd),
				//		(int)((windowBounds.Bottom - windowBounds.Top) / ppd)
				//	);
				//}
				return OffscreenGraphics.GetBounds();
			}
			else
			{
				return device.GetRootBounds();
			}
		}

		private void UpdateOffscreenViewLocation()
		{
			if (OffscreenGraphics is null)
				return;

			VirtualDevice device = this.Device;
			if (device is null)
			{
				Point screenPoint = PointToScreen(default);
				OffscreenGraphics.SetLocation(screenPoint.X, screenPoint.Y);
			}
			else
			{
				CefPoint screenPoint = default;
				device.PointToScreen(ref screenPoint);
				OffscreenGraphics.SetLocation(screenPoint.X, screenPoint.Y);
			}
		}

		CefRect IChromiumWebViewPrivate.GetCefViewBounds()
		{
			return OffscreenGraphics.GetBounds();
		}

		public void CefInvalidate()
		{
			CefBrowserHost browserHost = BrowserObject?.Host;
			if (browserHost != null)
			{
				browserHost.Invalidate(CefPaintElementType.View);
				browserHost.Invalidate(CefPaintElementType.Popup);
			}
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			SetDevicePixelRatio(this.DeviceDpi / 96f);

			Rectangle renderBounds = OffscreenGraphics.GetRenderBounds();

			

			Rectangle clipRectangle;
			if (e.Canvas.GetLocalClipBounds(out SKRect clip))
			{
				clipRectangle = new Rectangle((int)clip.Left, (int)clip.Top, (int)clip.Width, (int)clip.Height);
			}
			else
			{
				var skRect = e.Info.Rect;
				clipRectangle = new Rectangle(skRect.Left, skRect.Top, skRect.Width, skRect.Height);
			}
			DrawDeviceArea(e.Canvas, clipRectangle);

			OffscreenGraphics.Render(e.Canvas, clipRectangle);

			// redraw background if render has wrong size
			VirtualDevice device = Device;
			if (device != null)
			{
				CefRect deviceBounds = device.GetBounds(OffscreenGraphics.PixelsPerDip);
				if (renderBounds.Width > deviceBounds.Width || renderBounds.Height > deviceBounds.Height)
				{
					e.Canvas.ClipRect(new SKRect(deviceBounds.X, deviceBounds.Y, deviceBounds.Right, deviceBounds.Bottom), SKClipOperation.Difference);
					DrawDeviceArea(e.Canvas, ClientRectangle);
				}
			}

			base.OnPaint(e);
		}

		protected virtual void DrawDeviceArea(SKCanvas canvas, Rectangle rectangle)
		{
			VirtualDevice device = Device;
			if (device != null)
			{
				CefRect deviceBounds = device.GetBounds(OffscreenGraphics.PixelsPerDip);
				SKColor? background = this.Style?.BackgroundColor;
				if (background != null && background.Value.Alpha > 0)
				{
					canvas.DrawColor(background.Value);
				}
				canvas.DrawRectangle(deviceBounds.X - 1, deviceBounds.Y - 1, deviceBounds.Width + 1, deviceBounds.Height + 1, SKColors.Gray);
			}
		}

		/// <summary>
		/// Raises the <see cref="CefPaint"/> event.
		/// </summary>
		/// <param name="e">A <see cref="CefPaintEventArgs"/> that contains event data.</param>
		protected virtual void OnCefPaint(CefPaintEventArgs e)
		{
			CefPaint?.Invoke(this, e);

			if (GetState(State.Closing))
				return;

			CefRect invalidRect = OffscreenGraphics.Draw(e);
			Device?.MoveToDevice(ref invalidRect, OffscreenGraphics.PixelsPerDip);

			if (GetState(State.Closing))
				return;

			base.Invalidate(invalidRect.ToRectangle());
		}

		protected virtual void OnPopupShow(PopupShowEventArgs e)
		{
			Rectangle invalidRect = OffscreenGraphics.GetPopupBounds();

			CefRect bounds = e.Bounds;
			float ppd = OffscreenGraphics.PixelsPerDip;
			VirtualDevice device = Device;
			if (device != null)
			{
				invalidRect.Offset((int)(device.X * ppd), (int)(device.Y * ppd));
				device.ScaleToViewport(ref bounds, ppd);
			}
			else
			{
				bounds.Scale(ppd);
			}
			OffscreenGraphics.SetPopup(e.Visible, bounds);
			Invalidate(invalidRect);
		}

		/// <summary>
		/// Raises the <see cref="Control.SizeChanged"/> event.
		/// </summary>
		/// <param name="e">An <see cref="EventArgs"/> that contains event data.</param>
		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged(e);

			UpdateOffscreenViewLocation();

			VirtualDevice device = this.Device;
			if (device is null)
			{
				if (OffscreenGraphics.SetSize(this.ScaledWidth, this.ScaledHeight))
				{
					BrowserObject?.Host.WasResized();
				}
			}
			else
			{
				CefRect viewportRect = device.ViewportRect;
				if (OffscreenGraphics.SetSize(viewportRect.Width, viewportRect.Height))
				{
					BrowserObject?.Host.WasResized();
				}
			}
		}

		protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
		{
			if (specified == BoundsSpecified.All || specified == BoundsSpecified.None)
			{
				if (width == 0 || height == 0)
					return;
			}
			else if (specified == BoundsSpecified.Width && width == 0)
			{
				return;
			}
			else if (specified == BoundsSpecified.Height && height == 0)
			{
				return;
			}
			base.SetBoundsCore(x, y, width, height, specified);
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);
			ProcessKey(CefKeyEventType.KeyDown, e);
		}

		protected override void OnKeyUp(KeyEventArgs e)
		{
			base.OnKeyUp(e);
			ProcessKey(CefKeyEventType.KeyUp, e);
		}

		protected override void OnKeyPress(KeyPressEventArgs e)
		{
			base.OnKeyPress(e);

			char symbol = e.KeyChar;

			CefEventFlags modifiers = CefNet.Input.KeycodeConverter.IsShiftRequired(symbol) ? CefEventFlags.ShiftDown : CefEventFlags.None;
			VirtualKeys key = KeycodeConverter.CharacterToVirtualKey(symbol);

			var k = new CefKeyEvent();
			k.Type = CefKeyEventType.Char;
			k.WindowsKeyCode = PlatformInfo.IsLinux ? (int)key : symbol;
			k.Character = symbol;
			k.UnmodifiedCharacter = symbol;
			k.Modifiers = (uint)modifiers;
			k.NativeKeyCode = KeycodeConverter.VirtualKeyToNativeKeyCode(key, modifiers, false);
			this.BrowserObject?.Host.SendKeyEvent(k);
		}

		protected virtual bool ProcessKey(CefKeyEventType eventType, KeyEventArgs e)
		{
			if (PlatformInfo.IsWindows)
				SetWindowsKeyboardLayoutForCefUIThreadIfNeeded();

			CefEventFlags modifiers = GetCefKeyboardModifiers(e);
			Keys key = e.KeyCode;
			if (eventType == CefKeyEventType.KeyUp && key == Keys.None)
			{
				if (e.Modifiers == Keys.Shift)
				{
					key = Keys.LShiftKey;
					modifiers |= CefEventFlags.IsLeft;
				}
			}

			VirtualKeys virtualKey = key.ToVirtualKey();
			bool isSystemKey = e.Modifiers.HasFlag(Keys.Alt);

			CefBrowserHost browserHost = this.BrowserObject?.Host;
			if (browserHost != null)
			{
				var k = new CefKeyEvent();
				k.Type = eventType;
				k.Modifiers = (uint)modifiers;
				k.IsSystemKey = isSystemKey;
				k.WindowsKeyCode = (int)virtualKey;
				k.NativeKeyCode = KeycodeConverter.VirtualKeyToNativeKeyCode(virtualKey, modifiers, false);
				if (PlatformInfo.IsMacOS)
				{
					k.UnmodifiedCharacter = char.ToUpperInvariant(CefNet.Input.KeycodeConverter.TranslateVirtualKey(virtualKey, CefEventFlags.None));
					k.Character = CefNet.Input.KeycodeConverter.TranslateVirtualKey(virtualKey, modifiers);
				}
				this.BrowserObject?.Host.SendKeyEvent(k);

				if (key == Keys.Enter && eventType == CefKeyEventType.RawKeyDown)
				{
					k.Type = CefKeyEventType.Char;
					k.Character = '\r';
					k.UnmodifiedCharacter = '\r';
					this.BrowserObject?.Host.SendKeyEvent(k);
				}
			}

			if (isSystemKey)
				return true;

			// Prevent keyboard navigation using arrows and home and end keys
			if (key >= Keys.PageUp && key <= Keys.Down)
				return true;

			if (key == Keys.Tab)
				return true;

			// Allow Ctrl+A to work when the WebView control is put inside listbox.
			if (key == Keys.A && e.Modifiers.HasFlag(Keys.Control))
				return true;

			return false;
		}


		/// <inheritdoc/>
		protected override void OnMouseMove(MouseEventArgs e)
		{
			if (PointInViewport(e.X, e.Y))
			{
				CefEventFlags modifiers = CefEventFlags.None;
				if (e.Button == MouseButtons.Left)
					modifiers |= CefEventFlags.LeftMouseButton;
				if (e.Button == MouseButtons.Right)
					modifiers |= CefEventFlags.RightMouseButton;
				SendMouseMoveEvent(e.X, e.Y, modifiers);
			}
			base.OnMouseMove(e);
		}

		/// <inheritdoc/>
		protected override void OnMouseLeave(EventArgs e)
		{
			SendMouseLeaveEvent();
			base.OnMouseLeave(e);
		}

		private static CefMouseButtonType GetButton(MouseEventArgs e)
		{
			switch (e.Button)
			{
				case MouseButtons.Right:
					return CefMouseButtonType.Right;
				case MouseButtons.Middle:
					return CefMouseButtonType.Middle;
			}
			return CefMouseButtonType.Left;
		}

		/// <inheritdoc/>
		protected override void OnMouseDown(MouseEventArgs e)
		{
			if (PointInViewport(e.X, e.Y))
			{
				SendMouseDownEvent(e.X, e.Y, GetButton(e), e.Clicks, GetModifierKeys(e.Modifiers));
			}
			base.OnMouseDown(e);
		}

		/// <inheritdoc/>
		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			if (PointInViewport(e.X, e.Y))
			{
				SendMouseUpEvent(e.X, e.Y, GetButton(e), e.Clicks, GetModifierKeys(e.Modifiers));
			}
		}

		/// <inheritdoc/>
		protected override void OnMouseWheel(MouseEventArgs e)
		{
			base.OnMouseWheel(e);
			
			if (!e.Modifiers.HasFlag(Keys.Shift))
			{
				if (PointInViewport(e.X, e.Y))
				{
					const int WHEEL_DELTA = 120;
					SendMouseWheelEvent(e.X, e.Y, 0, e.Delta.Y * WHEEL_DELTA);
				}
				return;
			}
			OnMouseHWheel(e);
		}

		/// <summary>
		/// Called when the MouseWheel event is received to scroll horizontally.
		/// </summary>
		/// <param name="e">A <see cref="MouseEventArgs"/> that contains the event data.</param>
		protected virtual void OnMouseHWheel(MouseEventArgs e)
		{
			if (PointInViewport(e.X, e.Y))
			{
				const int WHEEL_DELTA = 120;
				SendMouseWheelEvent(e.X, e.Y, e.Delta.Y * WHEEL_DELTA, 0);
			}
		}

		protected static CefEventFlags GetModifierKeys(Keys modKeys)
		{
			CefEventFlags modifiers = CefEventFlags.None;
			if (modKeys.HasFlag(Keys.Shift))
				modifiers |= CefEventFlags.ShiftDown;
			if (modKeys.HasFlag(Keys.Control))
				modifiers |= CefEventFlags.ControlDown;
			if (modKeys.HasFlag(Keys.Alt))
				modifiers |= CefEventFlags.AltDown;
			return modifiers;
		}

		protected CefEventFlags GetCefKeyboardModifiers(KeyEventArgs e)
		{
			CefEventFlags modifiers = GetModifierKeys(e.Modifiers);


			// TODO:
			//if (Keyboard.IsKeyToggled(Key.NumLock))
			//	modifiers |= CefEventFlags.NumLockOn;
			//if (Keyboard.IsKeyToggled(Key.CapsLock))
			//	modifiers |= CefEventFlags.CapsLockOn;

			switch (e.KeyCode)
			{
				case Keys.Return:
					//if (e.IsExtendedKey())
					//	modifiers |= CefEventFlags.IsKeyPad;
					break;
				case Keys.Insert:
				case Keys.Delete:
				case Keys.Home:
				case Keys.End:
				case Keys.Prior:
				case Keys.Next:
				case Keys.Up:
				case Keys.Down:
				case Keys.Left:
				case Keys.Right:
					//if (!e.IsExtendedKey())
					//	modifiers |= CefEventFlags.IsKeyPad;
					break;
				case Keys.NumLock:
				case Keys.NumPad0:
				case Keys.NumPad1:
				case Keys.NumPad2:
				case Keys.NumPad3:
				case Keys.NumPad4:
				case Keys.NumPad5:
				case Keys.NumPad6:
				case Keys.NumPad7:
				case Keys.NumPad8:
				case Keys.NumPad9:
				case Keys.Divide:
				case Keys.Multiply:
				case Keys.Subtract:
				case Keys.Add:
				case Keys.Decimal:
				case Keys.Clear:
					modifiers |= CefEventFlags.IsKeyPad;
					break;
				case Keys.LShiftKey:
				case Keys.LControlKey:
				//case Keys.LeftAlt:
				case Keys.LWin:
					modifiers |= CefEventFlags.IsLeft;
					break;
				case Keys.RShiftKey:
				case Keys.RControlKey:
				//case Keys.RightAlt:
				case Keys.RWin:
					modifiers |= CefEventFlags.IsRight;
					break;
			}
			return modifiers;
		}

		/// <summary>
		/// Sets the current input locale identifier for the UI thread in the browser.
		/// </summary>
		protected void SetWindowsKeyboardLayoutForCefUIThreadIfNeeded()
		{
			IntPtr hkl = NativeMethods.GetKeyboardLayout(0);
			if (_keyboardLayout == hkl)
				return;

			if (CefApi.CurrentlyOn(CefThreadId.UI))
			{
				_keyboardLayout = hkl;
			}
			else
			{
				CefNetApi.Post(CefThreadId.UI, () =>
				{
					NativeMethods.ActivateKeyboardLayout(hkl, 0);
					_keyboardLayout = hkl;
				});
			}
		}
	}
}
