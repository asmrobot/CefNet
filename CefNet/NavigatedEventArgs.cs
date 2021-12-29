using System;
using System.Collections.Generic;
using System.Text;

namespace CefNet
{
	public class NavigatedEventArgs : EventArgs
	{
		public NavigatedEventArgs(CefFrame frame, string url)
		{
			this.Frame = frame;
			this.Url = url;
		}

		public CefFrame Frame { get; }

		public string Url { get; }
	}
}
