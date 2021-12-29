using System;
using System.Threading;
using System.Threading.Tasks;

namespace CefNet.Internal
{
	internal sealed class DeleteCookieVisitor : CefCookieVisitor
	{
		private readonly TaskCompletionSource<int> _completion;
		private readonly CancellationTokenRegistration _cancellation;
		private readonly string _domain;
		private readonly string _name;
		private readonly string _path;
		private bool _continue;
		private int _count;

		public DeleteCookieVisitor(string domain, string name, string path, CancellationToken cancellationToken)
		{
			if (domain is null)
				throw new ArgumentNullException(nameof(domain));

			_count = 0;
			_continue = true;
			_name = name;
			_domain = domain;
			_path = path;
#if NET45
			_completion = new TaskCompletionSource<int>();
#else
			_completion = new TaskCompletionSource<int>(TaskCreationOptions.RunContinuationsAsynchronously);
#endif
			_cancellation = cancellationToken.Register(Cancel, new WeakReference<DeleteCookieVisitor>(this));
		}

		public Task<int> CompletionTask
		{
			get { return _completion.Task; }
		}

		protected internal override bool Visit(CefCookie cookie, int count, int total, ref int deleteCookie)
		{
			if (string.Equals(cookie.Domain, _domain, StringComparison.OrdinalIgnoreCase))
			{
				if (_name is null || _name.Equals(cookie.Name, StringComparison.OrdinalIgnoreCase))
				{
					if (_path is null || _path.Equals(cookie.Path, StringComparison.InvariantCultureIgnoreCase))
					{
						_count++;
						deleteCookie = 1;
					}
				}
			}
			return _continue;
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			_cancellation.Dispose();
#if NET45
			ThreadPool.QueueUserWorkItem(_ => _completion.TrySetResult(_count));
#else
			_completion.TrySetResult(_count);
#endif
		}

		private static void Cancel(object state)
		{
			if (((WeakReference<DeleteCookieVisitor>)state).TryGetTarget(out DeleteCookieVisitor self))
			{
				self._cancellation.Dispose();
				self._completion.TrySetCanceled();
				self._continue = false;
			}
		}

	}
}
