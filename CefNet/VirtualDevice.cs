using System;
using System.Collections.Generic;
using System.Text;

namespace CefNet
{
	public class VirtualDevice
	{
		private float _scale;
		private CefScreenInfo _screenInfo;

		public VirtualDevice(CefScreenInfo screenInfo)
		{
			_scale = 1;
			_screenInfo = screenInfo;
		}

		/// <summary>
		/// Gets or sets the distance between the left edge of the device and the left edge of its drawing surface area.
		/// </summary>
		public int X { get; set; }

		/// <summary>
		/// Gets or sets the distance between the top edge of the device and the top edge of its drawing surface area.
		/// </summary>
		public int Y { get; set; }

		public CefScreenInfo ScreenInfo
		{
			get { return _screenInfo; }
		}

		public float DevicePixelRatio
		{
			get { return _screenInfo.DeviceScaleFactor; }
		}

		public float Scale
		{
			get
			{
				return _scale;
			}
			set
			{
				if (_scale <= 0)
					throw new ArgumentOutOfRangeException(nameof(value));
				_scale = value;
			}
		}

		public virtual CefRect ViewportRect
		{
			get { return _screenInfo.AvailableRect; }
		}

		public virtual void Rotate()
		{
			CefRect screenRect = _screenInfo.Rect;
			CefRect screenAvail = _screenInfo.AvailableRect;
			_screenInfo.Rect = new CefRect(screenRect.Y, screenRect.X, screenRect.Height, screenRect.Width);
			_screenInfo.AvailableRect = new CefRect(screenAvail.Y, screenAvail.X, screenAvail.Height, screenAvail.Width);
		}

		public CefRect GetBounds(float pixelPerDip)
		{
			CefRect viewportRect = this.ViewportRect;
			return new CefRect(
				(int)(X * pixelPerDip),
				(int)(Y * pixelPerDip),
				(int)(viewportRect.Width * Scale * pixelPerDip),
				(int)(viewportRect.Height * Scale * pixelPerDip)
			);
		}

		public void ScaleToViewport(ref CefRect rect, float pixelPerDip)
		{
			rect = new CefRect(
				(int)(rect.X * Scale * pixelPerDip),
				(int)(rect.Y * Scale * pixelPerDip),
				(int)(rect.Width * Scale * pixelPerDip),
				(int)(rect.Height * Scale * pixelPerDip)
			);
		}

		/// <summary>
		/// Translates the device point to drawing surface point.
		/// </summary>
		/// <param name="rect"></param>
		/// <param name="pixelPerDip"></param>
		public void MoveToDevice(ref CefRect rect, float pixelPerDip)
		{
			rect.X += (int)(X * pixelPerDip);
			rect.Y += (int)(Y * pixelPerDip);
		}

		/// <summary>
		/// Computes the location of the specified view point into screen coordinates. 
		/// </summary>
		/// <param name="point"></param>
		/// <returns>
		/// Return true if the screen coordinates were provided.
		/// </returns>
		public virtual bool PointToScreen(ref CefPoint point)
		{
			return false;
		}

		/// <summary>
		/// Returns the root window rectangle in screen coordinates.
		/// </summary>
		/// <returns></returns>
		public virtual CefRect GetRootBounds()
		{
			return ViewportRect;
		}

	}
}
