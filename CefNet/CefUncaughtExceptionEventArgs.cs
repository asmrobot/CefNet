using System;
using System.Collections.Generic;
using System.Text;

namespace CefNet
{
	/// <summary>
	/// Provides data for the <see cref="CefNetApplication.CefUncaughtException"/> event.
	/// </summary>
	public class CefUncaughtExceptionEventArgs : EventArgs
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="CefUncaughtExceptionEventArgs"/> class.
		/// </summary>
		/// <param name="browser">The <see cref="CefBrowser"/> that triggered the event.</param>
		/// <param name="frame">The <see cref="CefFrame"/> that triggered the event.</param>
		/// <param name="context">The <see cref="CefV8Context"/> in which the exception occurred.</param>
		/// <param name="exception">The unhandled V8 exception object.</param>
		/// <param name="stackTrace">The V8 stack trace.</param>
		public CefUncaughtExceptionEventArgs(CefBrowser browser, CefFrame frame, CefV8Context context, CefV8Exception exception, CefV8StackTrace stackTrace)
		{
			this.Browser = browser;
			this.Frame = frame;
			this.Context = context;
			this.Exception = exception;
			this.StackTrace = stackTrace;
		}

		/// <summary>
		/// Gets the <see cref="CefBrowser"/> that triggered the event.
		/// </summary>
		public CefBrowser Browser { get; }

		/// <summary>
		/// Gets the <see cref="CefFrame"/> that triggered the event.
		/// </summary>
		public CefFrame Frame { get; }

		/// <summary>
		/// Gets the <see cref="CefV8Context"/> in which the exception occurred.
		/// </summary>
		public CefV8Context Context { get; }

		/// <summary>
		/// Gets the unhandled V8 exception object.
		/// </summary>
		public CefV8Exception Exception { get; }

		/// <summary>
		/// Gets the V8 stack trace.
		/// </summary>
		public CefV8StackTrace StackTrace { get; }
	}
}
