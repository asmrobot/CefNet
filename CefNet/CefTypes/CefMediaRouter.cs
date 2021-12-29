using System;
using System.Collections.Generic;
using System.Text;
using CefNet.CApi;

namespace CefNet
{
	public unsafe partial class CefMediaRouter
	{
		/// <summary>
		/// Gets the <see cref="CefMediaRouter"/> object associated with the global request context.
		/// </summary>
		/// <param name="callback">
		/// If <paramref name="callback"/> is non-null it will be executed asnychronously on the CEF UI thread
		/// after the manager&apos;s storage has been initialized.
		/// </param>
		/// <returns>
		/// The <see cref="CefMediaRouter"/> object associated with the global request context.
		/// </returns>
		public static unsafe CefMediaRouter GetGlobal(CefCompletionCallback callback)
		{
			return CefMediaRouter.Wrap(CefMediaRouter.Create, CefNativeApi.cef_media_router_get_global(callback is null ? null : callback.GetNativeInstance()));
		}

	}
}
