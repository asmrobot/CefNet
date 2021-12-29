using System;
using System.Collections.Generic;
using System.Text;

namespace CefNet
{
	public sealed class DocumentTitleChangedEventArgs : EventArgs
	{
		public DocumentTitleChangedEventArgs(string title)
		{
			this.Title = title;
		}

		public string Title { get; }
	}
}
