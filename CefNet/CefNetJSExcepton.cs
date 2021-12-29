using System;
using System.Collections.Generic;
using System.Text;

namespace CefNet
{
	/// <summary>
	/// Represents a V8 exception.
	/// </summary>
	public class CefNetJSExcepton : CefRuntimeException
	{
		/// <summary>
		/// Initializes new instance of the <see cref="CefNetJSExcepton"/> class.
		/// </summary>
		/// <param name="message">
		/// The error message that explains the reason for the exception.
		/// </param>
		/// <param name="scriptName">
		/// The resource name for the script from where the function causing the error originates.
		/// </param>
		/// <param name="sourceLine">
		/// The line of source code that the exception occurred within.
		/// </param>
		/// <param name="line">
		/// The line where the error occurred or 0 if the line number is unknown.
		/// </param>
		/// <param name="column">
		/// The index within the line of the first character where the error occurred.
		/// </param>
		public CefNetJSExcepton(string message, string scriptName, string sourceLine, int line, int column)
			: base(message)
		{
			this.SourceLine = sourceLine;
			this.ScriptName = scriptName;
			this.Line = line;
			this.Column = column;
		}

		/// <summary>
		/// Initializes new instance of the <see cref="CefNetJSExcepton"/> class.
		/// </summary>
		/// <param name="exception">
		/// The CEF V8 exception that is the cause of the current exception.
		/// </param>
		public CefNetJSExcepton(CefV8Exception exception)
			: base(exception.Message)
		{
			this.SourceLine = exception.SourceLine;
			this.ScriptName = exception.ScriptResourceName;
			this.Line = exception.LineNumber;
			this.Column = exception.StartColumn;
		}

		/// <summary>
		/// Gets the line of source code that the exception occurred within.
		/// </summary>
		public string SourceLine { get; private set; }

		/// <summary>
		/// Gets the resource name for the script from where the function causing
		/// the error originates.
		/// </summary>
		public string ScriptName { get; private set; }

		/// <summary>
		/// Gets the 1-based number of the line where the error occurred or 0 if the
		/// line number is unknown.
		/// </summary>
		public int Line { get; private set; }

		/// <summary>
		/// Gets the index within the line of the first character where the error
		/// occurred.
		/// </summary>
		public int Column { get; private set; }
	}
}
