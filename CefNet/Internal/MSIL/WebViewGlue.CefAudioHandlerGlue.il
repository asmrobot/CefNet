.class public auto ansi beforefieldinit CefNet.Internal.WebViewGlue
{
	.method assembly hidebysig instance bool AvoidGetAudioParameters()
	{
		.maxstack 2
		ldarg.0
		ldvirtftn instance bool CefNet.Internal.WebViewGlue::GetAudioParameters(class CefNet.CefBrowser, valuetype CefNet.CefAudioParameters&)
		ldftn instance bool CefNet.Internal.WebViewGlue::GetAudioParameters(class CefNet.CefBrowser, valuetype CefNet.CefAudioParameters&)
		ceq
		ret
	}

	.method assembly hidebysig instance bool AvoidOnAudioStreamStarted()
	{
		.maxstack 2
		ldarg.0
		ldvirtftn instance void CefNet.Internal.WebViewGlue::OnAudioStreamStarted(class CefNet.CefBrowser, valuetype CefNet.CefAudioParameters, int32)
		ldftn instance void CefNet.Internal.WebViewGlue::OnAudioStreamStarted(class CefNet.CefBrowser, valuetype CefNet.CefAudioParameters, int32)
		ceq
		ret
	}

	.method assembly hidebysig instance bool AvoidOnAudioStreamPacket()
	{
		.maxstack 2
		ldarg.0
		ldvirtftn instance void CefNet.Internal.WebViewGlue::OnAudioStreamPacket(class CefNet.CefBrowser, native int, int32, int64)
		ldftn instance void CefNet.Internal.WebViewGlue::OnAudioStreamPacket(class CefNet.CefBrowser, native int, int32, int64)
		ceq
		ret
	}
	
	.method assembly hidebysig instance bool AvoidOnAudioStreamStopped()
	{
		.maxstack 2
		ldarg.0
		ldvirtftn instance void CefNet.Internal.WebViewGlue::OnAudioStreamStopped(class CefNet.CefBrowser)
		ldftn instance void CefNet.Internal.WebViewGlue::OnAudioStreamStopped(class CefNet.CefBrowser)
		ceq
		ret
	}
	
	.method assembly hidebysig instance bool AvoidOnAudioStreamError()
	{
		.maxstack 2
		ldarg.0
		ldvirtftn instance void CefNet.Internal.WebViewGlue::OnAudioStreamError(class CefNet.CefBrowser, string)
		ldftn instance void CefNet.Internal.WebViewGlue::OnAudioStreamError(class CefNet.CefBrowser, string)
		ceq
		ret
	}

}