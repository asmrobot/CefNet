using System;
using System.Collections.Generic;
using System.Text;

namespace CefNet
{
	/// <summary>
	/// Represents errors that occur during remote call execution.
	/// </summary>
	public class CefNetRemoteException : CefRuntimeException
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="CefNetRemoteException"/> class
		/// with a specified error message and a reference to the inner exception
		/// that is the cause of this exception.
		/// </summary>
		/// <param name="message">
		/// The error message that explains the reason for the exception.
		/// </param>
		/// <param name="exceptionType">
		/// The exception that is the cause of the current exception.
		/// </param>
		/// <param name="remoteStackTrace">
		/// A string that describes the immediate frames of the remote call stack.
		/// </param>
		public CefNetRemoteException(string message, string exceptionType, string remoteStackTrace)
			: base(message)
		{
			if (exceptionType == null)
				throw new ArgumentNullException(nameof(exceptionType));
			this.ExceptionType = exceptionType;
			this.RemoteStackTrace = remoteStackTrace;
		}

		/// <summary>
		/// The full name of the exception type that is the cause of the current exception.
		/// </summary>
		public string ExceptionType { get; }

		/// <summary>
		/// Gets a string representation of the immediate frames on the remote call stack.
		/// </summary>
		/// <returns>
		/// A string that describes the immediate frames of the remote call stack.
		/// </returns>
		public virtual string RemoteStackTrace { get; }
	}
}
