.class public auto ansi beforefieldinit CefNet.Internal.WebViewGlue
{
	.method assembly hidebysig instance bool AvoidOnTakeFocus()
	{
		.maxstack 2
		ldarg.0
		ldvirtftn instance void CefNet.Internal.WebViewGlue::OnTakeFocus(class CefNet.CefBrowser, bool)
		ldftn instance void CefNet.Internal.WebViewGlue::OnTakeFocus(class CefNet.CefBrowser, bool)
		ceq
		ret
	}

	.method assembly hidebysig instance bool AvoidOnSetFocus()
	{
		.maxstack 2
		ldarg.0
		ldvirtftn instance bool CefNet.Internal.WebViewGlue::OnSetFocus(class CefNet.CefBrowser, valuetype CefNet.CefFocusSource)
		ldftn instance bool CefNet.Internal.WebViewGlue::OnSetFocus(class CefNet.CefBrowser, valuetype CefNet.CefFocusSource)
		ceq
		ret
	}

	.method assembly hidebysig instance bool AvoidOnGotFocus()
	{
		.maxstack 2
		ldarg.0
		ldvirtftn instance void CefNet.Internal.WebViewGlue::OnGotFocus(class CefNet.CefBrowser)
		ldftn instance void CefNet.Internal.WebViewGlue::OnGotFocus(class CefNet.CefBrowser)
		ceq
		ret
	}
}