using System;
using CefNet.Linux;
using CefNet.WinApi;
using static CefNet.NativeMethods;
using static CefNet.Linux.NativeMethods;

namespace CefNet
{
	public unsafe partial struct CefScreenInfo
	{
		/// <summary>
		/// Gets the primary display.
		/// </summary>
		/// <returns>The information about the primary display.</returns>
		/// <exception cref="NotSupportedException"></exception>
		public unsafe static CefScreenInfo GetPrimaryScreenInfo()
		{
			CefRect bounds, workingArea;

			if (PlatformInfo.IsWindows)
			{
				var monitorInfo = new MONITORINFO();
				monitorInfo.Size = sizeof(MONITORINFO);
				IntPtr hMonitor = MonitorFromWindow(IntPtr.Zero, MonitorFlag.MONITOR_DEFAULTTOPRIMARY);
				GetMonitorInfo(hMonitor, ref monitorInfo);
				bounds = monitorInfo.Monitor.ToCefRect();
				workingArea = monitorInfo.Work.ToCefRect();
			}
			else if (PlatformInfo.IsLinux)
			{
				IntPtr display = XOpenDisplay(IntPtr.Zero);
				if (display == IntPtr.Zero)
					throw new InvalidOperationException();
				try
				{
					X11Screen* screen = XDefaultScreenOfDisplay(display);
					if (screen == null)
						throw new InvalidOperationException();
					bounds = new CefRect(0, 0, screen->width, screen->height);
				}
				finally
				{
					XCloseDisplay(display);
				}
				workingArea = bounds;
			}
			else
			{
				throw new NotSupportedException();
			}

			float devicePixelRatio = 1.0f;
			//bounds.Scale(1.0f / devicePixelRatio);
			//workingArea.Scale(1.0f / devicePixelRatio);

			var screenInfo = new CefScreenInfo();
			screenInfo.Depth = 24;
			screenInfo.DepthPerComponent = 24;
			screenInfo.DeviceScaleFactor = devicePixelRatio;
			screenInfo.Rect = bounds;
			screenInfo.AvailableRect = workingArea;
			return screenInfo;
		}

	}
}
