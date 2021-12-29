using System;
using System.Collections.Generic;
using System.Text;

namespace CefNet
{
	/// <summary>
	/// Provides data for the <see cref="IChromiumWebView.CefPaint"/> event.
	/// </summary>
	public class CefPaintEventArgs : EventArgs
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="CefPaintEventArgs"/> class.
		/// </summary>
		/// <param name="browser">The <see cref="CefBrowser"/> that triggered the event.</param>
		/// <param name="type">A paint element type.</param>
		/// <param name="dirtyRects">Specifies areas of the bitmap that changed.</param>
		/// <param name="buffer">The pointer to an array of bytes that contains the BGRA pixel data.</param>
		/// <param name="width">The width, in pixels, of the bitmap.</param>
		/// <param name="height">The height, in pixels, of the bitmap.</param>
		public CefPaintEventArgs(CefBrowser browser, CefPaintElementType type, CefRect[] dirtyRects, IntPtr buffer, int width, int height)
		{
			Browser = browser;
			PaintElementType = type;
			DirtyRects = dirtyRects;
			Buffer = buffer;
			Width = width;
			Height = height;
		}

		/// <summary>
		/// Gets the <see cref="CefBrowser"/> that triggered the event.
		/// </summary>
		public CefBrowser Browser { get; }

		/// <summary>
		/// Gets a value indicating whether the element is the view or the popup widget.
		/// </summary>
		public CefPaintElementType PaintElementType { get; }

		/// <summary>
		/// Gets areas of the bitmap that changed.
		/// </summary>
		public CefRect[] DirtyRects { get; }

		/// <summary>
		/// Gets the address of the first pixel data in the BGRA bitmap.
		/// </summary>
		public IntPtr Buffer { get; }

		/// <summary>
		/// Gets the width, in pixels, of the bitmap.
		/// </summary>
		public int Width { get; }

		/// <summary>
		/// Gets the height, in pixels, of the bitmap.
		/// </summary>
		public int Height { get; }
	}
}
