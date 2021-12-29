using System.Runtime.InteropServices;

namespace CefNet.WinApi
{
	static class NativeMethods
	{
		[DllImport("user32.dll", EntryPoint = "VkKeyScanW", CharSet = CharSet.Unicode, SetLastError = true)]
		public static extern ushort VkKeyScan(char ch);
	}
}