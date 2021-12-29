using System;
using System.Collections.Generic;
using System.Text;

namespace CefNet.Internal
{
	partial class WebViewGlue
	{
		internal CefContextMenuHandler GetContextMenuHandler()
		{
			return ContextMenuGlue;
		}

		internal CefAudioHandler GetAudioHandler()
		{
			return AudioGlue;
		}

		internal CefDialogHandler GetDialogHandler()
		{
			return DialogGlue;
		}

		internal CefDisplayHandler GetDisplayHandler()
		{
			return DisplayGlue;
		}

		internal CefDownloadHandler GetDownloadHandler()
		{
			return DownloadGlue;
		}

		internal CefDragHandler GetDragHandler()
		{
			return DragGlue;
		}

		internal CefFindHandler GetFindHandler()
		{
			return FindGlue;
		}

		internal CefFocusHandler GetFocusHandler()
		{
			return FocusGlue;
		}

		internal CefFrameHandler GetFrameHandler()
		{
			return FrameGlue;
		}

		internal CefJSDialogHandler GetJSDialogHandler()
		{
			return JSDialogGlue;
		}

		internal CefKeyboardHandler GetKeyboardHandler()
		{
			return KeyboardGlue;
		}

		internal CefLifeSpanHandler GetLifeSpanHandler()
		{
			return LifeSpanGlue;
		}

		internal CefLoadHandler GetLoadHandler()
		{
			return LoadGlue;
		}

		internal CefPrintHandler GetPrintHandler()
		{
			return PrintGlue;
		}

		internal CefRenderHandler GetRenderHandler()
		{
			return RenderGlue;
		}

		internal CefRequestHandler GetRequestHandler()
		{
			return RequestGlue;
		}

		internal bool AvoidOnProcessMessageReceived()
		{
			return false;
		}

		protected internal virtual bool OnProcessMessageReceived(CefBrowser browser, CefFrame frame, CefProcessId sourceProcess, CefProcessMessage message)
		{
			var ea = new CefProcessMessageReceivedEventArgs(browser, frame, sourceProcess, message);
			CefNetApplication.Instance.OnCefProcessMessageReceived(ea);
			return ea.Handled;
		}
	}
}
