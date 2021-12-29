using System;
using System.Collections.Generic;
using System.Text;

namespace CefNet
{
	/// <summary>
	/// Provides data for the <see cref="IChromiumWebView.LoadError"/> event.
	/// </summary>
	public class LoadErrorEventArgs : EventArgs
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="LoadErrorEventArgs"/> class.
		/// </summary>
		/// <param name="frame">The frame.</param>
		/// <param name="errorCode">The error code.</param>
		/// <param name="errorText">The error text.</param>
		/// <param name="failedUrl">The URL that failed to load.</param>
		public LoadErrorEventArgs(CefFrame frame, CefErrorCode errorCode, string errorText, string failedUrl)
		{
			this.Frame = frame;
			this.ErrorCode = errorCode;
			this.ErrorText = errorText;
			this.FailedUrl = failedUrl;
		}

		/// <summary>
		/// The frame.
		/// </summary>
		public CefFrame Frame { get; }

		/// <summary>
		/// The error code.
		/// </summary>
		public CefErrorCode ErrorCode { get; }

		/// <summary>
		/// The error text.
		/// </summary>
		public string ErrorText { get; }

		/// <summary>
		/// The URL that failed to load.
		/// </summary>
		public string FailedUrl { get; }
	}
}
