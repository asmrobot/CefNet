using System;
using System.Collections.Generic;
using System.Text;

namespace CefNet
{
	/// <summary>
	/// Defines the capture settings.
	/// </summary>
	public sealed class PageCaptureSettings
	{
		private int? _quality;

		/// <summary>
		/// Image compression format (defaults to png).
		/// </summary>
		public ImageCompressionFormat Format { get; set; }

		/// <summary>
		/// Compression quality from range [0..100] (jpeg only).
		/// </summary>
		public int? Quality
		{
			get
			{
				return _quality;
			}
			set
			{
				if (value != null)
				{
					if (value.Value < 0 || value.Value > 100)
						throw new ArgumentOutOfRangeException(nameof(value));
				}
				_quality = value;
			}
		}

		/// <summary>
		/// Capture the screenshot of a given region only.
		/// </summary>
		public PageViewport Viewport { get; set; }

		/// <summary>
		/// Capture the screenshot from the surface, rather than the view.
		/// Defaults to true.
		/// </summary>
		public bool FromSurface { get; set; } = true;

		/// <summary>
		/// Capture the screenshot beyond the viewport.
		/// </summary>
		public bool CaptureBeyondViewport { get; set; }

	}

	/// <summary>
	/// Image compression format.
	/// </summary>
	public enum ImageCompressionFormat
	{
		Png,
		Jpeg
	}

	/// <summary>
	/// Viewport for capturing screenshot.
	/// </summary>
	public sealed class PageViewport
	{
		public PageViewport(double x, double y, double width, double height)
			: this(x, y, width, height, 1.0)
		{

		}

		public PageViewport(double x, double y, double width, double height, double scale)
		{
			this.X = x;
			this.Y = y;
			this.Width = width;
			this.Height = height;
			this.Scale = scale;
		}

		/// <summary>
		/// X offset in device independent pixels (dip).
		/// </summary>
		public double X { get; }

		/// <summary>
		/// Y offset in device independent pixels (dip).
		/// </summary>
		public double Y { get; }

		/// <summary>
		/// Rectangle width in device independent pixels (dip).
		/// </summary>
		public double Width { get; }

		/// <summary>
		/// Rectangle height in device independent pixels (dip).
		/// </summary>
		public double Height { get; }

		/// <summary>
		/// Page scale factor.
		/// </summary>
		public double Scale { get; }
	}

}
