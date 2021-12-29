.class public auto ansi beforefieldinit CefNet.Internal.WebViewGlue
{
	.method assembly hidebysig instance bool AvoidOverloadCanSendCookie()
	{
		.maxstack 2
		ldarg.0
		ldvirtftn instance bool CefNet.Internal.WebViewGlue::CanSendCookie(class CefNet.CefBrowser, class CefNet.CefFrame, class CefNet.CefRequest, valuetype CefNet.CefCookie)
		ldftn instance bool CefNet.Internal.WebViewGlue::CanSendCookie(class CefNet.CefBrowser, class CefNet.CefFrame, class CefNet.CefRequest, valuetype CefNet.CefCookie)
		ceq
		ret
	}

	.method assembly hidebysig instance bool AvoidOverloadCanSaveCookie()
	{
		.maxstack 2
		ldarg.0
		ldvirtftn instance bool CefNet.Internal.WebViewGlue::CanSaveCookie(class CefNet.CefBrowser, class CefNet.CefFrame, class CefNet.CefRequest, class CefNet.CefResponse, valuetype CefNet.CefCookie)
		ldftn instance bool CefNet.Internal.WebViewGlue::CanSaveCookie(class CefNet.CefBrowser, class CefNet.CefFrame, class CefNet.CefRequest, class CefNet.CefResponse, valuetype CefNet.CefCookie)
		ceq
		ret
	}
}