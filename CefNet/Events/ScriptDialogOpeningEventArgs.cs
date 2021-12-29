using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CefNet
{
	/// <summary>
	/// Event args for the <see cref="IChromiumWebView.ScriptDialogOpening"/> event.
	/// </summary>
	public class ScriptDialogOpeningEventArgs : HandledEventArgs, IScriptDialogOpeningEventArgs
	{
		private readonly ScriptDialogDeferral _callback;

		/// <summary>
		/// Initializes a new instance of the <see cref="ScriptDialogOpeningEventArgs"/> class. 
		/// </summary>
		/// <param name="originUrl">The URI of the page that requested the dialog box.</param>
		/// <param name="kind">The kind of JavaScript dialog box.</param>
		/// <param name="message">The message of the dialog box.</param>
		/// <param name="defaultText">The default value to use for the result of the prompt JavaScript function.</param>
		/// <param name="callback"></param>
		public ScriptDialogOpeningEventArgs(string originUrl, ScriptDialogKind kind, string message, string defaultText, ScriptDialogDeferral callback)
		{
			this.Uri = Uri.TryCreate(originUrl, UriKind.Absolute, out Uri uri) ? uri : null;
			this.Kind = kind;
			this.Message = message;
			this.DefaultText = defaultText;
			_callback = callback;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ScriptDialogOpeningEventArgs"/> class. 
		/// </summary>
		/// <param name="kind">The kind of JavaScript dialog box.</param>
		/// <param name="message">The message of the dialog box.</param>
		/// <param name="reload">Indicates that the event is fired before page reloading.</param>
		/// <param name="callback"></param>
		public ScriptDialogOpeningEventArgs(string message, bool reload, ScriptDialogDeferral callback)
		{
			this.Uri = null;
			this.Kind = ScriptDialogKind.BeforeUnload;
			this.Message = message;
			this.IsReload = reload;
			_callback = callback;
		}

		/// <inheritdoc/>
		public Uri Uri { get; }

		/// <inheritdoc/>
		public ScriptDialogKind Kind { get; }

		/// <inheritdoc/>
		public string Message { get; }

		/// <inheritdoc/>
		public string DefaultText { get; }

		/// <inheritdoc/>
		public bool Suppress { get; set; }

		/// <inheritdoc />
		public bool IsReload { get; set; }

		/// <inheritdoc/>
		public ScriptDialogDeferral GetDeferral()
		{
			this.Handled = true;
			return _callback;
		}
	}
}
