using CefNet.WinApi;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.Threading;
using CefNet.Windows.Forms;
using System.Runtime.InteropServices;
using System.Drawing.Drawing2D;
using System.Diagnostics;

namespace CefNet.Internal
{
	public sealed class OffscreenGraphics
	{
		private class PixelBuffer : IDisposable
		{
			internal Bitmap Source;
			internal BITMAPINFO DIBInfo;

			public PixelBuffer(int width, int height)
			{
				DIBInfo = new BITMAPINFO { Size = 40, BitCount = 32, Planes = 1, Width = width, Height = -height, SizeImage = width * height * 4 };
				Source = new Bitmap(width, height, PixelFormat.Format32bppArgb);
			}

			~PixelBuffer()
			{
				Dispose(false);
			}

			private void Dispose(bool disposing)
			{
				Source?.Dispose();
				Source = null;
			}

			public void Dispose()
			{
				Dispose(true);
				GC.SuppressFinalize(this);
			}

			public int Stride
			{
				get { return DIBInfo.Width * 4; }
			}

			public int Size
			{
				get
				{
					return DIBInfo.SizeImage;
				}
			}

			public int Width
			{
				get { return DIBInfo.Width; }
			}

			public int Height
			{
				get { return -DIBInfo.Height; }
			}

		}

		private PixelBuffer ViewPixels;
		private PixelBuffer PopupPixels;

		private readonly object _syncRoot;
		private CefRect _bounds;
		private Rectangle _popupBounds;

		public OffscreenGraphics()
		{
			_syncRoot = new object();
			_bounds = new CefRect(0, 0, 1, 1);
		}

		public VirtualDevice Device { get; set; }

		public IntPtr WidgetHandle { get; set; }

		public Color Background { get; set; }

		public static float PixelsPerDip { get; set; } = 1.0f;

		public InterpolationMode InterpolationMode { get; set; } = InterpolationMode.Bilinear;

		public void SetLocation(int x, int y)
		{
			_bounds.X = x;
			_bounds.Y = y;
		}

		public bool SetSize(int width, int height)
		{
			width = Math.Max(width, 1);
			height = Math.Max(height, 1);
			_bounds.Width = width;
			_bounds.Height = height;

			lock (_syncRoot)
			{
				return ViewPixels == null || ViewPixels.Width != width || ViewPixels.Height != height;
			}
		}

		public CefRect GetBounds()
		{
			float ppd = PixelsPerDip;
			if (ppd == 1.0f || Device != null)
				return _bounds;
			return new CefRect((int)(_bounds.X / ppd), (int)(_bounds.Y / ppd), (int)(_bounds.Width / ppd), (int)(_bounds.Height / ppd));
		}

		public CefRect Draw(CefPaintEventArgs e)
		{
			float ppd = OffscreenGraphics.PixelsPerDip;
			VirtualDevice device = this.Device;

			CefRect[] dirtyRects = e.DirtyRects;
			if (dirtyRects.Length == 0)
				return new CefRect();

			CefRect r = dirtyRects[0];
			CefRect invalidRect = new CefRect(r.X, r.Y, r.Width, r.Height);
			for (int i = 1; i < dirtyRects.Length; i++)
			{
				invalidRect.Union(dirtyRects[i]);
			}
			
			if (device != null)
			{
				invalidRect.Scale(device.Scale * ppd / device.DevicePixelRatio);
			}

			if (e.PaintElementType == CefPaintElementType.Popup)
			{
				invalidRect.Offset(_popupBounds.X, _popupBounds.Y);
			}

			if (invalidRect.IsNullSize)
				return new CefRect();

			lock (_syncRoot)
			{
				int width = e.Width;
				int height = e.Height;

				if (device != null)
				{
					if (e.PaintElementType == CefPaintElementType.View)
					{
						width = (int)(_bounds.Width * device.Scale * ppd);
						height = (int)(_bounds.Height * device.Scale * ppd);
					}
					else if (e.PaintElementType == CefPaintElementType.Popup)
					{
						width = (int)(e.Width / device.DevicePixelRatio * device.Scale * ppd);
						height = (int)(e.Height / device.DevicePixelRatio * device.Scale * ppd);
					}
				}

				PixelBuffer pixelBuffer;
				if (e.PaintElementType == CefPaintElementType.View)
				{
					if (ViewPixels == null || ViewPixels.Width != width || ViewPixels.Height != height)
					{
						if (ViewPixels != null)
							ViewPixels.Dispose();

						ViewPixels = new PixelBuffer(width, height);
					}
					pixelBuffer = ViewPixels;
				}
				else if (e.PaintElementType == CefPaintElementType.Popup)
				{
					if (PopupPixels == null || PopupPixels.Width != width || PopupPixels.Height != height)
					{
						if (PopupPixels != null)
							PopupPixels.Dispose();
						PopupPixels = new PixelBuffer(width, height);
					}
					pixelBuffer = PopupPixels;
				}
				else
				{
					return new CefRect();
				}

				using (Graphics g = Graphics.FromImage(pixelBuffer.Source))
				{
					Color background = this.Background;
					if (background.A > 0)
					{
						using (var brush = new SolidBrush(background))
						{
							g.FillRectangle(brush, 0, 0, width, height);
						}
					}

					using (var bitmap = new Bitmap(e.Width, e.Height, e.Width << 2, PixelFormat.Format32bppArgb, e.Buffer))
					{
						if (e.Width != width || e.Height != height)
						{
							g.CompositingQuality = CompositingQuality.HighSpeed;
							g.InterpolationMode = this.InterpolationMode;
							g.DrawImage(bitmap, 0, 0, width, height);
						}
						else
						{
							g.DrawImage(bitmap, 0, 0);
						}
						g.Flush();
					}
				}

				
			}

			invalidRect.Inflate(2, 2);
			return invalidRect;
		}

		private unsafe void DrawPixels(PixelBuffer pixelBuffer, Graphics g, Rectangle r, int x, int y)
		{
			if (!Monitor.IsEntered(_syncRoot))
				throw new InvalidOperationException();

			r.Offset(-x, -y);

			Rectangle bitmapRect = new Rectangle(0, 0, pixelBuffer.Width, pixelBuffer.Height);
			r.Intersect(bitmapRect);
			if (r.Width == 0 || r.Height == 0)
				return;

			//g.DrawImage(pixelBuffer.Source, new Point(r.X + x, r.Y + y));

			IntPtr hdc = g.GetHdc();
			try
			{
				BitmapData DIB = pixelBuffer.Source.LockBits(new Rectangle(0, 0, pixelBuffer.Width, pixelBuffer.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
				try
				{
					NativeMethods.SetDIBitsToDevice(hdc, r.X + x, r.Y + y, r.Width, r.Height, r.X, bitmapRect.Height, r.Bottom, bitmapRect.Height, DIB.Scan0, ref pixelBuffer.DIBInfo, 0);
				}
				finally
				{
					pixelBuffer.Source.UnlockBits(DIB);
				}
			}
			finally
			{
				g.ReleaseHdc(hdc);
			}
		}

		public unsafe void Render(Graphics g, Rectangle r)
		{
			lock (_syncRoot)
			{
				if (ViewPixels != null)
				{
					float ppd = OffscreenGraphics.PixelsPerDip;

					int offsetX = 0;
					int offsetY = 0;
					VirtualDevice viewport = this.Device;
					if (viewport != null)
					{
						offsetX = viewport.X;
						offsetY = viewport.Y;
					}

					DrawPixels(ViewPixels, g, r, (int)(offsetX * ppd), (int)(offsetY * ppd));

					PixelBuffer pixelBuffer = PopupPixels;
					if (pixelBuffer == null)
						return;

					DrawPixels(pixelBuffer, g, r, (int)(_popupBounds.X + offsetX * ppd), (int)(_popupBounds.Y + offsetY * ppd));
				}
			}
		}

		public void SetPopup(bool visible, CefRect bounds)
		{
			if (visible)
			{
				_popupBounds = bounds.ToRectangle();
			}
			else
			{
				lock (_syncRoot)
				{
					PopupPixels = null;
				}
			}
		}

		public Rectangle GetRenderBounds()
		{
			lock (_syncRoot)
			{
				if (ViewPixels != null)
				{
					return new Rectangle(0, 0, ViewPixels.Width, ViewPixels.Height);
				}
			}
			return new Rectangle();
		}

		public Rectangle GetPopupBounds()
		{
			return _popupBounds;
		}
	}
}
