using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using CefNet.Internal;

namespace CefNet
{
	/// <summary>
	/// This is used used for asynchronous complete of JavaScript dialog requests.
	/// </summary>
	public sealed class ScriptDialogDeferral : IDisposable
	{
		private readonly WeakReference<WebViewGlue> _viewGlueRef;
		private CefJSDialogCallback _callback;

		internal ScriptDialogDeferral(WebViewGlue viewGlue, CefJSDialogCallback callback)
		{
			if (callback is null)
				throw new ArgumentNullException(nameof(callback));

			_viewGlueRef = new WeakReference<WebViewGlue>(viewGlue);
			_callback = callback;
		}

		/// <summary>
		/// Responds with OK.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Accept()
		{
			Continue(true, null);
		}

		/// <summary>
		/// Responds with OK.
		/// </summary>
		/// <param name="input">The value specified for prompt dialogs.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Accept(string input)
		{
			Continue(true, input);
		}

		private void Continue(bool success, string input)
		{
			if (_viewGlueRef.TryGetTarget(out WebViewGlue viewGlue)
				&& viewGlue.ReleaseScriptDialogDeferral(this))
			{
				Interlocked.Exchange(ref _callback, null)?.Continue(true, null);
			}
			else
			{
				Interlocked.Exchange(ref _callback, null)?.Dispose();
			}
		}

		/// <summary>
		/// Responds with Cancel.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Cancel()
		{
			Continue(false, null);
		}

		void IDisposable.Dispose()
		{
			if (_viewGlueRef.TryGetTarget(out WebViewGlue viewGlue))
				viewGlue.ReleaseScriptDialogDeferral(this);
			Interlocked.Exchange(ref _callback, null)?.Dispose();
		}

	}
}
