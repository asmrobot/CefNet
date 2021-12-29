using CefNet;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using WinFormsCoreApp;

namespace WpfCoreApp
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		private CefAppImpl app;
		private Timer messagePump;
		private int messagePumpDelay = 10;

		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);

			string cefPath = Path.Combine(Path.GetDirectoryName(GetProjectPath()), "cef");
			bool externalMessagePump = e.Args.Contains("--external-message-pump");

			var settings = new CefSettings();
			settings.MultiThreadedMessageLoop = !externalMessagePump;
			settings.ExternalMessagePump = externalMessagePump;
			settings.NoSandbox = true;
			settings.WindowlessRenderingEnabled = true;
			settings.LocalesDirPath = Path.Combine(cefPath, "Resources", "locales");
			settings.ResourcesDirPath = Path.Combine(cefPath, "Resources");
			settings.LogSeverity = CefLogSeverity.Warning;
			settings.UncaughtExceptionStackSize = 8;

			app = new CefAppImpl();
			app.ScheduleMessagePumpWorkCallback = OnScheduleMessagePumpWork;
			app.Initialize(Path.Combine(cefPath, "Release"), settings);

			if (externalMessagePump)
			{
				messagePump = new Timer(_ => Dispatcher.BeginInvoke(new Action(CefApi.DoMessageLoopWork)), null, messagePumpDelay, messagePumpDelay);
			}
		}

		protected override void OnExit(ExitEventArgs e)
		{
			using (var ev = new ManualResetEvent(false))
			{
				app.SignalForShutdown(() => ev.Set());
				ev.WaitOne();
			}
			messagePump?.Dispose();
			app?.Shutdown();
			base.OnExit(e);

		}

		private async void OnScheduleMessagePumpWork(long delayMs)
		{
			await Task.Delay((int)delayMs);
			await Dispatcher.InvokeAsync(CefApi.DoMessageLoopWork);
		}

		private static string GetProjectPath()
		{
			string projectPath = Path.GetDirectoryName(typeof(App).Assembly.Location);
			string rootPath = Path.GetPathRoot(projectPath);
			while (Path.GetFileName(projectPath) != "WpfCoreApp")
			{
				if (projectPath == rootPath)
					throw new DirectoryNotFoundException("Could not find the project directory.");
				projectPath = Path.GetDirectoryName(projectPath);
			}
			return projectPath;
		}
	}
}
