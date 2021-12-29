using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

#pragma warning disable CS1591

namespace CefNet.Internal
{
	public interface IChromiumWebViewPrivate
	{
		void RaisePopupBrowserCreating();
		float GetDevicePixelRatio();
		CefRect GetCefViewBounds();
		CefRect GetCefRootBounds();
		bool GetCefScreenInfo(ref CefScreenInfo screenInfo);
		bool CefPointToScreen(ref CefPoint point);
		void RaiseCefBrowserCreated();
		bool RaiseClosing();
		void RaiseClosed();
		//void RaiseBrowserDestroyed();
		void RaiseCefCreateWindow(CreateWindowEventArgs e);
		void RaiseCefPaint(CefPaintEventArgs e);
		void RaisePopupShow(PopupShowEventArgs e);
		void RaiseTitleChange(DocumentTitleChangedEventArgs e);
		void RaiseBeforeBrowse(BeforeBrowseEventArgs e);
		void RaiseAddressChange(AddressChangeEventArgs e);
		void RaiseLoadingStateChange(LoadingStateChangeEventArgs e);
		void RaiseLoadError(LoadErrorEventArgs e);
		bool RaiseRunContextMenu(CefFrame frame, CefContextMenuParams menuParams, CefMenuModel model, CefRunContextMenuCallback callback);
		void RaiseTextFound(ITextFoundEventArgs e);
		void RaisePdfPrintFinished(IPdfPrintFinishedEventArgs e);
		void RaiseDevToolsEventAvailable(DevToolsProtocolEventAvailableEventArgs e);
		void RaiseScriptDialogOpening(IScriptDialogOpeningEventArgs e);
		void RaiseCefFrameCreated(FrameEventArgs e);
		void RaiseCefFrameAttached(FrameEventArgs e);
		void RaiseCefFrameDetached(FrameEventArgs e);
	}
}
