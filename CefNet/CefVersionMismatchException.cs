namespace CefNet
{
	/// <summary>
	/// Specifies information about a version mismatch exception.
	/// </summary>
	public sealed class CefVersionMismatchException : CefRuntimeException
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="CefVersionMismatchException"/>
		/// class with a specified error message.
		/// </summary>
		/// <param name="message">The message that describes the error.</param>
		public CefVersionMismatchException(string message)
			: base(message)
		{
		}
	}
}
