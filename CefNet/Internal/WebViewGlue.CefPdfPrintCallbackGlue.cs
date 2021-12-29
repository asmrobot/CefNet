using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace CefNet.Internal
{
	partial class WebViewGlue
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal bool AvoidOnPdfPrintFinished()
		{
			return false;
		}

		/// <summary>
		/// Executed when the PDF printing has completed.
		/// </summary>
		/// <param name="path">The output path.</param>
		/// <param name="success">
		/// A value which indicates that the printing completed successfully.
		/// </param>
		protected internal virtual void OnPdfPrintFinished(string path, bool success)
		{
			WebView.RaisePdfPrintFinished(new PdfPrintFinishedEventArgs(path, success));
		}
	}
}
