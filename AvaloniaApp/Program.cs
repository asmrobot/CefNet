using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Threading;
using CefNet;
using WinFormsCoreApp;

namespace AvaloniaApp
{
	class Program
	{
		private static CefAppImpl app;
		private static Timer messagePump;
		private const int messagePumpDelay = 10;

		// Initialization code. Don't use any Avalonia, third-party APIs or any
		// SynchronizationContext-reliant code before AppMain is called: things aren't initialized
		// yet and stuff might break.
		[STAThread]
		public static void Main(string[] args)
		{
			string cefPath;
			bool externalMessagePump = args.Contains("--external-message-pump");

			if (PlatformInfo.IsMacOS)
			{
				externalMessagePump = true;
				cefPath = Path.Combine(GetProjectPath(), "Contents", "Frameworks", "Chromium Embedded Framework.framework");
			}
			else
			{
				cefPath = Path.Combine(Path.GetDirectoryName(GetProjectPath()), "cef");
			}

			var settings = new CefSettings();
			settings.MultiThreadedMessageLoop = !externalMessagePump;
			settings.ExternalMessagePump = externalMessagePump;
			settings.NoSandbox = true;
			settings.WindowlessRenderingEnabled = true;
			settings.LocalesDirPath = Path.Combine(cefPath, "Resources", "locales");
			settings.ResourcesDirPath = Path.Combine(cefPath, "Resources");
			settings.LogSeverity = CefLogSeverity.Warning;
			settings.UncaughtExceptionStackSize = 8;

			App.FrameworkInitialized += App_FrameworkInitialized;
			App.FrameworkShutdown += App_FrameworkShutdown;

			app = new CefAppImpl();
			app.CefProcessMessageReceived += App_CefProcessMessageReceived;
			app.ScheduleMessagePumpWorkCallback = OnScheduleMessagePumpWork;
			app.Initialize(PlatformInfo.IsMacOS ? cefPath : Path.Combine(cefPath, "Release"), settings);

			BuildAvaloniaApp()
			// workaround for https://github.com/AvaloniaUI/Avalonia/issues/3533
			.With(new AvaloniaNativePlatformOptions { UseGpu = !PlatformInfo.IsMacOS })
			.StartWithCefNetApplicationLifetime(args);
		}

		// Avalonia configuration, don't remove; also used by visual designer.
		public static AppBuilder BuildAvaloniaApp()
			=> AppBuilder.Configure<App>()
				.UsePlatformDetect()
				.LogToTrace();


		private static void App_FrameworkInitialized(object sender, EventArgs e)
		{
			if (CefNetApplication.Instance.UsesExternalMessageLoop)
			{
				messagePump = new Timer(_ => Dispatcher.UIThread.Post(CefApi.DoMessageLoopWork), null, messagePumpDelay, messagePumpDelay);
			}
		}

		private static void App_FrameworkShutdown(object sender, EventArgs e)
		{
			messagePump?.Dispose();
		}

		private static async void OnScheduleMessagePumpWork(long delayMs)
		{
			await Task.Delay((int)delayMs);
			Dispatcher.UIThread.Post(CefApi.DoMessageLoopWork);
		}

		private static string GetProjectPath()
		{
			string projectPath = Path.GetDirectoryName(typeof(App).Assembly.Location);
			string projectName = PlatformInfo.IsMacOS ? "AvaloniaApp.app" : "AvaloniaApp";
			string rootPath = Path.GetPathRoot(projectPath);
			while (Path.GetFileName(projectPath) != projectName)
			{
				if (projectPath == rootPath)
					throw new DirectoryNotFoundException("Could not find the project directory.");
				projectPath = Path.GetDirectoryName(projectPath);
			}
			return projectPath;
		}

		private static void App_CefProcessMessageReceived(object sender, CefProcessMessageReceivedEventArgs e)
		{
			if (e.Name == "TestV8ValueTypes")
			{
				TestV8ValueTypes(e.Frame);
				e.Handled = true;
				return;
			}

			if (e.Name == "MessageBox.Show")
			{
				string message = e.Message.ArgumentList.GetString(0);
				Dispatcher.UIThread.Post(() =>
				{
					var messageBoxStandardWindow = MessageBox.Avalonia.MessageBoxManager
						.GetMessageBoxStandardWindow("title", message);
					messageBoxStandardWindow.Show();
				});
				e.Handled = true;
				return;
			}
		}

		private static void TestV8ValueTypes(CefFrame frame)
		{
			var sb = new StringBuilder();
			CefV8Context context = frame.V8Context;
			if (!context.Enter())
				return;
			try
			{
				sb.Append("typeof 1 = ").Append(context.Eval("1", null).Type).AppendLine();
				sb.Append("typeof true = ").Append(context.Eval("true", null).Type).AppendLine();
				sb.Append("typeof 'string' = ").Append(context.Eval("'string'", null).Type).AppendLine();
				sb.Append("typeof 2.2 = ").Append(context.Eval("2.2", null).Type).AppendLine();
				sb.Append("typeof null = ").Append(context.Eval("null", null).Type).AppendLine();
				sb.Append("typeof new Object() = ").Append(context.Eval("new Object()", null).Type).AppendLine();
				sb.Append("typeof undefined = ").Append(context.Eval("undefined", null).Type).AppendLine();
				sb.Append("typeof new Date() = ").Append(context.Eval("new Date()", null).Type).AppendLine();
				sb.Append("(window == window) = ").Append(context.Eval("window", null) == context.Eval("window", null)).AppendLine();
			}
			finally
			{
				context.Exit();
			}
			var message = new CefProcessMessage("MessageBox.Show");
			message.ArgumentList.SetString(0, sb.ToString());
			frame.SendProcessMessage(CefProcessId.Browser, message);
		}


	}
}
