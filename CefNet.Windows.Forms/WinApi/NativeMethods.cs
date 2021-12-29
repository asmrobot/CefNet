using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using CefNet.Input;

namespace CefNet.WinApi
{
	static class NativeMethods
	{
		[DllImport("Kernel32.dll")]
		public static extern void SetLastError(uint errCode);

		[DllImport("kernel32.dll")]
		public static extern ushort GetCurrentProcessId();

		[DllImport("user32.dll")]
		public static extern IntPtr GetAncestor(IntPtr hwnd, int flags);

		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool GetWindowRect(IntPtr hWnd, out RECT rect);

		public static IntPtr GetWindowLong(IntPtr hWnd, int nIndex)
		{
			if (IntPtr.Size == 4)
			{
				return GetWindowLong32(hWnd, nIndex);
			}
			return GetWindowLongPtr64(hWnd, nIndex);
		}

		[DllImport("user32.dll", EntryPoint = "GetWindowLong", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern IntPtr GetWindowLong32(IntPtr hWnd, int nIndex);

		[DllImport("user32.dll", EntryPoint = "GetWindowLongPtr", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern IntPtr GetWindowLongPtr64(IntPtr hWnd, int nIndex);

		public static IntPtr SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong)
		{
			if (IntPtr.Size == 8)
				return SetWindowLongPtr64(hWnd, nIndex, dwNewLong);
			else
				return new IntPtr(SetWindowLong32(hWnd, nIndex, dwNewLong.ToInt32()));
		}

		[DllImport("user32.dll", EntryPoint = "SetWindowLong", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern int SetWindowLong32(IntPtr hWnd, int nIndex, int dwNewLong);

		[DllImport("user32.dll", EntryPoint = "SetWindowLongPtr", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern IntPtr SetWindowLongPtr64(IntPtr hWnd, int nIndex, IntPtr dwNewLong);
		
		[DllImport("user32.dll", SetLastError = false)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern int MapWindowPoints(IntPtr hWndFrom, IntPtr hWndTo, ref WinApi.RECT rect, int cPoints);

		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern int MapWindowPoints(IntPtr hWndFrom, IntPtr hWndTo, ref CefPoint pt, int cPoints);

		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool GetIconInfo(IntPtr hIcon, out ICONINFO iconInfo);

		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern IntPtr CreateIconIndirect(ref ICONINFO iconinfo);

		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public extern static bool DestroyIcon(IntPtr handle);

		[DllImport("gdi32", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = false)]
		public extern static int SetDIBitsToDevice(IntPtr hDC, int xDest, int yDest, int dwWidth, int dwHeight, int XSrc, int YSrc, int uStartScan, int cScanLines, IntPtr lpvBits, ref BITMAPINFO lpbmi, uint colorUse);

		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public extern static bool PostMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public extern static KeyState GetKeyState(VirtualKeys key);

		[DllImport("user32.dll")]
		public static extern uint MapVirtualKey(uint code, MapVirtualKeyType type);

		[DllImport("user32.dll")]
		public static extern IntPtr GetKeyboardLayout(uint idThread);

		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern IntPtr ActivateKeyboardLayout(IntPtr hkl, int flags);

		[DllImport("Dwmapi.dll", CharSet = CharSet.Auto)]
		public static unsafe extern int DwmGetWindowAttribute(IntPtr hwnd, DWMWINDOWATTRIBUTE attribute, void* value, int size);

		[DllImport("Dwmapi.dll", CharSet = CharSet.Auto, PreserveSig = false)]
		public static extern bool DwmIsCompositionEnabled();

		[DllImport("User32.dll")]
		public static extern int GetSystemMetrics(int nIndex);

		[DllImport("User32.dll", SetLastError = true)]
		public static extern bool RegisterTouchWindow(IntPtr hWnd, int flags);

		[DllImport("User32.dll", SetLastError = true)]
		public static extern bool CloseTouchInputHandle(IntPtr hTouchInput);

		[DllImport("User32.dll", SetLastError = true)]
		public unsafe static extern bool GetTouchInputInfo(IntPtr hTouchInput, int cInputs, TOUCHINPUT* pInputs, int cbSize);

		[DllImport("User32.dll")]
		public static extern IntPtr GetMessageExtraInfo();

		public static IntPtr MakeParam(short high, short low)
		{
			unchecked
			{
				return new IntPtr(((ushort)high) << 16 | (ushort)low);
			}
		}

		public static short HiWord(IntPtr param)
		{
			return unchecked((short)((unchecked((int)(long)param) >> 16) & 0xFFFF));
		}

		public static short LoWord(IntPtr param)
		{
			return unchecked((short)(unchecked((int)(long)param) & 0xFFFF));
		}
	}

	[StructLayout(LayoutKind.Sequential)]
	struct BITMAPINFO
	{
		public int Size;
		public int Width;
		public int Height;
		public short Planes;
		public short BitCount;
		public int Compression;
		public int SizeImage;
		public int XPelsPerMeter;
		public int YPelsPerMeter;
		public int ClrUsed;
		public int ClrImportant;
		public int Colors;
	}

	struct ICONINFO
	{
		/// <summary>
		/// Specifies whether this structure defines an icon or a cursor.
		/// A value of TRUE specifies an icon; FALSE specifies a cursor
		/// </summary>
		[MarshalAs(UnmanagedType.Bool)]
		public bool IsIcon;
		/// <summary>
		/// A cursor's hot spot
		/// </summary>
		public CefPoint Hotspot;
		/// <summary>
		/// The icon bitmask bitmap
		/// </summary>
		public IntPtr HbmMask;
		/// <summary>
		/// A handle to the icon color bitmap.
		/// </summary>
		public IntPtr HbmColor;
	}

	[StructLayout(LayoutKind.Sequential)]
	unsafe struct TOUCHINPUT
	{
		public int x;
		public int y;
		public IntPtr hSource;
		public int id;
		public int flags;
		public int mask;
		public int time;
		public IntPtr dwExtraInfo;
		public int cxContact;
		public int cyContact;
	}
}
