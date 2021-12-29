using System;
using System.Collections.Generic;
using System.Text;

namespace CefNet.Internal
{
	internal sealed partial class CefAppGlue
	{
		private readonly CefNetApplication _application;

		public CefAppGlue(CefNetApplication application)
		{
			_application = application;
			this.RenderProcessGlue = new CefRenderProcessHandlerGlue(this);
			this.BrowserProcessGlue = new CefBrowserProcessHandlerGlue(this);
		}

		internal CefRenderProcessHandler RenderProcessGlue { get; private set; }

		internal CefBrowserProcessHandler BrowserProcessGlue { get; private set; }
	}
}
