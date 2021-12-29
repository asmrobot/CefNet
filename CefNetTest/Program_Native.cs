#if false
using CefNet;
using CefNet.CApi;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace CefNetTest
{
	public class Program
	{


		const uint WS_OVERLAPPED = 0x00000000;
		const uint WS_CAPTION = 0x00C00000;
		const uint WS_SYSMENU = 0x00080000;
		const uint WS_THICKFRAME = 0x00040000;
		const uint WS_MINIMIZEBOX = 0x00020000;
		const uint WS_MAXIMIZEBOX = 0x00010000;

		const uint WS_OVERLAPPEDWINDOW = (WS_OVERLAPPED | WS_CAPTION | WS_SYSMENU | WS_THICKFRAME | WS_MINIMIZEBOX | WS_MAXIMIZEBOX);
		const uint WS_CLIPCHILDREN = 0x02000000;
		const uint WS_CLIPSIBLINGS = 0x04000000;
		const uint WS_VISIBLE = 0x10000000;

		const int CW_USEDEFAULT = unchecked((int)0x80000000);

		private const string CefPath = @"D:\Projects\Libs\cefnet77\cef";

		[STAThread]
		public unsafe static void Main(string[] args)
		{
						//string cefPath = Path.Combine(Path.GetDirectoryName(GetProjectPath()), "cef", "Release");
			string cefPath = "/home/vlad/work/cefnetdev/CefNetTest/bin/Debug/netcoreapp3.0";
			var path = Environment.GetEnvironmentVariable("PATH");
			Environment.SetEnvironmentVariable("PATH", cefPath + ";" + path);
			string libname = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "libcef.dll" : "libcef.so";
			IntPtr pLibCef = NativeMethods.LoadLibrary(Path.Combine(cefPath, libname));

			// This executable is called many times, because it
			// is also used for subprocesses. Let's print args
			// so we can differentiate between main process and
			// subprocesses. If one of the first args is for
			// example "--type=renderer" then it means that
			// this is a Renderer process. There may be more
			// subprocesses like GPU (--type=gpu-process) and
			// others. On Linux there are also special Zygote
			// processes.
			Console.Write("\nProcess args: ");
			if (args.Length == 0)
			{
				Console.Write("none (Main process)");
			}
			else
			{
				Console.WriteLine();
				for (int i = 0; i < args.Length; i++)
				{
					if (args[i].Length > 128)
						Console.WriteLine(args[i].Remove(128) + "...");
					else
						Console.WriteLine(args[i]);
				}
			}
			Console.Write("\n\n");

			// CEF version
			if (args.Length == 0)
			{
				var version = new int[8];
				for (int i = 0; i < version.Length; i++)
				{
					version[i] = CefApi.CefVersionInfo((CefVersionEntry)i);
				}
				Console.Write("CEF version: {0}\n", string.Join(".", version));
			}

			// Main args
			CefMainArgs main_args = CefMainArgs.CreateDefault();

			// Main args
			//cef_main_args_t main_args = default;
			//main_args..instance = GetModuleHandle(null);

			// Cef app
			var cef_app = CefApp2.NewCefApp();

			// Execute subprocesses. It is also possible to have
			// a separate executable for subprocesses by setting
			// cef_settings_t.browser_subprocess_path. In such
			// case cef_execute_process should not be called here.
			Console.Write("cef_execute_process, argc={0}\n", args.Length);
			int code = CefNativeApi.cef_execute_process((cef_main_args_t*)&main_args, cef_app, (void*)0);
			if (code >= 0)
			{
				Environment.Exit(code);
			}

			// Application settings. It is mandatory to set the
			// "size" member.
			var settings = new CefSettings();


			//settings.MultiThreadedMessageLoop = true;
			settings.LocalesDirPath = Path.Combine(CefPath, "Resources", "locales");
			settings.ResourcesDirPath = Path.Combine(CefPath, "Resources");



			settings.LogSeverity = CefLogSeverity.Warning; // Show only warnings/errors
			settings.NoSandbox = true;

			// Initialize CEF
			Console.Write("cef_initialize\n");
			CefNativeApi.cef_initialize((cef_main_args_t*)&main_args, settings.GetNativeInstance(), cef_app, (void*)0);
			GC.KeepAlive(settings);

			// Window info
			//cef_window_info_windows_t window_info = default;
			//window_info.style = WS_OVERLAPPEDWINDOW | WS_CLIPCHILDREN | WS_CLIPSIBLINGS | WS_VISIBLE;
			//window_info.parent_window = IntPtr.Zero;
			//window_info.x = CW_USEDEFAULT;
			//window_info.y = CW_USEDEFAULT;
			//window_info.width = CW_USEDEFAULT;
			//window_info.height = CW_USEDEFAULT;

			cef_window_info_linux_t window_info = default;

			// Window info - window title
			byte[] window_name = Encoding.ASCII.GetBytes("cefcapi example");
			cef_string_t cef_window_name = default;
			fixed (byte* aWindowName = window_name)
			{
				CefNativeApi.cef_string_utf8_to_utf16(aWindowName, (UIntPtr)window_name.Length, (cef_string_utf16_t*)&cef_window_name);
			}
			window_info.window_name = cef_window_name;

			// Initial url
			byte[] url = Encoding.ASCII.GetBytes("https://www.google.com/ncr");
			cef_string_t cef_url = default;
			fixed (byte* aUrl = url)
			{
				CefNativeApi.cef_string_utf8_to_utf16(aUrl, (UIntPtr)url.Length, (cef_string_utf16_t*)&cef_url);
			}

			// Browser settings. It is mandatory to set the
			// "size" member.
			var browser_settings = new CefBrowserSettings();

			// Client handlers
			var client = new CefClientClass();


			// Create browser asynchronously. There is also a
			// synchronous version of this function available.
			Console.WriteLine("cef_browser_host_create_browser\n");
			CefNativeApi.cef_browser_host_create_browser((cef_window_info_t*)&window_info, client.GetNativeInstance(), &cef_url, browser_settings.GetNativeInstance(), default, default);

			// Message loop. There is also cef_do_message_loop_work()
			// that allow for integrating with existing message loops.
			// On Windows for best performance you should set
			// cef_settings_t.multi_threaded_message_loop to true.
			// Note however that when you do that CEF UI thread is no
			// more application main thread and using CEF API is more
			// difficult and require using functions like cef_post_task
			// for running tasks on CEF UI thread.
			Console.WriteLine("cef_run_message_loop\n");
			CefNativeApi.cef_run_message_loop();

			// Shutdown CEF
			Console.WriteLine("cef_shutdown\n");
			CefNativeApi.cef_shutdown();


			GC.KeepAlive(client);
		}



	}


	sealed class CefLifeSpanHandlerClass2 : CefLifeSpanHandler
	{
		public override void OnBeforeClose(CefBrowser browser)
		{
			// TODO: Check how many browsers do exist and quit message
			//       loop only when last browser is closed. Otherwise
			//       closing a popup window will exit app while main
			//       window shouldn't be closed.
			CefNativeApi.cef_quit_message_loop();
		}

	}

	static unsafe class CefApp2
	{


	
		private static readonly OnBeforeCommandLineProcessingDelegate fnOnBeforeCommandLineProcessing = OnBeforeCommandLineProcessingImpl;

		private static readonly OnRegisterCustomSchemesDelegate fnOnRegisterCustomSchemes = OnRegisterCustomSchemesImpl;

		private static readonly GetResourceBundleHandlerDelegate fnGetResourceBundleHandler = GetResourceBundleHandlerImpl;

		private static readonly GetBrowserProcessHandlerDelegate fnGetBrowserProcessHandler = GetBrowserProcessHandlerImpl;

		private static readonly GetRenderProcessHandlerDelegate fnGetRenderProcessHandler = GetRenderProcessHandlerImpl;

		private static readonly unsafe CefActionDelegate fnAddRef = AddRefImpl;
		private static readonly unsafe CefIntFunctionDelegate fnRelease = ReleaseImpl;
		private static readonly unsafe CefIntFunctionDelegate fnHasOneRef = HasOneRefImpl;
		private static readonly unsafe CefIntFunctionDelegate fnHasAtLeastOneRef = HasAtLeastOneRefImpl;

		public static cef_app_t* NewCefApp()
		{
			cef_app_t* self = (cef_app_t*)Marshal.AllocHGlobal(sizeof(cef_app_t));
			self->on_before_command_line_processing = (void*)Marshal.GetFunctionPointerForDelegate(fnOnBeforeCommandLineProcessing);
			self->on_register_custom_schemes = (void*)Marshal.GetFunctionPointerForDelegate(fnOnRegisterCustomSchemes);
			self->get_resource_bundle_handler = (void*)Marshal.GetFunctionPointerForDelegate(fnGetResourceBundleHandler);
			self->get_browser_process_handler = (void*)Marshal.GetFunctionPointerForDelegate(fnGetBrowserProcessHandler);
			self->get_render_process_handler = (void*)Marshal.GetFunctionPointerForDelegate(fnGetRenderProcessHandler);

			cef_base_ref_counted_t* rc = (cef_base_ref_counted_t*)self;
			rc->size = new UIntPtr((uint)sizeof(cef_app_t)); 
			rc->add_ref = (void*)Marshal.GetFunctionPointerForDelegate(fnAddRef);
			rc->release = (void*)Marshal.GetFunctionPointerForDelegate(fnRelease);
			rc->has_one_ref = (void*)Marshal.GetFunctionPointerForDelegate(fnHasOneRef);
			rc->has_at_least_one_ref = (void*)Marshal.GetFunctionPointerForDelegate(fnHasAtLeastOneRef);
			return self;
		}


		[UnmanagedFunctionPointer(CallingConvention.Winapi)]
		internal unsafe delegate void CefActionDelegate(cef_base_ref_counted_t* self);

		[UnmanagedFunctionPointer(CallingConvention.Winapi)]
		internal unsafe delegate int CefIntFunctionDelegate(cef_base_ref_counted_t* self);
		

		private unsafe static void AddRefImpl(cef_base_ref_counted_t* self)
		{
			
		}

		private unsafe static int ReleaseImpl(cef_base_ref_counted_t* self)
		{
			return 1;
		}

		private unsafe static int HasOneRefImpl(cef_base_ref_counted_t* self)
		{
			return 1;
		}

		private unsafe static int HasAtLeastOneRefImpl(cef_base_ref_counted_t* self)
		{
			return 0;
		}


		[UnmanagedFunctionPointer(CallingConvention.Winapi)]
		private unsafe delegate void OnBeforeCommandLineProcessingDelegate(cef_app_t* self, cef_string_t* process_type, cef_command_line_t* command_line);

		// void (*)(_cef_app_t* self, const cef_string_t* process_type, _cef_command_line_t* command_line)*
		private static unsafe void OnBeforeCommandLineProcessingImpl(cef_app_t* self, cef_string_t* process_type, cef_command_line_t* command_line)
		{
			Console.WriteLine("OnBeforeCommandLineProcessingImpl");
		}

		[UnmanagedFunctionPointer(CallingConvention.Winapi)]
		private unsafe delegate void OnRegisterCustomSchemesDelegate(cef_app_t* self, cef_scheme_registrar_t* registrar);

		// void (*)(_cef_app_t* self, _cef_scheme_registrar_t* registrar)*
		private static unsafe void OnRegisterCustomSchemesImpl(cef_app_t* self, cef_scheme_registrar_t* registrar)
		{
			Console.WriteLine("OnRegisterCustomSchemes");
		}


		[UnmanagedFunctionPointer(CallingConvention.Winapi)]
		private unsafe delegate cef_resource_bundle_handler_t* GetResourceBundleHandlerDelegate(cef_app_t* self);

		// _cef_resource_bundle_handler_t* (*)(_cef_app_t* self)*
		private static unsafe cef_resource_bundle_handler_t* GetResourceBundleHandlerImpl(cef_app_t* self)
		{
			Console.WriteLine("GetResourceBundleHandlerImpl");
			return null;
		}


		[UnmanagedFunctionPointer(CallingConvention.Winapi)]
		private unsafe delegate cef_browser_process_handler_t* GetBrowserProcessHandlerDelegate(cef_app_t* self);

		// _cef_browser_process_handler_t* (*)(_cef_app_t* self)*
		private static unsafe cef_browser_process_handler_t* GetBrowserProcessHandlerImpl(cef_app_t* self)
		{
			Console.WriteLine("GetBrowserProcessHandlerImpl");
			return null;
		}
		

		[UnmanagedFunctionPointer(CallingConvention.Winapi)]
		private unsafe delegate cef_render_process_handler_t* GetRenderProcessHandlerDelegate(cef_app_t* self);

		// _cef_render_process_handler_t* (*)(_cef_app_t* self)*
		private static unsafe cef_render_process_handler_t* GetRenderProcessHandlerImpl(cef_app_t* self)
		{
			Console.WriteLine("GetRenderProcessHandlerImpl");
			return null;
		}
	}



}
#endif
