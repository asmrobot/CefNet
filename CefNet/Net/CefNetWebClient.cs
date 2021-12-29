using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CefNet.Net
{

	/// <summary>
	/// Provides a base class for sending HTTP requests and receiving HTTP responses from a resource identified by a URI.
	/// </summary>
	/// <remarks>
	/// This class is only available in the browser process.
	/// </remarks>
	public class CefNetWebClient : ICefNetCredentialProvider
	{
		private readonly CefRequestContext _context;

		/// <summary>
		/// Initializes a new instance of the <see cref="CefNetWebClient"/> class.
		/// </summary>
		public CefNetWebClient()
			: this(null, CefUrlRequestFlags.AllowStoredCredentials, Encoding.UTF8)
		{

		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CefNetWebClient"/> class.
		/// </summary>
		/// <param name="context">
		/// A request context or null, if <paramref name="context"/> is empty the global
		/// request context will be used. For requests originating from the render process
		/// this parameter must be null.
		/// </param>
		/// <param name="encoding">
		/// The default <see cref="Encoding"/> that is used to encode strings. 
		/// If <paramref name="encoding"/> is null then UTF-8 will be used.
		/// </param>
		/// <param name="flags">Flags used to customize the behavior of request.</param>
		public CefNetWebClient(CefRequestContext context, CefUrlRequestFlags flags, Encoding encoding)
		{
			_context = context;
			this.DefaultEncoding = encoding ?? Encoding.UTF8;
			this.RequestFlags = flags;
		}

		/// <summary>
		/// Gets the <see cref="Encoding"/> used to upload and download strings.
		/// </summary>
		public Encoding DefaultEncoding { get; }

		/// <summary>
		/// Gets flags used to customize the behavior of request.
		/// </summary>
		public CefUrlRequestFlags RequestFlags { get; }

		/// <summary>
		/// Send a GET request to the specified <see cref="Uri"/>.
		/// </summary>
		/// <param name="requestUri">
		/// The <see cref="Uri"/> the request is sent to.
		/// </param>
		/// <param name="referrerUri">
		/// The <see cref="Uri"/> of the referring site for a request. Can be null.
		/// </param>
		/// <param name="referrerPolicy">
		/// The policy for how the Referrer HTTP header value will be sent during request.
		/// </param>
		/// <param name="headers">
		/// A <see cref="NameValueCollection"/> containing header name/value pairs associated with a request. Can be null.
		/// </param>
		/// <param name="cancellationToken">
		/// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
		/// </param>
		/// <returns>The task object representing the asynchronous operation.</returns>
		/// <exception cref="ArgumentNullException">The <see cref="requestUri"/> parameter is null.</exception>
		public async Task<CefNetWebRequest> GetAsync(Uri requestUri, Uri referrerUri, CefReferrerPolicy referrerPolicy, NameValueCollection headers, CancellationToken cancellationToken)
		{
			if (requestUri is null)
				throw new ArgumentNullException(nameof(requestUri));

			var r = new CefRequest();
			r.Flags = (int)this.RequestFlags;
			r.Url = requestUri.AbsoluteUri;
			if (referrerUri != null)
				r.SetReferrer(referrerUri.AbsoluteUri, referrerPolicy);
			if (headers != null && headers.Count > 0)
			{
				using (var map = new CefStringMultimap())
				{
					map.Add(headers);
					r.SetHeaderMap(map);
				}
			}

			var request = new CefNetWebRequest(this);
			await request.SendAsync(r, _context, cancellationToken);
			return request;
		}

		/// <summary>
		/// Send a GET request to the specified <see cref="Uri"/>.
		/// </summary>
		/// <param name="requestUri">
		/// The URI the request is sent to.
		/// </param>
		/// <param name="referrerUri">
		/// The URI of the referring site for a request. Can be null.
		/// </param>
		/// <param name="referrerPolicy">
		/// The policy for how the Referrer HTTP header value will be sent during request.
		/// </param>
		/// <param name="headers">
		/// A <see cref="NameValueCollection"/> containing header name/value pairs associated with a request. Can be null.
		/// </param>
		/// <param name="cancellationToken">
		/// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
		/// </param>
		/// <returns>The task object representing the asynchronous operation.</returns>
		/// <exception cref="ArgumentNullException">The <see cref="requestUri"/> parameter is null.</exception>
		public Task<CefNetWebRequest> GetAsync(string requestUri, string referrerUri, CefReferrerPolicy referrerPolicy, NameValueCollection headers, CancellationToken cancellationToken)
		{
			return GetAsync(new Uri(requestUri, UriKind.Absolute), referrerUri is null ? null : new Uri(referrerUri, UriKind.Absolute), referrerPolicy, headers, cancellationToken);
		}

		/// <summary>
		/// Send a GET request to the specified <see cref="Uri"/>.
		/// </summary>
		/// <param name="requestUri">
		/// The <see cref="Uri"/> the request is sent to.
		/// </param>
		/// <param name="headers">
		/// A <see cref="NameValueCollection"/> containing header name/value pairs associated with a request. Can be null.
		/// </param>
		/// <param name="cancellationToken">
		/// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
		/// </param>
		/// <returns>The task object representing the asynchronous operation.</returns>
		/// <exception cref="ArgumentNullException">The <see cref="requestUri"/> parameter is null.</exception>
		public Task<CefNetWebRequest> GetAsync(Uri requestUri, NameValueCollection headers, CancellationToken cancellationToken)
		{
			return GetAsync(requestUri, null, CefReferrerPolicy.Default, headers, cancellationToken);
		}

		/// <summary>
		/// Send a GET request to the specified <see cref="Uri"/>.
		/// </summary>
		/// <param name="requestUri">
		/// The URI the request is sent to.
		/// </param>
		/// <param name="headers">
		/// A <see cref="NameValueCollection"/> containing header name/value pairs associated with a request. Can be null.
		/// </param>
		/// <param name="cancellationToken">
		/// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
		/// </param>
		/// <returns>The task object representing the asynchronous operation.</returns>
		/// <exception cref="ArgumentNullException">The <see cref="requestUri"/> parameter is null.</exception>
		public Task<CefNetWebRequest> GetAsync(string requestUri, NameValueCollection headers, CancellationToken cancellationToken)
		{
			return GetAsync(new Uri(requestUri, UriKind.Absolute), null, CefReferrerPolicy.Default, headers, cancellationToken);
		}

		/// <summary>
		/// Send a GET request to the specified <see cref="Uri"/>.
		/// </summary>
		/// <param name="requestUri">
		/// The <see cref="Uri"/> the request is sent to.
		/// </param>
		/// <param name="cancellationToken">
		/// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
		/// </param>
		/// <returns>The task object representing the asynchronous operation.</returns>
		/// <exception cref="ArgumentNullException">The <see cref="requestUri"/> parameter is null.</exception>
		public Task<CefNetWebRequest> GetAsync(Uri requestUri, CancellationToken cancellationToken)
		{
			return GetAsync(requestUri, null, CefReferrerPolicy.Default, null, cancellationToken);
		}

		/// <summary>
		/// Send a GET request to the specified <see cref="Uri"/>.
		/// </summary>
		/// <param name="requestUri">
		/// The URI the request is sent to.
		/// </param>
		/// <param name="cancellationToken">
		/// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
		/// </param>
		/// <returns>The task object representing the asynchronous operation.</returns>
		/// <exception cref="ArgumentNullException">The <see cref="requestUri"/> parameter is null.</exception>
		public Task<CefNetWebRequest> GetAsync(string requestUri, CancellationToken cancellationToken)
		{
			return GetAsync(new Uri(requestUri, UriKind.Absolute), null, CefReferrerPolicy.Default, null, cancellationToken);
		}

		/// <summary>
		/// Send a GET request to the specified <see cref="Uri"/> and return the response body
		/// as a string in an asynchronous operation.
		/// </summary>
		/// <param name="requestUri">The <see cref="Uri"/> the request is sent to.</param>
		/// <param name="referrerUri">
		/// The <see cref="Uri"/> of the referring site for a request. Can be null.
		/// </param>
		/// <param name="referrerPolicy">
		/// The policy for how the Referrer HTTP header value will be sent during request.
		/// </param>
		/// <param name="headers">
		/// A <see cref="NameValueCollection"/> containing header name/value pairs associated with a request. Can be null.
		/// </param>
		/// <param name="cancellationToken">
		/// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
		/// </param>
		/// <returns>The task object representing the asynchronous operation.</returns>
		public async Task<string> GetStringAsync(Uri requestUri, Uri referrerUri, CefReferrerPolicy referrerPolicy, NameValueCollection headers, CancellationToken cancellationToken)
		{
			CefNetWebRequest request = await GetAsync(requestUri, referrerUri, referrerPolicy, headers, cancellationToken).ConfigureAwait(false);
			AssertSuccess(request);

			Stream responseStream = request.GetResponseStream();
			if (responseStream is null)
				return string.Empty;

			CefResponse response = request.Response;
			if (response is null)
				return null;

			Encoding encoding;
			try
			{
				encoding = Encoding.GetEncoding(response.Charset);
			}
			catch (ArgumentException)
			{
				encoding = this.DefaultEncoding;
			}
			return new StreamReader(responseStream, encoding).ReadToEnd();
		}

		/// <summary>
		/// Send a GET request to the specified <see cref="Uri"/> and return the response body
		/// as a <see cref="Stream"/> in an asynchronous operation.
		/// </summary>
		/// <param name="requestUri">The <see cref="Uri"/> the request is sent to.</param>
		/// <param name="referrerUri">
		/// The <see cref="Uri"/> of the referring site for a request. Can be null.
		/// </param>
		/// <param name="referrerPolicy">
		/// The policy for how the Referrer HTTP header value will be sent during request.
		/// </param>
		/// <param name="headers">
		/// A <see cref="NameValueCollection"/> containing header name/value pairs associated with a request. Can be null.
		/// </param>
		/// <param name="cancellationToken">
		/// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
		/// </param>
		/// <returns>The task object representing the asynchronous operation.</returns>
		public async Task<Stream> GetStreamAsync(Uri requestUri, Uri referrerUri, CefReferrerPolicy referrerPolicy, NameValueCollection headers, CancellationToken cancellationToken)
		{
			if (requestUri is null)
				throw new ArgumentNullException(nameof(requestUri));

			CefNetWebRequest request = await GetAsync(requestUri, referrerUri, referrerPolicy, headers, cancellationToken).ConfigureAwait(false);
			AssertSuccess(request);

			return request.GetResponseStream();
		}

		/// <summary>
		/// Send a GET request to the specified <see cref="Uri"/> and return the response body
		/// as a <see cref="Byte"/> array in an asynchronous operation.
		/// </summary>
		/// <param name="requestUri">The <see cref="Uri"/> the request is sent to.</param>
		/// <param name="referrerUri">
		/// The <see cref="Uri"/> of the referring site for a request. Can be null.
		/// </param>
		/// <param name="referrerPolicy">
		/// The policy for how the Referrer HTTP header value will be sent during request.
		/// </param>
		/// <param name="headers">
		/// A <see cref="NameValueCollection"/> containing header name/value pairs associated with a request. Can be null.
		/// </param>
		/// <param name="cancellationToken">
		/// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
		/// </param>
		/// <returns>The task object representing the asynchronous operation.</returns>
		public async Task<byte[]> GetDataAsync(Uri requestUri, Uri referrerUri, CefReferrerPolicy referrerPolicy, NameValueCollection headers, CancellationToken cancellationToken)
		{
			using (Stream stream = await GetStreamAsync(requestUri, referrerUri, referrerPolicy, headers, cancellationToken).ConfigureAwait(false))
			{
				if (stream is CefNetMemoryStream mem && mem.Capacity == mem.Length)
				{
					return mem.GetBuffer();
				}
				var buffer = new byte[stream.Length];
				if (buffer.Length != await stream.ReadAsync(buffer, 0, buffer.Length).ConfigureAwait(false))
					throw new InvalidOperationException();
				return buffer;
			}
		}

		/// <summary>
		/// Send a POST request to the specified <see cref="Uri"/> with a cancellation token as an asynchronous operation.
		/// </summary>
		/// <param name="requestUri">The <see cref="Uri"/> the request is sent to.</param>
		/// <param name="referrerUri">
		/// The <see cref="Uri"/> of the referring site for a request. Can be null.
		/// </param>
		/// <param name="referrerPolicy">
		/// The policy for how the Referrer HTTP header value will be sent during request.
		/// </param>
		/// <param name="headers">
		/// A <see cref="NameValueCollection"/> containing header name/value pairs associated with a request. Can be null.
		/// </param>
		/// <param name="content">The request content sent to the specified <see cref="Uri"/>.</param>
		/// <param name="cancellationToken">
		/// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
		/// </param>
		/// <returns>The task object representing the asynchronous operation.</returns>
		public async Task<CefNetWebRequest> PostAsync(Uri requestUri, Uri referrerUri, CefReferrerPolicy referrerPolicy, CefPostData content, NameValueCollection headers, CancellationToken cancellationToken)
		{
			if (requestUri is null)
				throw new ArgumentNullException(nameof(requestUri));

			var r = new CefRequest();
			r.Flags = (int)this.RequestFlags;
			if (referrerUri != null)
				r.SetReferrer(referrerUri.AbsoluteUri, referrerPolicy);
			if (headers != null && headers.Count > 0)
			{
				using (var map = new CefStringMultimap())
				{
					map.Add(headers);
					r.Set(requestUri.AbsoluteUri, "POST", content, map);
				}
			}
			else
			{
				r.Url = requestUri.AbsoluteUri;
				r.Method = "POST";
				r.PostData = content;
			}

			var request = new CefNetWebRequest(this);
			await request.SendAsync(r, _context, cancellationToken);
			return request;
		}

		/// <summary>
		/// Send a POST request to the specified <see cref="Uri"/> with a cancellation token as an asynchronous operation.
		/// </summary>
		/// <param name="requestUri">The <see cref="Uri"/> the request is sent to.</param>
		/// <param name="referrerUri">
		/// The <see cref="Uri"/> of the referring site for a request. Can be null.
		/// </param>
		/// <param name="referrerPolicy">
		/// The policy for how the Referrer HTTP header value will be sent during request.
		/// </param>
		/// <param name="headers">
		/// A <see cref="NameValueCollection"/> containing header name/value pairs associated with a request. Can be null.
		/// </param>
		/// <param name="content">The request content sent to the specified <see cref="Uri"/>.</param>
		/// <param name="cancellationToken">
		/// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
		/// </param>
		/// <returns>The task object representing the asynchronous operation.</returns>
		public async Task<CefNetWebRequest> PostAsync(Uri requestUri, Uri referrerUri, CefReferrerPolicy referrerPolicy, IEnumerable<KeyValuePair<string, string>> content, NameValueCollection headers, CancellationToken cancellationToken)
		{
			if (requestUri is null)
				throw new ArgumentNullException(nameof(requestUri));

			var r = new CefRequest();
			r.Flags = (int)this.RequestFlags;
			if (referrerUri != null)
				r.SetReferrer(referrerUri.AbsoluteUri, referrerPolicy);

			using (var postData = new CefPostData(content))
			{
				if (headers != null && headers.Count > 0)
				{
					using (var map = new CefStringMultimap())
					{
						map.Add(headers);
						r.Set(requestUri.AbsoluteUri, "POST", postData, map);
					}
				}
				else
				{
					r.Url = requestUri.AbsoluteUri;
					r.Method = "POST";
					r.PostData = postData;
				}
			}
			var request = new CefNetWebRequest(this);
			await request.SendAsync(r, _context, cancellationToken);
			return request;
		}

		/// <summary>
		/// Send a POST request to the specified <see cref="Uri"/> with a cancellation token as an asynchronous operation.
		/// </summary>
		/// <param name="requestUri">The <see cref="Uri"/> the request is sent to.</param>
		/// <param name="referrerUri">
		/// The <see cref="Uri"/> of the referring site for a request. Can be null.
		/// </param>
		/// <param name="referrerPolicy">
		/// The policy for how the Referrer HTTP header value will be sent during request.
		/// </param>
		/// <param name="headers">
		/// A <see cref="NameValueCollection"/> containing header name/value pairs associated with a request. Can be null.
		/// </param>
		/// <param name="content">The request content sent to the specified <see cref="Uri"/>.</param>
		/// <param name="cancellationToken">
		/// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
		/// </param>
		/// <returns>The task object representing the asynchronous operation.</returns>
		public async Task<CefNetWebRequest> PostAsync(Uri requestUri, Uri referrerUri, CefReferrerPolicy referrerPolicy, NameValueCollection content, NameValueCollection headers, CancellationToken cancellationToken)
		{
			if (requestUri is null)
				throw new ArgumentNullException(nameof(requestUri));

			var r = new CefRequest();
			r.Flags = (int)this.RequestFlags;
			if (referrerUri != null)
				r.SetReferrer(referrerUri.AbsoluteUri, referrerPolicy);

			using (var postData = new CefPostData(content))
			{
				if (headers != null && headers.Count > 0)
				{
					using (var map = new CefStringMultimap())
					{
						map.Add(headers);
						r.Set(requestUri.AbsoluteUri, "POST", postData, map);
					}
				}
				else
				{
					r.Url = requestUri.AbsoluteUri;
					r.Method = "POST";
					r.PostData = postData;
				}
			}
			var request = new CefNetWebRequest(this);
			await request.SendAsync(r, _context, cancellationToken);
			return request;
		}

		/// <summary>
		/// Send a POST request to the specified <see cref="Uri"/>.
		/// </summary>
		/// <param name="requestUri">The <see cref="Uri"/> the request is sent to.</param>
		/// <param name="headers">
		/// A <see cref="NameValueCollection"/> containing header name/value pairs associated with a request. Can be null.
		/// </param>
		/// <param name="content">The request content sent to the specified <see cref="Uri"/>.</param>
		/// <param name="cancellationToken">
		/// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
		/// </param>
		/// <returns>The task object representing the asynchronous operation.</returns>
		public Task<CefNetWebRequest> PostAsync(Uri requestUri, CefPostData content, NameValueCollection headers, CancellationToken cancellationToken)
		{
			return PostAsync(requestUri, null, CefReferrerPolicy.Default, content, headers, cancellationToken);
		}

		/// <summary>
		/// Send a POST request to the specified <see cref="Uri"/>.
		/// </summary>
		/// <param name="requestUri">The <see cref="Uri"/> the request is sent to.</param>
		/// <param name="headers">
		/// A <see cref="NameValueCollection"/> containing header name/value pairs associated with a request. Can be null.
		/// </param>
		/// <param name="content">The request content sent to the specified <see cref="Uri"/>.</param>
		/// <param name="cancellationToken">
		/// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
		/// </param>
		/// <returns>The task object representing the asynchronous operation.</returns>
		public Task<CefNetWebRequest> PostAsync(Uri requestUri, IEnumerable<KeyValuePair<string, string>> content, NameValueCollection headers, CancellationToken cancellationToken)
		{
			return PostAsync(requestUri, null, CefReferrerPolicy.Default, content, headers, cancellationToken);
		}

		/// <summary>
		/// Send a POST request to the specified <see cref="Uri"/>.
		/// </summary>
		/// <param name="requestUri">The <see cref="Uri"/> the request is sent to.</param>
		/// <param name="content">The request content sent to the specified <see cref="Uri"/>.</param>
		/// <param name="cancellationToken">
		/// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
		/// </param>
		/// <returns>The task object representing the asynchronous operation.</returns>
		public Task<CefNetWebRequest> PostAsync(Uri requestUri, CefPostData content, CancellationToken cancellationToken)
		{
			return PostAsync(requestUri, null, CefReferrerPolicy.Default, content, null, cancellationToken);
		}

		/// <summary>
		/// Send a POST request to the specified <see cref="Uri"/>.
		/// </summary>
		/// <param name="requestUri">The <see cref="Uri"/> the request is sent to.</param>
		/// <param name="content">
		/// The request content sent to the specified <see cref="Uri"/>
		/// as an application/x-www-form-urlencoded string.
		/// </param>
		/// <param name="cancellationToken">
		/// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
		/// </param>
		/// <returns>The task object representing the asynchronous operation.</returns>
		public async Task<CefNetWebRequest> PostAsync(Uri requestUri, NameValueCollection content, CancellationToken cancellationToken)
		{
			using (var postData = new CefPostData(content))
			{
				return await PostAsync(requestUri, null, CefReferrerPolicy.Default, postData, null, cancellationToken);
			}
		}

		/// <summary>
		/// Send a POST request to the specified <see cref="Uri"/>.
		/// </summary>
		/// <param name="requestUri">The <see cref="Uri"/> the request is sent to.</param>
		/// <param name="content">
		/// The request content sent to the specified <see cref="Uri"/>
		/// as an application/x-www-form-urlencoded string.
		/// </param>
		/// <param name="cancellationToken">
		/// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
		/// </param>
		/// <returns>The task object representing the asynchronous operation.</returns>
		public async Task<CefNetWebRequest> PostAsync(Uri requestUri, IEnumerable<KeyValuePair<string, string>> content, CancellationToken cancellationToken)
		{
			using (var postData = new CefPostData(content))
			{
				return await PostAsync(requestUri, null, CefReferrerPolicy.Default, postData, null, cancellationToken);
			}
		}

		/// <summary>
		/// Send a POST request to the specified <see cref="Uri"/> and return the response body
		/// as a string in an asynchronous operation.
		/// </summary>
		/// <param name="requestUri">The <see cref="Uri"/> the request is sent to.</param>
		/// <param name="referrerUri">
		/// The <see cref="Uri"/> of the referring site for a request. Can be null.
		/// </param>
		/// <param name="referrerPolicy">
		/// The policy for how the Referrer HTTP header value will be sent during request.
		/// </param>
		/// <param name="headers">
		/// A <see cref="NameValueCollection"/> containing header name/value pairs associated with a request. Can be null.
		/// </param>
		/// <param name="content">The request content sent to the specified <see cref="Uri"/>.</param>
		/// <param name="cancellationToken">
		/// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
		/// </param>
		/// <returns>The task object representing the asynchronous operation.</returns>
		public async Task<string> PostReadStringAsync(Uri requestUri, Uri referrerUri, CefReferrerPolicy referrerPolicy, CefPostData content, NameValueCollection headers, CancellationToken cancellationToken)
		{
			var request = await PostAsync(requestUri, referrerUri, referrerPolicy, content, headers, cancellationToken).ConfigureAwait(false);
			AssertSuccess(request);

			Stream responseStream = request.GetResponseStream();
			if (responseStream is null)
				return string.Empty;

			CefResponse response = request.Response;
			if (response is null)
				return null;

			Encoding encoding;
			try
			{
				encoding = Encoding.GetEncoding(response.Charset);
			}
			catch (ArgumentException)
			{
				encoding = this.DefaultEncoding;
			}
			return new StreamReader(responseStream, encoding).ReadToEnd();
		}

		/// <summary>
		/// Send a POST request to the specified <see cref="Uri"/> and return the response body
		/// as a string in an asynchronous operation.
		/// </summary>
		/// <param name="requestUri">The <see cref="Uri"/> the request is sent to.</param>
		/// <param name="content">The request content sent to the specified <see cref="Uri"/>.</param>
		/// <param name="cancellationToken">
		/// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
		/// </param>
		/// <returns>The task object representing the asynchronous operation.</returns>
		public async Task<string> PostReadStringAsync(Uri requestUri, NameValueCollection content, CancellationToken cancellationToken)
		{
			var request = await PostAsync(requestUri, content, cancellationToken).ConfigureAwait(false);
			AssertSuccess(request);

			Stream responseStream = request.GetResponseStream();
			if (responseStream is null)
				return string.Empty;

			CefResponse response = request.Response;
			if (response is null)
				return null;

			Encoding encoding;
			try
			{
				encoding = Encoding.GetEncoding(response.Charset);
			}
			catch (ArgumentException)
			{
				encoding = this.DefaultEncoding;
			}
			return new StreamReader(responseStream, encoding).ReadToEnd();
		}

		/// <summary>
		/// Send a POST request to the specified <see cref="Uri"/> and return the response body
		/// as a string in an asynchronous operation.
		/// </summary>
		/// <param name="requestUri">The <see cref="Uri"/> the request is sent to.</param>
		/// <param name="content">The request content sent to the specified <see cref="Uri"/>.</param>
		/// <param name="cancellationToken">
		/// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
		/// </param>
		/// <returns>The task object representing the asynchronous operation.</returns>
		public async Task<string> PostReadStringAsync(Uri requestUri, IEnumerable<KeyValuePair<string, string>> content, CancellationToken cancellationToken)
		{
			var request = await PostAsync(requestUri, content, cancellationToken).ConfigureAwait(false);
			AssertSuccess(request);

			Stream responseStream = request.GetResponseStream();
			if (responseStream is null)
				return string.Empty;

			CefResponse response = request.Response;
			if (response is null)
				return null;

			Encoding encoding;
			try
			{
				encoding = Encoding.GetEncoding(response.Charset);
			}
			catch (ArgumentException)
			{
				encoding = this.DefaultEncoding;
			}
			return new StreamReader(responseStream, encoding).ReadToEnd();
		}

		/// <summary>
		/// Send a POST request to the specified <see cref="Uri"/> and return the response body
		/// as a <see cref="Stream"/> in an asynchronous operation.
		/// </summary>
		/// <param name="requestUri">The <see cref="Uri"/> the request is sent to.</param>
		/// <param name="referrerUri">
		/// The <see cref="Uri"/> of the referring site for a request. Can be null.
		/// </param>
		/// <param name="referrerPolicy">
		/// The policy for how the Referrer HTTP header value will be sent during request.
		/// </param>
		/// <param name="headers">
		/// A <see cref="NameValueCollection"/> containing header name/value pairs associated with a request. Can be null.
		/// </param>
		/// <param name="content">The request content sent to the specified <see cref="Uri"/>.</param>
		/// <param name="cancellationToken">
		/// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
		/// </param>
		/// <returns>The task object representing the asynchronous operation.</returns>
		public async Task<Stream> PostReadStreamAsync(Uri requestUri, Uri referrerUri, CefReferrerPolicy referrerPolicy, CefPostData content, NameValueCollection headers, CancellationToken cancellationToken)
		{
			var request = await PostAsync(requestUri, referrerUri, referrerPolicy, content, headers, cancellationToken).ConfigureAwait(false);
			AssertSuccess(request);
			return request.GetResponseStream();
		}

		/// <summary>
		/// Send a POST request to the specified <see cref="Uri"/> and return the response body
		/// as a <see cref="Stream"/> in an asynchronous operation.
		/// </summary>
		/// <param name="requestUri">The <see cref="Uri"/> the request is sent to.</param>
		/// <param name="referrerUri">
		/// The <see cref="Uri"/> of the referring site for a request. Can be null.
		/// </param>
		/// <param name="referrerPolicy">
		/// The policy for how the Referrer HTTP header value will be sent during request.
		/// </param>
		/// <param name="headers">
		/// A <see cref="NameValueCollection"/> containing header name/value pairs associated with a request. Can be null.
		/// </param>
		/// <param name="content">The request content sent to the specified <see cref="Uri"/>.</param>
		/// <param name="cancellationToken">
		/// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
		/// </param>
		/// <returns>The task object representing the asynchronous operation.</returns>
		public async Task<Stream> PostReadStreamAsync(Uri requestUri, Uri referrerUri, CefReferrerPolicy referrerPolicy, IEnumerable<KeyValuePair<string, string>> content, NameValueCollection headers, CancellationToken cancellationToken)
		{
			var request = await PostAsync(requestUri, referrerUri, referrerPolicy, content, headers, cancellationToken).ConfigureAwait(false);
			AssertSuccess(request);
			return request.GetResponseStream();
		}

		/// <summary>
		/// Send a POST request to the specified <see cref="Uri"/> and return the response body
		/// as a <see cref="Stream"/> in an asynchronous operation.
		/// </summary>
		/// <param name="requestUri">The <see cref="Uri"/> the request is sent to.</param>
		/// <param name="referrerUri">
		/// The <see cref="Uri"/> of the referring site for a request. Can be null.
		/// </param>
		/// <param name="referrerPolicy">
		/// The policy for how the Referrer HTTP header value will be sent during request.
		/// </param>
		/// <param name="headers">
		/// A <see cref="NameValueCollection"/> containing header name/value pairs associated with a request. Can be null.
		/// </param>
		/// <param name="content">The request content sent to the specified <see cref="Uri"/>.</param>
		/// <param name="cancellationToken">
		/// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
		/// </param>
		/// <returns>The task object representing the asynchronous operation.</returns>
		public async Task<Stream> PostReadStreamAsync(Uri requestUri, Uri referrerUri, CefReferrerPolicy referrerPolicy, NameValueCollection content, NameValueCollection headers, CancellationToken cancellationToken)
		{
			var request = await PostAsync(requestUri, referrerUri, referrerPolicy, content, headers, cancellationToken).ConfigureAwait(false);
			AssertSuccess(request);
			return request.GetResponseStream();
		}

		/// <summary>
		/// Send a POST request to the specified <see cref="Uri"/> and return the response body
		/// as a <see cref="Stream"/> in an asynchronous operation.
		/// </summary>
		/// <param name="requestUri">The <see cref="Uri"/> the request is sent to.</param>
		/// <param name="content">The request content sent to the specified <see cref="Uri"/>.</param>
		/// <param name="cancellationToken">
		/// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
		/// </param>
		/// <returns>The task object representing the asynchronous operation.</returns>
		public async Task<Stream> PostReadStreamAsync(Uri requestUri, NameValueCollection content, CancellationToken cancellationToken)
		{
			var request = await PostAsync(requestUri, content, cancellationToken).ConfigureAwait(false);
			AssertSuccess(request);
			return request.GetResponseStream();
		}

		/// <summary>
		/// Send a POST request to the specified <see cref="Uri"/> and return the response body
		/// as a <see cref="Stream"/> in an asynchronous operation.
		/// </summary>
		/// <param name="requestUri">The <see cref="Uri"/> the request is sent to.</param>
		/// <param name="content">The request content sent to the specified <see cref="Uri"/>.</param>
		/// <param name="cancellationToken">
		/// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
		/// </param>
		/// <returns>The task object representing the asynchronous operation.</returns>
		public async Task<Stream> PostReadStreamAsync(Uri requestUri, IEnumerable<KeyValuePair<string, string>> content, CancellationToken cancellationToken)
		{
			var request = await PostAsync(requestUri, content, cancellationToken).ConfigureAwait(false);
			AssertSuccess(request);
			return request.GetResponseStream();
		}

		/// <summary>
		/// Send a POST request to the specified <see cref="Uri"/> and return the response body
		/// as a <see cref="Byte"/> array in an asynchronous operation.
		/// </summary>
		/// <param name="requestUri">The <see cref="Uri"/> the request is sent to.</param>
		/// <param name="referrerUri">
		/// The <see cref="Uri"/> of the referring site for a request. Can be null.
		/// </param>
		/// <param name="referrerPolicy">
		/// The policy for how the Referrer HTTP header value will be sent during request.
		/// </param>
		/// <param name="headers">
		/// A <see cref="NameValueCollection"/> containing header name/value pairs associated with a request. Can be null.
		/// </param>
		/// <param name="content">The request content sent to the specified <see cref="Uri"/>.</param>
		/// <param name="cancellationToken">
		/// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
		/// </param>
		/// <returns>The task object representing the asynchronous operation.</returns>
		public async Task<byte[]> PostReadDataAsync(Uri requestUri, Uri referrerUri, CefReferrerPolicy referrerPolicy, CefPostData content, NameValueCollection headers, CancellationToken cancellationToken)
		{
			using (Stream stream = await PostReadStreamAsync(requestUri, referrerUri, referrerPolicy, content, headers, cancellationToken).ConfigureAwait(false))
			{
				if (stream is CefNetMemoryStream mem && mem.Capacity == mem.Length)
				{
					return mem.GetBuffer();
				}
				var buffer = new byte[stream.Length];
				if (buffer.Length != await stream.ReadAsync(buffer, 0, buffer.Length).ConfigureAwait(false))
					throw new InvalidOperationException();
				return buffer;
			}
		}

		/// <summary>
		/// Send a POST request to the specified <see cref="Uri"/> and return the response body
		/// as a <see cref="Byte"/> array in an asynchronous operation.
		/// </summary>
		/// <param name="requestUri">The <see cref="Uri"/> the request is sent to.</param>
		/// <param name="referrerUri">
		/// The <see cref="Uri"/> of the referring site for a request. Can be null.
		/// </param>
		/// <param name="referrerPolicy">
		/// The policy for how the Referrer HTTP header value will be sent during request.
		/// </param>
		/// <param name="headers">
		/// A <see cref="NameValueCollection"/> containing header name/value pairs associated with a request. Can be null.
		/// </param>
		/// <param name="content">The request content sent to the specified <see cref="Uri"/>.</param>
		/// <param name="cancellationToken">
		/// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
		/// </param>
		/// <returns>The task object representing the asynchronous operation.</returns>
		public async Task<byte[]> PostReadDataAsync(Uri requestUri, Uri referrerUri, CefReferrerPolicy referrerPolicy, NameValueCollection content, NameValueCollection headers, CancellationToken cancellationToken)
		{
			using (Stream stream = await PostReadStreamAsync(requestUri, referrerUri, referrerPolicy, content, headers, cancellationToken).ConfigureAwait(false))
			{
				if (stream is CefNetMemoryStream mem && mem.Capacity == mem.Length)
				{
					return mem.GetBuffer();
				}
				var buffer = new byte[stream.Length];
				if (buffer.Length != await stream.ReadAsync(buffer, 0, buffer.Length).ConfigureAwait(false))
					throw new InvalidOperationException();
				return buffer;
			}
		}

		/// <summary>
		/// Send a POST request to the specified <see cref="Uri"/> and return the response body
		/// as a <see cref="Byte"/> array in an asynchronous operation.
		/// </summary>
		/// <param name="requestUri">The <see cref="Uri"/> the request is sent to.</param>
		/// <param name="referrerUri">
		/// The <see cref="Uri"/> of the referring site for a request. Can be null.
		/// </param>
		/// <param name="referrerPolicy">
		/// The policy for how the Referrer HTTP header value will be sent during request.
		/// </param>
		/// <param name="headers">
		/// A <see cref="NameValueCollection"/> containing header name/value pairs associated with a request. Can be null.
		/// </param>
		/// <param name="content">The request content sent to the specified <see cref="Uri"/>.</param>
		/// <param name="cancellationToken">
		/// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
		/// </param>
		/// <returns>The task object representing the asynchronous operation.</returns>
		public async Task<byte[]> PostReadDataAsync(Uri requestUri, Uri referrerUri, CefReferrerPolicy referrerPolicy, IEnumerable<KeyValuePair<string, string>> content, NameValueCollection headers, CancellationToken cancellationToken)
		{
			using (Stream stream = await PostReadStreamAsync(requestUri, referrerUri, referrerPolicy, content, headers, cancellationToken).ConfigureAwait(false))
			{
				if (stream is CefNetMemoryStream mem && mem.Capacity == mem.Length)
				{
					return mem.GetBuffer();
				}
				var buffer = new byte[stream.Length];
				if (buffer.Length != await stream.ReadAsync(buffer, 0, buffer.Length).ConfigureAwait(false))
					throw new InvalidOperationException();
				return buffer;
			}
		}

		/// <summary>
		/// Send a POST request to the specified <see cref="Uri"/> and return the response body
		/// as a <see cref="Byte"/> array in an asynchronous operation.
		/// </summary>
		/// <param name="requestUri">The <see cref="Uri"/> the request is sent to.</param>
		/// <param name="content">The request content sent to the specified <see cref="Uri"/>.</param>
		/// <param name="cancellationToken">
		/// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
		/// </param>
		/// <returns>The task object representing the asynchronous operation.</returns>
		public async Task<byte[]> PostReadDataAsync(Uri requestUri, NameValueCollection content, CancellationToken cancellationToken)
		{
			using (Stream stream = await PostReadStreamAsync(requestUri, content, cancellationToken).ConfigureAwait(false))
			{
				if (stream is CefNetMemoryStream mem && mem.Capacity == mem.Length)
				{
					return mem.GetBuffer();
				}
				var buffer = new byte[stream.Length];
				if (buffer.Length != await stream.ReadAsync(buffer, 0, buffer.Length).ConfigureAwait(false))
					throw new InvalidOperationException();
				return buffer;
			}
		}

		/// <summary>
		/// Send a POST request to the specified <see cref="Uri"/> and return the response body
		/// as a <see cref="Byte"/> array in an asynchronous operation.
		/// </summary>
		/// <param name="requestUri">The <see cref="Uri"/> the request is sent to.</param>
		/// <param name="content">The request content sent to the specified <see cref="Uri"/>.</param>
		/// <param name="cancellationToken">
		/// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
		/// </param>
		/// <returns>The task object representing the asynchronous operation.</returns>
		public async Task<byte[]> PostReadDataAsync(Uri requestUri, IEnumerable<KeyValuePair<string, string>> content, CancellationToken cancellationToken)
		{
			using (Stream stream = await PostReadStreamAsync(requestUri, content, cancellationToken).ConfigureAwait(false))
			{
				if (stream is CefNetMemoryStream mem && mem.Capacity == mem.Length)
				{
					return mem.GetBuffer();
				}
				var buffer = new byte[stream.Length];
				if (buffer.Length != await stream.ReadAsync(buffer, 0, buffer.Length).ConfigureAwait(false))
					throw new InvalidOperationException();
				return buffer;
			}
		}

		/// <summary>
		/// Tests that the request completed successfully.
		/// </summary>
		/// <param name="request">The request for testing.</param>
		protected virtual void AssertSuccess(CefNetWebRequest request)
		{
			if (request.RequestStatus != CefUrlRequestStatus.Success)
				throw new WebException("Error: " + request.RequestError.ToString());
		}

		/// <summary>
		/// Called on the IO thread when the browser needs credentials from the user.
		/// </summary>
		/// <param name="proxy">Indicates whether the <paramref name="host"/> is a proxy server.</param>
		/// <param name="host">The hostname.</param>
		/// <param name="port">The port number.</param>
		/// <param name="realm">
		/// The realm is used to describe the protected area or to indicate the scope of protection.
		/// </param>
		/// <param name="scheme">The authentication scheme.</param>
		/// <returns>
		/// The <see cref="NetworkCredential"/> that is associated with the specified URI
		/// and authentication type, or, if no credentials are available, null.
		/// </returns>
		/// <remarks>
		/// This function will only be called for requests initiated from the browser process.
		/// </remarks>
		public virtual Task<NetworkCredential> GetCredentialAsync(bool proxy, string host, int port, string realm, string scheme, CancellationToken cancellationToken)
		{
			return Task.FromResult<NetworkCredential>(null);
		}
	}

}
