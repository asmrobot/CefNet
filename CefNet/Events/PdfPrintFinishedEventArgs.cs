using System;
using System.Collections.Generic;
using System.Text;

namespace CefNet
{
	/// <summary>
	/// Represents PDF printing results.
	/// </summary>
	public class PdfPrintFinishedEventArgs : EventArgs, IPdfPrintFinishedEventArgs
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="PdfPrintFinishedEventArgs"/> class.
		/// </summary>
		/// <param name="path">The path to PDF file.</param>
		/// <param name="success">A value which indicates that the PDF printing completed successfully.</param>
		public PdfPrintFinishedEventArgs(string path, bool success)
		{
			this.Path = path;
			this.Success = success;
		}

		/// <summary>
		/// Gets the path to PDF file.
		/// </summary>
		public string Path { get; }

		/// <summary>
		/// Gets a value which indicates that the PDF printing completed successfully.
		/// </summary>
		/// <value></value>
		public bool Success { get; }
	}
}
