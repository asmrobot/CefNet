using CefNet.WinApi;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace CefNet.Internal
{
	public class WindowlessWebViewGlue : WebViewGlue
	{
		public WindowlessWebViewGlue(IChromiumWebViewPrivate view) 
			: base(view)
		{

		}

		/// <inheritdoc />
		internal protected override bool OnFileDialog(CefBrowser browser, CefFileDialogMode mode, string title, string defaultFilePath, CefStringList acceptFilters, int selectedAcceptFilter, CefFileDialogCallback callback)
		{
			callback.Cancel();
			return true;
		}

	}
}
