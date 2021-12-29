using System;
using System.Collections.Generic;
using System.Text;

namespace CefNet
{
	/// <summary>
	/// Specifies the JavaScript dialog kind used in <see cref="IScriptDialogOpeningEventArgs"/>.
	/// </summary>
	public enum ScriptDialogKind
	{
		/// <summary>
		/// Indicates that the dialog uses window.alert JavaScript function.
		/// </summary>
		Alert = CefJSDialogType.Alert,

		/// <summary>
		/// Indicates that the dialog uses window.confirm JavaScript function.
		/// </summary>
		Confirm = CefJSDialogType.Confirm,

		/// <summary>
		/// Indicates that the dialog uses window.prompt JavaScript function.
		/// </summary>
		Prompt = CefJSDialogType.Prompt,

		/// <summary>
		/// Indicates that the dialog uses window.beforeunload JavaScript event.
		/// </summary>
		BeforeUnload,

	}
}
