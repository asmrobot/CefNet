.class CefNet.Internal.CefAppGlue
{
	.method assembly hidebysig instance bool AvoidOnBrowserCreated()
	{
		.maxstack 2
		ldarg.0
		ldfld class CefNet.CefNetApplication CefNet.Internal.CefAppGlue::_application
		ldvirtftn instance void CefNet.CefNetApplication::OnBrowserCreated(class CefNet.CefBrowser, class CefNet.CefDictionaryValue)
		ldftn instance void CefNet.CefNetApplication::OnBrowserCreated(class CefNet.CefBrowser, class CefNet.CefDictionaryValue)
		ceq
		ret
	}

	.method assembly hidebysig instance bool AvoidOnBrowserDestroyed()
	{
		.maxstack 2
		ldarg.0
		ldfld class CefNet.CefNetApplication CefNet.Internal.CefAppGlue::_application
		ldvirtftn instance void CefNet.CefNetApplication::OnBrowserDestroyed(class CefNet.CefBrowser)
		ldftn instance void CefNet.CefNetApplication::OnBrowserDestroyed(class CefNet.CefBrowser)
		ceq
		ret
	}

	.method assembly hidebysig instance bool AvoidOnFocusedNodeChanged()
	{
		.maxstack 2
		ldarg.0
		ldfld class CefNet.CefNetApplication CefNet.Internal.CefAppGlue::_application
		ldvirtftn instance void CefNet.CefNetApplication::OnFocusedNodeChanged(class CefNet.CefBrowser, class CefNet.CefFrame, class CefNet.CefDOMNode)
		ldftn instance void CefNet.CefNetApplication::OnFocusedNodeChanged(class CefNet.CefBrowser, class CefNet.CefFrame, class CefNet.CefDOMNode)
		ceq
		ret
	}
	
}
