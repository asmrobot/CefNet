.class public auto ansi beforefieldinit CefNet.Internal.WebViewGlue
{
	.method assembly hidebysig instance bool AvoidOnBeforeDownload()
	{
		.maxstack 2
		ldarg.0
		ldvirtftn instance void CefNet.Internal.WebViewGlue::OnBeforeDownload(class CefNet.CefBrowser, class CefNet.CefDownloadItem, string, class CefNet.CefBeforeDownloadCallback)
		ldftn instance void CefNet.Internal.WebViewGlue::OnBeforeDownload(class CefNet.CefBrowser, class CefNet.CefDownloadItem, string, class CefNet.CefBeforeDownloadCallback)
		ceq
		ret
	}

	.method assembly hidebysig instance bool AvoidOnDownloadUpdated()
	{
		.maxstack 2
		ldarg.0
		ldvirtftn instance void CefNet.Internal.WebViewGlue::OnDownloadUpdated(class CefNet.CefBrowser, class CefNet.CefDownloadItem, class CefNet.CefDownloadItemCallback)
		ldftn instance void CefNet.Internal.WebViewGlue::OnDownloadUpdated(class CefNet.CefBrowser, class CefNet.CefDownloadItem, class CefNet.CefDownloadItemCallback)
		ceq
		ret
	}

}