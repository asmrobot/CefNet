using CefNet.CApi;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace CefNet
{
	partial class CefBrowser
	{
#if USESAFECACHE

		private static readonly HashSet<WeakReference<CefBrowser>> WeakRefs = new HashSet<WeakReference<CefBrowser>>();

		private WeakReference<CefBrowser> _weakRef;

		public WeakReference<CefBrowser> WeakRef
		{
			get
			{
				if (_weakRef == null)
				{
					lock (WeakRefs)
					{
						if (_weakRef == null)
							_weakRef = new WeakReference<CefBrowser>(this);
					}
				}
				return _weakRef;
			}
		}

		protected override void Dispose(bool disposing)
		{
			lock (WeakRefs)
			{
				WeakRefs.Remove(WeakRef);
			}
			base.Dispose(disposing);
		}

		public unsafe static CefBrowser Wrap(Func<IntPtr, CefBrowser> create, cef_browser_t* instance)
		{
			if (instance == null)
				return null;

			IntPtr key = new IntPtr(instance);
			lock (WeakRefs)
			{
				CefBrowser wrapper;
				foreach (WeakReference<CefBrowser> weakRef in WeakRefs)
				{
					if (weakRef.TryGetTarget(out wrapper))
					{
						if (wrapper._instance == key
							|| instance->IsSame(wrapper.GetNativeInstance()) != 0)
						{
							instance->@base.Release();
							return wrapper;
						}
					}
				}
				wrapper = CefBaseRefCounted<cef_browser_t>.Wrap(create, instance);
				WeakRefs.Add(wrapper.WeakRef);
				return wrapper;
			}
		}

#endif // USESAFECACHE

		public long[] GetFrameIdentifiers()
		{
			long count = FrameCount << 1;
			long[] identifiers = new long[count];
			GetFrameIdentifiers(ref count, ref identifiers);
			return identifiers;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public CefFrame GetFrame(long identifier)
		{
			return GetFrameByIdent(identifier);
		}

	}
}
