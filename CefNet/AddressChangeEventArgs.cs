using System;
using System.Collections.Generic;
using System.Text;

namespace CefNet
{
	public class AddressChangeEventArgs : NavigatedEventArgs
	{
		public AddressChangeEventArgs(CefFrame frame, string url)
			: base(frame, url)
		{

		}

		public bool IsMainFrame
		{
			get { return Frame?.IsMain ?? false; }
		}

	}
}
