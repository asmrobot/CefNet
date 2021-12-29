using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CefNet.Net;

namespace CefNet.Internal
{
	internal sealed class GetCookieVisitor : CefCookieVisitor
	{
#if !NET45
		private readonly CancellationToken _cancellationToken;
#endif
		private readonly CancellationTokenRegistration _cancellation;
		private readonly TaskCompletionSource<CefNetCookie[]> _completionSource;
		private readonly List<CefNetCookie> _cookies;
		private readonly Func<CefCookie, bool> _filter;
		private bool _continue;

		public GetCookieVisitor(Func<CefCookie, bool> filter, CancellationToken cancellationToken)
		{
			_continue = true;
			_filter = filter;
			_cookies = new List<CefNetCookie>(0);
#if NET45
			_completionSource = new TaskCompletionSource<CefNetCookie[]>();
#else
			_completionSource = new TaskCompletionSource<CefNetCookie[]>(TaskCreationOptions.RunContinuationsAsynchronously);
			_cancellationToken = cancellationToken;
#endif
			_cancellation = cancellationToken.Register(Cancel, new WeakReference<GetCookieVisitor>(this));
		}

		private static void Cancel(object state)
		{
			if (((WeakReference<GetCookieVisitor>)state).TryGetTarget(out GetCookieVisitor self))
			{
#if NET45
				self._completionSource.TrySetCanceled();
#else
				self._completionSource.TrySetCanceled(self._cancellationToken);
#endif
				self._cancellation.Dispose();
				self._continue = false;
			}
		}

		public Task<CefNetCookie[]> Task
		{
			get { return _completionSource.Task; }
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			_cancellation.Dispose();
#if NET45
			ThreadPool.QueueUserWorkItem(_ => _completionSource.TrySetResult(_cookies.ToArray()));
#else
			_completionSource.TrySetResult(_cookies.ToArray());
#endif
		}

		protected internal override bool Visit(CefCookie cookie, int count, int total, ref int deleteCookie)
		{
			try
			{
				_cookies.Capacity = total;
			}
			catch (OutOfMemoryException e)
			{
				_completionSource.TrySetException(e);
				return false;
			}
			if (_filter is null || _filter(cookie))
				_cookies.Add(new CefNetCookie(cookie));
			return _continue;
		}
	}
}
