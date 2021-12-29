using CefNet.CApi;
using System;
using System.Collections.Generic;

namespace CefNet
{
	public unsafe partial class CefExtension
	{
#if USESAFECACHE

		private static readonly HashSet<WeakReference<CefExtension>> WeakRefs = new HashSet<WeakReference<CefExtension>>();

		public unsafe static CefExtension Wrap(Func<IntPtr, CefExtension> create, cef_extension_t* instance)
		{
			if (instance == null)
				return null;

			IntPtr key = new IntPtr(instance);
			lock (WeakRefs)
			{
				CefExtension wrapper;
				foreach (WeakReference<CefExtension> weakRef in WeakRefs)
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
				wrapper = CefBaseRefCounted<cef_extension_t>.Wrap(create, instance);
				WeakRefs.Add(wrapper.WeakRef);
				return wrapper;
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

		private WeakReference<CefExtension> _weakRef;

		public WeakReference<CefExtension> WeakRef
		{
			get
			{
				if (_weakRef == null)
				{
					lock (WeakRefs)
					{
						if (_weakRef == null)
							_weakRef = new WeakReference<CefExtension>(this);
					}
				}
				return _weakRef;
			}
		}

#endif // USESAFECACHE

	}
}
