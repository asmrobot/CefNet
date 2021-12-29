using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

namespace CefNet.Internal
{
	public partial class WebViewGlue
	{
		public void CreateOrDestroyJSDialogGlue()
		{
			if (this.JSDialogGlue is null)
				this.JSDialogGlue = new CefJSDialogHandlerGlue(this);
		}

		internal bool AvoidOnJSDialog()
		{
			return false;
		}

		/// <summary>
		/// Called to run a JavaScript dialog. Return true if the application will use a custom dialog or
		/// if the callback has been executed immediately. Custom dialogs may be either modal or modeless.
		/// If a custom dialog is used the application must execute <paramref name="callback"/> once the
		/// custom dialog is dismissed.
		/// </summary>
		/// <param name="browser"></param>
		/// <param name="originUrl">
		/// If <paramref name="originUrl"/> is non-empty it can be passed to the CefFormatUrlForSecurityDisplay
		/// function to retrieve a secure and user-friendly display string.
		/// </param>
		/// <param name="dialogType"></param>
		/// <param name="messageText"></param>
		/// <param name="defaultPromptText">
		/// The <paramref name="defaultPromptText"/> value will be specified for prompt dialogs only.
		/// </param>
		/// <param name="callback"></param>
		/// <param name="suppressMessage">
		/// Set <paramref name="suppressMessage"/> to 1 and return false to suppress the message (suppressing messages
		/// is preferable to immediately executing the callback as this is used to detect presumably malicious behavior
		/// like spamming alert messages in onbeforeunload). Set <paramref name="suppressMessage"/> to 0 and return false
		/// to use the default implementation (the default implementation will show one modal dialog at a time and suppress
		/// any additional dialog request until the displayed dialog is dismissed).
		/// </param>
		/// <returns></returns>
		internal protected virtual bool OnJSDialog(CefBrowser browser, string originUrl, CefJSDialogType dialogType, string messageText, string defaultPromptText, CefJSDialogCallback callback, ref int suppressMessage)
		{
			ScriptDialogDeferral dialogDeferral = CreateScriptDialogDeferral(callback);
			var ea = new ScriptDialogOpeningEventArgs(originUrl, (ScriptDialogKind)dialogType, messageText, defaultPromptText, dialogDeferral);
			WebView.RaiseScriptDialogOpening(ea);
			suppressMessage = ea.Suppress ? 1 : 0;
			if (!ea.Handled) ((IDisposable)dialogDeferral).Dispose();
			return ea.Handled;
		}

		internal bool AvoidOnBeforeUnloadDialog()
		{
			return false;
		}

		/// <summary>
		/// Called to run a dialog asking the user if they want to leave a page.
		/// </summary>
		/// <param name="browser">The browser.</param>
		/// <param name="messageText">The message of the dialog box.</param>
		/// <param name="isReload">Indicates that the method is called before page reloading.</param>
		/// <param name="callback">
		/// If a custom dialog is used the application must execute <paramref name="callback"/> once the custom dialog is dismissed.
		/// </param>
		/// <returns>
		/// Return false to use the default dialog implementation. Return true if the application will
		/// use a custom dialog or if the callback has been executed immediately.
		/// </returns>
		/// <remarks>Custom dialogs may be either modal or modeless.</remarks>
		internal protected virtual bool OnBeforeUnloadDialog(CefBrowser browser, string messageText, bool isReload, CefJSDialogCallback callback)
		{
			ScriptDialogDeferral dialogDeferral = CreateScriptDialogDeferral(callback);
			var ea = new ScriptDialogOpeningEventArgs(messageText, isReload, dialogDeferral);
			WebView.RaiseScriptDialogOpening(ea);
			if (!ea.Handled) ((IDisposable)dialogDeferral).Dispose();
			return ea.Handled;
		}

		internal bool AvoidOnResetDialogState()
		{
			return false;
		}

		/// <summary>
		/// Called to cancel any pending dialogs and reset any saved dialog state. Will be called due to events like
		/// page navigation irregardless of whether any dialogs are currently pending.
		/// </summary>
		/// <param name="browser"></param>
		internal protected virtual void OnResetDialogState(CefBrowser browser)
		{
			Interlocked.Exchange(ref _scriptDialogDeferral, null)?.Dispose();
		}

		[MethodImpl(MethodImplOptions.ForwardRef)]
		internal extern bool AvoidOnDialogClosed();

		/// <summary>
		/// Called when the default implementation dialog is closed.
		/// </summary>
		/// <param name="browser"></param>
		internal protected virtual void OnDialogClosed(CefBrowser browser)
		{

		}
	}
}
