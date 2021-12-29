using System;
using System.Collections.Generic;
using System.Text;

namespace CefNet
{
	public class PopupShowEventArgs: EventArgs
	{
		public PopupShowEventArgs()
		{
			this.Visible = false;
		}

		public PopupShowEventArgs(CefRect rect)
		{
			this.Visible = (rect.Width | rect.Height) != 0;
			this.Bounds = rect;
		}

		public bool Visible { get; }

		public CefRect Bounds { get; }
	}
}
