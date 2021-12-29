using CefNet;
using CefNet.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfCoreApp
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		bool isFirstLoad = true;

		public MainWindow()
		{
			InitializeComponent();
			this.Loaded += MainWindow_Loaded;
			EventManager.RegisterClassHandler(typeof(WebView), CustomWebView.FullscreenEvent, new EventHandler<FullscreenModeChangeEventArgs>(HandleFullscreenEvent));
			EventManager.RegisterClassHandler(typeof(WebView), WebView.ScriptDialogOpeningEvent, new EventHandler<IScriptDialogOpeningEventArgs>(HandleScriptDialogOpeningEvent));
		}

		private WindowStyle defaultWindowStyle;
		private Style defaultTabsStyle; 

		private void HandleFullscreenEvent(object sender, FullscreenModeChangeEventArgs e)
		{
			TabPanel tabHeaders = tabs.FindChild<TabPanel>(null);
			if (e.Fullscreen)
			{
				Visibility = Visibility.Collapsed;
				defaultTabsStyle = tabs.ItemContainerStyle;
				defaultWindowStyle = WindowStyle;
				menu.Visibility = Visibility.Collapsed;
				controlsPanel.Visibility = Visibility.Collapsed;
				tabHeaders.Visibility = Visibility.Collapsed;
				WindowStyle = WindowStyle.None;
				WindowState = WindowState.Maximized;
				Topmost = true;
				ResizeMode = ResizeMode.NoResize;
				Visibility = Visibility.Visible;
			}
			else
			{
				tabHeaders.Visibility = Visibility.Collapsed;
				tabs.ItemContainerStyle = defaultTabsStyle;
				menu.Visibility = Visibility.Visible;
				controlsPanel.Visibility = Visibility.Visible;
				tabHeaders.Visibility = Visibility.Visible;
				WindowStyle = defaultWindowStyle;
				WindowState = WindowState.Normal;
				ResizeMode = ResizeMode.CanResize;
				Topmost = false;
			}
		}

		private void HandleScriptDialogOpeningEvent(object sender, IScriptDialogOpeningEventArgs e)
		{
			if (e.Kind == ScriptDialogKind.Alert)
			{
				e.Handled = true;

				ScriptDialogDeferral dialogCallback = e.GetDeferral();
				Dispatcher.InvokeAsync(() => MessageBox.Show(e.Message, "WPF dialog")).Task.ContinueWith(t =>
				{
					dialogCallback.Accept();
				});
			}
		}

		private void MainWindow_Loaded(object sender, RoutedEventArgs e)
		{
			if (!isFirstLoad)
				return;
			isFirstLoad = false;

			AddTab(true);
		}

		private void AddTab(bool useGlobalContext)
		{
			WebViewTab viewTab;
			if (useGlobalContext)
			{
				viewTab = new WebViewTab();
				viewTab.WebView.Navigated += WebView_Navigated;
				tabs.Items.Add(viewTab);
				viewTab.Title = "about:blank";
				tabs.SelectedItem = viewTab;
			}
			else
			{
				//var cx = new CefRequestContext(new CefRequestContextSettings());
				//tabs.Controls.Add(new WebViewTab(new CefBrowserSettings(), cx));
			}
		}

		private void WebView_Navigated(object sender, NavigatedEventArgs e)
		{
			txtAddress.Text = e.Url.ToString();
		}

		private IChromiumWebView SelectedView
		{
			get
			{
				return (tabs.SelectedItem as WebViewTab)?.WebView;
			}
		}


		private void AddTab_Click(object sender, RoutedEventArgs e)
		{
			AddTab(true);
		}

		private void BackButton_Click(object sender, RoutedEventArgs e)
		{
			SelectedView?.GoBack();
		}

		private void ForwardButton_Click(object sender, RoutedEventArgs e)
		{
			SelectedView?.GoForward();
		}

		private void NavigateButton_Click(object sender, RoutedEventArgs e)
		{
			//SelectedView?.Navigate("http://yandex.ru");
			SelectedView?.Navigate("https://cefnet.github.io/winsize.html");
		}

		private void txtAddress_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
			{
				if (Uri.TryCreate(txtAddress.Text, UriKind.Absolute, out Uri url))
				{
					SelectedView?.Navigate(url.AbsoluteUri);
				}
			}
		}

		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			ItemCollection tabItems = tabs.Items;
			for (int i = tabItems.Count - 1; i >= 0; i--)
			{
				if (tabItems[i] is WebViewTab tab)
				{
					tab.Close();
				}
			}
		}

		private void WebView_TextFound(object sender, ITextFoundEventArgs e)
		{
			if (e.FinalUpdate)
			{
				SelectedView?.StopFinding(false);
			}
		}

		private void Find_Click(object sender, RoutedEventArgs e)
		{
			SelectedView?.Find(0, "i", true, true, false);
		}
	}
}
