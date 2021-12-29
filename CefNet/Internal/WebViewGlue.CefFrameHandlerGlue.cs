using System.Runtime.CompilerServices;

namespace CefNet.Internal
{
	partial class WebViewGlue
	{
		public static object FrameCreatedEvent = new object();
		public static object FrameAttachedEvent = new object();
		public static object FrameDetachedEvent = new object();

		[MethodImpl(MethodImplOptions.ForwardRef)]
		private extern bool AvoidOnFrameCreatedImpl();

		internal bool AvoidOnFrameCreated()
		{
			return AvoidOnFrameCreatedImpl() && Events[FrameCreatedEvent] == null;
		}

		/// <summary>
		/// Called when a new frame is created. This will be the first notification
		/// that references <paramref name="frame"/>. Any commands that require transport to the
		/// associated renderer process (LoadRequest, SendProcessMessage, GetSource,
		/// etc.) will be queued until OnFrameAttached is called for <paramref name="frame"/>.
		/// </summary>
		/// <param name="browser">The <see cref="CefBrowser"/> object.</param>
		/// <param name="frame">The newly created frame.</param>
		internal protected virtual void OnFrameCreated(CefBrowser browser, CefFrame frame)
		{
			WebView.RaiseCefFrameCreated(new FrameEventArgs(frame));
		}

		[MethodImpl(MethodImplOptions.ForwardRef)]
		private extern bool AvoidOnFrameAttachedImpl();

		internal bool AvoidOnFrameAttached()
		{
			return AvoidOnFrameAttachedImpl() && Events[FrameAttachedEvent] == null;
		}

		/// <summary>
		/// Called when a frame can begin routing commands to/from the associated
		/// renderer process. Any commands that were queued have now been dispatched.
		/// </summary>
		internal protected virtual void OnFrameAttached(CefBrowser browser, CefFrame frame, bool reattached)
		{
			WebView.RaiseCefFrameAttached(new FrameEventArgs(frame, reattached));
		}

		[MethodImpl(MethodImplOptions.ForwardRef)]
		private extern bool AvoidOnFrameDetachedImpl();

		internal bool AvoidOnFrameDetached()
		{
			return AvoidOnFrameDetachedImpl() && Events[FrameDetachedEvent] == null;
		}

		/// <summary>
		/// Called when a frame loses its connection to the renderer process and will
		/// be destroyed.
		/// </summary>
		/// <param name="browser">The <see cref="CefBrowser"/> object.</param>
		/// <param name="frame">The detached <see cref="CefFrame"/> object.</param>
		/// <remarks>
		/// Any pending or future commands will be discarded and <see cref="CefFrame.IsValid"/>
		/// will now return false for <paramref name="frame"/>. If called after
		/// <see cref="OnBeforeClose(CefBrowser)"/> during browser destruction then
		/// <see cref="CefBrowser.IsValid"/> will return false for <paramref name="browser"/>.
		/// </remarks>
		internal protected virtual void OnFrameDetached(CefBrowser browser, CefFrame frame)
		{
			WebView.RaiseCefFrameDetached(new FrameEventArgs(frame));
		}

		[MethodImpl(MethodImplOptions.ForwardRef)]
		internal extern bool AvoidOnMainFrameChanged();

		/// <summary>
		/// Called when the main frame changes due to
		/// <list type="bullet">
		/// <item>initial browser creation;</item>
		/// <item>final browser destruction;</item>
		/// <item>cross-origin navigation;</item>
		/// <item>re-navigation after renderer process termination (due to crashes, etc).</item>
		/// </list>
		/// </summary>
		/// <param name="browser">
		/// The <see cref="CefBrowser"/> object.<para/>
		/// If <see cref="OnMainFrameChanged"/> called after <see cref="OnBeforeClose"/> during browser
		/// destruction then <see cref="CefBrowser.IsValid"/> will return false.
		/// </param>
		/// <param name="oldFrame">
		/// The old frame or null when a main frame is assigned to <paramref name="browser"/> for the first time.
		/// </param>
		/// <param name="newFrame">
		/// The new frame or null when a main frame is removed from
		/// <paramref name="browser"/> for the last time.</param>
		/// <remarks>
		/// This function will be called after <see cref="OnFrameCreated"/> for <paramref name="newFrame"/>
		/// and/or after <see cref="OnFrameDetached"/> for <paramref name="oldFrame"/>.
		/// </remarks>
		internal protected virtual void OnMainFrameChanged(CefBrowser browser, CefFrame oldFrame, CefFrame newFrame)
		{

		}

	}
}
