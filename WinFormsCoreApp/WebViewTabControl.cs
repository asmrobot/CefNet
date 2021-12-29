using CefNet;
using CefNet.WinApi;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsCoreApp
{
	sealed class WebViewTabControl : TabControl
	{
		private const int TCM_ADJUSTRECT = 0x1328;
		private const int TCM_GETITEMRECT = 0x1300 + 10;
		private const int WM_PAINT = 0x000F;

		public WebViewTabControl()
		{

		}

		protected unsafe override void WndProc(ref Message m)
		{
			switch (m.Msg)
			{
				case TCM_ADJUSTRECT:
					// https://stackoverflow.com/a/32055608
					RECT* rect = (RECT*)m.LParam;
					rect->Left = rect->Left-1;
					rect->Top = rect->Top;
					rect->Right = rect->Right + 4;
					rect->Bottom = rect->Bottom + 4;
					break;
				case WM_PAINT:
					base.DefWndProc(ref m);

					Point mousePos = PointToClient(MousePosition);
					int activeTabIndex = SelectedIndex;
					using (Graphics g = CreateGraphics())
					using (var normalPen = new Pen(Brushes.LightGray, 2))
					using (var selectedPen = new Pen(Brushes.Black, 2))
					{
						float scale = g.DpiX / 96f;
						g.SmoothingMode = SmoothingMode.AntiAlias;
						for (int index = 0; index < TabCount; index++)
						{
							Pen pen = normalPen;
							float dy = (index == activeTabIndex) ? -1 : 2;
							if (TryGetTabRect(index, out RECT r))
							{
								float size = 14 * scale;
								var closeEllipseRect = new RectangleF(r.Right - 17 * scale, r.Top + (r.Bottom - r.Top - size) / 2f + dy, size, size);
								if (closeEllipseRect.Contains(mousePos))
								{
									pen = selectedPen;
									g.FillEllipse(Brushes.LightGray, closeEllipseRect);
								}

								size = 5 * scale;
								float x0 = closeEllipseRect.X + (closeEllipseRect.Width - size) / 2f;
								float x1 = x0 + size;
								float y0 = r.Top + (r.Bottom - r.Top - size) / 2f + dy;
								float y1 = y0 + size;
								g.DrawLine(pen, x0, y0, x1, y1);
								g.DrawLine(pen, x0, y1, x1, y0);
							}
						}
					}
					
					return;
			}

			base.WndProc(ref m);
		}



		private bool TryGetActiveCloseButton(MouseEventArgs e, out int index, out Rectangle rect)
		{
			float scale;
			using (var g = CreateGraphics())
			{
				scale = g.DpiX / 96f;
			}

			float size = 14 * scale;
			for (int i = 0; i < TabCount; i++)
			{
				if (TryGetTabRect(i, out RECT r))
				{
					RectangleF closeButton = new RectangleF(r.Right - 17 * scale, r.Top + (r.Bottom - r.Top - size) / 2f, size, size);
					if (closeButton.Contains(e.Location))
					{
						rect = Rectangle.Ceiling(closeButton);
						index = i;
						return true;
					}
				}
			}
			index = 0;
			rect = Rectangle.Empty;
			return false;
		}

		private Rectangle? activeCloseButtonRect;

		private void ActivateCloseButton(Rectangle closeButtonRect)
		{
			activeCloseButtonRect = closeButtonRect;
			closeButtonRect.Inflate(1, 1);
			Invalidate(closeButtonRect, false);
		}

		public bool CloseAllTabs(bool force)
		{
			TabPage tab;
			while ((tab = SelectedTab) != null)
			{
				var ea = new TabPageCloseEventArgs(tab, force);
				OnCloseTab(ea);
				if (ea.Cancel)
					return false;
			}
			return true;
		}

		private void DeactivateCloseButton()
		{
			Rectangle? closeButtonRect = activeCloseButtonRect;
			activeCloseButtonRect = null;
			if (closeButtonRect.HasValue)
			{
				Rectangle r = closeButtonRect.Value;
				r.Inflate(1, 1);
				Invalidate(r, false);
			}
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);

			if (TryGetActiveCloseButton(e, out int index, out Rectangle closeButtonRect))
			{
				if (activeCloseButtonRect.GetValueOrDefault() != closeButtonRect)
				{
					ActivateCloseButton(closeButtonRect);
				}
			}
			else if(activeCloseButtonRect != null)
			{
				DeactivateCloseButton();
			}
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			if (TryGetActiveCloseButton(e, out int index, out Rectangle closeButtonRect))
			{
				closeButtonRect.Inflate(-1, -1);
				if (closeButtonRect.Contains(e.Location))
				{
					OnCloseTab(new TabPageCloseEventArgs(TabPages[index]));
				}
			}
			base.OnMouseDown(e);
		}

		private void OnCloseTab(TabPageCloseEventArgs e)
		{
			if (e.Force || MessageBox.Show(this, "Do you want to close this tab?", e.Tab.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
			{
				TabPages.Remove(e.Tab);
				e.Tab.Dispose();
			}
			else
			{
				e.Cancel = true;
			}
		}

		private unsafe bool TryGetTabRect(int tabIndex, out RECT rect)
		{
			fixed (RECT* pRect = &rect)
			{
				var msg = Message.Create(Handle, TCM_GETITEMRECT, new IntPtr(tabIndex), (IntPtr)(pRect));
				base.WndProc(ref msg);
				return msg.Result != IntPtr.Zero;
			}
		}

		public void NotifyRootMovedOrResized()
		{
			foreach (TabPage tab in TabPages)
			{
				(tab as IWebViewTab)?.WebView.NotifyRootMovedOrResized();
			}
		}

	}
}
