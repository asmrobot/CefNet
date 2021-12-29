#if MODERNFORMS
using CefNet.Modern.Forms;
#else
using CefNet.Windows.Forms;
#endif

namespace CefNet.Internal
{
#if MODERNFORMS
	public interface IModernFormsWebViewPrivate: IChromiumWebViewPrivate
#else
	public interface IWinFormsWebViewPrivate: IChromiumWebViewPrivate
#endif
	{
		void RaiseCefCursorChange(CursorChangeEventArgs e);
		void CefSetToolTip(string text);
		void CefSetStatusText(string statusText);
		void RaiseStartDragging(StartDraggingEventArgs e);
	}
}
