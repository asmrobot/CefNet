using CefNet.Internal;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CefNet
{
	/// <summary>
	/// Provide global methods.
	/// </summary>
	public static class CefNetApi
	{
		/// <summary>
		/// Compares two instances of the specified reference type <typeparamref name="T"/>
		/// for reference equality and, if they are equal, replaces the first one.
		/// </summary>
		/// <typeparam name="T">
		/// The type to be used for <paramref name="location"/>, <paramref name="value"/>,
		/// and <paramref name="comparand"/>. This type must be a reference type.
		/// </typeparam>
		/// <param name="location">
		/// The destination, whose value is compared by reference with <paramref name="comparand"/>
		/// and possibly replaced.
		/// </param>
		/// <param name="value">
		/// The value that replaces the destination value if the comparison by reference
		/// results in equality.
		/// </param>
		/// <param name="comparand">
		/// The value that is compared by reference to the value at <paramref name="location"/>.
		/// </param>
		/// <returns></returns>
		public static T CompareExchange<T>(in T location, T value, T comparand)
			where T : class
		{
			return UtilsExtensions.As<T>(Interlocked.CompareExchange(ref UtilsExtensions.AsRef<T, object>(in location), value, comparand));
		}

		/// <summary>
		/// Post an action for execution on the specified thread.
		/// </summary>
		/// <param name="action">The specified action.</param>
		/// <param name="threadId">The specified thread id.</param>
		/// <returns>True on success.</returns>
		/// <remarks>
		/// It is an error to request a thread from the wrong process.
		/// </remarks>
		public static bool Post(CefThreadId threadId, Action action)
		{
			return CefApi.PostTask(threadId, new CefActionTask(action));
		}

		/// <summary>
		/// Post an action for execution on the specified thread.
		/// </summary>
		/// <param name="action">The specified action.</param>
		/// <param name="threadId">The specified thread id.</param>
		/// <param name="delayMs">
		/// The number of milliseconds to wait before executing the posted action.
		/// </param>
		/// <returns>True on success.</returns>
		/// <remarks>
		/// It is an error to request a thread from the wrong process. Execution will
		/// occur asynchronously. Delayed tasks are not supported on V8 WebWorker threads
		/// and will be executed without the specified delay.
		/// </remarks>
		public static bool Post(CefThreadId threadId, Action action, long delayMs)
		{
			return CefApi.PostTask(threadId, new CefActionTask(action), delayMs);
		}

		/// <summary>
		/// Performs a function on the specified thread.
		/// </summary>
		/// <typeparam name="T">The return type of a function.</typeparam>
		/// <param name="threadId">The CEF-thread identifier.</param>
		/// <param name="func">The function.</param>
		/// <param name="cancellationToken">The cancellation token to cancel operation.</param>
		/// <returns>The task object representing the asynchronous operation.</returns>
		/// <exception cref="ArgumentNullException">The <paramref name="func"/> is null.</exception>
		/// <exception cref="InvalidOperationException">Access a thread from the wrong process.</exception>
		public static async Task<T> SendAsync<T>(CefThreadId threadId, Func<T> func, CancellationToken cancellationToken)
		{
			if (func == null)
				throw new ArgumentNullException(nameof(func));

			var tcs = new TaskCompletionSource<T>();
			var internalAction = new Action(() =>
			{
				if (cancellationToken.IsCancellationRequested)
				{
					tcs.SetCanceled();
				}
				else
				{
					T result = default;
					try
					{
						result = func();
					}
					catch (Exception e)
					{
						tcs.SetException(e);
						return;
					}
					tcs.SetResult(result);
				}
			});

			if (!Post(threadId, internalAction))
				throw new InvalidOperationException();

			if (!cancellationToken.CanBeCanceled)
				return await tcs.Task.ConfigureAwait(false);

			using (var reg = cancellationToken.Register(() => tcs.TrySetCanceled()))
			{
				return await tcs.Task.ConfigureAwait(false);
			}
		}

		/// <summary>
		/// Performs an action on the specified thread.
		/// </summary>
		/// <param name="threadId">The CEF-thread identifier.</param>
		/// <param name="action">The action.</param>
		/// <param name="cancellationToken">The cancellation token to cancel operation.</param>
		/// <returns>The task object representing the asynchronous operation.</returns>
		/// <exception cref="ArgumentNullException">The <paramref name="func"/> is null.</exception>
		/// <exception cref="InvalidOperationException">Access a thread from the wrong process.</exception>
		public static async Task SendAsync(CefThreadId threadId, Action action, CancellationToken cancellationToken)
		{
			if (action == null)
				throw new ArgumentNullException(nameof(action));

			var tcs = new TaskCompletionSource<bool>();
			var internalAction = new Action(() =>
			{
				if (cancellationToken.IsCancellationRequested)
				{
					tcs.SetCanceled();
				}
				else
				{
					try
					{
						action();
					}
					catch (Exception e)
					{
						tcs.SetException(e);
						return;
					}
					tcs.SetResult(true);
				}
			});

			if (!Post(threadId, internalAction))
				throw new InvalidOperationException();

			if (cancellationToken.CanBeCanceled)
			{
				using (var reg = cancellationToken.Register(() => tcs.TrySetCanceled()))
				{
					await tcs.Task.ConfigureAwait(false);
				}
			}
			else
			{
				await tcs.Task.ConfigureAwait(false);
			}
		}

		/// <summary>
		/// Performs a function on the specified thread.
		/// </summary>
		/// <typeparam name="T">The return type of a function.</typeparam>
		/// <param name="threadId">The CEF-thread identifier.</param>
		/// <param name="func">The function.</param>
		/// <param name="delayMs">
		/// The number of milliseconds to wait before performs the sended function.
		/// </param>
		/// <param name="cancellationToken">The cancellation token to cancel operation.</param>
		/// <returns>The task object representing the asynchronous operation.</returns>
		/// <exception cref="ArgumentNullException">The <paramref name="func"/> is null.</exception>
		/// <exception cref="InvalidOperationException">Access a thread from the wrong process.</exception>
		public static async Task<T> SendAsync<T>(CefThreadId threadId, Func<T> func, int delayMs, CancellationToken cancellationToken)
		{
			if (func == null)
				throw new ArgumentNullException(nameof(func));

			var tcs = new TaskCompletionSource<T>();
			var internalAction = new Action(() =>
			{
				if (cancellationToken.IsCancellationRequested)
				{
					tcs.SetCanceled();
				}
				else
				{
					T result = default;
					try
					{
						result = func();
					}
					catch (Exception e)
					{
						tcs.SetException(e);
						return;
					}
					tcs.SetResult(result);
				}
			});

			if (!Post(threadId, internalAction, delayMs))
				throw new InvalidOperationException();

			if (!cancellationToken.CanBeCanceled)
				return await tcs.Task.ConfigureAwait(false);

			using (var reg = cancellationToken.Register(() => tcs.TrySetCanceled()))
			{
				return await tcs.Task.ConfigureAwait(false);
			}
		}

		/// <summary>
		/// Performs an action on the specified thread.
		/// </summary>
		/// <param name="threadId">The CEF-thread identifier.</param>
		/// <param name="action">The action.</param>
		/// <param name="delayMs">
		/// The number of milliseconds to wait before executing the sended action.
		/// </param>
		/// <param name="cancellationToken">The cancellation token to cancel operation.</param>
		/// <returns>The task object representing the asynchronous operation.</returns>
		/// <exception cref="ArgumentNullException">The <paramref name="action"/> is null.</exception>
		/// <exception cref="InvalidOperationException">Access a thread from the wrong process.</exception>
		public static async Task SendAsync(CefThreadId threadId, Action action, int delayMs, CancellationToken cancellationToken)
		{
			if (action == null)
				throw new ArgumentNullException(nameof(action));

			var tcs = new TaskCompletionSource<bool>();
			var internalAction = new Action(() =>
			{
				if (cancellationToken.IsCancellationRequested)
				{
					tcs.SetCanceled();
				}
				else
				{
					try
					{
						action();
					}
					catch (Exception e)
					{
						tcs.SetException(e);
						return;
					}
					tcs.SetResult(true);
				}
			});

			if (!Post(threadId, internalAction))
				throw new InvalidOperationException();

			if (cancellationToken.CanBeCanceled)
			{
				using (var reg = cancellationToken.Register(() => tcs.TrySetCanceled()))
				{
					await tcs.Task.ConfigureAwait(false);
				}
			}
			else
			{
				await tcs.Task.ConfigureAwait(false);
			}
		}

	}
}
