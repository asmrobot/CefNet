using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace WpfCoreApp
{
	public class FullscreenModeChangeEventArgs : RoutedEventArgs
	{
		public FullscreenModeChangeEventArgs(object source, bool fullscreen)
			: base(CustomWebView.FullscreenEvent, source)
		{
			this.Fullscreen = fullscreen;
		}

		public bool Fullscreen { get; }
	}
}
