using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using CefNet;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WinFormsCoreApp;

namespace AvaloniaApp
{
	public class App : Application
	{
		public static event EventHandler FrameworkInitialized;
		public static event EventHandler FrameworkShutdown;

		public override void Initialize()
		{
			AvaloniaXamlLoader.Load(this);
		}

		public override void OnFrameworkInitializationCompleted()
		{
			if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
			{
				desktop.MainWindow = new MainWindow();
				desktop.Startup += Startup;
				desktop.Exit += Exit;
			}

			base.OnFrameworkInitializationCompleted();
		}

		private void Startup(object sender, ControlledApplicationLifetimeStartupEventArgs e)
		{
			FrameworkInitialized?.Invoke(this, EventArgs.Empty);
		}

		private void Exit(object sender, ControlledApplicationLifetimeExitEventArgs e)
		{
			FrameworkShutdown?.Invoke(this, EventArgs.Empty);
		}


	}
}
