using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading;

namespace CefNet.Internal
{
	public partial class WebViewGlue
	{
		private IDisposable _scriptDialogDeferral;

		protected internal IChromiumWebViewPrivate WebView { get; private set; }

		public CefBrowser BrowserObject { get; protected set; }

		private EventHandlerList Events { get; }

		public CefClient Client { get; private set; }
		private CefLifeSpanHandlerGlue LifeSpanGlue { get; }
		private CefRenderHandlerGlue RenderGlue { get; }
		private CefDisplayHandlerGlue DisplayGlue { get; }
		private CefRequestHandlerGlue RequestGlue { get; }
		private CefFindHandlerGlue FindGlue { get; }
		private CefPrintHandler PrintGlue { get; }
		private CefContextMenuHandlerGlue ContextMenuGlue { get; }
		private CefLoadHandlerGlue LoadGlue { get; }
		private CefResourceRequestHandler ResourceRequestGlue { get; }
		private CefCookieAccessFilter CookieAccessFilterGlue { get; }
		private CefFrameHandlerGlue FrameGlue { get; }

		// optional
		private CefDownloadHandlerGlue DownloadGlue { get; set; }
		private CefAudioHandlerGlue AudioGlue { get; set; }
		private CefJSDialogHandlerGlue JSDialogGlue { get; set; }
		private CefKeyboardHandlerGlue KeyboardGlue { get; set; }
		private CefDragHandlerGlue DragGlue { get; set; }
		private CefFocusHandlerGlue FocusGlue { get; set; }
		private CefDialogHandlerGlue DialogGlue { get; set; }

		public WebViewGlue(IChromiumWebViewPrivate view)
		{
			this.Events = new EventHandlerList();
			this.WebView = view;
			this.Client = new CefClientGlue(this);
			this.LifeSpanGlue = new CefLifeSpanHandlerGlue(this);
			this.RenderGlue = new CefRenderHandlerGlue(this);
			this.DisplayGlue = new CefDisplayHandlerGlue(this);
			this.RequestGlue = new CefRequestHandlerGlue(this);
			this.DownloadGlue = new CefDownloadHandlerGlue(this);
			this.FindGlue = new CefFindHandlerGlue(this);
			this.FrameGlue = new CefFrameHandlerGlue(this);
			this.ContextMenuGlue = new CefContextMenuHandlerGlue(this);
			this.LoadGlue = new CefLoadHandlerGlue(this);
			this.PrintGlue = new CefPrintHandlerGlue(this);

			this.CookieAccessFilterGlue = new CefCookieAccessFilterGlue(this);
			this.ResourceRequestGlue = new CefResourceRequestHandlerGlue(this);

			CreateOrDestroyAudioGlue();
			CreateOrDestroyJSDialogGlue();
			CreateOrDestroyKeyboardGlue();
			CreateOrDestroyDragGlue();
			CreateOrDestroyFocusGlue();
			CreateOrDestroyDownloadGlue();
			CreateOrDestroyDialogGlue();
		}


		internal void NotifyPopupBrowserCreating()
		{
			WebView.RaisePopupBrowserCreating();
		}

		internal bool ReleaseScriptDialogDeferral(ScriptDialogDeferral deferral)
		{
			return ReferenceEquals(deferral, Interlocked.CompareExchange(ref _scriptDialogDeferral, null, deferral));
		}

		protected ScriptDialogDeferral CreateScriptDialogDeferral(CefJSDialogCallback callback)
		{
			var deferral = new ScriptDialogDeferral(this, callback);
			_scriptDialogDeferral = deferral;
			return deferral;
		}

		public void AddEventHandler(object key, Delegate handler)
		{
			lock (Events)
			{
				Events.AddHandler(key, handler);
			}
		}

		public void RemoveEventHandler(object key, Delegate handler)
		{
			lock (Events)
			{
				Events.RemoveHandler(key, handler);
			}
		}

		public Delegate GetEventHandler(object key)
		{
			lock (Events)
			{
				return Events[key];
			}
		}

	}
}
