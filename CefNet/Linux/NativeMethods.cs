using System;
using System.Runtime.InteropServices;
using CefNet.Input.Linux;

namespace CefNet.Linux
{
	static class NativeMethods
	{
		[DllImport("X11", CallingConvention = CallingConvention.Cdecl)]
		public extern static XKeySym XStringToKeysym([MarshalAs(UnmanagedType.LPStr)] string s);

		[DllImport("X11", CallingConvention = CallingConvention.Cdecl)]
		public extern static IntPtr XOpenDisplay(IntPtr display);

		[DllImport("X11", CallingConvention = CallingConvention.Cdecl)]
		public extern static int XCloseDisplay(IntPtr display);

		[DllImport("X11", CallingConvention = CallingConvention.Cdecl)]
		public extern static byte XKeysymToKeycode(IntPtr display, XKeySym keysym);

		[DllImport("X11", CallingConvention = CallingConvention.Cdecl)]
		public extern static XKeySym XKeycodeToKeysym(IntPtr display, byte keycode, int index);

		[DllImport("X11", CallingConvention = CallingConvention.Cdecl)]
		public unsafe extern static X11Screen* XDefaultScreenOfDisplay(IntPtr display);
	}
}