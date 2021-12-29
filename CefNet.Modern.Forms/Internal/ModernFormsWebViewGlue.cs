using System;
using System.Collections.Generic;
using System.Text;
using CefNet.Modern.Forms;
using Modern.Forms;

namespace CefNet.Internal
{
	public class ModernFormsWebViewGlue : WebViewGlue
	{
		public ModernFormsWebViewGlue(IModernFormsWebViewPrivate view)
			: base(view)
		{

		}

		protected new IModernFormsWebViewPrivate WebView
		{
			get { return (IModernFormsWebViewPrivate)base.WebView; }
		}

		protected override bool OnCursorChange(CefBrowser browser, IntPtr cursorHandle, CefCursorType type, CefCursorInfo customCursorInfo)
		{
			//WebView.RaiseCefCursorChange(
			//	new CursorChangeEventArgs(type != CefCursorType.Custom ? CursorInteropHelper.Create(cursorHandle) : CustomCursor.Create(ref customCursorInfo), type)
			//);
			return false;
		}

		protected override bool OnTooltip(CefBrowser browser, ref string text)
		{
			WebView.CefSetToolTip(text);
			return true;
		}

		protected override void OnStatusMessage(CefBrowser browser, string message)
		{
			WebView.CefSetStatusText(message);
		}

		/// <summary>
		/// Called when the user starts dragging content in the web view. OS APIs that run a system message
		/// loop may be used within the StartDragging call. Don't call any of CefBrowserHost::DragSource*Ended*
		/// methods after returning false. Call CefBrowserHost::DragSourceEndedAt and DragSourceSystemDragEnded
		/// either synchronously or asynchronously to inform the web view that the drag operation has ended.
		/// </summary>
		/// <param name="browser"></param>
		/// <param name="dragData">The contextual information about the dragged content.</param>
		/// <param name="allowedOps"></param>
		/// <param name="x">The X-location in screen coordinates.</param>
		/// <param name="y">The Y-location in screen coordinates.</param>
		/// <returns>Return false to abort the drag operation or true to handle the drag operation.</returns>
		protected override bool StartDragging(CefBrowser browser, CefDragData dragData, CefDragOperationsMask allowedOps, int x, int y)
		{
			var e = new StartDraggingEventArgs(dragData, allowedOps, x, y);
			WebView.RaiseStartDragging(e);
			return e.Handled;
		}
	}
}
