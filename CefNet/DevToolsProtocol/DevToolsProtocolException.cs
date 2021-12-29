namespace CefNet
{
	/// <summary>
	/// The exception that is thrown when a Chrome DevTools Protocol call fails.
	/// </summary>
	public class DevToolsProtocolException : CefRuntimeException
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="DevToolsProtocolException"/>
		/// class with a specified error message.
		/// </summary>
		/// <param name="message">The message that describes the error.</param>
		public DevToolsProtocolException(string message)
			: base(message)
		{
		}

	}
}
