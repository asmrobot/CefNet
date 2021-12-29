using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using CefNet.WinApi;

namespace CefNet
{
	internal static class NativeMethods
	{
		[DllImport("user32.dll")]
		public static extern IntPtr GetDesktopWindow();

		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool GetMonitorInfo(IntPtr monitorHandle, ref MONITORINFO mInfo);

		[DllImport("user32.dll")]
		public static extern IntPtr MonitorFromWindow(IntPtr hwnd, MonitorFlag flag);

		[DllImport("user32.dll")]
		public static extern IntPtr MonitorFromPoint(POINT pt, MonitorFlag flag);

		[DllImport("user32.dll")]
		public static extern uint MapVirtualKey(uint code, MapVirtualKeyType type);

		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern IntPtr GetModuleHandle(string lpModuleName);

		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern IntPtr LoadLibraryEx(string fileName, IntPtr file, int flags);

		[DllImport("libdl", CallingConvention = CallingConvention.Cdecl, SetLastError = false)]
		public static extern IntPtr dlopen(string path, int mode);
	}
}
