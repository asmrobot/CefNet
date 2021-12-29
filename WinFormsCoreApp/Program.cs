using CefNet;
using CefNet.Internal;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsCoreApp
{
	static class Program
	{
		private static readonly int messagePumpDelay = 10;
		private static SynchronizationContext UIContext;
		private static System.Threading.Timer messagePump;

		/// <summary>
		///  The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			Application.SetHighDpiMode(HighDpiMode.SystemAware);
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);


			string cefPath = Path.Combine(Path.GetDirectoryName(GetProjectPath()), "cef");
			bool externalMessagePump = args.Contains("--external-message-pump");

			AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
			Application.ThreadException += Application_ThreadException;

			var settings = new CefSettings();
			settings.MultiThreadedMessageLoop = !externalMessagePump;
			settings.ExternalMessagePump = externalMessagePump;
			settings.NoSandbox = true;
			settings.WindowlessRenderingEnabled = true;
			settings.LocalesDirPath = Path.Combine(cefPath, "Resources", "locales");
			settings.ResourcesDirPath = Path.Combine(cefPath, "Resources");
			settings.LogSeverity = CefLogSeverity.Warning;
			settings.UncaughtExceptionStackSize = 8;
			
			var app = new CefAppImpl();
			app.ScheduleMessagePumpWorkCallback = OnScheduleMessagePumpWork;
			app.CefProcessMessageReceived += ScriptableObjectTests.HandleScriptableObjectTestMessage;
			try
			{
				UIContext = new WindowsFormsSynchronizationContext();
				SynchronizationContext.SetSynchronizationContext(UIContext);

				app.Initialize(Path.Combine(cefPath, "Release"), settings);

				if (externalMessagePump)
				{
					messagePump = new System.Threading.Timer(_ => UIContext.Post(_ => CefApi.DoMessageLoopWork(), null), null, messagePumpDelay, messagePumpDelay);
				}

				Application.Run(new MainForm());

				using (var ev = new ManualResetEvent(false))
				{
					app.SignalForShutdown(() => ev.Set());
					ev.WaitOne();
				}
			}
			finally
			{
				messagePump?.Dispose();
				app.Shutdown();
				app.Dispose();
			}
		}

		private static async void OnScheduleMessagePumpWork(long delayMs)
		{
			await Task.Delay((int)delayMs);
			UIContext.Post(_ => CefApi.DoMessageLoopWork(), null);
		}

		private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			ShowUnhandledException(e.ExceptionObject as Exception, "AppDomain::UnhandledException");
		}

		private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
		{
			ShowUnhandledException(e.Exception, "Application::ThreadException");
		}

		internal static void ShowUnhandledException(Exception exception, string from)
		{
			if (exception == null)
				return;
			MessageBox.Show(string.Format("{0}: {1}\r\n{2}", exception.GetType().Name, exception.Message, exception.StackTrace), from);
		}

		private static string GetProjectPath()
		{
			string projectPath = Path.GetDirectoryName(typeof(Program).Assembly.Location);
			string rootPath = Path.GetPathRoot(projectPath);
			while (Path.GetFileName(projectPath) != "WinFormsCoreApp")
			{
				if (projectPath == rootPath)
					throw new DirectoryNotFoundException("Could not find the project directory.");
				projectPath = Path.GetDirectoryName(projectPath);
			}
			return projectPath;
		}

	}
}
