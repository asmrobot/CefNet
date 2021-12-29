.class public auto ansi beforefieldinit CefNet.Internal.WebViewGlue
{

	.method assembly hidebysig instance bool AvoidOnDialogClosed()
	{
		.maxstack 2
		ldarg.0
		ldvirtftn instance void CefNet.Internal.WebViewGlue::OnDialogClosed(class CefNet.CefBrowser)
		ldftn instance void CefNet.Internal.WebViewGlue::OnDialogClosed(class CefNet.CefBrowser)
		ceq
		ret
	}

}