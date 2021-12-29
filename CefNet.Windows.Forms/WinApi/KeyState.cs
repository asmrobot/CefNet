using System;
using System.Collections.Generic;
using System.Text;

namespace CefNet.WinApi
{
	[Flags]
	internal enum KeyState : ushort
	{
		None = 0,
		Pressed = 0x8000,
		Toggled = 0x0001,
	}
}
