using System;
using System.Collections.Generic;
using System.Text;

namespace CefNet
{
	/// <summary>
	/// Provides data for the <see cref="IChromiumWebView.LoadingStateChange"/> event.
	/// </summary>
	public class LoadingStateChangeEventArgs : EventArgs
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="LoadingStateChangeEventArgs"/> class.
		/// </summary>
		/// <param name="busy">true if the control is busy loading a document; otherwise, false.</param>
		/// <param name="canGoBack">true if the control can navigate backward; otherwise, false.</param>
		/// <param name="canGoForward">true if the control can navigate forward; otherwise, false.</param>
		public LoadingStateChangeEventArgs(bool busy, bool canGoBack, bool canGoForward)
		{
			this.Busy = busy;
			this.CanGoBack = canGoBack;
			this.CanGoForward = canGoForward;
		}

		/// <summary>
		/// Gets a value indicating whether the WebView control is currently loading a new document.
		/// </summary>
		/// <value>true if the control is busy loading a document; otherwise, false.</value>
		public bool Busy { get; }

		/// <summary>
		/// Gets a value indicating whether a previous page in navigation history is available,
		/// which allows the <see cref="IChromiumWebView.GoBack"/> method to succeed.
		/// </summary>
		/// <value>true if the control can navigate backward; otherwise, false.</value>
		public bool CanGoBack { get; }

		/// <summary>
		/// Gets a value indicating whether a subsequent page in navigation history
		/// is available, which allows the <see cref="IChromiumWebView.GoForward"/>
		/// method to succeed.
		/// </summary>
		/// <value>true if the control can navigate forward; otherwise, false.</value>
		public bool CanGoForward { get; }

	}
}
