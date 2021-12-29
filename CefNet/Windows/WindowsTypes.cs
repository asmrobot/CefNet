using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace CefNet.WinApi
{
	/// <summary>
	/// Contains message information from a thread's message queue.
	/// </summary>
	/// <remarks>
	/// See <see href="https://docs.microsoft.com/windows/win32/api/winuser/ns-winuser-msg">Microsoft Docs</see> for details.
	/// </remarks>
	[StructLayout(LayoutKind.Sequential)]
	public struct MSG
	{
		/// <summary>
		/// A handle to the window whose window procedure receives the message. This member is NULL when the message is a thread message.
		/// </summary>
		public IntPtr hwnd;
		/// <summary>
		/// The message identifier. Applications can only use the low word; the high word is reserved by the system.
		/// </summary>
		public uint message;
		/// <summary>
		/// Additional information about the message. The exact meaning depends on the value of the message member.
		/// </summary>
		public IntPtr wParam;
		/// <summary>
		/// Additional information about the message. The exact meaning depends on the value of the message member.
		/// </summary>
		public IntPtr lParam;
		/// <summary>
		/// The time at which the message was posted.
		/// </summary>
		public uint time;
		/// <summary>
		/// The cursor position, in screen coordinates, when the message was posted.
		/// </summary>
		public POINT pt;
		/// <summary>
		/// 
		/// </summary>
		public uint lPrivate;
	}

	/// <summary>
	/// The <see cref="POINT"/> structure defines the x- and y- coordinates of a point.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct POINT
	{
		/// <summary>
		/// The x-coordinate of the point.
		/// </summary>
		public int x;
		/// <summary>
		/// The y-coordinate of the point.
		/// </summary>
		public int y;
	}

	/// <summary>
	/// The <see cref="RECT"/> structure defines a rectangle by the coordinates of its upper-left and lower-right corners.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct RECT
	{
		/// <summary>
		/// Specifies the x-coordinate of the upper-left corner of the rectangle.
		/// </summary>
		public int Left;
		/// <summary>
		/// Specifies the y-coordinate of the upper-left corner of the rectangle.
		/// </summary>
		public int Top;
		/// <summary>
		/// Specifies the x-coordinate of the lower-right corner of the rectangle.
		/// </summary>
		public int Right;
		/// <summary>
		/// Specifies the y-coordinate of the lower-right corner of the rectangle.
		/// </summary>
		public int Bottom;

		/// <summary>
		/// Converts a <see cref="RECT"/> structure to a <see cref="CefRect"/> structure. 
		/// </summary>
		/// <returns>A <see cref="CefRect"/> structure.</returns>
		public CefRect ToCefRect()
		{
			return new CefRect { X = Left, Y = Top, Width = Right - Left, Height = Bottom - Top };
		}

		/// <summary>
		/// Creates a <see cref="RECT"/> structure from the specified <see cref="CefRect"/> structure. 
		/// </summary>
		/// <param name="rect">The <see cref="CefRect"/> to be converted.</param>
		/// <returns>The new <see cref="RECT"/> that this method creates.</returns>
		public static RECT FromCefRect(ref CefRect rect)
		{
			return new RECT { Left = rect.X, Top = rect.Y, Right = rect.X + rect.Width, Bottom = rect.Y + rect.Height };
		}

		/// <summary>
		/// Creates a <see cref="RECT"/> structure from the specified <see cref="CefRect"/> structure. 
		/// </summary>
		/// <param name="rect">The <see cref="CefRect"/> to be converted.</param>
		/// <returns>The new <see cref="RECT"/> that this method creates.</returns>
		public static RECT FromCefRect(CefRect rect)
		{
			return new RECT { Left = rect.X, Top = rect.Y, Right = rect.X + rect.Width, Bottom = rect.Y + rect.Height };
		}
	}

}

