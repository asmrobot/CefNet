using CefNet.CApi;
using System;
using System.Collections.Generic;

namespace CefNet
{
	public unsafe partial class CefImage
	{

#if USESAFECACHE

		private static readonly HashSet<WeakReference<CefImage>> WeakRefs = new HashSet<WeakReference<CefImage>>();

		public unsafe static CefImage Wrap(Func<IntPtr, CefImage> create, cef_image_t* instance)
		{
			if (instance == null)
				return null;

			IntPtr key = new IntPtr(instance);
			lock (WeakRefs)
			{
				CefImage wrapper;
				foreach (WeakReference<CefImage> weakRef in WeakRefs)
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
				wrapper = CefBaseRefCounted<cef_image_t>.Wrap(create, instance);
				WeakRefs.Add(wrapper.WeakRef);
				return wrapper;
			}
		}

#endif // USESAFECACHE

		/// <summary>
		/// Create a new CefImage. It will initially be NULL. Use the Add*() functions
		/// to add representations at different scale factors.
		/// </summary>
		public CefImage()
			: this(CefNativeApi.cef_image_create())
		{
#if USESAFECACHE
			lock (WeakRefs)
			{
				WeakRefs.Add(this.WeakRef);
			}
#endif
		}

#if USESAFECACHE

		protected override void Dispose(bool disposing)
		{
			lock (WeakRefs)
			{
				WeakRefs.Remove(WeakRef);
			}
			base.Dispose(disposing);
		}

		private WeakReference<CefImage> _weakRef;

		public WeakReference<CefImage> WeakRef
		{
			get
			{
				if (_weakRef == null)
				{
					lock (WeakRefs)
					{
						if (_weakRef == null)
							_weakRef = new WeakReference<CefImage>(this);
					}
				}
				return _weakRef;
			}
		}

#endif

	}
}
