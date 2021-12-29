using System;
using System.Collections.Generic;
using System.Text;

namespace CefNet
{
	/// <summary>
	/// Represents PDF printing results.
	/// </summary>
	public interface IPdfPrintFinishedEventArgs
	{
		/// <summary>
		/// Gets the path to PDF file.
		/// </summary>
		string Path { get; }

		/// <summary>
		/// Gets a value which indicates that the PDF printing completed successfully.
		/// </summary>
		bool Success { get; }
	}
}
