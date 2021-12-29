using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace CefNet.WinApi
{
	/// <summary>
	/// The <see cref="MONITORINFO"/> structure contains information about a display monitor.
	/// </summary>
	/// <remarks>
	/// See <see href="https://docs.microsoft.com/windows/win32/api/winuser/ns-winuser-monitorinfo">Microsoft Docs</see> for details.
	/// </remarks>
	[StructLayout(LayoutKind.Sequential)]
	public struct MONITORINFO
	{
		/// <summary>
		/// The size of the structure, in bytes.
		/// </summary>
		public int Size;
		/// <summary>
		/// A <see cref="RECT"/> structure that specifies the display monitor rectangle,
		/// expressed in virtual-screen coordinates.
		/// </summary>
		public RECT Monitor;
		/// <summary>
		/// A <see cref="RECT"/> structure that specifies the work area rectangle of
		/// the display monitor, expressed in virtual-screen coordinates.
		/// </summary>
		public RECT Work;
		/// <summary>
		/// A set of flags that represent attributes of the display monitor.
		/// </summary>
		public int Flags;
	}
}
