using System;
using System.Collections.Generic;
using System.Threading;
using CefNet.Internal;

namespace CefNet
{
	public partial class WindowlessWebView : IDisposable
	{
		private object SyncRoot = new Dictionary<InitialPropertyKeys, object>();

		private bool _layoutOff;
		private CefRect _bounds;
		private float _devicePixelRatio = 1;
		private Thread _uiThread;

		private EventHandler<ITextFoundEventArgs> TextFoundEvent;
		private EventHandler<IPdfPrintFinishedEventArgs> PdfPrintFinishedEvent;
		private EventHandler<EventArgs> StatusTextChangedEvent;
		private EventHandler<IScriptDialogOpeningEventArgs> ScriptDialogOpeningEvent;

		public event EventHandler PopupShow;

		public WindowlessWebView()
			: this(null, null, null, null)
		{

		}

		public WindowlessWebView(WindowlessWebView opener)
		{
			if (opener != null)
			{
				this.Opener = opener;
				this.BrowserSettings = opener.BrowserSettings;
			}

			using (var windowInfo = new CefWindowInfo())
			{
				InitializeInternal(windowInfo);
			}
			SetState(State.Creating, true);
		}

		public WindowlessWebView(string url, CefBrowserSettings settings, CefDictionaryValue extraInfo, CefRequestContext requestContext)
		{
			CefWindowInfo windowInfo = null;
			try
			{
				windowInfo = new CefWindowInfo();
				InitializeInternal(windowInfo);
				this.BrowserSettings = settings ?? DefaultBrowserSettings;
				this.ExtraInfo = extraInfo;
				this.RequestContext = requestContext;
				SetState(State.Creating, true);
				if (!CefApi.CreateBrowser(windowInfo, ViewGlue.Client, url ?? "about:blank", this.BrowserSettings, extraInfo, requestContext))
					throw new InvalidOperationException();
			}
			finally
			{
				windowInfo?.Dispose();
			}
		}

		~WindowlessWebView()
		{
			Dispose(false);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (this.ViewGlue != null)
			{
				BrowserObject?.Host.CloseBrowser(true);
				this.ViewGlue = null;
			}
		}

		public void Dispose()
		{
			Dispose(true);
		}

		private void SetInitProperty(InitialPropertyKeys key, object value)
		{
			var propertyBag = SyncRoot as Dictionary<InitialPropertyKeys, object>;
			if (_state != State.NotInitialized || propertyBag is null)
				throw new InvalidOperationException("This property must be set before the underlying CEF browser is created.");
			propertyBag[key] = value;
		}

		private T GetInitProperty<T>(InitialPropertyKeys key)
		{
			return default;
		}

		private void InitializeInternal(CefWindowInfo windowInfo)
		{
			_uiThread = Thread.CurrentThread;
			this.ViewGlue = CreateWebViewGlue();
			Initialize(windowInfo);
		}

		protected virtual void Initialize(CefWindowInfo windowInfo)
		{
			windowInfo.SetAsWindowless(IntPtr.Zero);
			Width = Math.Max(200, windowInfo.Width);
			Height = Math.Max(100, windowInfo.Height);
		}

		protected bool InvokeRequired
		{
			get { return Thread.CurrentThread != _uiThread; }
		}

		protected void VerifyAccess()
		{
			if (InvokeRequired)
				throw new InvalidOperationException("Cross-thread operation not valid. The WebView accessed from a thread other than the thread it was created on.");
		}

		private WindowlessWebViewGlue WindowlessViewGlue
		{
			get { return (WindowlessWebViewGlue)ViewGlue; }
		}

		protected virtual WindowlessWebViewGlue CreateWebViewGlue()
		{
			return new WindowlessWebViewGlue(this);
		}

		protected virtual void RaiseCrossThreadEvent<TEventArgs>(Action<TEventArgs> raiseEvent, TEventArgs e, bool synchronous)
		{
			raiseEvent(e);
		}

		private void AddHandler<TEventHadler>(in TEventHadler eventKey, TEventHadler handler)
			where TEventHadler : Delegate
		{
			TEventHadler current;
			TEventHadler key = eventKey;
			do
			{
				current = key;
				key = CefNetApi.CompareExchange<TEventHadler>(in eventKey, (TEventHadler)Delegate.Combine(current, handler), current);
			}
			while (key != current);
		}

		private void RemoveHandler<TEventHadler>(in TEventHadler eventKey, TEventHadler handler)
			where TEventHadler : Delegate
		{
			TEventHadler current;
			TEventHadler key = eventKey;
			do
			{
				current = key;
				key = CefNetApi.CompareExchange<TEventHadler>(in eventKey, (TEventHadler)Delegate.Combine(current, handler), current);
			}
			while (key != current);
		}

		float IChromiumWebViewPrivate.GetDevicePixelRatio()
		{
			return 1;
		}

		CefRect IChromiumWebViewPrivate.GetCefRootBounds()
		{
			return _bounds;
		}

		CefRect IChromiumWebViewPrivate.GetCefViewBounds()
		{
			return _bounds;
		}

		bool IChromiumWebViewPrivate.GetCefScreenInfo(ref CefScreenInfo screenInfo)
		{
			return GetCefScreenInfo(ref screenInfo);
		}

		protected virtual bool GetCefScreenInfo(ref CefScreenInfo screenInfo)
		{
			return false;
		}

		bool IChromiumWebViewPrivate.CefPointToScreen(ref CefPoint point)
		{
			return false;
		}

		void IChromiumWebViewPrivate.RaisePopupBrowserCreating()
		{
			SetState(State.Creating, true);
			SyncRoot = new object();
		}

		protected virtual void OnBrowserCreated(EventArgs e)
		{
			SyncRoot = new object();
			BrowserCreated?.Invoke(this, e);
		}

		/// <summary>
		/// Raises the <see cref="CefPaint"/> event.
		/// </summary>
		/// <param name="e">A <see cref="CefPaintEventArgs"/> that contains the event data.</param>
		/// <remarks>This method can be called on a thread other than the UI thread.</remarks>
		protected virtual void OnCefPaint(CefPaintEventArgs e)
		{
			CefPaint?.Invoke(this, e);
		}

		protected virtual void OnPopupShow(PopupShowEventArgs e)
		{
			PopupShow?.Invoke(this, e);
		}

		bool IChromiumWebViewPrivate.RaiseRunContextMenu(CefFrame frame, CefContextMenuParams menuParams, CefMenuModel model, CefRunContextMenuCallback callback)
		{
			callback.Cancel();
			return true;
		}

		protected virtual void OnLoadingStateChange(LoadingStateChangeEventArgs e)
		{
			LoadingStateChange?.Invoke(this, e);
		}

		protected virtual CefPoint PointToViewport(CefPoint point)
		{
			return point;
		}

		public float DevicePixelRatio
		{
			get
			{
				return _devicePixelRatio;
			}
			set
			{
				if (value <= 0)
					throw new ArgumentOutOfRangeException(nameof(value));
				_devicePixelRatio = value;
			}
		}

		public int X
		{
			get { return _bounds.X; }
			set { _bounds.X = value; }
		}

		public int Y
		{
			get { return _bounds.Y; }
			set { _bounds.Y = value; }
		}

		public int Width
		{
			get
			{
				return _bounds.Width;
			}
			set
			{
				if (value <= 0)
					throw new ArgumentOutOfRangeException(nameof(value));
				_bounds.Width = value;
				PerformLayout(false);
			}
		}
		
		public int Height
		{
			get
			{
				return _bounds.Height;
			}
			set
			{
				if (value <= 0)
					throw new ArgumentOutOfRangeException(nameof(value));
				_bounds.Height = value;
				PerformLayout(false);
			}
		}

		public void SuspendLayout()
		{
			_layoutOff = true;
		}

		public void ResumeLayout()
		{
			_layoutOff = false;
			PerformLayout(true);
		}

		public void ResumeLayout(bool performLayout)
		{
			_layoutOff = false;
			PerformLayout(performLayout);
		}

		private void PerformLayout(bool immediate)
		{
			if (!_layoutOff || immediate)
			{
				ViewGlue.BrowserObject?.Host.WasResized();
			}
		}

		public void Invalidate()
		{
			PerformLayout(true);
		}

		public void SetFocus(bool focus)
		{
			this.BrowserObject.Host.SetFocus(focus);
		}

		private void UpdateOffscreenViewLocation() { }

		/// <summary>
		/// Gets the rectangle that represents the bounds of the WebView control.
		/// </summary>
		/// <returns>
		/// A <see cref="CefRect"/> representing the bounds within which the WebView control is scaled.
		/// </returns>
		public CefRect GetBounds()
		{
			return _bounds;
		}

		/// <summary>
		/// Sets the bounds of the control to the specified location and size.
		/// </summary>
		/// <param name="x">The new <see cref="X"/> property value of the control.</param>
		/// <param name="y">The new <see cref="Y"/> property value of the control.</param>
		/// <param name="width">The new <see cref="Width"/> property value of the control.</param>
		/// <param name="height">The new <see cref="Height"/> property value of the control.</param>
		public void SetBounds(int x, int y, int width, int height)
		{
			if (width <= 0)
				throw new ArgumentOutOfRangeException(nameof(width));
			if (height <= 0)
				throw new ArgumentOutOfRangeException(nameof(height));

			_bounds = new CefRect(x, y, width, height);
			PerformLayout(false);
		}

	}
}
