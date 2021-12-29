using System.Runtime.CompilerServices;

namespace CefNet.Internal
{
	public partial class WebViewGlue
	{
		public void CreateOrDestroyDialogGlue()
		{
			if (AvoidOnFileDialog())
				this.DialogGlue = null;
			else if (this.DialogGlue is null)
				this.DialogGlue = new CefDialogHandlerGlue(this);
		}

		[MethodImpl(MethodImplOptions.ForwardRef)]
		internal extern bool AvoidOnFileDialog();

		/// <summary>
		/// Called to run a file chooser dialog.
		/// </summary>
		/// <param name="browser">The browser object.</param>
		/// <param name="mode">
		/// Represents the type of dialog to display.
		/// </param>
		/// <param name="title">
		/// The title to be used for the dialog and may be empty to show the default title
		/// (&apos;Open&apos; or &apos;Save&apos; depending on the mode).
		/// </param>
		/// <param name="defaultFilePath">
		///  The path with optional directory and/or file name component that should be
		///  initially selected in the dialog.
		/// </param>
		/// <param name="acceptFilters">
		/// Used to restrict the selectable file types and may any combination of
		/// <list type="bullet">
		/// <item>valid lower-cased MIME types (e.g. &apos;text/*&apos; or &apos;image/*&apos;);</item>
		/// <item>individual file extensions (e.g. &apos;.txt&apos; or &apos;.png&apos;);</item>
		/// <item>
		/// combined description and file extension delimited using &apos;|&apos; and
		/// &apos;;&apos; (e.g. &apos;Image Types|.png;.gif;.jpg&apos;).
		/// </item>
		/// </list>
		/// </param>
		/// <param name="selectedAcceptFilter">
		/// The 0-based index of the filter that should be selected by default.
		/// </param>
		/// <param name="callback">A callback object.</param>
		/// <returns>
		/// To display a custom dialog return true and execute <paramref name="callback"/>
		/// either inline or at a later time. To display the default dialog return false.
		/// </returns>
		internal protected virtual bool OnFileDialog(CefBrowser browser, CefFileDialogMode mode, string title, string defaultFilePath, CefStringList acceptFilters, int selectedAcceptFilter, CefFileDialogCallback callback)
		{
			return false;
		}
	}
}
