using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace CefNet.Internal
{
	public partial class WebViewGlue
	{
		[MethodImpl(MethodImplOptions.ForwardRef)]
		internal extern bool AvoidOverloadGetCookieAccessFilter();

		internal bool AvoidGetCookieAccessFilter()
		{
			return CookieAccessFilterGlue == null && AvoidOverloadGetCookieAccessFilter();
		}

		/// <summary>
		/// Called on the IO thread before a resource request is loaded.
		/// </summary>
		/// <param name="browser">
		/// Represent the source browser of the request, and may be NULL for requests originating from service workers or CefURLRequest.
		/// </param>
		/// <param name="frame">
		/// Represent the source frame of the request, and may be NULL for requests originating from service workers or CefURLRequest.
		/// </param>
		/// <param name="request">
		/// The CefRequest object cannot be modified in this callback.
		/// </param>
		/// <returns>
		/// To optionally filter cookies for the request return a CefCookieAccessFilter object.
		/// </returns>
		internal protected virtual CefCookieAccessFilter GetCookieAccessFilter(CefBrowser browser, CefFrame frame, CefRequest request)
		{
			return CookieAccessFilterGlue;
		}

		[MethodImpl(MethodImplOptions.ForwardRef)]
		internal extern bool AvoidOverloadOnBeforeResourceLoad();

		internal bool AvoidOnBeforeResourceLoad()
		{
			return false;
		}

		/// <summary>
		/// Called on the IO thread before a resource request is loaded. To redirect or change the resource load optionally modify |request|.
		/// Modification of the request URL will be treated as a redirect.
		/// </summary>
		/// <param name="browser">
		/// Represent the source browser of the request, and may be NULL for requests originating from service workers or CefURLRequest.
		/// </param>
		/// <param name="frame">
		/// Represent the source frame of the request, and may be NULL for requests originating from service workers or CefURLRequest.
		/// </param>
		/// <param name="request">
		/// The CefRequest object.
		/// </param>
		/// <param name="callback">
		/// </param>
		/// <returns>
		/// Return Continue to continue the request immediately. Return ContinueAsync and call CefRequestCallback:: Continue() at a later time to continue
		/// or cancel the request asynchronously. Return Cancel to cancel the request immediately.
		/// </returns>
		internal protected virtual CefReturnValue OnBeforeResourceLoad(CefBrowser browser, CefFrame frame, CefRequest request, CefCallback callback)
		{
			return CefReturnValue.Continue;
		}

		[MethodImpl(MethodImplOptions.ForwardRef)]
		internal extern bool AvoidGetResourceHandler();

		/// <summary>
		/// Called on the IO thread before a resource request is loaded.
		/// </summary>
		/// <param name="browser">
		/// Represent the source browser of the request, and may be NULL for requests originating from service workers or CefURLRequest.
		/// </param>
		/// <param name="frame">
		/// Represent the source frame of the request, and may be NULL for requests originating from service workers or CefURLRequest.
		/// </param>
		/// <param name="request">
		/// The CefRequest object cannot not be modified in this callback.
		/// </param>
		/// <returns>
		/// To allow the resource to load using the default network loader return NULL. To specify a handler for the resource return a CefResourceHandler object.
		/// </returns>
		internal protected virtual CefResourceHandler GetResourceHandler(CefBrowser browser, CefFrame frame, CefRequest request)
		{
			return null;
		}

		[MethodImpl(MethodImplOptions.ForwardRef)]
		internal extern bool AvoidOnResourceRedirect();

		/// <summary>
		/// Called on the IO thread when a resource load is redirected.
		/// </summary>
		/// <param name="browser">
		/// Represent the source browser of the request, and may be NULL for requests originating from service workers or CefURLRequest.
		/// </param>
		/// <param name="frame">
		/// Represent the source frame of the request, and may be NULL for requests originating from service workers or CefURLRequest.
		/// </param>
		/// <param name="request">
		/// The |request| parameter will contain the old URL and other request-related information. Cannot be modified in this callback.
		/// </param>
		/// <param name="response">
		/// The |response| parameter will contain the response that resulted in the redirect. Cannot be modified in this callback.
		/// </param>
		/// <param name="newUrl">
		/// The |newUrl| parameter will contain the new URL and can be changed if desired.
		/// </param>
		internal protected virtual void OnResourceRedirect(CefBrowser browser, CefFrame frame, CefRequest request, CefResponse response, ref string newUrl)
		{

		}

		[MethodImpl(MethodImplOptions.ForwardRef)]
		internal extern bool AvoidOnResourceResponse();

		/// <summary>
		/// Called on the IO thread when a resource response is received. Modification of the request URL will be treated as a redirect.
		/// Requests handled using the default network loader cannot be redirected in this callback. WARNING: Redirecting using this
		/// method is deprecated. Use OnBeforeResourceLoad or GetResourceHandler to perform redirects.
		/// </summary>
		/// <param name="browser">
		/// Represent the source browser of the request, and may be NULL for requests originating from service workers or CefURLRequest.
		/// </param>
		/// <param name="frame">
		/// Represent the source frame of the request, and may be NULL for requests originating from service workers or CefURLRequest.
		/// </param>
		/// <param name="request">
		/// The |request| object.
		/// </param>
		/// <param name="response">
		/// The |response| object cannot be modified in this callback.
		/// </param>
		/// <returns>
		/// To allow the resource load to proceed without modification return false. To redirect or retry the resource load
		/// optionally modify |request| and return true.
		/// </returns>
		internal protected virtual bool OnResourceResponse(CefBrowser browser, CefFrame frame, CefRequest request, CefResponse response)
		{
			return false;
		}

		[MethodImpl(MethodImplOptions.ForwardRef)]
		internal extern bool AvoidGetResourceResponseFilter();

		/// <summary>
		/// Called on the IO thread to optionally filter resource response content.
		/// </summary>
		/// <param name="browser">
		/// Represent the source browser of the request, and may be NULL for requests originating from service workers or CefURLRequest.
		/// </param>
		/// <param name="frame">
		/// Represent the source frame of the request, and may be NULL for requests originating from service workers or CefURLRequest.
		/// </param>
		/// <param name="request">
		/// Represent the request respectively and cannot be modified in this callback.
		/// </param>
		/// <param name="response">
		/// Represent the response respectively and cannot be modified in this callback.
		/// </param>
		/// <returns></returns>
		internal protected virtual CefResponseFilter GetResourceResponseFilter(CefBrowser browser, CefFrame frame, CefRequest request, CefResponse response)
		{
			return null;
		}

		[MethodImpl(MethodImplOptions.ForwardRef)]
		internal extern bool AvoidOnResourceLoadComplete();

		/// <summary>
		/// Called on the IO thread when a resource load has completed. This method will be called for all requests, including requests
		/// that are aborted due to CEF shutdown or destruction of the associated browser. In cases where the associated browser
		/// is destroyed this callback may arrive after the CefLifeSpanHandler::OnBeforeClose callback for that browser.
		/// The CefFrame::IsValid method can be used to test for this situation, and care should be taken not to call
		/// |browser| or |frame| methods that modify state (like LoadURL, SendProcessMessage, etc.) if the frame is invalid.
		/// </summary>
		/// <param name="browser">
		/// Represent the source browser of the request, and may be NULL for requests originating from service workers or CefURLRequest.
		/// </param>
		/// <param name="frame">
		/// Represent the source frame of the request, and may be NULL for requests originating from service workers or CefURLRequest.
		/// </param>
		/// <param name="request">
		/// Represent the request respectively and cannot be modified in this callback.
		/// </param>
		/// <param name="response">
		/// Represent the response respectively and cannot be modified in this callback.
		/// </param>
		/// <param name="status">
		/// Indicates the load completion status.
		/// </param>
		/// <param name="receivedContentLength">
		/// The |receivedContentLength| is the number of response bytes actually read.
		/// </param>
		internal protected virtual void OnResourceLoadComplete(CefBrowser browser, CefFrame frame, CefRequest request, CefResponse response, CefUrlRequestStatus status, long receivedContentLength)
		{

		}

		[MethodImpl(MethodImplOptions.ForwardRef)]
		internal extern bool AvoidOnProtocolExecution();

		/// <summary>
		/// Called on the IO thread to handle requests for URLs with an unknown protocol component.
		/// SECURITY WARNING: YOU SHOULD USE THIS METHOD TO ENFORCE RESTRICTIONS BASED ON SCHEME, HOST OR OTHER URL ANALYSIS BEFORE ALLOWING OS EXECUTION.
		/// </summary>
		/// <param name="browser">
		/// Represent the source browser of the request, and may be NULL for requests originating from service workers or CefURLRequest.
		/// </param>
		/// <param name="frame">
		/// Represent the source frame of the request, and may be NULL for requests originating from service workers or CefURLRequest.
		/// </param>
		/// <param name="request">
		/// The CefRequest object cannot be modified in this callback.
		/// </param>
		/// <param name="allowOsExecution">
		/// Set |allowOsExecution| to 1 to attempt execution via the registered OS protocol handler, if any.
		/// </param>
		internal protected virtual void OnProtocolExecution(CefBrowser browser, CefFrame frame, CefRequest request, ref int allowOsExecution)
		{

		}
	}
}
