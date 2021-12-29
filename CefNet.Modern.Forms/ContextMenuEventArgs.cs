using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using Modern.Forms;

namespace CefNet.Modern.Forms
{
	public sealed class ContextMenuEventArgs : HandledEventArgs
	{
		public ContextMenuEventArgs(ContextMenu menu, Point location)
		{
			this.ContextMenu = menu;
			this.Location = location;
		}

		public ContextMenu ContextMenu { get; }

		public Point Location { get; }

	}
}
