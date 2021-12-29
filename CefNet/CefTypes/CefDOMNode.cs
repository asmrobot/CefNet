using CefNet.CApi;
using System;
using System.Collections.Generic;

namespace CefNet
{
	public unsafe partial class CefDOMNode
	{
#if USESAFECACHE
		private static readonly HashSet<WeakReference<CefDOMNode>> WeakRefs = new HashSet<WeakReference<CefDOMNode>>();

		private WeakReference<CefDOMNode> _weakRef;

		public unsafe static CefDOMNode Wrap(Func<IntPtr, CefDOMNode> create, cef_domnode_t* instance)
		{
			if (instance == null)
				return null;

			IntPtr key = new IntPtr(instance);
			lock (WeakRefs)
			{
				CefDOMNode wrapper;
				foreach (WeakReference<CefDOMNode> weakRef in WeakRefs)
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
				wrapper = CefBaseRefCounted<cef_domnode_t>.Wrap(create, instance);
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

		public WeakReference<CefDOMNode> WeakRef
		{
			get
			{
				if (_weakRef == null)
				{
					lock (WeakRefs)
					{
						if (_weakRef == null)
							_weakRef = new WeakReference<CefDOMNode>(this);
					}
				}
				return _weakRef;
			}
		}

#endif // USESAFECACHE

	}
}
