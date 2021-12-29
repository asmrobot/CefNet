using System;
using CefNet.CApi;

namespace CefNet
{
	public unsafe partial class CefThread
	{
		/// <summary>
		/// Create and start a new thread. This function does not block waiting for the
		/// thread to run initialization.
		/// </summary>
		/// <param name="name">
		/// The name that will be used to identify the thread.
		/// </param>
		/// <param name="priority">
		///  The thread execution priority.
		/// </param>
		/// <param name="stoppable">
		/// If |stoppable| is True the thread will stopped and joined on
		/// destruction or when stop() is called; otherwise, the thread cannot be
		/// stopped and will be leaked on shutdown.
		/// </param>
		/// <param name="messageLoopType">
		/// Indicates the set of asynchronous events that the thread can process.
		/// </param>
		/// <param name="comInitMode">
		/// On Windows the |comInitMode| value specifies how COM will be initialized
		/// for the thread. If |comInitMode| is set to COM_INIT_MODE_STA then
		/// |messageLoopType| must be set to ML_TYPE_UI.
		/// </param>
		public CefThread(string name, CefThreadPriority priority, CefMessageLoopType messageLoopType, bool stoppable, CefComInitMode comInitMode)
			: this(CreateInternal(name, priority, messageLoopType, stoppable, comInitMode))
		{

		}

		private static cef_thread_t* CreateInternal(string name, CefThreadPriority priority, CefMessageLoopType messageLoopType, bool stoppable, CefComInitMode comInitMode)
		{
			if (name == null)
				throw new ArgumentNullException(nameof(name));

			fixed (char* s0 = name)
			{
				var cstr0 = new cef_string_t { Str = s0, Length = name.Length };
				return CefNativeApi.cef_thread_create(&cstr0, priority, messageLoopType, stoppable ? 1 : 0, comInitMode);
			}
		}


	}
}
