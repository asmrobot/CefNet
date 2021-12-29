using CefNet.CApi;
using CefNet.WinApi;
using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace CefNet
{
	/// <summary>
	/// Structure representing CefExecuteProcess arguments.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct CefMainArgs : IDisposable
	{
		private cef_main_args_t _instance;

		public unsafe static CefMainArgs Create(Encoding encoding, string[] args)
		{
			if (PlatformInfo.IsWindows)
				throw new PlatformNotSupportedException();

			if (args != null)
			{
				if (encoding == null)
					throw new ArgumentNullException(nameof(encoding));
				if (args.Any(arg => arg == null))
					throw new ArgumentOutOfRangeException(nameof(args));

				var mainArgs = new CefMainArgs();
				mainArgs._instance.posix.argc = args.Length;
				mainArgs._instance.posix.argv = CreateArgv(encoding, args);
				return mainArgs;
			}
			return new CefMainArgs();
		}

		public unsafe static CefMainArgs CreateDefault()
		{
			var mainArgs = new CefMainArgs();
			if (PlatformInfo.IsWindows)
			{
				mainArgs._instance.windows.instance = NativeMethods.GetModuleHandle(null);
			}
			else
			{
				string[] args = Environment.GetCommandLineArgs();
				mainArgs._instance.posix.argc = args.Length;
				mainArgs._instance.posix.argv = CreateArgv(Encoding.UTF8, args);
			}
			return mainArgs;
		}

		private static byte** CreateArgv(Encoding encoding, string[] args)
		{
			int arraySize = IntPtr.Size * (args.Length + 1);
			int memorySize = arraySize + args.Length;
			foreach (string arg in args)
			{
				memorySize += encoding.GetByteCount(arg);
			}

			byte** argv = (byte**)Marshal.AllocHGlobal(memorySize);
			byte* data = (byte*)argv + arraySize;
			byte* bufferEnd = (byte*)argv + memorySize;

			for (var i = 0; i < args.Length; i++)
			{
				argv[i] = data;
				string arg = args[i];
				fixed (char* arg_ptr = arg)
				{
					data += encoding.GetBytes(arg_ptr, arg.Length, data, (int)(bufferEnd - data));
				}
				data[0] = 0;
				data++;
			}
			argv[args.Length] = null;
			return argv;
		}

		public void Dispose()
		{
			if (PlatformInfo.IsLinux || PlatformInfo.IsMacOS)
			{
				byte** argv = _instance.posix.argv;
				if (argv != null)
				{
					Marshal.FreeHGlobal(new IntPtr(_instance.posix.argv));
					_instance.posix.argv = null;
					_instance.posix.argc = 0;
				}
			}
		}

		public static implicit operator CefMainArgs(cef_main_args_t instance)
		{
			return new CefMainArgs { _instance = instance };
		}

		public static implicit operator cef_main_args_t(CefMainArgs instance)
		{
			return instance._instance;
		}
	}
}
