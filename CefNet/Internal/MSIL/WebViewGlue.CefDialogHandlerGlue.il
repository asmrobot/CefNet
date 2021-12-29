.class public auto ansi beforefieldinit CefNet.Internal.WebViewGlue
{

	.method assembly hidebysig instance bool AvoidOnFileDialog()
	{
		.maxstack 2
		ldarg.0
		ldvirtftn instance bool CefNet.Internal.WebViewGlue::OnFileDialog(class CefNet.CefBrowser, valuetype CefNet.CefFileDialogMode, string, string, class CefNet.CefStringList, int32, class CefNet.CefFileDialogCallback)
		ldftn instance bool CefNet.Internal.WebViewGlue::OnFileDialog(class CefNet.CefBrowser, valuetype CefNet.CefFileDialogMode, string, string, class CefNet.CefStringList, int32, class CefNet.CefFileDialogCallback)
		ceq
		ret
	}

}