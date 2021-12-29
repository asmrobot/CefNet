using System;
using System.Collections.Generic;
using System.Text;

namespace CefNet.WinApi
{
	/// <summary>
	/// Specifies key translations.
	/// </summary>
	/// <remarks>
	/// See <see href="https://docs.microsoft.com/windows/win32/api/winuser/nf-winuser-mapvirtualkeyw">Microsoft Docs</see> for details.
	/// </remarks>
	public enum MapVirtualKeyType
	{
		/// <summary>
		/// A virtual key code to a scan code translation.
		/// </summary>
		MAPVK_VK_TO_VSC = 0,
		/// <summary>
		/// A scan code to a virtual key translation.
		/// </summary>
		MAPVK_VSC_TO_VK = 1,
		/// <summary>
		/// A virtual key code to an unshifted character translation.
		/// </summary>
		MAPVK_VK_TO_CHAR = 2,
		/// <summary>
		/// A scan code to a virtual key code translation that distinguishes between left- and right-hand keys.
		/// </summary>
		MAPVK_VSC_TO_VK_EX = 3,
	}
}
