using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CefNet;
using Modern.Forms;
using WinFormsCoreApp;

namespace ModernFormsApp
{
	static class Program
	{
		private static readonly int messagePumpDelay = 10;
		private static System.Threading.Timer messagePump;
		private static Form mainForm;

		/// <summary>
		///  The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			//Application.SetHighDpiMode(HighDpiMode.SystemAware);
			//Application.EnableVisualStyles();
			//Application.SetCompatibleTextRenderingDefault(false);


			string cefPath = Path.Combine(Path.GetDirectoryName(GetProjectPath()), "cef");
			bool externalMessagePump = args.Contains("--external-message-pump");

			AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

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
			try
			{
				app.BeforeInitialize += App_BeforeInitialize;
				app.Initialize(Path.Combine(cefPath, "Release"), settings);

				if (externalMessagePump)
				{
					messagePump = new System.Threading.Timer(_ => Application.RunOnUIThread(CefApi.DoMessageLoopWork), null, messagePumpDelay, messagePumpDelay);
				}

				mainForm = new MainForm();
				//mainForm.UseSystemDecorations = true;
				Application.Run(mainForm);
			}
			finally
			{
				messagePump?.Dispose();
				app.Shutdown();
				app.Dispose();
			}
		}

		private static void App_BeforeInitialize(object sender, EventArgs e)
		{
			CefApi.EnableHighDPISupport();
		}

		private static async void OnScheduleMessagePumpWork(long delayMs)
		{
			await Task.Delay((int)delayMs);
			Application.RunOnUIThread(CefApi.DoMessageLoopWork);
		}

		private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			ShowUnhandledException(e.ExceptionObject as Exception, "AppDomain::UnhandledException");
		}

		private static void ShowUnhandledException(Exception exception, string from)
		{
			if (exception == null)
				return;
			new MessageBoxForm(from, string.Format("{0}: {1}\r\n{2}", exception.GetType().Name, exception.Message, exception.StackTrace)).ShowDialog(mainForm);
		}

		private static string GetProjectPath()
		{
			string projectPath = Path.GetDirectoryName(typeof(Program).Assembly.Location);
			string rootPath = Path.GetPathRoot(projectPath);
			while (Path.GetFileName(projectPath) != "ModernFormsApp")
			{
				if (projectPath == rootPath)
					throw new DirectoryNotFoundException("Could not find the project directory.");
				projectPath = Path.GetDirectoryName(projectPath);
			}
			return projectPath;
		}
	}
}
