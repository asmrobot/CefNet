using CefNet.WinApi;
using CefNet.Wpf;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;

namespace CefNet.Internal
{
	sealed class GlobalHooks
	{
		private static bool IsInitialized;

		private static Dictionary<IntPtr, GlobalHooks> _HookedWindows = new Dictionary<IntPtr, GlobalHooks>();
		private static List<WeakReference<WebView>> _Views = new List<WeakReference<WebView>>();

		internal static void Initialize(WebView view)
		{
			lock(_Views)
			{
				_Views.Add(new WeakReference<WebView>(view));
			}

			if (IsInitialized)
				return;

			IsInitialized = true;

			EventManager.RegisterClassHandler(typeof(Window), FrameworkElement.SizeChangedEvent, new RoutedEventHandler(TryAddGlobalHook));

			foreach (Window window in Application.Current.Windows)
			{
				TryAddGlobalHook(window);
			}
		}

		private HwndSource _source;
		private WebView _target;
		private HwndSourceHook _delegate;

		private GlobalHooks(HwndSource source)
		{
			_source = source;
			_delegate = WndProcHook;
			source.AddHook(WndProcHook);
		}

		private unsafe IntPtr WndProcHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
		{
			const int WM_ENTERMENULOOP = 0x0211;
			const int WM_EXITMENULOOP = 0x0212;

			switch (msg)
			{
				case 0x020E: // WM_MOUSEHWHEEL
					handled = WmMouseHWheel((long)lParam, (long)wParam);
					break;
				case 0x0002: // WM_DESTROY
					_source.RemoveHook(_delegate);
					foreach (var tuple in GetViews(hwnd))
					{
						tuple.Item1.Close();
					}
					lock (_HookedWindows)
					{
						_HookedWindows.Remove(_source.Handle);
						_source.Dispose();
					}
					break;
				case 0x0047: // WM_WINDOWPOSCHANGED
					WINDOWPOS* windowPos = (WINDOWPOS*)lParam;
					if ((windowPos->flags & 0x0002) != 0) // SWP_NOMOVE
						break;

					foreach (var tuple in GetViews(hwnd))
					{
						tuple.Item1.OnUpdateRootBounds();
					}
					break;
				case 0x0231: // WM_ENTERSIZEMOVE
					foreach (var tuple in GetViews(hwnd))
					{
						tuple.Item1.OnRootResizeBegin(EventArgs.Empty);
					}
					break;
				case 0x0232: // WM_EXITSIZEMOVE
					foreach (var tuple in GetViews(hwnd))
					{
						tuple.Item1.OnRootResizeEnd(EventArgs.Empty);
					}
					break;
				case 0x0112: // WM_SYSCOMMAND
					const int SC_KEYMENU = 0xF100;
					// Menu loop must not be runned with Alt key
					handled = ((int)(wParam.ToInt64() & 0xFFF0) == SC_KEYMENU && lParam == IntPtr.Zero);
					break;
				case WM_ENTERMENULOOP:
					if (wParam == IntPtr.Zero)
						CefApi.SetOSModalLoop(true);
					break;
				case WM_EXITMENULOOP:
					if (wParam == IntPtr.Zero)
						CefApi.SetOSModalLoop(false);
					break;
			}
			return IntPtr.Zero;
		}

		private IEnumerable<Tuple<WebView, Window>> GetViews(IntPtr hwnd)
		{
			if (hwnd != _source.Handle)
				yield break;

			lock (_Views)
			{
				for (int i = 0; i < _Views.Count; i++)
				{
					WeakReference<WebView> viewRef = _Views[i];
					if (viewRef.TryGetTarget(out WebView view))
					{
						Window window = Window.GetWindow(view);
						if (window != null && window == _source.RootVisual)
						{
							yield return new Tuple<WebView, Window>(view, window);
						}
					}
					else
					{
						_Views.RemoveAt(i--);
					}
				}
			}
		}

		private bool WmMouseHWheel(long lParam, long wParam)
		{
			Point point = new Point(unchecked((int)lParam) & 0xFFFF, (unchecked((int)lParam) >> 16) & 0xFFFF);
			point = _source.RootVisual.PointFromScreen(point);
			_target = null;
			VisualTreeHelper.HitTest(_source.RootVisual, null, HitTestResultCallbackHandler, new PointHitTestParameters(point));

			if (_target == null)
				return false;

			try
			{
				point = _source.RootVisual.TransformToDescendant(_target).Transform(point);
				var ea = new MouseWheelEventArgs(InputManager.Current.PrimaryMouseDevice, Environment.TickCount, -(short)((unchecked((int)wParam) >> 16) & 0xFFFF));
				ea.RoutedEvent = UIElement.MouseWheelEvent;
				_target.OnMouseHWheel(ea);
			}
			finally
			{
				_target = null;
			}
			return true;
		}

		private HitTestResultBehavior HitTestResultCallbackHandler(HitTestResult result)
		{
			if (result?.VisualHit is WebView view)
			{
				_target = view;
				return HitTestResultBehavior.Stop;
			}
			return HitTestResultBehavior.Continue;
		}

		private static void TryAddGlobalHook(object sender, RoutedEventArgs e)
		{
			TryAddGlobalHook(e.OriginalSource as Window);
		}

		private static void TryAddGlobalHook(Window window)
		{
			if (window == null)
				return;

			IntPtr hwnd = new WindowInteropHelper(window).Handle;
			if (hwnd == IntPtr.Zero)
				return;

			if (_HookedWindows.ContainsKey(hwnd))
				return;

			HwndSource source = HwndSource.FromHwnd(hwnd);
			if (source == null)
				return;

			_HookedWindows.Add(hwnd, new GlobalHooks(source));

		}
	}
}
