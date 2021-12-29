using System;
using System.Runtime.InteropServices;

namespace CefNet.WinApi
{
	static class NativeMethods
	{
		[DllImport("user32.dll")]
		public static extern IntPtr GetKeyboardLayout(uint idThread);

		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern IntPtr ActivateKeyboardLayout(IntPtr hkl, int flags);

		[DllImport("user32.dll", EntryPoint = "VkKeyScanW", CharSet = CharSet.Unicode, SetLastError = true)]
		public static extern ushort VkKeyScan(char ch);

		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool GetWindowRect(IntPtr hWnd, out RECT rect);

		[DllImport("Dwmapi.dll", CharSet = CharSet.Auto)]
		public static unsafe extern int DwmGetWindowAttribute(IntPtr hwnd, DWMWINDOWATTRIBUTE attribute, void* value, int size);

		[DllImport("Dwmapi.dll", CharSet = CharSet.Auto, PreserveSig = false)]
		public static extern bool DwmIsCompositionEnabled();
	}
}
