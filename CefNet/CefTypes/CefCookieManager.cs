using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using CefNet.CApi;
using CefNet.Internal;
using CefNet.Net;

namespace CefNet
{
	public partial class CefCookieManager
	{
		/// <summary>
		/// Returns the global cookie manager. By default data will be stored at CefSettings.CachePath
		/// if specified or in memory otherwise. Using this function is equivalent to calling
		/// CefRequestContext.GetGlobalContext().GetDefaultCookieManager().
		/// </summary>
		/// <param name="callback">
		/// If |callback| is non-NULL it will be executed asnychronously on the UI thread after the
		/// manager&apos;s storage has been initialized.
		/// </param>
		public static unsafe CefCookieManager GetGlobalManager(CefCompletionCallback callback)
		{
			return CefCookieManager.Wrap(CefCookieManager.Create, CefNativeApi.cef_cookie_manager_get_global_manager(callback != null ? callback.GetNativeInstance() : null));
		}

		/// <summary>
		/// Gets an array that contains the <see cref="CefNetCookie"/> instances.
		/// </summary>
		/// <param name="filter">
		/// If the <paramref name="filter"/> function returns true, the cookie will be included in the result.
		/// </param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
		/// <returns>The task object representing the asynchronous operation.</returns>
		/// <remarks>The result of the task can be null if cookies cannot be accessed.</remarks>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Task<CefNetCookie[]> GetCookiesAsync(CancellationToken cancellationToken)
		{
			return GetCookiesAsync(null, cancellationToken);
		}

		/// <summary>
		/// Gets an array that contains the <see cref="CefNetCookie"/> instances.
		/// </summary>
		/// <param name="filter">
		/// If the <paramref name="filter"/> function returns true, the cookie will be included in the result.
		/// </param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
		/// <returns>The task object representing the asynchronous operation.</returns>
		/// <remarks>The result of the task can be null if cookies cannot be accessed.</remarks>
		public Task<CefNetCookie[]> GetCookiesAsync(Func<CefCookie, bool> filter, CancellationToken cancellationToken)
		{
			var cookieVisitor = new GetCookieVisitor(filter, cancellationToken);
			if (!VisitAllCookies(cookieVisitor))
				return Task.FromResult<CefNetCookie[]>(null);
			return cookieVisitor.Task;
		}

		/// <summary>
		/// Gets an array that contains the <see cref="CefNetCookie"/> instances that are associated with a specific URI.
		/// </summary>
		/// <param name="url">The URI of the <see cref="CefNetCookie"/> instances desired.</param>
		/// <param name="includeHttpOnly">A value indicating whether HTTP-only cookies should be included in the result.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
		/// <returns>The task object representing the asynchronous operation.</returns>
		/// <remarks>The result of the task can be null if cookies cannot be accessed.</remarks>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Task<CefNetCookie[]> GetCookiesAsync(string url, bool includeHttpOnly, CancellationToken cancellationToken)
		{
			return GetCookiesAsync(url, includeHttpOnly, null, cancellationToken);
		}

		/// <summary>
		/// Gets an array that contains the <see cref="CefNetCookie"/> instances that are associated with a specific URI.
		/// </summary>
		/// <param name="url">The URI of the <see cref="CefNetCookie"/> instances desired.</param>
		/// <param name="includeHttpOnly">
		/// A value indicating whether HTTP-only cookies should be included in the result.
		/// </param>
		/// <param name="filter">
		/// If the <paramref name="filter"/> function returns true, the cookie will be included in the result.
		/// </param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
		/// <returns>The task object representing the asynchronous operation.</returns>
		/// <remarks>The result of the task can be null if cookies cannot be accessed.</remarks>
		public Task<CefNetCookie[]> GetCookiesAsync(string url, bool includeHttpOnly, Func<CefCookie, bool> filter, CancellationToken cancellationToken)
		{
			if (Uri.TryCreate(url, UriKind.Absolute, out Uri uri)
				&& (Uri.UriSchemeHttp.Equals(uri.Scheme, StringComparison.Ordinal) || Uri.UriSchemeHttps.Equals(uri.Scheme, StringComparison.Ordinal)))
			{
				var cookieVisitor = new GetCookieVisitor(filter, cancellationToken);
				if (!VisitUrlCookies(url, includeHttpOnly, cookieVisitor))
					return Task.FromResult<CefNetCookie[]>(null);
				return cookieVisitor.Task;
			}
			throw new ArgumentOutOfRangeException(nameof(url));
		}

		/// <summary>
		/// Sets a cookie given a valid URL and explicit user-provided cookie
		/// attributes.
		/// </summary>
		/// <param name="url">The cookie URL.</param>
		/// <param name="cookie">The cookie.</param>
		/// <param name="callback">
		/// If <paramref name="callback"/> is non-NULL it will be executed
		/// asnychronously on the CEF UI thread after the cookie has been set.
		/// </param>
		/// <returns>
		/// true if the operation is completed successfully; false if cookies cannot be accessed.
		/// </returns>
		/// <exception cref="ArgumentNullException">The <paramref name="url"/> is null or the <paramref name="cookie"/> is null.</exception>
		/// <exception cref="ArgumentOutOfRangeException">An invalid URL is specified.</exception>
		public unsafe bool SetCookie(string url, CefNetCookie cookie, CefSetCookieCallback callback)
		{
			if (url is null)
				throw new ArgumentNullException(nameof(url));
			if (cookie is null)
				throw new ArgumentNullException(nameof(cookie));

			if (Uri.TryCreate(url, UriKind.Absolute, out Uri uri)
				&& (Uri.UriSchemeHttp.Equals(uri.Scheme, StringComparison.Ordinal) || Uri.UriSchemeHttps.Equals(uri.Scheme, StringComparison.Ordinal)))
			{
				CefCookie aCookie = cookie.ToCefCookie();
				try
				{
					if (cookie.Domain != null && !cookie.Domain.StartsWith("."))
						aCookie.Domain = null;

					fixed (char* s0 = url)
					{
						var cstr0 = new cef_string_t { Str = s0, Length = url.Length };
						return SafeCall(NativeInstance->SetCookie(&cstr0, (cef_cookie_t*)&aCookie, (callback != null) ? callback.GetNativeInstance() : null) != 0);
					}
				}
				finally
				{
					aCookie.Dispose();
				}
			}
			throw new ArgumentOutOfRangeException(nameof(url));
		}

		/// <summary>
		/// Deletes all cookies that match the specified parameters.
		/// </summary>
		/// <param name="domain">The host or the domain that the cookie is available to.</param>
		/// <param name="name">The name of the cookie to delete; or null to delete all host cookies.</param>
		/// <param name="path">
		/// The path of the cookie to delete; or null to delete all host cookies with the specified
		/// <paramref name="name"/>.
		/// </param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
		/// <returns>
		/// The task object representing the asynchronous operation.
		/// The result of the task is the number of cookies that were deleted.
		/// </returns>
		/// <exception cref="ArgumentNullException">The <paramref name="domain"/> is null.</exception>
		/// <exception cref="InvalidOperationException">Cookies cannot be accessed.</exception>
		public Task<int> DeleteCookiesAsync(string domain, string name, string path, CancellationToken cancellationToken)
		{
			var deleteCookieVisitor = new DeleteCookieVisitor(domain, name, path, cancellationToken);
			if (!VisitAllCookies(deleteCookieVisitor))
				throw new InvalidOperationException();
			return deleteCookieVisitor.CompletionTask;
		}

		/// <summary>
		/// Flush the backing store (if any) to disk.
		/// </summary>
		/// <param name="cancellationToken">
		/// The token to monitor for cancellation requests.
		/// The default value is <see cref="CancellationToken.None"/>.
		/// </param>
		/// <returns>
		/// The result of an asynchronous operation is false if cookies cannot be accessed.
		/// </returns>
		public async Task<bool> FlushStoreAsync(CancellationToken cancellationToken = default)
		{
			bool result;
			var tcs = new TaskCompletionSource<int>();
			using (cancellationToken.Register(() => tcs.TrySetCanceled()))
			{
				cancellationToken.ThrowIfCancellationRequested();
				result = this.FlushStore(new CefCompletionCallbackImpl(tcs.TrySetResult));
				await tcs.Task.ConfigureAwait(false);
			}
			return result;
		}

	}
}
