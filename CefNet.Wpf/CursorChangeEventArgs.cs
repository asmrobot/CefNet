using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;

namespace CefNet.Wpf
{
	public class CursorChangeEventArgs : HandledEventArgs
	{
		public CursorChangeEventArgs(Cursor cursor, CefCursorType cursorType)
		{
			this.Cursor = cursor;
			this.CursorType = cursorType;
		}

		public Cursor Cursor { get; }

		public CefCursorType CursorType { get; }
	}
}
