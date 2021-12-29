.class public auto ansi beforefieldinit CefNet.Internal.WebViewGlue
{
	.method private hidebysig instance bool AvoidOnFrameCreatedImpl()
	{
		.maxstack 2
		ldarg.0
		ldvirtftn instance void CefNet.Internal.WebViewGlue::OnFrameCreated(class CefNet.CefBrowser, class CefNet.CefFrame)
		ldftn instance void CefNet.Internal.WebViewGlue::OnFrameCreated(class CefNet.CefBrowser, class CefNet.CefFrame)
		ceq
		ret
	}

	.method private hidebysig instance bool AvoidOnFrameAttachedImpl()
	{
		.maxstack 2
		ldarg.0
		ldvirtftn instance void CefNet.Internal.WebViewGlue::OnFrameAttached(class CefNet.CefBrowser, class CefNet.CefFrame, bool)
		ldftn instance void CefNet.Internal.WebViewGlue::OnFrameAttached(class CefNet.CefBrowser, class CefNet.CefFrame, bool)
		ceq
		ret
	}

	.method private hidebysig instance bool AvoidOnFrameDetachedImpl()
	{
		.maxstack 2
		ldarg.0
		ldvirtftn instance void CefNet.Internal.WebViewGlue::OnFrameDetached(class CefNet.CefBrowser, class CefNet.CefFrame)
		ldftn instance void CefNet.Internal.WebViewGlue::OnFrameDetached(class CefNet.CefBrowser, class CefNet.CefFrame)
		ceq
		ret
	}

	.method assembly hidebysig instance bool AvoidOnMainFrameChanged()
	{
		.maxstack 2
		ldarg.0
		ldvirtftn instance void CefNet.Internal.WebViewGlue::OnMainFrameChanged(class CefNet.CefBrowser, class CefNet.CefFrame, class CefNet.CefFrame)
		ldftn instance void CefNet.Internal.WebViewGlue::OnMainFrameChanged(class CefNet.CefBrowser, class CefNet.CefFrame, class CefNet.CefFrame)
		ceq
		ret
	}

}