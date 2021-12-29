using System;
using System.Runtime.InteropServices;

namespace CefNet.Linux
{
	/// <summary>
	/// Part of the X11 Screen structure.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	internal struct X11Screen
	{
		public IntPtr ext_data; // XExtData*: hook for extension to hang data
		public IntPtr display; // struct _XDisplay*: back pointer to display structure
		public IntPtr root; // Window: root window ID
		public int width; // width of screen
		public int height; // height of screen
		public int mwidth; // width of screen in millimeters
		public int mheight; // height of screen in millimeters
		public int ndepths; // number of depths possible
		public IntPtr depths; // Depth*: list of allowable depths on the screen
		public int root_depth; // bits per pixel
		public IntPtr root_visual; // Visual*: root visual

		//GC default_gc;                  /* GC for the root root visual */
		//Colormap cmap;                  /* default colormap */
		//unsigned long white_pixel;
		//unsigned long black_pixel;      /* white and black pixel values */
		//int max_maps, min_maps;         /* max and min colormaps */
		//int backing_store;              /* Never, WhenMapped, Always */
		//Bool save_unders;
		//long root_input_mask;           /* initial root input mask */
	}
}
