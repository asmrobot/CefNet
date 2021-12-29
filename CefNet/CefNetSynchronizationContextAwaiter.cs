using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

namespace CefNet
{
	/// <summary>
	/// Provides an awaiter for an CEF thread.
	/// </summary>
	public sealed class CefNetSynchronizationContextAwaiter : INotifyCompletion
	{
		private readonly static ReaderWriterLockSlim _SyncRoot;
		private static Dictionary<CefThreadId, CefNetSynchronizationContextAwaiter> _Awaiters;
		private readonly CefThreadId _threadId;

		static CefNetSynchronizationContextAwaiter()
		{
			_SyncRoot = new ReaderWriterLockSlim();

			_Awaiters = new Dictionary<CefThreadId, CefNetSynchronizationContextAwaiter>((int)CefThreadId.Renderer);
			if (CefNetApplication.ProcessType == ProcessType.Main)
				_Awaiters.Add(CefThreadId.UI, new CefNetSynchronizationContextAwaiter(CefThreadId.UI));
			else if (CefNetApplication.ProcessType == ProcessType.Renderer)
				_Awaiters.Add(CefThreadId.Renderer, new CefNetSynchronizationContextAwaiter(CefThreadId.Renderer));
		}

		/// <summary>
		/// Returns the <see cref="CefNetSynchronizationContextAwaiter"/> for the specified CEF thread.
		/// </summary>
		/// <param name="threadId">The CEF thread identifier.</param>
		/// <returns>The <see cref="CefNetSynchronizationContextAwaiter"/> for the CEF thread.</returns>
		public static CefNetSynchronizationContextAwaiter GetForThread(CefThreadId threadId)
		{
			CefNetSynchronizationContextAwaiter instance;
			_SyncRoot.EnterReadLock();
			try
			{
				_Awaiters.TryGetValue(threadId, out instance);
			}
			finally
			{
				_SyncRoot.ExitReadLock();
			}
			if (instance != null)
				return instance;

			// These awaiters should have been added in the static constructor.
			if (threadId == CefThreadId.Renderer || threadId == CefThreadId.UI)
				throw new ArgumentOutOfRangeException(nameof(threadId));

			_SyncRoot.EnterWriteLock();
			try
			{
				if(!_Awaiters.TryGetValue(threadId, out instance))
				{
					instance = new CefNetSynchronizationContextAwaiter(threadId);
					_Awaiters.Add(threadId, instance);
				}
			}
			finally
			{
				_SyncRoot.ExitWriteLock();
			}
			return instance;
		}

		private CefNetSynchronizationContextAwaiter(CefThreadId tid)
		{
			_threadId = tid;
		}

		/// <summary>
		/// Gets a value that specifies whether the task being awaited is completed.
		/// </summary>
		public bool IsCompleted
		{
			get { return CefApi.CurrentlyOn(_threadId); }
		}

		/// <summary>
		/// Schedules the continuation action for the task associated with this awaiter.
		/// </summary>
		/// <param name="continuation">
		/// The action to invoke when the await operation completes.
		/// </param>
		public void OnCompleted(Action continuation)
		{
			if (CefNetApi.Post(_threadId, continuation))
				return;
			throw new InvalidOperationException();
		}

		/// <summary>
		/// Ends the await on the completed task.
		/// </summary>
		public void GetResult() { }

		/// <summary>
		/// Returns an awaiter for this awaitable object.
		/// </summary>
		/// <returns>The awaiter.</returns>
		public CefNetSynchronizationContextAwaiter GetAwaiter()
		{
			return this;
		}
	}

}
