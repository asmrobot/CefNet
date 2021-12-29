using System;
using System.Runtime.InteropServices;

namespace CefNet.CApi
{
	/// <summary>
	/// Structure representing CefExecuteProcess arguments.
	/// </summary>
	[StructLayout(LayoutKind.Explicit)]
	public unsafe struct cef_main_args_t
	{
		/// <summary>
		/// Contains platform dependent instance.
		/// </summary>
		[FieldOffset(0)]
		public cef_main_args_windows_t windows;

		/// <summary>
		/// Contains platform dependent instance.
		/// </summary>
		[FieldOffset(0)]
		public cef_main_args_posix_t posix;
	}

	/// <summary>
	/// Structure representing CefExecuteProcess arguments.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct cef_main_args_windows_t
	{
		/// <summary>
		/// Contains a handle to the file used to create the calling process (.exe file).
		/// </summary>
		public IntPtr instance;
	}

	/// <summary>
	/// Structure representing CefExecuteProcess arguments.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct cef_main_args_posix_t
	{
		/// <summary>
		/// A count of the number of command-line arguments passed to the program.
		/// </summary>
		public int argc;
		/// <summary>
		/// A pointer to an array of strings that contain the program arguments.
		/// </summary>
		public byte** argv;
	}
}
