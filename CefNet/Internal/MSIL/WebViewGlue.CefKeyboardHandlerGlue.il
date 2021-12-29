.class public auto ansi beforefieldinit CefNet.Internal.WebViewGlue
{
	.method assembly hidebysig instance bool AvoidOnPreKeyEvent()
	{
		.maxstack 2
		ldarg.0
		ldvirtftn instance bool CefNet.Internal.WebViewGlue::OnPreKeyEvent(class CefNet.CefBrowser, valuetype CefNet.CefKeyEvent, valuetype CefNet.CefEventHandle, int32&)
		ldftn instance bool CefNet.Internal.WebViewGlue::OnPreKeyEvent(class CefNet.CefBrowser, valuetype CefNet.CefKeyEvent, valuetype CefNet.CefEventHandle, int32&)
		ceq
		ret
	}

	.method assembly hidebysig instance bool AvoidOnKeyEvent()
	{
		.maxstack 2
		ldarg.0
		ldvirtftn instance bool CefNet.Internal.WebViewGlue::OnKeyEvent(class CefNet.CefBrowser, valuetype CefNet.CefKeyEvent, valuetype CefNet.CefEventHandle)
		ldftn instance bool CefNet.Internal.WebViewGlue::OnKeyEvent(class CefNet.CefBrowser, valuetype CefNet.CefKeyEvent, valuetype CefNet.CefEventHandle)
		ceq
		ret
	}

}