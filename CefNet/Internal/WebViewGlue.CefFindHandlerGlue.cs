using System;
using System.Collections.Generic;
using System.Text;

namespace CefNet.Internal
{
	public partial class WebViewGlue
	{
		internal bool AvoidOnFindResult()
		{
			return false;
		}

		internal protected virtual void OnFindResult(CefBrowser browser, int identifier, int count, CefRect selectionRect, int activeMatchOrdinal, bool finalUpdate)
		{
			WebView.RaiseTextFound(new TextFoundEventArgs(identifier, count, selectionRect, activeMatchOrdinal, finalUpdate));
		}
	}
}
