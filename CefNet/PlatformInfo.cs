using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace CefNet
{
	public sealed class PlatformInfo
	{
		static PlatformInfo()
		{
#if NET45
			PlatformID platform = Environment.OSVersion.Platform;
			IsWindows = (platform == PlatformID.Win32NT);
			IsLinux = (platform == PlatformID.Unix);
			IsMacOS = (platform == PlatformID.MacOSX);
#else
			IsWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
			IsMacOS = RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
			IsLinux = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
#endif
		}

		public static bool IsWindows { get; }

		public static bool IsMacOS { get; }

		public static bool IsLinux { get; }

		
	}


}
