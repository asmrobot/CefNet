using CefNet.WinApi;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace CefNet
{
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct CefEventHandle
	{
		private void* _instance;

		public ref MSG ToWindowsEvent()
		{
			return ref *((MSG*)_instance);
		}
	}
}
