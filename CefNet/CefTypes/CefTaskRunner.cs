using CefNet.CApi;
using System;
using System.Collections.Generic;
using System.Text;
namespace CefNet
{
	public unsafe partial class CefTaskRunner
	{

#if USESAFECACHE

		private static readonly HashSet<WeakReference<CefTaskRunner>> WeakRefs = new HashSet<WeakReference<CefTaskRunner>>();

#endif

		/// <summary>
		/// Returns the task runner for the current thread. Only CEF threads will have
		/// task runners. An NULL reference will be returned if this function is called
		/// on an invalid thread.
		/// </summary>
		public static CefTaskRunner GetForCurrentThread()
		{
			return CefTaskRunner.Wrap(CefTaskRunner.Create, CefNativeApi.cef_task_runner_get_for_current_thread());
		}

		/// <summary>
		/// Returns the task runner for the specified CEF thread.
		/// </summary>
		public static CefTaskRunner GetForThread(CefThreadId threadId)
		{
			return CefTaskRunner.Wrap(CefTaskRunner.Create, CefNativeApi.cef_task_runner_get_for_thread(threadId));
		}

#if USESAFECACHE

		public unsafe static CefTaskRunner Wrap(Func<IntPtr, CefTaskRunner> create, cef_task_runner_t* instance)
		{
			if (instance == null)
				return null;

			IntPtr key = new IntPtr(instance);
			lock (WeakRefs)
			{
				CefTaskRunner wrapper;
				foreach (WeakReference<CefTaskRunner> weakRef in WeakRefs)
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
				wrapper = CefBaseRefCounted<cef_task_runner_t>.Wrap(create, instance);
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

		private WeakReference<CefTaskRunner> _weakRef;

		public WeakReference<CefTaskRunner> WeakRef
		{
			get
			{
				if (_weakRef == null)
				{
					lock (WeakRefs)
					{
						if (_weakRef == null)
							_weakRef = new WeakReference<CefTaskRunner>(this);
					}
				}
				return _weakRef;
			}
		}

#endif

	}
}
