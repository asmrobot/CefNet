using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CefNet
{
	/// <summary>
	/// Provides data for the <see cref="CefNetApplication.CefProcessMessageReceived"/> event.
	/// </summary>
	public class CefProcessMessageReceivedEventArgs : HandledEventArgs
	{
		private string _name;

		/// <summary>
		/// Initializes a new instance of the <see cref="CefProcessMessageReceivedEventArgs"/> class.
		/// </summary>
		/// <param name="browser">The <see cref="CefBrowser"/> that triggered the event.</param>
		/// <param name="frame">The <see cref="CefFrame"/> that triggered the event.</param>
		/// <param name="sourceProcess">The type of a source process.</param>
		/// <param name="message">A received message.</param>
		public CefProcessMessageReceivedEventArgs(CefBrowser browser, CefFrame frame, CefProcessId sourceProcess, CefProcessMessage message)
		{
			this.Browser = browser;
			this.Frame = frame;
			this.SourceProcess = sourceProcess;
			this.Message = message;
		}

		/// <summary>
		/// Gets the name of the message.
		/// </summary>
		public string Name
		{
			get
			{
				if (_name == null)
					_name = Message.Name;
				return _name;
			}
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
		/// Gets the type of a source process.
		/// </summary>
		public CefProcessId SourceProcess { get; }

		/// <summary>
		/// Gets the message.
		/// </summary>
		public CefProcessMessage Message { get; }
	}
}
