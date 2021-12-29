using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CefNet.Windows.Forms
{
	public sealed class ContextMenuEventArgs : HandledEventArgs
	{
		public ContextMenuEventArgs(ContextMenuStrip menu, Point location)
		{
			this.ContextMenu = menu;
			this.Location = location;
		}

		public ContextMenuStrip ContextMenu { get; }

		public Point Location { get; }

	}
}
