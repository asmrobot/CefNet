using System;
using System.Collections.Generic;
using System.Text;

namespace CefNet.Internal
{
	partial class WebViewGlue
	{
		internal bool AvoidOnPrintStart()
		{
			return false;
		}

		/// <summary>
		/// Called when printing has started for the specified <paramref name="browser"/>.
		/// This function will be called before the other OnPrint*() functions and irrespective
		/// of how printing was initiated (e.g. <see cref="CefBrowserHost.Print" />, JavaScript
		/// window.print() or PDF extension print button).
		/// </summary>
		/// <param name="browser">The browser.</param>
		protected internal virtual void OnPrintStart(CefBrowser browser)
		{

		}

		internal bool AvoidOnPrintSettings()
		{
			return false;
		}

		/// <summary>
		/// Synchronize <paramref name="settings"/> with client state. 
		/// </summary>
		/// <param name="browser">The browser.</param>
		/// <param name="settings">
		/// Do not keep a reference to <paramref name="settings"/> outside of this callback.
		/// </param>
		/// <param name="getDefaults">
		/// If true then populate <paramref name="settings"/> with the default print settings.
		/// </param>
		protected internal virtual void OnPrintSettings(CefBrowser browser, CefPrintSettings settings, bool getDefaults)
		{

		}

		internal bool AvoidOnPrintDialog()
		{
			return false;
		}

		/// <summary>
		/// Show the print dialog.
		/// </summary>
		/// <param name="browser">The browser.</param>
		/// <param name="hasSelection"></param>
		/// <param name="callback">Execute <paramref name="callback"/>once the dialog is dismissed.</param>
		/// <returns>
		/// Return true if the dialog will be displayed or false to cancel the printing immediately.
		/// </returns>
		protected internal virtual bool OnPrintDialog(CefBrowser browser, bool hasSelection, CefPrintDialogCallback callback)
		{
			return false;
		}

		internal bool AvoidOnPrintJob()
		{
			return false;
		}

		/// <summary>
		/// Send the print job to the printer.
		/// </summary>
		/// <param name="browser">The browser.</param>
		/// <param name="documentName">The document name.</param>
		/// <param name="pdfFilePath"></param>
		/// <param name="callback">Execute <paramref name="callback"/> once the job is completed. </param>
		/// <returns>Return true if the job will proceed or false to cancel the job immediately.</returns>
		protected internal virtual bool OnPrintJob(CefBrowser browser, string documentName, string pdfFilePath, CefPrintJobCallback callback)
		{
			return false;
		}

		internal bool AvoidOnPrintReset()
		{
			return false;
		}

		protected internal virtual void OnPrintReset(CefBrowser browser)
		{

		}

		internal bool AvoidGetPdfPaperSize()
		{
			return false;
		}

		/// <summary>
		/// Return the PDF paper size in device units. 
		/// </summary>
		/// <returns>The PDF paper size in device units.</returns>
		/// <remarks>Used in combination with <see cref="CefBrowserHost.PrintToPdf" />.</remarks>
		protected internal virtual CefSize GetPdfPaperSize(CefBrowser browser, int deviceUnitsPerInch)
		{
			return new CefSize();
		}
	}
}
