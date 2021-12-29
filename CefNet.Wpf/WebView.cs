using CefNet.Input;
using CefNet.Internal;
using CefNet.WinApi;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Threading;

namespace CefNet.Wpf
{
	public partial class WebView : Control, IWpfWebViewPrivate, IDisposable
	{
		private CefRect _windowBounds;
		private bool _allowResizeNotifications = true;
		private IntPtr _keyboardLayout;
		private bool _lastKeydownIsExtendedKey;
		private Dictionary<InitialPropertyKeys, object> InitialPropertyBag = new Dictionary<InitialPropertyKeys, object>();

		/// <summary>
		/// Identifies the <see cref="TextFound"/> routed event.
		/// </summary>
		public static readonly RoutedEvent TextFoundEvent = EventManager.RegisterRoutedEvent(nameof(TextFound), RoutingStrategy.Bubble, typeof(EventHandler<ITextFoundEventArgs>), typeof(WebView));

		/// <summary>
		/// Identifies the <see cref="PdfPrintFinished"/> routed event.
		/// </summary>
		public static readonly RoutedEvent PdfPrintFinishedEvent = EventManager.RegisterRoutedEvent(nameof(PdfPrintFinished), RoutingStrategy.Bubble, typeof(EventHandler<IPdfPrintFinishedEventArgs>), typeof(WebView));

		/// <summary>
		/// Identifies the <see cref="StatusTextChanged"/> routed event.
		/// </summary>
		public static readonly RoutedEvent StatusTextChangedEvent = EventManager.RegisterRoutedEvent(nameof(StatusTextChanged), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(WebView));

		/// <summary>
		/// Identifies the <see cref="ScriptDialogOpening"/> routed event.
		/// </summary>
		public static readonly RoutedEvent ScriptDialogOpeningEvent = EventManager.RegisterRoutedEvent(nameof(ScriptDialogOpening), RoutingStrategy.Bubble, typeof(EventHandler<IScriptDialogOpeningEventArgs>), typeof(WebView));

		/// <summary>
		/// Identifies the <see cref="StartDragging"/> routed event.
		/// </summary>
		public static readonly RoutedEvent StartDraggingEvent = EventManager.RegisterRoutedEvent(nameof(StartDragging), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(WebView));

		/// <summary>
		/// Occurs when the user starts dragging content in the web view.
		/// </summary>
		/// <remarks>
		/// OS APIs that run a system message loop may be used within the StartDragging event handler.
		/// Call <see cref="WebView.DragSourceEndedAt"/> and <see cref="WebView.DragSourceSystemDragEnded"/>
		/// either synchronously or asynchronously to inform the web view that the drag operation has ended.
		/// </remarks>
		public event RoutedEventHandler StartDragging
		{
			add { AddHandler(StartDraggingEvent, value); }
			remove { RemoveHandler(StartDraggingEvent, value); }
		}

		public WebView()
			: this(null)
		{

		}

		public WebView(WebView opener)
		{
			if (DesignMode)
			{
				//BackColor = System.Drawing.Color.White;
				return;
			}
			if (opener != null)
			{
				this.Opener = opener;
				this.BrowserSettings = opener.BrowserSettings;
			}
			Initialize();
		}

		/// <summary>
		/// Gets a value that indicates whether the <see cref="WebView"/> is currently in design mode.
		/// </summary>
		protected bool DesignMode
		{
			get
			{
				return DesignerProperties.GetIsInDesignMode(this);
				//Windows.ApplicationModel.DesignMode.DesignModeEnabled
				// DesignerProperties.IsInDesignTool;
			}
		}

		protected OffscreenGraphics OffscreenGraphics { get; private set; }
		
		protected virtual void OnCreateBrowser()
		{
			if (this.Opener != null)
				return;

			if (GetState(State.Creating) || GetState(State.Created))
				throw new InvalidOperationException();

			SetState(State.Creating, true);

			Dictionary<InitialPropertyKeys, object> propertyBag = InitialPropertyBag;
			InitialPropertyBag = null;

			var wpfwindow = System.Windows.Window.GetWindow(this);
			if (wpfwindow == null)
				throw new InvalidOperationException("Window not found!");

			using (var windowInfo = new CefWindowInfo())
			{
				windowInfo.SetAsWindowless(new WindowInteropHelper(wpfwindow).Handle);

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

		private void SetInitProperty(InitialPropertyKeys key, object value)
		{
			var propertyBag = InitialPropertyBag;
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
			var propertyBag = InitialPropertyBag;
			if (propertyBag != null && propertyBag.TryGetValue(key, out object value))
			{
				return (T)value;
			}
			return default;
		}

		protected virtual void Initialize()
		{
			this.AllowDrop = true;
			this.FocusVisualStyle = null;
			//defaultTooltip = new ToolTip();
			ToolTip = new ToolTip { Visibility = Visibility.Collapsed };
			this.ViewGlue = CreateWebViewGlue();
		}

		protected virtual WebViewGlue CreateWebViewGlue()
		{
			return new WpfWebViewGlue(this);
		}

		protected override void OnInitialized(EventArgs e)
		{
			base.OnInitialized(e);
			GlobalHooks.Initialize(this);
		}

		void IDisposable.Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (this.ViewGlue != null)
			{
				SetState(State.Closing, true);
				BrowserObject?.Host.CloseBrowser(true);
				this.ViewGlue = null;
			}
		}

		public string StatusText { get; protected set; }

		protected override void OnRender(DrawingContext drawingContext)
		{
			base.OnRender(drawingContext);
			if (OffscreenGraphics != null)
			{
				OffscreenGraphics.Render(drawingContext);
			}
			else
			{
				drawingContext.DrawText(
					new FormattedText(this.GetType().Name,
					CultureInfo.InvariantCulture,
					FlowDirection.LeftToRight,
					new Typeface(FontFamily, FontStyle, FontWeight, FontStretch),
					FontSize,
					Brushes.Black,
					VisualTreeHelper.GetDpi(this).PixelsPerDip),
					new Point(10, 10));
			}
		}

		private int _fixResizeGlitchesFlag;
		/// <summary>
		/// Fixes resize glitches when maximizing or restoring the parent window if a static page is displayed.
		/// </summary>
		private void FixResizeGlitches()
		{
			// We must force the browser to be redrawn so that the new size is applied.
			// See for CEF implementation details:
			// https://bitbucket.org/chromiumembedded/cef/issues/2733/viz-osr-might-be-causing-some-graphic#comment-56271100

			if (Interlocked.Exchange(ref _fixResizeGlitchesFlag, 0) != 1)
				return;
			BrowserObject?.Host.Invalidate(CefPaintElementType.View);
		}

		protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
		{
			if (_allowResizeNotifications)
			{
				OnUpdateRootBounds();
				if (OffscreenGraphics != null && OffscreenGraphics.SetSize((int)this.ActualWidth, (int)this.ActualHeight))
				{
					OffscreenGraphics.DpiScale = VisualTreeHelper.GetDpi(this);
					BrowserObject?.Host.WasResized();

					if (Interlocked.Exchange(ref _fixResizeGlitchesFlag, 1) != 0)
						return;
					CefNetApi.Post(CefThreadId.UI, FixResizeGlitches, 30);
				}
			}

			base.OnRenderSizeChanged(sizeInfo);
		}

		protected internal virtual unsafe void OnUpdateRootBounds()
		{
			Window window = Window.GetWindow(this);
			if (window != null)
			{
				IntPtr hwnd = new WindowInteropHelper(window).Handle;
				RECT windowBounds;
				if ((NativeMethods.DwmIsCompositionEnabled() && NativeMethods.DwmGetWindowAttribute(hwnd, DWMWINDOWATTRIBUTE.ExtendedFrameBounds, &windowBounds, sizeof(RECT)) == 0)
					|| NativeMethods.GetWindowRect(hwnd, out windowBounds))
				{
					DpiScale scale = OffscreenGraphics.DpiScale;

					windowBounds = new RECT
					{
						Left = (int)Math.Floor(windowBounds.Left / scale.DpiScaleX),
						Top = (int)Math.Floor(windowBounds.Top / scale.DpiScaleY),
						Right = (int)Math.Ceiling(windowBounds.Right / scale.DpiScaleX),
						Bottom = (int)Math.Ceiling(windowBounds.Bottom / scale.DpiScaleY)
					};
					RootBoundsChanged(windowBounds.ToCefRect());
				}
				else
				{
					RootBoundsChanged(new CefRect((int)window.Left, (int)window.Top, (int)window.ActualWidth, (int)window.ActualHeight));
				}
			}
		}

		protected void SuspendResizeNotifications()
		{
			_allowResizeNotifications = false;
		}

		protected void ResumeResizeNotifications()
		{
			_allowResizeNotifications = true;
			OnRenderSizeChanged(new SizeChangedInfo(this, new Size(ActualWidth, ActualHeight), false, false));
		}

		protected internal virtual void OnRootResizeBegin(EventArgs e)
		{
			SuspendResizeNotifications();
		}

		protected internal virtual void OnRootResizeEnd(EventArgs e)
		{
			ResumeResizeNotifications();
		}

		protected internal void RootBoundsChanged(CefRect bounds)
		{
			_windowBounds = bounds;

			if (_allowResizeNotifications)
			{
				NotifyRootMovedOrResized();
			}
		}

		private void UpdateOffscreenViewLocation()
		{
			Point screenPoint = PointToScreen(default);
			OffscreenGraphics.SetLocation((int)screenPoint.X, (int)screenPoint.Y);
		}

		protected override Size MeasureOverride(Size constraint)
		{
			if (constraint.IsEmpty)
				return new Size(1, 1);
			return base.MeasureOverride(constraint);
		}

		protected override Size ArrangeOverride(Size arrangeBounds)
		{
			arrangeBounds = base.ArrangeOverride(arrangeBounds);
			if (!DesignMode)
			{
				if (OffscreenGraphics == null)
				{
					OffscreenGraphics = new OffscreenGraphics();
					OnCreateBrowser();
				}
			}
			return arrangeBounds;
		}

		/// <summary>
		/// Gets the rectangle that represents the bounds of the WebView control.
		/// </summary>
		/// <returns>
		/// A <see cref="CefRect"/> representing the bounds within which the WebView control is scaled.
		/// </returns>
		public CefRect GetBounds()
		{
			if (OffscreenGraphics is null)
				return new CefRect(0,0, (int)this.ActualWidth, (int)this.ActualHeight);
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

			Width = width;
			Height = height;
		}

		protected virtual void RaiseCrossThreadEvent<TEventArgs>(Action<TEventArgs> raiseEvent, TEventArgs e, bool synchronous)
			where TEventArgs : class
		{
			if (synchronous)
				Dispatcher.Invoke(new CrossThreadEventMethod<TEventArgs>(raiseEvent, e).Invoke, this);
			else
				Dispatcher.BeginInvoke(new CrossThreadEventMethod<TEventArgs>(raiseEvent, e).Invoke, this);
		}

		/// <summary>
		/// Adds a routed event handler for a specified routed event, adding the handler
		/// to the handler collection on the current element.
		/// </summary>
		/// <param name="routedEvent">An identifier for the routed event to be handled.</param>
		/// <param name="handler">A reference to the handler implementation.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void AddHandler(in RoutedEvent routedEvent, Delegate handler)
		{
			AddHandler(routedEvent, handler);
		}

		/// <summary>
		/// Removes the specified routed event handler from this element.
		/// </summary>
		/// <param name="routedEvent">The identifier of the routed event for which the handler is attached.</param>
		/// <param name="handler">The specific handler implementation to remove from the event handler collection on this element.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void RemoveHandler(in RoutedEvent routedEvent, Delegate handler)
		{
			RemoveHandler(routedEvent, handler);
		}

		protected virtual void OnBrowserCreated(EventArgs e)
		{
			BrowserCreated?.Invoke(this, e);
		}

		protected virtual void OnLoadingStateChange(LoadingStateChangeEventArgs e)
		{
			LoadingStateChange?.Invoke(this, e);
		}

		protected virtual void OnCefPaint(CefPaintEventArgs e)
		{
			OffscreenGraphics.Draw(e);
			CefPaint?.Invoke(this, e);
			Dispatcher.BeginInvoke(new Action(() => { this.InvalidateVisual(); }), DispatcherPriority.Render);
		}

		protected virtual void OnPopupShow(PopupShowEventArgs e)
		{
			OffscreenGraphics.SetPopup(e);
		}

		private CefPoint PointToViewport(CefPoint point)
		{
			return point;
		}

		public new Point PointToScreen(Point point)
		{
			if (PresentationSource.FromVisual(this) != null)
			{
				point = base.PointToScreen(point);
				DpiScale dpi = OffscreenGraphics.DpiScale;
				if (dpi.PixelsPerDip == 1.0)
					return point;
				return new Point(point.X / dpi.PixelsPerDip, point.Y / dpi.PixelsPerDip);
			}
			CefRect viewRect = OffscreenGraphics.GetBounds();
			point.Offset(viewRect.X, viewRect.Y);
			return point;
		}

		void IChromiumWebViewPrivate.RaisePopupBrowserCreating()
		{
			SetState(State.Creating, true);
			InitialPropertyBag = null;
		}

		bool IChromiumWebViewPrivate.GetCefScreenInfo(ref CefScreenInfo screenInfo)
		{
			return false;
		}

		unsafe bool IChromiumWebViewPrivate.CefPointToScreen(ref CefPoint point)
		{
			Point pt = new Point(point.X, point.Y);
			Thread.MemoryBarrier();
			Dispatcher.Invoke(new Action(() =>
			{
				Thread.MemoryBarrier();
				pt = PointToScreen(pt);
				Thread.MemoryBarrier();
			}), DispatcherPriority.Render);
			Thread.MemoryBarrier();

			//NativeMethods.MapWindowPoints(OffscreenGraphics.WidgetHandle, IntPtr.Zero, ref point, 1);
			
			point.X = (int)Math.Round(pt.X);
			point.Y = (int)Math.Round(pt.Y);
			return true;
		}

		float IChromiumWebViewPrivate.GetDevicePixelRatio()
		{
			return (float)OffscreenGraphics.DpiScale.PixelsPerDip;
		}

		CefRect IChromiumWebViewPrivate.GetCefRootBounds()
		{
			return _windowBounds;
		}

		CefRect IChromiumWebViewPrivate.GetCefViewBounds()
		{
			return OffscreenGraphics.GetBounds();
		}

		bool IChromiumWebViewPrivate.RaiseRunContextMenu(CefFrame frame, CefContextMenuParams menuParams, CefMenuModel model, CefRunContextMenuCallback callback)
		{
			if (model.Count == 0)
			{
				callback.Cancel();
				return true;
			}
			return (bool)Dispatcher.Invoke(
				new Func<WpfContextMenuRunner, Point, bool>(RunContextMenu), 
				new WpfContextMenuRunner(model, callback),
				new Point(menuParams.XCoord, menuParams.YCoord)
			);
		}

		private bool RunContextMenu(WpfContextMenuRunner runner, Point position)
		{
			if (this.ContextMenu != null)
			{
				runner.Cancel();
				return true;
			}
			runner.Build();
			runner.RunMenuAt(this, position);
			return true;
		}

		void IWpfWebViewPrivate.RaiseCefCursorChange(CursorChangeEventArgs e)
		{
			RaiseCrossThreadEvent(OnCursorChange, e, true);
		}

		protected virtual void OnCursorChange(CursorChangeEventArgs e)
		{
			this.Cursor = e.Cursor;
		}

		void IWpfWebViewPrivate.CefSetToolTip(string text)
		{
			Dispatcher.BeginInvoke(new Action<string>(OnSetToolTip), text);
		}

		protected virtual void OnSetToolTip(string text)
		{
			if (this.ToolTip is ToolTip tooltip)
			{
				if (string.IsNullOrWhiteSpace(text))
				{
					tooltip.IsOpen = false;
					tooltip.Visibility = Visibility.Collapsed;
				}
				else
				{
					if (!string.Equals(text, tooltip.Content as string))
						tooltip.Content = text;
					if (tooltip.Visibility != Visibility.Visible)
						tooltip.Visibility = Visibility.Visible;
					if (!tooltip.IsOpen)
						tooltip.IsOpen = true;
				}
			}
		}

		void IWpfWebViewPrivate.CefSetStatusText(string statusText)
		{
			this.StatusText = statusText;
			RaiseCrossThreadEvent(OnStatusTextChanged, new RoutedEventArgs(StatusTextChangedEvent, this), false);
		}

		void IWpfWebViewPrivate.RaiseStartDragging(StartDraggingEventArgs e)
		{
			RaiseCrossThreadEvent(OnStartDragging, e, true);
		}

		/// <summary>
		/// Raises <see cref="WebView.StartDragging"/> event.
		/// </summary>
		/// <param name="e">The event data.</param>
		protected virtual void OnStartDragging(StartDraggingEventArgs e)
		{
			RaiseEvent(e);

			if (e.Handled)
				return;

			DragDrop.DoDragDrop(this, new CefNetDragData(this, e.Data), e.AllowedEffects.ToDragDropEffects());
			DragSourceSystemDragEnded();
			e.Handled = true;
		}

		protected override void OnGotFocus(RoutedEventArgs e)
		{
			BrowserObject?.Host.SetFocus(true);
			base.OnGotFocus(e);
		}

		protected override void OnLostFocus(RoutedEventArgs e)
		{
			BrowserObject?.Host.SetFocus(false);
			base.OnLostFocus(e);
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			CefEventFlags modifiers = GetModifierKeys();
			if (e.LeftButton == MouseButtonState.Pressed)
				modifiers |= CefEventFlags.LeftMouseButton;
			if (e.RightButton == MouseButtonState.Pressed)
				modifiers |= CefEventFlags.RightMouseButton;
			Point mousePos = e.GetPosition(this);
			SendMouseMoveEvent((int)mousePos.X, (int)mousePos.Y, modifiers);
			base.OnMouseMove(e);
		}

		protected override void OnMouseLeave(MouseEventArgs e)
		{
			SendMouseLeaveEvent();
			base.OnMouseLeave(e);
		}

		protected override void OnMouseDown(MouseButtonEventArgs e)
		{
			if (!IsFocused)
			{
				Focus();
			}
			if (e.ChangedButton <= MouseButton.Right)
			{
				Point mousePos = e.GetPosition(this);
				SendMouseDownEvent((int)mousePos.X, (int)mousePos.Y, GetButton(e), e.ClickCount, GetModifierKeys());
			}
			base.OnMouseDown(e);
		}

		protected override void OnMouseUp(MouseButtonEventArgs e)
		{
			base.OnMouseUp(e);
			if (e.ChangedButton > MouseButton.Right)
				return;

			Point mousePos = e.GetPosition(this);
			SendMouseUpEvent((int)mousePos.X, (int)mousePos.Y, GetButton(e), e.ClickCount, GetModifierKeys());
		}

		protected override void OnMouseWheel(MouseWheelEventArgs e)
		{
			base.OnMouseWheel(e);
			
			if (!Keyboard.Modifiers.HasFlag(ModifierKeys.Shift))
			{
				Point mousePos = e.GetPosition(this);
				SendMouseWheelEvent((int)mousePos.X, (int)mousePos.Y, 0, e.Delta);
				e.Handled = true;
				return;
			}
			OnMouseHWheel(e);
		}

		protected internal virtual void OnMouseHWheel(MouseWheelEventArgs e)
		{
			Point mousePos = e.GetPosition(this);
			SendMouseWheelEvent((int)mousePos.X, (int)mousePos.Y, e.Delta, 0);
			e.Handled = true;
		}

		protected override void OnTouchDown(TouchEventArgs e)
		{
			OnTouch(e);
			e.Handled = true;
		}

		protected override void OnTouchMove(TouchEventArgs e)
		{
			OnTouch(e);
			e.Handled = true;
		}

		protected override void OnTouchUp(TouchEventArgs e)
		{
			OnTouch(e);
			e.Handled = true;
		}

		private void OnTouch(TouchEventArgs e)
		{
			TouchPoint touchPoint = e.GetTouchPoint(this);

			var eventInfo = new CefTouchEvent();
			switch (touchPoint.Action)
			{
				case TouchAction.Down:
					eventInfo.Type = CefTouchEventType.Pressed;
					break;
				case TouchAction.Move:
					eventInfo.Type = CefTouchEventType.Moved;
					break;
				case TouchAction.Up:
					eventInfo.Type = CefTouchEventType.Released;
					break;
				default:
					throw new NotSupportedException();
			}

			Point pt = touchPoint.Position;
			CefPoint point = PointToViewport(new CefPoint((int)pt.X, (int)pt.Y));
			eventInfo.X = point.X;
			eventInfo.Y = point.Y;
			eventInfo.PointerType = CefPointerType.Touch;
			eventInfo.RadiusX = (float)touchPoint.Size.Width / 2;
			eventInfo.RadiusY = (float)touchPoint.Size.Height / 2;
			eventInfo.Id = touchPoint.TouchDevice.Id;
			SendTouchEvent(eventInfo);
		}

		protected virtual bool ProcessPreviewKey(CefKeyEventType eventType, KeyEventArgs e)
		{
			SetKeyboardLayoutForCefUIThreadIfNeeded();

			Key key = e.Key;
			VirtualKeys virtualKey = (key == Key.System ? e.SystemKey.ToVirtualKey() : key.ToVirtualKey());
			CefEventFlags modifiers = GetCefKeyboardModifiers(e);
			if (e.IsRepeat)
				modifiers |= CefEventFlags.IsRepeat;
			_lastKeydownIsExtendedKey = e.IsExtendedKey();

			var k = new CefKeyEvent();
			k.Type = eventType;
			k.Modifiers = (uint)modifiers;
			k.IsSystemKey = (key == Key.System);
			k.WindowsKeyCode = (int)virtualKey;
			k.NativeKeyCode = KeycodeConverter.VirtualKeyToNativeKeyCode(virtualKey, modifiers, _lastKeydownIsExtendedKey);
			this.BrowserObject?.Host.SendKeyEvent(k);

			if (k.IsSystemKey)
				return true;

			// Prevent keyboard navigation using arrows and home and end keys
			if (key >= Key.PageUp && key <= Key.Down)
				return true;

			if (key == Key.Tab)
				return true;

			// Allow Ctrl+A to work when the WebView control is put inside listbox.
			if (key == Key.A && ((CefEventFlags)k.Modifiers).HasFlag(CefEventFlags.ControlDown))
				return true;

			return false;
		}

		protected override void OnPreviewKeyDown(KeyEventArgs e)
		{
			e.Handled = ProcessPreviewKey(CefKeyEventType.RawKeyDown, e);
		}

		protected override void OnPreviewKeyUp(KeyEventArgs e)
		{
			e.Handled = ProcessPreviewKey(CefKeyEventType.KeyUp, e);
		}

		protected override void OnPreviewTextInput(TextCompositionEventArgs e)
		{
			foreach (char symbol in e.Text)
			{
				CefEventFlags modifiers = GetCefKeyboardModifiers();
				if (CefNet.Input.KeycodeConverter.IsShiftRequired(symbol))
					modifiers |= CefEventFlags.ShiftDown;
				if (_lastKeydownIsExtendedKey)
					modifiers |= CefEventFlags.IsKeyPad;

				var k = new CefKeyEvent();
				k.Type = CefKeyEventType.Char;
				k.WindowsKeyCode = symbol;
				k.NativeKeyCode = KeycodeConverter.VirtualKeyToNativeKeyCode(KeycodeConverter.CharacterToVirtualKey(symbol), modifiers, _lastKeydownIsExtendedKey);
				k.Modifiers = (uint)modifiers;
				this.BrowserObject?.Host.SendKeyEvent(k);
			}
			e.Handled = true;
		}

		protected override void OnDragEnter(DragEventArgs e)
		{
			base.OnDragEnter(e);
			if (e.Handled)
				return;

			Point mousePos = e.GetPosition(this);
			SendDragEnterEvent((int)mousePos.X, (int)mousePos.Y, e.GetModifiers(), e.GetCefDragData(), e.AllowedEffects.ToCefDragOperationsMask());
			e.Effects = DragDropEffects.Copy & e.AllowedEffects;
		}

		protected override void OnDragOver(DragEventArgs e)
		{
			base.OnDragOver(e);
			if (e.Handled)
				return;

			Point mousePos = e.GetPosition(this);
			SendDragOverEvent((int)mousePos.X, (int)mousePos.Y, e.GetModifiers(), e.AllowedEffects.ToCefDragOperationsMask());
		}

		protected override void OnDragLeave(DragEventArgs e)
		{
			base.OnDragLeave(e);
			if (e.Handled)
				return;

			SendDragLeaveEvent();
		}

		protected override void OnDrop(DragEventArgs e)
		{
			base.OnDrop(e);
			if (e.Handled)
				return;

			Point mousePos = e.GetPosition(this);
			SendDragDropEvent((int)mousePos.X, (int)mousePos.Y, e.GetModifiers());

			if (e.Data.GetDataPresent(nameof(CefNetDragData)))
			{
				CefNetDragData data = (CefNetDragData)e.Data.GetData(nameof(CefNetDragData));
				if (data != null && data.Source == this)
				{
					DragSourceEndedAt((int)mousePos.X, (int)mousePos.Y, e.AllowedEffects.ToCefDragOperationsMask());
				}
			}
		}

		private static CefMouseButtonType GetButton(MouseButtonEventArgs e)
		{
			switch (e.ChangedButton)
			{
				case MouseButton.Right:
					return CefMouseButtonType.Right;
				case MouseButton.Middle:
					return CefMouseButtonType.Middle;
			}
			return CefMouseButtonType.Left;
		}

		protected static CefEventFlags GetModifierKeys()
		{
			CefEventFlags modifiers = CefEventFlags.None;
			ModifierKeys modKeys = Keyboard.Modifiers;
			if (modKeys.HasFlag(ModifierKeys.Shift))
				modifiers |= CefEventFlags.ShiftDown;
			if (modKeys.HasFlag(ModifierKeys.Control))
				modifiers |= CefEventFlags.ControlDown;
			if (modKeys.HasFlag(ModifierKeys.Alt))
				modifiers |= CefEventFlags.AltDown;
			return modifiers;
		}

		protected CefEventFlags GetCefKeyboardModifiers()
		{
			CefEventFlags modifiers = GetModifierKeys();

			if (Keyboard.IsKeyToggled(Key.NumLock))
				modifiers |= CefEventFlags.NumLockOn;
			if (Keyboard.IsKeyToggled(Key.CapsLock))
				modifiers |= CefEventFlags.CapsLockOn;
			return modifiers;
		}

		protected CefEventFlags GetCefKeyboardModifiers(KeyEventArgs e)
		{
			CefEventFlags modifiers = GetCefKeyboardModifiers();

			switch (e.Key)
			{
				case Key.Return:
					if (e.IsExtendedKey())
						modifiers |= CefEventFlags.IsKeyPad;
					break;
				case Key.Insert:
				case Key.Delete:
				case Key.Home:
				case Key.End:
				case Key.Prior:
				case Key.Next:
				case Key.Up:
				case Key.Down:
				case Key.Left:
				case Key.Right:
					if (!e.IsExtendedKey())
						modifiers |= CefEventFlags.IsKeyPad;
					break;
				case Key.NumLock:
				case Key.NumPad0:
				case Key.NumPad1:
				case Key.NumPad2:
				case Key.NumPad3:
				case Key.NumPad4:
				case Key.NumPad5:
				case Key.NumPad6:
				case Key.NumPad7:
				case Key.NumPad8:
				case Key.NumPad9:
				case Key.Divide:
				case Key.Multiply:
				case Key.Subtract:
				case Key.Add:
				case Key.Decimal:
				case Key.Clear:
					modifiers |= CefEventFlags.IsKeyPad;
					break;
				case Key.LeftShift:
				case Key.LeftCtrl:
				case Key.LeftAlt:
				case Key.LWin:
					modifiers |= CefEventFlags.IsLeft;
					break;
				case Key.RightShift:
				case Key.RightCtrl:
				case Key.RightAlt:
				case Key.RWin:
					modifiers |= CefEventFlags.IsRight;
					break;
				case Key.System:
					if (e.SystemKey == Key.LeftAlt)
						modifiers |= CefEventFlags.IsLeft;
					else if (e.SystemKey == Key.RightAlt)
						modifiers |= CefEventFlags.IsRight;
					break;
			}
			return modifiers;
		}

		/// <summary>
		/// Sets the current input locale identifier for the UI thread in the browser.
		/// </summary>
		protected void SetKeyboardLayoutForCefUIThreadIfNeeded()
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
				CefNetApi.Post(CefThreadId.UI, () => {
					NativeMethods.ActivateKeyboardLayout(hkl, 0);
					_keyboardLayout = hkl;
				});
			}
		}

	}
}
