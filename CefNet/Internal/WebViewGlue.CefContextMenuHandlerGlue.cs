using System;
using System.Collections.Generic;
using System.Text;

namespace CefNet.Internal
{
	public partial class WebViewGlue
	{

		internal bool AvoidOnBeforeContextMenu()
		{
			return false;
		}

		internal protected virtual void OnBeforeContextMenu(CefBrowser browser, CefFrame frame, CefContextMenuParams menuParams, CefMenuModel model)
		{

		}

		internal bool AvoidRunContextMenu()
		{
			return false;
		}

		internal protected virtual bool RunContextMenu(CefBrowser browser, CefFrame frame, CefContextMenuParams menuParams, CefMenuModel model, CefRunContextMenuCallback callback)
		{
			return WebView.RaiseRunContextMenu(frame, menuParams, model, callback);
		}

		internal bool AvoidOnContextMenuCommand()
		{
			return false;
		}

		internal protected virtual bool OnContextMenuCommand(CefBrowser browser, CefFrame frame, CefContextMenuParams menuParams, int commandId, CefEventFlags eventFlags)
		{
			return false;
		}

		internal bool AvoidOnContextMenuDismissed()
		{
			return false;
		}

		internal protected virtual void OnContextMenuDismissed(CefBrowser browser, CefFrame frame)
		{

		}
	}
}
