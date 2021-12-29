using CefNet.CApi;
using System;
using System.Collections.Generic;
using System.Text;
namespace CefNet
{
	public unsafe partial class CefV8Context
	{
		private static readonly HashSet<WeakReference<CefV8Context>> WeakRefs = new HashSet<WeakReference<CefV8Context>>();

		private WeakReference<CefV8Context> _weakRef;

		/// <summary>
		/// Returns the current (top) context object in the V8 context stack.
		/// </summary>
		public static CefV8Context GetCurrentContext()
		{
			return CefV8Context.Wrap(CefV8Context.Create, CefNativeApi.cef_v8context_get_current_context());
		}

		/// <summary>
		/// Returns the entered (bottom) context object in the V8 context stack.
		/// </summary>
		public static CefV8Context GetEnteredContext()
		{
			return CefV8Context.Wrap(CefV8Context.Create, CefNativeApi.cef_v8context_get_entered_context());
		}

		public unsafe static CefV8Context Wrap(Func<IntPtr, CefV8Context> create, cef_v8context_t* instance)
		{
			if (instance == null)
				return null;

			IntPtr key = new IntPtr(instance);
			lock (WeakRefs)
			{
				CefV8Context wrapper;
				foreach (WeakReference<CefV8Context> weakRef in WeakRefs)
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
				wrapper = CefBaseRefCounted<cef_v8context_t>.Wrap(create, instance);
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

		public WeakReference<CefV8Context> WeakRef
		{
			get
			{
				if (_weakRef == null)
				{
					lock (WeakRefs)
					{
						if (_weakRef == null)
							_weakRef = new WeakReference<CefV8Context>(this);
					}
				}
				return _weakRef;
			}
		}

		/// <summary>
		/// Returns true if V8 is currently inside a context.
		/// </summary>
		public static bool InContext
		{
			get { return CefNativeApi.cef_v8context_in_context() != 0; }
		}

		/// <summary>
		/// Evaluates JavaScript code represented as a <see cref="String"/> in this V8 context.
		/// </summary>
		/// <param name="code">The JavaScript code.</param>
		/// <param name="scriptUrl">The URL where the script in question can be found, if any.</param>
		/// <param name="line">The base line number to use for error reporting.</param>
		/// <returns>The completion value of evaluating the given code.</returns>
		/// <exception cref="CefNetJSExcepton">
		/// Thrown when an exception is raised in the JavaScript engine.
		/// </exception>
		public CefV8Value Eval(string code, string scriptUrl, int line = 1)
		{
			if (line <= 0)
				throw new ArgumentOutOfRangeException(nameof(line));
			if (code is null)
				throw new ArgumentNullException(nameof(code));

			fixed (char* s0 = code)
			fixed (char* s1 = scriptUrl)
			{
				var cstr0 = new cef_string_t { Str = s0, Length = code.Length };
				var cstr1 = new cef_string_t { Str = s1, Length = scriptUrl != null ? scriptUrl.Length : 0 };


				cef_v8value_t* rv = null;
				cef_v8value_t** pRv = &rv;
				cef_v8exception_t* jsex = null;
				cef_v8exception_t** pJsex = &jsex;
				if (NativeInstance->Eval(&cstr0, &cstr1, line, pRv, pJsex) != 0)
				{
					return CefV8Value.Wrap(CefV8Value.Create, rv);
				}
				GC.KeepAlive(this);

				throw new CefNetJSExcepton(CefV8Exception.Wrap(CefV8Exception.Create, jsex));
			}
			
		}
	}
}
