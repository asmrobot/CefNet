using System;
using System.Collections.Generic;
using System.Text;

namespace CefNet.Internal
{
	partial class CefClientGlue
	{
		internal WebViewGlue Implementation
		{
			get { return _implementation; }
		}

		public void NotifyPopupBrowserCreating()
		{
			_implementation.NotifyPopupBrowserCreating();
		}
	}
}
