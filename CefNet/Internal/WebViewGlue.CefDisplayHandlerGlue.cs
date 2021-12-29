using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace CefNet.Internal
{
	public partial class WebViewGlue
	{

		internal bool AvoidOnAddressChange()
		{
			return false;
		}

		/// <summary>
		/// Called when a frame&apos;s address has changed.
		/// </summary>
		/// <param name="browser">The <see cref="CefBrowser"/> object.</param>
		/// <param name="frame">A <see cref="CefFrame"/> object.</param>
		/// <param name="url">
		/// A string representing the location of the document
		/// to which the <paramref name="frame"/> has navigated.
		/// </param>
		internal protected virtual void OnAddressChange(CefBrowser browser, CefFrame frame, string url)
		{
			WebView.RaiseAddressChange(new AddressChangeEventArgs(frame, url));
		}

		internal bool AvoidOnTitleChange()
		{
			return false;
		}

		/// <summary>
		/// Called when the page title changes.
		/// </summary>
		/// <param name="browser"></param>
		/// <param name="title"></param>
		internal protected virtual void OnTitleChange(CefBrowser browser, string title)
		{
			WebView.RaiseTitleChange(new DocumentTitleChangedEventArgs(title));
		}

		[MethodImpl(MethodImplOptions.ForwardRef)]
		internal extern bool AvoidOnFaviconUrlChange();

		/// <summary>
		/// Called when the page icon changes.
		/// </summary>
		/// <param name="browser"></param>
		/// <param name="iconUrls"></param>
		internal protected virtual void OnFaviconUrlChange(CefBrowser browser, CefStringList iconUrls)
		{

		}

		[MethodImpl(MethodImplOptions.ForwardRef)]
		internal extern bool AvoidOnFullscreenModeChange();

		/// <summary>
		/// Called when web content in the page has toggled fullscreen mode.
		/// The client is responsible for resizing the browser if desired.
		/// </summary>
		/// <param name="browser"></param>
		/// <param name="fullscreen">
		/// If |fullscreen| is true the content will automatically be sized to fill the browser content area.
		/// If |fullscreen| is false the content will automatically return to its original size and position.
		/// </param>
		internal protected virtual void OnFullscreenModeChange(CefBrowser browser, bool fullscreen)
		{

		}

		[MethodImpl(MethodImplOptions.ForwardRef)]
		internal extern bool AvoidOnTooltip();

		/// <summary>
		/// Called when the browser is about to display a tooltip. To handle the display of the tooltip yourself return true.
		/// Otherwise, you can optionally modify |text| and then return false to allow the browser to display the tooltip.
		/// When window rendering is disabled the application is responsible for drawing tooltips and the return value is ignored.
		/// </summary>
		/// <param name="browser"></param>
		/// <param name="text">
		/// Contains the text that will be displayed in the tooltip.
		/// </param>
		/// <returns></returns>
		internal protected virtual bool OnTooltip(CefBrowser browser, ref string text)
		{
			return false;
		}

		[MethodImpl(MethodImplOptions.ForwardRef)]
		internal extern bool AvoidOnStatusMessage();

		/// <summary>
		/// Called when the browser receives a status message.
		/// </summary>
		/// <param name="browser"></param>
		/// <param name="message">
		/// Contains the text that will be displayed in the status message.
		/// </param>
		internal protected virtual void OnStatusMessage(CefBrowser browser, string message)
		{

		}

		[MethodImpl(MethodImplOptions.ForwardRef)]
		internal extern bool AvoidOnConsoleMessage();

		/// <summary>
		/// Called to display a console message. 
		/// </summary>
		/// <param name="browser"></param>
		/// <param name="level">Log severity level.</param>
		/// <param name="message">The message.</param>
		/// <param name="source">The source.</param>
		/// <param name="line">The line.</param>
		/// <returns>
		/// Return true to stop the message from being output to the console.
		/// </returns>
		internal protected virtual bool OnConsoleMessage(CefBrowser browser, CefLogSeverity level, string message, string source, int line)
		{
			return false;
		}

		[MethodImpl(MethodImplOptions.ForwardRef)]
		internal extern bool AvoidOnAutoResize();

		/// <summary>
		/// Called when auto-resize is enabled via CefBrowserHost::SetAutoResizeEnabled and the contents have auto-resized.
		/// </summary>
		/// <param name="browser"></param>
		/// <param name="newSize">
		/// The desired size in view coordinates.
		/// </param>
		/// <returns>
		/// Return true if the resize was handled or false for default handling.
		/// </returns>
		internal protected virtual bool OnAutoResize(CefBrowser browser, CefSize newSize)
		{
			return false;
		}

		[MethodImpl(MethodImplOptions.ForwardRef)]
		internal extern bool AvoidOnLoadingProgressChange();

		/// <summary>
		/// Called when the overall page loading progress has changed. .
		/// </summary>
		/// <param name="browser"></param>
		/// <param name="progress">
		/// The |progress| ranges from 0.0 to 1.0
		/// </param>
		internal protected virtual void OnLoadingProgressChange(CefBrowser browser, double progress)
		{

		}

		internal bool AvoidOnCursorChange()
		{
			return false;
		}

		/// <summary>
		/// Called when the browser&apos;s cursor has changed.
		/// </summary>
		/// <param name="browser">The browser.</param>
		/// <param name="cursor">The cursor handle.</param>
		/// <param name="type">The cursor type.</param>
		/// <param name="customCursorInfo">
		/// The custom cursor information (if <paramref name="type"/> is <see cref="CefCursorType.Custom"/>).
		/// </param>
		/// <returns>true if the cursor change was handled or false for default handling.</returns>
		internal protected virtual bool OnCursorChange(CefBrowser browser, IntPtr cursor, CefCursorType type, CefCursorInfo customCursorInfo)
		{
			return false;
		}

	}
}
