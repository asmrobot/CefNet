using CefNet.Input;
using CefNet.Internal;
using CefNet.WinApi;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace CefNet.Windows.Forms
{


	partial class WebView: IWinFormsWebViewPrivate
	{
		private CefEventFlags _keydownModifiers;
		private IntPtr _keyboardLayout;

		/// <summary>
		/// Occurs when the user starts dragging content in the web view.
		/// </summary>
		/// <remarks>
		/// OS APIs that run a system message loop may be used within the StartDragging event handler.
		/// Call <see cref="WebView.DragSourceEndedAt"/> and <see cref="WebView.DragSourceSystemDragEnded"/>
		/// either synchronously or asynchronously to inform the web view that the drag operation has ended.
		/// </remarks>
		public event EventHandler<StartDraggingEventArgs> StartDragging;

		/// <summary>
		/// Gets emulated device.
		/// </summary>
		protected VirtualDevice Device { get; private set; }

		/// <summary>
		/// Enable or disable device simulation.
		/// </summary>
		/// <param name="device">The simulated device or null.</param>
		public void SimulateDevice(VirtualDevice device)
		{
			if (IsHandleCreated)
			{
				VerifyAccess();
			}

			if (Device == device)
				return;

			OffscreenGraphics offscreenGraphics = this.OffscreenGraphics;
			if (offscreenGraphics != null)
				offscreenGraphics.Device = device;
			
			if (IsHandleCreated)
			{
				if (!WindowlessRenderingEnabled)
					throw new InvalidOperationException();
				OnSizeChanged(EventArgs.Empty);
			}
			else
			{
				Device = device;
			}
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

		/// <summary>
		/// Gets and sets a value indicating whether the browser using off-screen rendering.
		/// </summary>
		public bool WindowlessRenderingEnabled
		{
			get
			{
				return OffscreenGraphics != null;
			}
			set
			{
				SetInitProperty(InitialPropertyKeys.Windowless, value);

				if (value)
				{
					if (OffscreenGraphics == null)
					{
						VirtualDevice device = this.Device;
						OffscreenGraphics = new OffscreenGraphics { Background = this.BackColor, Device = device };
						if(device is null)
							OffscreenGraphics.SetSize(this.Width, this.Height);
						else
							OffscreenGraphics.SetSize(device.ViewportRect.Width, device.ViewportRect.Height);
					}
				}
				else
				{
					OffscreenGraphics = null;
				}
			}
		}

		protected override void OnBackColorChanged(EventArgs e)
		{
			OffscreenGraphics offscreenGraphics = this.OffscreenGraphics;
			if (offscreenGraphics != null) 
				offscreenGraphics.Background = this.BackColor;

			base.OnBackColorChanged(e);
		}

		protected override void OnHandleCreated(EventArgs e)
		{
			if (OffscreenGraphics != null)
				OffscreenGraphics.WidgetHandle = this.Handle;
			else
				this.Device = null;

			float devicePixelRatio;
			using (var g = CreateGraphics())
			{
				devicePixelRatio = g.DpiX / 96f;
			}

			if (OffscreenGraphics.PixelsPerDip != devicePixelRatio)
			{
				SetDevicePixelRatio(devicePixelRatio);
			}
			else
			{
				OnSizeChanged(EventArgs.Empty);
			}

			// enable WM_TOUCH
			const int TWF_WANTPALM = 0x00000002;
			const int SM_MAXIMUMTOUCHES = 95;
			if (NativeMethods.GetSystemMetrics(SM_MAXIMUMTOUCHES) > 0)
				NativeMethods.RegisterTouchWindow(Handle, TWF_WANTPALM);

			base.OnHandleCreated(e);
		}

		protected override void OnHandleDestroyed(EventArgs e)
		{
			if (GetState(State.Created) && !GetState(State.Closing))
			{
				OnDestroyBrowser();
			}

			if (OffscreenGraphics != null)
				OffscreenGraphics.WidgetHandle = IntPtr.Zero;

			base.OnHandleDestroyed(e);
		}


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
				NativeMethods.MapWindowPoints(OffscreenGraphics.WidgetHandle, IntPtr.Zero, ref point, 1);
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
				const int GA_ROOT = 2;

				RECT windowBounds;
				IntPtr hwnd = NativeMethods.GetAncestor(OffscreenGraphics.WidgetHandle, GA_ROOT);
				if ((NativeMethods.DwmIsCompositionEnabled() && NativeMethods.DwmGetWindowAttribute(hwnd, DWMWINDOWATTRIBUTE.ExtendedFrameBounds, &windowBounds, sizeof(RECT)) == 0)
					|| NativeMethods.GetWindowRect(hwnd, out windowBounds))
				{
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

		void IWinFormsWebViewPrivate.RaiseCefCursorChange(CursorChangeEventArgs e)
		{
			RaiseCrossThreadEvent(OnCursorChange, e, true);
		}

		/// <summary>
		/// Called when the browser&apos;s cursor has changed.
		/// </summary>
		/// <param name="e">A <see cref="CursorChangeEventArgs"/> that contains the event data.</param>
		protected virtual void OnCursorChange(CursorChangeEventArgs e)
		{
			if (this.WindowlessRenderingEnabled)
				this.Cursor = e.Cursor;
		}

		void IWinFormsWebViewPrivate.CefSetToolTip(string text)
		{
			RaiseCrossThreadEvent(OnSetToolTip, new TooltipEventArgs(text), false);
		}

		protected virtual void OnSetToolTip(TooltipEventArgs e)
		{
			ToolTip toolTip = this.ToolTip;
			if (toolTip != null && toolTip.GetToolTip(this) != e.Text)
			{
				toolTip.SetToolTip(this, e.Text);
			}
		}

		void IWinFormsWebViewPrivate.RaiseStartDragging(StartDraggingEventArgs e)
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
			
			DoDragDrop(new CefNetDragData(this, e.Data), e.AllowedEffects.ToDragDropEffects());
			DragSourceSystemDragEnded();
			e.Handled = true;
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
			if (this.DesignMode)
			{
				e.Graphics.DrawString(this.GetType().Name, Font, Brushes.Black, new PointF(2, 2));
				return;
			}

			SetDevicePixelRatio(e.Graphics.DpiX / 96f);
			if (WindowlessRenderingEnabled)
			{
				Rectangle renderBounds = OffscreenGraphics.GetRenderBounds();
				DrawDeviceArea(e.Graphics, e.ClipRectangle);
				OffscreenGraphics.Render(e.Graphics, e.ClipRectangle);

				// redraw background if render has wrong size
				VirtualDevice device = Device;
				if (device != null)
				{
					CefRect deviceBounds = device.GetBounds(OffscreenGraphics.PixelsPerDip);
					if (renderBounds.Width > deviceBounds.Width || renderBounds.Height > deviceBounds.Height)
					{
						e.Graphics.ExcludeClip(deviceBounds.ToRectangle());
						DrawDeviceArea(e.Graphics, ClientRectangle);
					}
				}
			}
			base.OnPaint(e);
		}

		protected virtual void DrawDeviceArea(Graphics graphics, Rectangle rectangle)
		{
			VirtualDevice device = Device;
			if (device != null)
			{
				CefRect deviceBounds = device.GetBounds(OffscreenGraphics.PixelsPerDip);
				Color background = this.BackColor;
				if (background.A > 0)
				{
					using (var brush = new SolidBrush(background))
					{
						graphics.FillRectangle(brush, rectangle);
					}
				}
				graphics.DrawRectangle(Pens.Gray, deviceBounds.X - 1, deviceBounds.Y - 1, deviceBounds.Width + 1, deviceBounds.Height + 1);
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

			try
			{
				if (invalidRect.Width > 0 && invalidRect.Height > 0)
				{
					bool useInvalidate = false;

					Graphics graphics = null;
					try
					{
						if (!IsHandleCreated)
							return;
						graphics = CreateGraphics();

						Rectangle renderBounds = OffscreenGraphics.GetRenderBounds();
						OffscreenGraphics.Render(graphics, invalidRect.ToRectangle());

						// draw background if render has wrong size
						VirtualDevice device = Device;
						if (device != null)
						{
							CefRect deviceBounds = device.GetBounds(OffscreenGraphics.PixelsPerDip);
							if (renderBounds.Width > deviceBounds.Width || renderBounds.Height > deviceBounds.Height)
							{
								graphics.ExcludeClip(deviceBounds.ToRectangle());
								DrawDeviceArea(graphics, ClientRectangle);
							}
						}
					}
					catch (ObjectDisposedException) { throw; }
					catch { useInvalidate = true; }
					finally
					{
						graphics?.Dispose();
					}

					if (useInvalidate)
					{
						if (!IsHandleCreated)
							return;
						Invalidate(invalidRect.ToRectangle(), false);
					}
				}
				else
				{
					if (!IsHandleCreated)
						return;
					Invalidate();
				}
			}
			catch (ObjectDisposedException) { }
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
			Invalidate(invalidRect, false);
		}

		/// <summary>
		/// Raises the <see cref="Control.SizeChanged"/> event.
		/// </summary>
		/// <param name="e">An <see cref="EventArgs"/> that contains event data.</param>
		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged(e);

			if (WindowlessRenderingEnabled)
			{
				UpdateOffscreenViewLocation();

				VirtualDevice device = this.Device;
				if (device is null)
				{
					if (OffscreenGraphics.SetSize(this.Width, this.Height))
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
		}

		protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
		{
			if (specified == BoundsSpecified.All || specified == BoundsSpecified.None)
			{
				if (width == 0 || height == 0)
					return;
			}
			else if(specified == BoundsSpecified.Width && width == 0)
			{
				return;
			}
			else if(specified == BoundsSpecified.Height && height == 0)
			{
				return;
			}
			base.SetBoundsCore(x, y, width, height, specified);
		}

		protected override Size DefaultSize
		{
			get { return new Size(100, 100); }
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			if (PointInViewport(e.X, e.Y))
			{
				if (WindowlessRenderingEnabled)
				{
					CefEventFlags modifiers = GetModifierKeys();
					if (e.Button == MouseButtons.Left)
						modifiers |= CefEventFlags.LeftMouseButton;
					if (e.Button == MouseButtons.Right)
						modifiers |= CefEventFlags.RightMouseButton;
					SendMouseMoveEvent(e.X, e.Y, modifiers);
				}
			}
			base.OnMouseMove(e);
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			if (WindowlessRenderingEnabled)
			{
				SendMouseLeaveEvent();
			}
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

		protected override void OnMouseDown(MouseEventArgs e)
		{
			if (PointInViewport(e.X, e.Y))
			{
				if (WindowlessRenderingEnabled)
				{
					SendMouseDownEvent(e.X, e.Y, GetButton(e), e.Clicks, GetModifierKeys());
				}
			}
			base.OnMouseDown(e);
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			if (PointInViewport(e.X, e.Y))
			{
				if (WindowlessRenderingEnabled)
				{
					SendMouseUpEvent(e.X, e.Y, GetButton(e), e.Clicks, GetModifierKeys());
				}
			}
		}

		protected override void OnMouseWheel(MouseEventArgs e)
		{
			base.OnMouseWheel(e);

			if (!ModifierKeys.HasFlag(Keys.Shift))
			{
				if (PointInViewport(e.X, e.Y))
				{
					if (WindowlessRenderingEnabled)
					{
						SendMouseWheelEvent(e.X, e.Y, 0, e.Delta);
						((HandledMouseEventArgs)e).Handled = true;
					}
				}
				return;
			}
			OnMouseHWheel(e);
		}

		protected virtual void OnMouseHWheel(MouseEventArgs e)
		{
			if (PointInViewport(e.X, e.Y))
			{
				if (WindowlessRenderingEnabled)
				{
					SendMouseWheelEvent(e.X, e.Y, e.Delta, 0);
				}
			}
			((HandledMouseEventArgs)e).Handled = true;
		}

		private void WmMouseHWheel(ref Message m)
		{
			Point p = PointToClient(new Point(NativeMethods.LoWord(m.LParam), NativeMethods.HiWord(m.LParam)));
			int delta = -NativeMethods.HiWord(m.WParam);
			if (Math.Abs(delta) < 10)
			{
				delta = Math.Sign(delta) * 10;
			}
			HandledMouseEventArgs handledMouseEventArgs = new HandledMouseEventArgs(MouseButtons.None, 0, p.X, p.Y, delta);
			OnMouseHWheel(handledMouseEventArgs);
			m.Result = new IntPtr((!handledMouseEventArgs.Handled) ? 1 : 0);
			if (!handledMouseEventArgs.Handled)
			{
				DefWndProc(ref m);
			}
		}

		protected override void OnDragEnter(DragEventArgs e)
		{
			base.OnDragEnter(e);

			Point mousePos = PointToClient(new Point(e.X, e.Y));
			if (WindowlessRenderingEnabled && PointInViewport(mousePos.X, mousePos.Y))
			{
				SendDragEnterEvent(mousePos.X, mousePos.Y, e.GetModifiers(), e.GetCefDragData(), e.AllowedEffect.ToCefDragOperationsMask());
				e.Effect = DragDropEffects.Copy & e.AllowedEffect;
			}
		}

		protected override void OnDragOver(DragEventArgs e)
		{
			base.OnDragOver(e);

			Point mousePos = PointToClient(new Point(e.X, e.Y));
			if (WindowlessRenderingEnabled && PointInViewport(mousePos.X, mousePos.Y))
			{
				SendDragOverEvent(mousePos.X, mousePos.Y, e.GetModifiers(), e.AllowedEffect.ToCefDragOperationsMask());
			}
		}

		protected override void OnDragLeave(EventArgs e)
		{
			base.OnDragLeave(e);

			if (!WindowlessRenderingEnabled)
				return;

			SendDragLeaveEvent();
		}

		protected override void OnDragDrop(DragEventArgs e)
		{
			base.OnDragDrop(e);

			if (!WindowlessRenderingEnabled)
				return;

			Point mousePos = PointToClient(new Point(e.X, e.Y));
			if (WindowlessRenderingEnabled && PointInViewport(mousePos.X, mousePos.Y))
			{
				SendDragDropEvent(mousePos.X, mousePos.Y, e.GetModifiers());
				if (e.Data.GetDataPresent(nameof(CefNetDragData)))
				{
					CefNetDragData data = (CefNetDragData)e.Data.GetData(nameof(CefNetDragData));
					if (data != null && data.Source == this)
					{
						DragSourceEndedAt(mousePos.X, mousePos.Y, e.AllowedEffect.ToCefDragOperationsMask());
					}
				}
			}
		}

		private bool ProcessWindowlessMessage(ref Message m)
		{
			const int MA_ACTIVATE = 0x1;
			const int WM_MOUSEHWHEEL = 0x20E;
			const int WM_INPUTLANGCHANGE = 0x0051;

			if (IsMouseEventFromTouch(in m))
				return true;

			switch (m.Msg)
			{
				case WM_INPUTLANGCHANGE:
					SetKeyboardLayoutForCefUIThreadIfNeeded();
					return false;
				case WM_MOUSEHWHEEL:
					WmMouseHWheel(ref m);
					return true;
				case 0x21: // WM_MOUSEACTIVATE:
					Focus();
					m.Result = new IntPtr(MA_ACTIVATE);
					return true;
				case 0x0083: //	WM_NCCALCSIZE:
					m.Result = IntPtr.Zero;
					return true;
				case 0x114: // WM_HSCROLL:
					short delta;
					switch (m.WParam.ToInt64())
					{
						case 0: // SB_LINELEFT
							delta = -1;
							break;
						case 1: // SB_LINERIGHT
							delta = 1;
							break;
						default:
							base.WndProc(ref m);
							return true;
					}
					Point mousePos = Control.MousePosition;
					NativeMethods.PostMessage(m.HWnd, WM_MOUSEHWHEEL, NativeMethods.MakeParam(delta, 0), NativeMethods.MakeParam((short)mousePos.Y, (short)mousePos.X));
					m.Result = IntPtr.Zero;
					return true;
				case 0x84: // WM_NCHITTEST
					m.Result = new IntPtr(1); // HTCLIENT
					return true;
				case 0x0240: // WM_TOUCH
					if (!OnTouch(ref m))
						return false;
					m.Result = IntPtr.Zero;
					return true;
				case 0x0020: // WM_SETCURSOR
					if (m.LParam == new IntPtr(1))
						return false;
					m.Result = new IntPtr(1);
					return true;
			}
			return false;
		}

		private unsafe bool OnTouch(ref Message m)
		{
			int inputCount = NativeMethods.LoWord(m.WParam);

			TOUCHINPUT[] inputs;
			inputs = new TOUCHINPUT[inputCount];

			bool handled = false;
			fixed (TOUCHINPUT* pInputs = inputs)
			{
				if (!NativeMethods.GetTouchInputInfo(m.LParam, inputCount, pInputs, sizeof(TOUCHINPUT)))
					return false;
				
				const int TOUCHEVENTF_DOWN = 0x0002;
				const int TOUCHEVENTF_MOVE = 0x0001;
				const int TOUCHEVENTF_UP = 0x0004;
				const int TOUCHINPUTMASKF_CONTACTAREA = 0x0004;

				for (int i = 0; i < inputCount; i++)
				{
					TOUCHINPUT* touchInput = &pInputs[i];

					var eventInfo = new CefTouchEvent();
					eventInfo.Id = touchInput->id;
					eventInfo.PointerType = CefPointerType.Touch;
					Point p = PointToClient(new Point(touchInput->x / 100, touchInput->y / 100));
					CefPoint cp = new CefPoint(p.X, p.Y);
					cp = PointToViewport(cp);
					eventInfo.X = cp.X;
					eventInfo.Y = cp.Y;

					if ((touchInput->mask & TOUCHINPUTMASKF_CONTACTAREA) != 0)
					{
						eventInfo.RadiusX = touchInput->cxContact / 200;
						eventInfo.RadiusY = touchInput->cyContact / 200;
					}

					if ((touchInput->flags & TOUCHEVENTF_DOWN) != 0)
						eventInfo.Type = CefTouchEventType.Pressed;
					else if ((touchInput->flags & TOUCHEVENTF_UP) != 0)
						eventInfo.Type = CefTouchEventType.Released;
					else if ((touchInput->flags & TOUCHEVENTF_MOVE) != 0)
						eventInfo.Type = CefTouchEventType.Moved;

					SendTouchEvent(eventInfo);

					handled = true;
				}
			}

			NativeMethods.CloseTouchInputHandle(m.LParam);

			return handled;
		}

		protected override bool ProcessCmdKey(ref Message m, Keys keyData)
		{
			const int WM_SYSKEYDOWN = 0x0104;
			const int WM_KEYDOWN = 0x0100;

			if (WindowlessRenderingEnabled)
			{
				if (m.Msg == WM_KEYDOWN || m.Msg == WM_SYSKEYDOWN)
				{
					Keys key = (Keys)m.WParam.ToInt64();
					if ((key >= Keys.PageUp && key <= Keys.Down) || key == Keys.Tab)
					{
						var e = new CefKeyEvent();
						e.WindowsKeyCode = unchecked((int)m.WParam);
						e.NativeKeyCode = CefNet.Input.KeycodeConverter.GetWindowsScanCodeFromLParam(m.LParam);
						e.IsSystemKey = (m.Msg == WM_SYSKEYDOWN);
						e.Type = CefKeyEventType.RawKeyDown;
						e.Modifiers = (uint)GetCefKeyboardModifiers((Keys)m.WParam.ToInt64(), m.LParam);
						this.BrowserObject?.Host.SendKeyEvent(e);
						return true;
					}
				}
			}
			return base.ProcessCmdKey(ref m, keyData);
		}

		protected unsafe override bool ProcessKeyEventArgs(ref Message m)
		{
			const int WM_KEYDOWN = 0x0100;
			const int WM_KEYUP = 0x0101;
			const int WM_SYSKEYDOWN = 0x0104;
			const int WM_SYSKEYUP = 0x0105;
			const int WM_SYSCHAR = 0x0106;
			const int KF_REPEAT = 0x4000;

			if (WindowlessRenderingEnabled)
			{
				var k = new CefKeyEvent();
				k.WindowsKeyCode = unchecked((int)m.WParam);
				k.NativeKeyCode = CefNet.Input.KeycodeConverter.GetWindowsScanCodeFromLParam(m.LParam);
				k.IsSystemKey = m.Msg >= WM_SYSKEYDOWN && m.Msg <= WM_SYSCHAR;
				
				CefEventFlags modifiers;
				if (m.Msg == WM_KEYDOWN || m.Msg == WM_SYSKEYDOWN)
				{
					modifiers = GetCefKeyboardModifiers((Keys)m.WParam.ToInt64(), m.LParam);
					if ((((uint)m.LParam.ToPointer() >> 16) & KF_REPEAT) != 0)
						modifiers |= CefEventFlags.IsRepeat;
					_keydownModifiers = modifiers;
					k.Type = CefKeyEventType.RawKeyDown;
					SetKeyboardLayoutForCefUIThreadIfNeeded();
				}
				else if (m.Msg == WM_KEYUP || m.Msg == WM_SYSKEYUP)
				{
					modifiers = GetCefKeyboardModifiers((Keys)m.WParam.ToInt64(), m.LParam);
					if (_keydownModifiers.HasFlag(CefEventFlags.IsRight))
						modifiers = CefEventFlags.IsRight & ~CefEventFlags.IsLeft;
					_keydownModifiers = CefEventFlags.None;
					k.Type = CefKeyEventType.KeyUp;
					SetKeyboardLayoutForCefUIThreadIfNeeded();
				}
				else
				{
					k.Type = CefKeyEventType.Char;
					modifiers = GetCefKeyboardModifiers((Keys)NativeMethods.MapVirtualKey(((uint)m.LParam.ToPointer() >> 16) & 0xFFU, MapVirtualKeyType.MAPVK_VSC_TO_VK_EX), m.LParam);
				}
				k.Modifiers = (uint)modifiers;

				this.BrowserObject?.Host.SendKeyEvent(k);
			}
			return base.ProcessKeyEventArgs(ref m);
		}

		protected static CefEventFlags GetModifierKeys()
		{
			CefEventFlags modifiers = CefEventFlags.None;
			Keys modKeys = Control.ModifierKeys;
			if (modKeys.HasFlag(Keys.Shift))
				modifiers |= CefEventFlags.ShiftDown;
			if (modKeys.HasFlag(Keys.Control))
				modifiers |= CefEventFlags.ControlDown;
			if (modKeys.HasFlag(Keys.Alt))
				modifiers |= CefEventFlags.AltDown;
			return modifiers;
		}

		protected unsafe CefEventFlags GetCefKeyboardModifiers(Keys key, IntPtr lparam)
		{
			const int KF_EXTENDED = 0x100;

			CefEventFlags modifiers = GetModifierKeys();

			if (IsKeyLocked(Keys.NumLock))
				modifiers |= CefEventFlags.NumLockOn;
			if (IsKeyLocked(Keys.CapsLock))
				modifiers |= CefEventFlags.CapsLockOn;

			switch (key)
			{
				case Keys.Return:
					if ((((uint)lparam.ToPointer() >> 16) & KF_EXTENDED) != 0)
						modifiers |= CefEventFlags.IsKeyPad;
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
					if ((((uint)lparam.ToPointer() >> 16) & KF_EXTENDED) == 0)
						modifiers |= CefEventFlags.IsKeyPad;
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
				case Keys.LMenu:
				case Keys.LWin:
					modifiers |= CefEventFlags.IsLeft;
					break;
				case Keys.RShiftKey:
				case Keys.RControlKey:
				case Keys.RMenu:
				case Keys.RWin:
					modifiers |= CefEventFlags.IsLeft;
					break;
				case Keys.ShiftKey:
					if (NativeMethods.GetKeyState(VirtualKeys.RShiftKey).HasFlag(KeyState.Pressed))
						modifiers |= CefEventFlags.IsRight;
					else
						modifiers |= CefEventFlags.IsLeft;
					break;
				case Keys.ControlKey:
					if (NativeMethods.GetKeyState(VirtualKeys.RControlKey).HasFlag(KeyState.Pressed))
						modifiers |= CefEventFlags.IsRight;
					else
						modifiers |= CefEventFlags.IsLeft;
					break;
				case Keys.Menu:
					if (NativeMethods.GetKeyState(VirtualKeys.RMenu).HasFlag(KeyState.Pressed))
						modifiers |= CefEventFlags.IsRight;
					else
						modifiers |= CefEventFlags.IsLeft;
					break;
				default:
					if ((((uint)lparam.ToPointer() >> 16) & KF_EXTENDED) != 0)
						modifiers |= CefEventFlags.IsKeyPad;
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

		/// <summary>
		/// Helper function to detect mouse messages coming from emulation of touch
		/// events. These should be ignored.
		/// </summary>
		protected static bool IsMouseEventFromTouch(in Message message)
		{
			const int WM_MOUSEFIRST = 0x0200;
			const int WM_MOUSELAST = 0x20D;
			const uint MOUSEEVENTF_FROMTOUCH = 0xFF515700;

			return (message.Msg >= WM_MOUSEFIRST) && (message.Msg <= WM_MOUSELAST) &&
				(NativeMethods.GetMessageExtraInfo().ToInt64() & MOUSEEVENTF_FROMTOUCH) == MOUSEEVENTF_FROMTOUCH;
		}

	}
}
