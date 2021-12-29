using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace CefNet.Internal
{
	public partial class WebViewGlue
	{
		[MethodImpl(MethodImplOptions.ForwardRef)]
		internal extern bool AvoidOverloadCanSendCookie();

		internal bool AvoidCanSendCookie()
		{
			return false;
		}

		/// <summary>
		/// Called on the IO thread before a resource request is sent.
		/// </summary>
		/// <param name="browser">
		/// Represent the source browser of the request, and may be NULL for requests originating from service workers or CefURLRequest.
		/// </param>
		/// <param name="frame">
		/// Represent the source frame of the request, and may be NULL for requests originating from service workers or CefURLRequest.
		/// </param>
		/// <param name="request">
		/// The request. Cannot be modified in this callback.
		/// </param>
		/// <param name="cookie">
		/// The cookie.
		/// </param>
		/// <returns>
		/// Return true if the specified cookie can be sent with the request or false otherwise.
		/// </returns>
		internal protected virtual bool CanSendCookie(CefBrowser browser, CefFrame frame, CefRequest request, CefCookie cookie)
		{
			return true;
		}

		[MethodImpl(MethodImplOptions.ForwardRef)]
		internal extern bool AvoidOverloadCanSaveCookie();

		internal bool AvoidCanSaveCookie()
		{
			return false;
		}

		/// <summary>
		/// Called on the IO thread after a resource response is received.
		/// </summary>
		/// <param name="browser">
		/// Represent the source browser of the request, and may be NULL for requests originating from service workers or CefURLRequest.
		/// </param>
		/// <param name="frame">
		/// Represent the source frame of the request, and may be NULL for requests originating from service workers or CefURLRequest.
		/// </param>
		/// <param name="request">
		/// The request. Cannot be modified in this callback.
		/// </param>
		/// <param name="cookie">
		/// The cookie.
		/// </param>
		/// <returns>
		/// Return true if the specified cookie returned with the response can be saved or false otherwise.
		/// </returns>
		internal protected virtual bool CanSaveCookie(CefBrowser browser, CefFrame frame, CefRequest request, CefResponse response, CefCookie cookie)
		{
			return true;
		}
	}
}
