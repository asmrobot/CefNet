using System;
using System.Collections.Generic;
using System.Text;

namespace CefNet
{
	/// <summary>
	/// Provides data for the <see cref="IChromiumWebView.DevToolsProtocolEventAvailable"/> event.
	/// </summary>
	public sealed class DevToolsProtocolEventAvailableEventArgs
	{
		internal DevToolsProtocolEventAvailableEventArgs(string eventName, string data)
		{
			this.EventName = eventName;
			this.Data = data;
		}

		/// <summary>
		/// Gets the name of a DevTools Protocol event.
		/// </summary>
		public string EventName { get; }

		/// <summary>
		/// Gets a data for a DevTools Protocol event.
		/// </summary>
		public string Data { get; }
	}
}
