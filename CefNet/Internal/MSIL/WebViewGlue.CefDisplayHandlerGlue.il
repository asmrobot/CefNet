.class public auto ansi beforefieldinit CefNet.Internal.WebViewGlue
{
	.method assembly hidebysig instance bool AvoidOnFaviconUrlChange()
	{
		.maxstack 2
		ldarg.0
		ldvirtftn instance void CefNet.Internal.WebViewGlue::OnFaviconUrlChange(class CefNet.CefBrowser, class CefNet.CefStringList)
		ldftn instance void CefNet.Internal.WebViewGlue::OnFaviconUrlChange(class CefNet.CefBrowser, class CefNet.CefStringList)
		ceq
		ret
	}

	.method assembly hidebysig instance bool AvoidOnFullscreenModeChange()
	{
		.maxstack 2
		ldarg.0
		ldvirtftn instance void CefNet.Internal.WebViewGlue::OnFullscreenModeChange(class CefNet.CefBrowser, bool)
		ldftn instance void CefNet.Internal.WebViewGlue::OnFullscreenModeChange(class CefNet.CefBrowser, bool)
		ceq
		ret
	}

	.method assembly hidebysig instance bool AvoidOnTooltip()
	{
		.maxstack 2
		ldarg.0
		ldvirtftn instance bool CefNet.Internal.WebViewGlue::OnTooltip(class CefNet.CefBrowser, string&)
		ldftn instance bool CefNet.Internal.WebViewGlue::OnTooltip(class CefNet.CefBrowser, string&)
		ceq
		ret
	}

	.method assembly hidebysig instance bool AvoidOnStatusMessage()
	{
		.maxstack 2
		ldarg.0
		ldvirtftn instance void CefNet.Internal.WebViewGlue::OnStatusMessage(class CefNet.CefBrowser, string)
		ldftn instance void CefNet.Internal.WebViewGlue::OnStatusMessage(class CefNet.CefBrowser, string)
		ceq
		ret
	}

	.method assembly hidebysig instance bool AvoidOnConsoleMessage()
	{
		.maxstack 2
		ldarg.0
		ldvirtftn instance bool CefNet.Internal.WebViewGlue::OnConsoleMessage(class CefNet.CefBrowser, valuetype CefNet.CefLogSeverity, string, string, int32) 
		ldftn instance bool CefNet.Internal.WebViewGlue::OnConsoleMessage(class CefNet.CefBrowser, valuetype CefNet.CefLogSeverity, string, string, int32)
		ceq
		ret
	}

	.method assembly hidebysig instance bool AvoidOnAutoResize()
	{
		.maxstack 2
		ldarg.0
		ldvirtftn instance bool CefNet.Internal.WebViewGlue::OnAutoResize(class CefNet.CefBrowser, valuetype CefNet.CefSize) 
		ldftn instance bool CefNet.Internal.WebViewGlue::OnAutoResize(class CefNet.CefBrowser, valuetype CefNet.CefSize)
		ceq
		ret
	}

	.method assembly hidebysig instance bool AvoidOnLoadingProgressChange()
	{
		.maxstack 2
		ldarg.0
		ldvirtftn instance void CefNet.Internal.WebViewGlue::OnLoadingProgressChange(class CefNet.CefBrowser, float64)
		ldftn instance void CefNet.Internal.WebViewGlue::OnLoadingProgressChange(class CefNet.CefBrowser, float64)
		ceq
		ret
	}

}