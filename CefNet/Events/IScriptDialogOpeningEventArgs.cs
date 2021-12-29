using System;
using System.Collections.Generic;
using System.Text;

namespace CefNet
{
	/// <summary>
	/// Event args for the <see cref="IChromiumWebView.ScriptDialogOpening"/> event.
	/// </summary>
	public interface IScriptDialogOpeningEventArgs
	{
		/// <summary>
		/// Gets the URI of the page that requested the dialog box.
		/// </summary>
		Uri Uri { get; }

		/// <summary>
		/// Gets the kind of JavaScript dialog box.
		/// </summary>
		ScriptDialogKind Kind { get; }

		/// <summary>
		/// Gets the message of the dialog box.
		/// </summary>
		string Message { get; }

		/// <summary>
		/// Gets the default value to use for the result of the prompt JavaScript function.
		/// </summary>
		string DefaultText { get; }

		/// <summary>
		/// Returns a deferral object used for asynchronous continuation of JavaScript dialog requests.
		/// </summary>
		/// <returns>The deferral object to complete or cancel.</returns>
		ScriptDialogDeferral GetDeferral();

		/// <summary>
		/// Gets or sets a value that indicates that messages should be suppressed.
		/// </summary>
		bool Suppress { get; set; }

		/// <summary>
		/// Gets a value that indicates that the event is fired before page reloading.
		/// </summary>
		bool IsReload { get; set; }

		/// <summary>
		/// Gets or sets a value that indicates whether the event handler has completely handled the event or whether the system should continue its own processing.
		/// </summary>
		bool Handled { get; set; }

	}
}
