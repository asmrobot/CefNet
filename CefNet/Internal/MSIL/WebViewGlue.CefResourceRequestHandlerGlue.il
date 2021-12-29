.class public auto ansi beforefieldinit CefNet.Internal.WebViewGlue
{
	.method assembly hidebysig instance bool AvoidOverloadGetCookieAccessFilter()
	{
		.maxstack 2
		ldarg.0
		ldvirtftn instance class CefNet.CefCookieAccessFilter CefNet.Internal.WebViewGlue::GetCookieAccessFilter(class CefNet.CefBrowser, class CefNet.CefFrame, class CefNet.CefRequest)
		ldftn instance class CefNet.CefCookieAccessFilter CefNet.Internal.WebViewGlue::GetCookieAccessFilter(class CefNet.CefBrowser, class CefNet.CefFrame, class CefNet.CefRequest)
		ceq
		ret
	}
	
	.method assembly hidebysig instance bool AvoidOverloadOnBeforeResourceLoad()
	{
		.maxstack 2
		ldarg.0
		ldvirtftn instance valuetype CefNet.CefReturnValue CefNet.Internal.WebViewGlue::OnBeforeResourceLoad(class CefNet.CefBrowser, class CefNet.CefFrame, class CefNet.CefRequest, class CefNet.CefCallback)
		ldftn instance valuetype CefNet.CefReturnValue CefNet.Internal.WebViewGlue::OnBeforeResourceLoad(class CefNet.CefBrowser, class CefNet.CefFrame, class CefNet.CefRequest, class CefNet.CefCallback)
		ceq
		ret
	}

	.method assembly hidebysig instance bool AvoidGetResourceHandler()
	{
		.maxstack 2
		ldarg.0
		ldvirtftn instance class CefNet.CefResourceHandler CefNet.Internal.WebViewGlue::GetResourceHandler(class CefNet.CefBrowser, class CefNet.CefFrame, class CefNet.CefRequest)
		ldftn instance class CefNet.CefResourceHandler CefNet.Internal.WebViewGlue::GetResourceHandler(class CefNet.CefBrowser, class CefNet.CefFrame, class CefNet.CefRequest)
		ceq
		ret
	}

	.method assembly hidebysig instance bool AvoidOnResourceRedirect()
	{
		.maxstack 2
		ldarg.0
		ldvirtftn instance void CefNet.Internal.WebViewGlue::OnResourceRedirect(class CefNet.CefBrowser, class CefNet.CefFrame, class CefNet.CefRequest, class CefNet.CefResponse, string& newUrl)
		ldftn instance void CefNet.Internal.WebViewGlue::OnResourceRedirect(class CefNet.CefBrowser, class CefNet.CefFrame, class CefNet.CefRequest, class CefNet.CefResponse, string& newUrl)
		ceq
		ret
	}

	.method assembly hidebysig instance bool AvoidOnResourceResponse()
	{
		.maxstack 2
		ldarg.0
		ldvirtftn instance bool CefNet.Internal.WebViewGlue::OnResourceResponse(class CefNet.CefBrowser, class CefNet.CefFrame, class CefNet.CefRequest, class CefNet.CefResponse)
		ldftn instance bool CefNet.Internal.WebViewGlue::OnResourceResponse(class CefNet.CefBrowser, class CefNet.CefFrame, class CefNet.CefRequest, class CefNet.CefResponse)
		ceq
		ret
	}

	.method assembly hidebysig instance bool AvoidGetResourceResponseFilter()
	{
		.maxstack 2
		ldarg.0
		ldvirtftn instance class CefNet.CefResponseFilter CefNet.Internal.WebViewGlue::GetResourceResponseFilter(class CefNet.CefBrowser, class CefNet.CefFrame, class CefNet.CefRequest, class CefNet.CefResponse)
		ldftn instance class CefNet.CefResponseFilter CefNet.Internal.WebViewGlue::GetResourceResponseFilter(class CefNet.CefBrowser, class CefNet.CefFrame, class CefNet.CefRequest, class CefNet.CefResponse)
		ceq
		ret
	}

	.method assembly hidebysig instance bool AvoidOnResourceLoadComplete()
	{
		.maxstack 2
		ldarg.0
		ldvirtftn instance void CefNet.Internal.WebViewGlue::OnResourceLoadComplete(class CefNet.CefBrowser, class CefNet.CefFrame, class CefNet.CefRequest, class CefNet.CefResponse,
																					valuetype CefNet.CefUrlRequestStatus, int64 receivedContentLength)
		ldftn instance void CefNet.Internal.WebViewGlue::OnResourceLoadComplete(class CefNet.CefBrowser, class CefNet.CefFrame, class CefNet.CefRequest, class CefNet.CefResponse,
																					valuetype CefNet.CefUrlRequestStatus, int64 receivedContentLength)
		ceq
		ret
	}

	.method assembly hidebysig instance bool AvoidOnProtocolExecution()
	{
		.maxstack 2
		ldarg.0
		ldvirtftn instance void CefNet.Internal.WebViewGlue::OnProtocolExecution(class CefNet.CefBrowser, class CefNet.CefFrame, class CefNet.CefRequest, int32&)
		ldftn instance void CefNet.Internal.WebViewGlue::OnProtocolExecution(class CefNet.CefBrowser, class CefNet.CefFrame, class CefNet.CefRequest, int32&)
		ceq
		ret
	}

}