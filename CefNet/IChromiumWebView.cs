using CefNet.Input;
using System;
using System.ComponentModel;

namespace CefNet
{
	/// <summary>
	/// Provides the functionality for a WebView control.
	/// </summary>
	public interface IChromiumWebView
	{
		/// <summary>
		/// Occurs before a new browser window is opened.
		/// </summary>
		event EventHandler<CreateWindowEventArgs> CreateWindow;

		/// <summary>
		/// Occurs before the WebView control navigates to a new document.
		/// </summary>
		event EventHandler<BeforeBrowseEventArgs> Navigating;

		/// <summary>
		/// Occurs when the WebView control has navigated to a new document
		/// and has begun loading it.
		/// </summary>
		event EventHandler<NavigatedEventArgs> Navigated;

		/// <summary>
		/// Occurs when a navigation fails or is canceled.
		/// </summary>
		event EventHandler<LoadErrorEventArgs> LoadError;

		/// <summary>
		/// Occurs when a new frame is created. This will be the first notification
		/// that references <paramref name="frame"/>.
		/// </summary>
		/// <remarks>
		/// Any commands that require transport to the
		/// associated renderer process (LoadRequest, SendProcessMessage, GetSource,
		/// etc.) will be queued until <see cref="FrameAttached"/> is called for frame.
		/// </remarks>
		event EventHandler<FrameEventArgs> CefFrameCreated;

		/// <summary>
		/// Occurs when a frame can begin routing commands to/from the associated
		/// renderer process.
		/// </summary>
		/// <remarks>
		/// Any commands that were queued have now been dispatched.
		/// </remarks>
		event EventHandler<FrameEventArgs> CefFrameAttached;

		/// <summary>
		/// Occurs when a frame loses its connection to the renderer process and will
		/// be destroyed.
		/// </summary>
		/// <remarks>
		/// Any pending or future commands will be discarded and <see cref="CefFrame.IsValid"/>
		/// will now return false for <paramref name="frame"/>.
		/// </remarks>
		event EventHandler<FrameEventArgs> CefFrameDetached;

		/// <summary>
		/// Occurs before a CefFrame navigates to a new document.
		/// </summary>
		event EventHandler<BeforeBrowseEventArgs> BeforeBrowse;

		/// <summary>
		/// Occurs when a frame&apos;s address has changed.
		/// </summary>
		event EventHandler<AddressChangeEventArgs> AddressChange;

		/// <summary>
		/// Occurs when the loading state has changed.
		/// </summary>
		event EventHandler<LoadingStateChangeEventArgs> LoadingStateChange;

		/// <summary>
		/// Occurs when a browser has recieved a request to close. This event will be occurred after
		/// the JavaScript &apos;onunload&apos; event has been fired.
		/// </summary>
		event EventHandler<CancelEventArgs> Closing;

		/// <summary>
		/// Occurs just before a browser is destroyed.
		/// </summary>
		/// <remarks>
		/// Release all references to the <see cref="BrowserObject"/> and do not attempt to execute
		/// any methods on the <see cref="CefBrowser"/> object (other than <see cref="CefBrowser.Identifier"/>
		/// or <see cref="CefBrowser.IsSame"/> after this event.<para/>
		/// This event will be the last notification that references <see cref="BrowserObject"/> on the UI thread.
		/// Any in-progress network requests associated with <see cref="BrowserObject"/> will be aborted when
		/// the browser is destroyed, and <see cref="CefResourceRequestHandler"/> callbacks related to those
		/// requests may still arrive on the IO thread after this method is called.
		/// </remarks>
		event EventHandler Closed;

		/// <summary>
		/// Occurs when an view or a pop-up window should be painted.<para/>
		/// This event can be occurred on a thread other than the UI thread.
		/// </summary>
		/// <remarks>
		/// This event is only occurred when <see cref="CefWindowInfo.SharedTextureEnabled"/>
		/// is set to false.
		/// </remarks>
		event EventHandler<CefPaintEventArgs> CefPaint;

		/// <summary>
		/// Occurs when a new browser is created.
		/// </summary>
		event EventHandler BrowserCreated;

		/// <summary>
		/// Occurs when the page title changes.
		/// </summary>
		event EventHandler<DocumentTitleChangedEventArgs> DocumentTitleChanged;

		/// <summary>
		/// Occurs to report find results returned by <see cref="Find"/>.
		/// </summary>
		event EventHandler<ITextFoundEventArgs> TextFound;

		/// <summary>
		/// Occurs when the PDF printing has completed.
		/// </summary>
		event EventHandler<IPdfPrintFinishedEventArgs> PdfPrintFinished;

		/// <summary>
		/// Occurs when a DevTools protocol event is available.
		/// </summary>
		event EventHandler<DevToolsProtocolEventAvailableEventArgs> DevToolsProtocolEventAvailable;

		/// <summary>
		/// Occurs when a JavaScript dialog (alert, confirm, prompt, beforeunload) displays for the WebView.
		/// </summary>
		event EventHandler<IScriptDialogOpeningEventArgs> ScriptDialogOpening;

		/// <summary>
		/// Gets and sets a default URL.
		/// </summary>
		/// <remarks>
		/// This property cannot be used after the browser is created.
		/// </remarks>
		string InitialUrl { get; set; }

		/// <summary>
		/// Gets a value indicating whether a previous page in navigation history
		/// is available, which allows the <see cref="GoBack"/> method to succeed.
		/// </summary>
		/// <value>true if the control can navigate backward; otherwise, false.</value>
		bool CanGoBack { get; }

		/// <summary>
		/// Gets a value indicating whether a subsequent page in navigation history
		/// is available, which allows the <see cref="GoForward"/> method to succeed.
		/// </summary>
		/// <value>true if the control can navigate forward; otherwise, false.</value>
		bool CanGoForward { get; }

		/// <summary>
		/// Gets a value indicating whether the WebView control is currently loading
		/// a new document.
		/// </summary>
		/// <value>
		/// true if the control is busy loading a document; otherwise, false.
		/// </value>
		bool IsBusy { get; }

		/// <summary>
		/// Get the globally unique identifier for this browser. This value is also
		/// used as the tabId for extension APIs.
		/// </summary>
		int Identifier { get; }

		/// <summary>
		/// Gets a value indicating whether a document has been loaded in the browser.
		/// </summary>
		bool HasDocument { get; }

		/// <summary>
		/// Gets a WebView that opened the current WebView. If this WebView was not
		/// opened by being linked to or created by another, returns null.
		/// </summary>
		IChromiumWebView Opener { get; }

		/// <summary>
		/// Gets the <see cref="CefClient"/> for this WebView.
		/// </summary>
		CefClient Client { get; }

		/// <summary>
		/// Gets the rectangle that represents the bounds of the WebView control.
		/// </summary>
		/// <returns>
		/// A <see cref="CefRect"/> representing the bounds within which the WebView control is scaled.
		/// </returns>
		CefRect GetBounds();

		/// <summary>
		/// Sets the bounds of the control to the specified location and size.
		/// </summary>
		/// <param name="x">The new Left property value of the control.</param>
		/// <param name="y">The new Top property value of the control.</param>
		/// <param name="width">The new Width property value of the control.</param>
		/// <param name="height">The new Height property value of the control.</param>
		void SetBounds(int x, int y, int width, int height);

		/// <summary>
		/// Navigates the WebView control to the previous page in the navigation history, if one is available.
		/// </summary>
		/// <returns>
		/// True if the navigation succeeds; false if a previous page in the navigation history is not available.
		/// </returns>
		bool GoBack();

		/// <summary>
		/// Navigates the WebView control to the next page in the navigation history, if one is available.
		/// </summary>
		/// <returns>
		/// True if the navigation succeeds; false if a subsequent page in the navigation history is not available.
		/// </returns>
		bool GoForward();

		/// <summary>
		/// Reload the current page.
		/// </summary>
		void Reload();

		/// <summary>
		/// Reload the current page ignoring any cached data.
		/// </summary>
		void ReloadIgnoreCache();

		/// <summary>
		/// Stop loading the page.
		/// </summary>
		void Stop();

		/// <summary>
		/// Print the current browser contents.
		/// </summary>
		void Print();

		/// <summary>
		/// Print the WebView contents to the PDF file.
		/// </summary>
		/// <param name="path">The PDF file path.</param>
		/// <param name="settings">A PDF print settings.</param>
		void PrintToPdf(string path, CefPdfPrintSettings settings);

		/// <summary>
		/// If a misspelled word is currently selected in an editable node calling this
		/// function will replace it with the specified <paramref name="word"/>.
		/// </summary>
		/// <param name="word">The word to replace.</param>
		void ReplaceMisspelling(string word);

		/// <summary>
		/// Add the specified <paramref name="word"/> to the spelling dictionary.
		/// </summary>
		/// <param name="word">The word to be added to the spelling dictionary.</param>
		void AddWordToDictionary(string word);

		/// <summary>
		/// Returns the main (top-level) frame for the browser window.
		/// </summary>
		CefFrame GetMainFrame();

		/// <summary>
		/// Returns the focused frame for the browser window.
		/// </summary>
		CefFrame GetFocusedFrame();

		/// <summary>
		/// Returns the frame with the specified identifier, or null if not found.
		/// </summary>
		CefFrame GetFrame(long identifier);

		/// <summary>
		/// Returns the frame with the specified name, or null if not found.
		/// </summary>
		CefFrame GetFrame(string name);

		/// <summary>
		/// Returns the number of frames that currently exist.
		/// </summary>
		int GetFrameCount();

		/// <summary>
		/// Gets the identifiers of all existing frames.
		/// </summary>
		long[] GetFrameIdentifiers();

		/// <summary>
		/// Gets the names of all existing frames.
		/// </summary>
		string[] GetFrameNames();

		/// <summary>
		/// Open developer tools.
		/// </summary>
		void ShowDevTools();

		/// <summary>
		/// Open developer tools (DevTools). If the DevTools is already open then it will be focused.
		/// </summary>
		/// <param name="inspectElementAt">
		/// If <paramref name="inspectElementAt"/> is non-empty then the element at the specified (x,y) location will be inspected.
		/// </param>
		void ShowDevTools(CefPoint inspectElementAt);

		/// <summary>
		/// Explicitly close the associated developer tools, if any.
		/// </summary>
		void CloseDevTools();

		/// <summary>
		/// Send a notification to the browser that the screen info has changed.<para/>
		/// This function is only used when window rendering is disabled.
		/// </summary>
		/// <remarks>
		/// The browser will then call <see cref="CefRenderHandler.GetScreenInfo"/>
		/// to update the screen information with the new values. This simulates moving
		/// the webview window from one display to another, or changing the properties
		/// of the current display.
		/// </remarks>
		void NotifyScreenInfoChanged();

		/// <summary>
		/// Sends a notification to the browser that the root window has been moved or resized.<para/>
		/// This function is only used when window rendering is disabled.
		/// </summary>
		void NotifyRootMovedOrResized();

		/// <summary>
		/// Closes the current browser window.
		/// </summary>
		void Close();

		/// <summary>
		/// Loads the document at the specified location into the WebView control.
		/// </summary>
		/// <param name="url">The URL of the document to load.</param>
		void Navigate(string url);

		/// <summary>
		/// Gets the associated <see cref="CefBrowser"/> object.
		/// </summary>
		CefBrowser BrowserObject { get; }

		/// <summary>
		/// Sends a mouse move event to the browser.
		/// </summary>
		/// <param name="x">The x-coordinate of the mouse relative to the left edge of the view.</param>
		/// <param name="y">The y-coordinate of the mouse relative to the top edge of the view.</param>
		/// <param name="modifiers">A bitwise combination of the <see cref="CefEventFlags"/> values.</param>
		void SendMouseMoveEvent(int x, int y, CefEventFlags modifiers);

		/// <summary>
		/// Sends a mouse leave event to the browser.
		/// </summary>
		void SendMouseLeaveEvent();

		/// <summary>
		/// Sends a mouse down event to the browser.
		/// </summary>
		/// <param name="x">The x-coordinate of the mouse relative to the left edge of the view.</param>
		/// <param name="y">The y-coordinate of the mouse relative to the top edge of the view.</param>
		/// <param name="button">One of the <see cref="CefMouseButtonType"/> values.</param>
		/// <param name="clicks">A click count.</param>
		/// <param name="modifiers">A bitwise combination of the <see cref="CefEventFlags"/> values.</param>
		void SendMouseDownEvent(int x, int y, CefMouseButtonType button, int clicks, CefEventFlags modifiers);

		/// <summary>
		/// Sends a mouse up event to the browser.
		/// </summary>
		/// <param name="x">The x-coordinate of the mouse relative to the left edge of the view.</param>
		/// <param name="y">The y-coordinate of the mouse relative to the top edge of the view.</param>
		/// <param name="button">One of the <see cref="CefMouseButtonType"/> values.</param>
		/// <param name="clicks">A click count.</param>
		/// <param name="modifiers">A bitwise combination of the <see cref="CefEventFlags"/> values.</param>
		void SendMouseUpEvent(int x, int y, CefMouseButtonType button, int clicks, CefEventFlags modifiers);

		/// <summary>
		/// Sends a mouse wheel event to the browser.
		/// </summary>
		/// <param name="x">The x-coordinate of the mouse relative to the left edge of the view.</param>
		/// <param name="y">The y-coordinate of the mouse relative to the top edge of the view.</param>
		/// <param name="deltaX">A movement delta in the X direction.</param>
		/// <param name="deltaY">A movement delta in the Y direction.</param>
		void SendMouseWheelEvent(int x, int y, int deltaX, int deltaY);

		/// <summary>
		/// Send a touch event to the browser.
		/// </summary>
		/// <param name="eventInfo">The touch event information.</param>
		void SendTouchEvent(CefTouchEvent eventInfo);

		/// <summary>
		/// Sends the KeyDown event to the browser.
		/// </summary>
		/// <param name="c">The character associated with the key.</param>
		/// <param name="ctrlKey">The Control key flag.</param>
		/// <param name="altKey">The Alt key flag.</param>
		/// <param name="shiftKey">The Shift key flag.</param>
		/// <param name="metaKey">The Meta key flag.</param>
		/// <param name="repeatCount">The repeat count.</param>
		/// <param name="extendedKey">The extended key flag.</param>
		void SendKeyDown(char c, bool ctrlKey = false, bool altKey = false, bool shiftKey = false, bool metaKey = false, int repeatCount = 0, bool extendedKey = false);

		/// <summary>
		/// Sends the KeyDown event to the browser.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="ctrlKey">The Control key flag.</param>
		/// <param name="altKey">The Alt key flag.</param>
		/// <param name="shiftKey">The Shift key flag.</param>
		/// <param name="metaKey">The Meta key flag.</param>
		/// <param name="repeatCount">The repeat count.</param>
		/// <param name="extendedKey">The extended key flag.</param>
		void SendKeyDown(VirtualKeys key, bool ctrlKey = false, bool altKey = false, bool shiftKey = false, bool metaKey = false, int repeatCount = 0, bool extendedKey = false);

		/// <summary>
		/// Sends the KeyUp event to the browser.
		/// </summary>
		/// <param name="c">The character associated with the key.</param>
		/// <param name="ctrlKey">The Control key flag.</param>
		/// <param name="altKey">The Alt key flag.</param>
		/// <param name="shiftKey">The Shift key flag.</param>
		/// <param name="metaKey">The Meta key flag.</param>
		/// <param name="extendedKey">The extended key flag.</param>
		void SendKeyUp(char c, bool ctrlKey = false, bool altKey = false, bool shiftKey = false, bool metaKey = false, bool extendedKey = false);


		/// <summary>
		/// Sends the KeyUp event to the browser.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="ctrlKey">The Control key flag.</param>
		/// <param name="altKey">The Alt key flag.</param>
		/// <param name="shiftKey">The Shift key flag.</param>
		/// <param name="metaKey">The Meta key flag.</param>
		/// <param name="extendedKey">The extended key flag.</param>
		void SendKeyUp(VirtualKeys key, bool ctrlKey = false, bool altKey = false, bool shiftKey = false, bool metaKey = false, bool extendedKey = false);

		/// <summary>
		/// Sends the KeyPress event to the browser.
		/// </summary>
		/// <param name="c">The character associated with the key.</param>
		/// <param name="ctrlKey">The Control key flag.</param>
		/// <param name="altKey">The Alt key flag.</param>
		/// <param name="shiftKey">The Shift key flag.</param>
		/// <param name="metaKey">The Meta key flag.</param>
		/// <param name="extendedKey">The extended key flag.</param>
		void SendKeyPress(char c, bool ctrlKey = false, bool altKey = false, bool shiftKey = false, bool metaKey = false, bool extendedKey = false);

		/// <summary>
		/// Search for <paramref name="searchText"/>. The <see cref="TextFound"/> event
		/// will be occurred to report find results.
		/// </summary>
		/// <param name="identifier">
		/// An unique ID and these IDs must strictly increase so that newer requests always
		/// have greater IDs than older requests. If <paramref name="identifier"/> is zero or less than the
		/// previous ID value then it will be automatically assigned a new valid ID.
		/// </param>
		/// <param name="searchText">The string to seek.</param>
		/// <param name="forward">A value which indicates whether to search forward or backward within the page.</param>
		/// <param name="matchCase">The true value indicates that the search should be case-sensitive.</param>
		/// <param name="findNext">A value which indicates whether this is the first request or a follow-up.</param>
		void Find(int identifier, string searchText, bool forward, bool matchCase, bool findNext);

		/// <summary>
		/// Cancel all searches that are currently going on.
		/// </summary>
		void StopFinding(bool clearSelection);

	}
}
