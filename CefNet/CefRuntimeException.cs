using System;
using System.Collections.Generic;
using System.Text;

namespace CefNet
{
	/// <summary>
	/// Represents errors that occur during CEF-application execution.
	/// </summary>
	public class CefRuntimeException : Exception
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="CefRuntimeException"/> class
		/// with a specified error message.
		/// </summary>
		/// <param name="message">The message that describes the error.</param>
		public CefRuntimeException(string message)
			: base(message)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CefRuntimeException"/> class
		/// with a specified error message and a reference to the inner exception that
		/// is the cause of this exception.
		/// </summary>
		/// <param name="message">
		/// The error message that explains the reason for the exception.
		/// </param>
		/// <param name="innerException">
		/// The exception that is the cause of the current exception, or a null reference
		/// if no inner exception is specified.
		/// </param>
		public CefRuntimeException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}

}
