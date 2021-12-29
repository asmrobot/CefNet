using CefNet;
using CefNet.Internal;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsCoreApp
{
	sealed class CustomWebViewGlue :
#if MODERNFORMS
		ModernFormsWebViewGlue
#else
		WinFormsWebViewGlue
#endif
	{
		private const int SHOW_DEV_TOOLS = (int)CefMenuId.UserFirst + 0;
		private const int INSPECT_ELEMENT = (int)CefMenuId.UserFirst + 1;

		private Dictionary<Guid, CefResourceHandler> _customSources = new Dictionary<Guid, CefResourceHandler>();

		public CustomWebViewGlue(CustomWebView view)
			: base(view)
		{

		}

		private new CustomWebView WebView
		{
			get { return (CustomWebView)base.WebView; }
		}

		protected override void OnBeforeContextMenu(CefBrowser browser, CefFrame frame, CefContextMenuParams menuParams, CefMenuModel model)
		{
			if (model.Count > 0)
				model.AddSeparator();

			model.AddItem(SHOW_DEV_TOOLS, "&Show DevTools");
			model.AddItem(INSPECT_ELEMENT, "Inspect element");
			model.AddSeparator();

			CefMenuModel submenu = model.AddSubMenu(0, "Submenu Test");
			submenu.AddItem((int)CefMenuId.Copy, "Copy");
			submenu.AddItem((int)CefMenuId.Paste, "Paste");
			submenu.SetColorAt(submenu.Count - 1, CefMenuColorType.Text, CefColor.FromArgb(Color.Blue.ToArgb()));
			submenu.AddCheckItem(0, "Checked Test");
			submenu.SetCheckedAt(submenu.Count - 1, true);
			submenu.AddRadioItem(0, "Radio Off", 0);
			submenu.AddRadioItem(0, "Radio On", 1);
			submenu.SetCheckedAt(submenu.Count - 1, true);
		}

		protected override bool OnContextMenuCommand(CefBrowser browser, CefFrame frame, CefContextMenuParams menuParams, int commandId, CefEventFlags eventFlags)
		{
			if (commandId >= (int)CefMenuId.UserFirst && commandId <= (int)CefMenuId.UserLast)
			{
				switch(commandId)
				{
					case SHOW_DEV_TOOLS:
						WebView.ShowDevTools();
						break;
					case INSPECT_ELEMENT:
						WebView.ShowDevTools(new CefPoint(menuParams.XCoord, menuParams.YCoord));
						break;
				}
				return true;
			}
			return false; ;
		}

		protected override bool OnConsoleMessage(CefBrowser browser, CefLogSeverity level, string message, string source, int line)
		{
			Debug.Print("[{0}]: {1} ({2}, line: {3})", level, message, source, line);
			return false;
		}

		internal void AddSource(Guid sourceKey, CefResourceHandler source)
		{
			_customSources.Add(sourceKey, source);
		}

		protected override CefResourceHandler GetResourceHandler(CefBrowser browser, CefFrame frame, CefRequest request)
		{
			if (Guid.TryParse(request.GetHeaderByName("CefNet-Source"), out Guid sourceKey)
				&& _customSources.Remove(sourceKey, out CefResourceHandler resourceHandler))
			{
				return resourceHandler;
			}
			return base.GetResourceHandler(browser, frame, request);
		}
	}
}
