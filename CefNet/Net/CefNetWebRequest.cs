using System;
using System.IO;
using System.Net;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace CefNet.Net
{

	/// <summary>
	/// Provides an URL request that is not associated with a specific browser or frame.
	/// </summary>
	/// <remarks>
	/// This class is only available in the browser process.
	/// </remarks>
	public class CefNetWebRequest : CefUrlRequestClient, INotifyCompletion
	{
		private sealed class RequestOperation
		{
			public CefUrlRequest request;
			public Action continuation;
			public CancellationToken cancellationToken;
		}

		private readonly ICefNetCredentialProvider _authentication;
		private CefRequest _request;
		private CefResponse _response;
		private CefUrlRequestStatus _requestStatus;
		private Stream _stream;
		private RequestOperation _activeOperation;
		private Exception _exception;

		/// <summary>
		/// Initializes a new instance of the <see cref="CefNetWebRequest"/> class.
		/// </summary>
		public CefNetWebRequest()
			: this(null)
		{

		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CefNetWebRequest"/> class.
		/// </summary>
		/// <param name="authentication">
		/// Provides the base authentication interface for retrieving credentials.
		/// </param>
		public CefNetWebRequest(ICefNetCredentialProvider authentication)
		{
			_activeOperation = null;
			_authentication = authentication;
		}

		/// <inheritdoc />
		protected override void Dispose(bool disposing)
		{
			_stream?.Dispose();
			base.Dispose(disposing);
		}

		/// <summary>
		/// Called on the IO thread when the browser needs credentials from the user.
		/// </summary>
		/// <param name="isProxy">Indicates whether the <paramref name="host"/> is a proxy server.</param>
		/// <param name="host">The hostname.</param>
		/// <param name="port">The port number.</param>
		/// <param name="realm">
		/// The realm is used to describe the protected area or to indicate the scope of protection.
		/// </param>
		/// <param name="scheme">The authentication scheme.</param>
		/// <param name="callback">
		/// The callback used to asynchronous continuation/cancellation of authentication requests.
		/// </param>
		/// <returns>
		/// Return true to continue the request and call <see cref="CefAuthCallback.Continue"/>
		/// when the authentication information is available. If the request has an associated
		/// browser/frame then returning false will result in a call to GetAuthCredentials on the
		/// <see cref="CefReadHandler"/> associated with that browser, if any. Otherwise,
		/// returning false will cancel the request immediately. 
		/// </returns>
		/// <remarks>
		/// This function will only be called for requests initiated from the browser process.
		/// </remarks>
		protected internal override bool GetAuthCredentials(bool isProxy, string host, int port, string realm, string scheme, CefAuthCallback callback)
		{
			if (_authentication is null)
				return false;

			RequestOperation op = Volatile.Read(ref _activeOperation);
			if (op is null)
				return false;

			Task<NetworkCredential> getCredentialTask = _authentication.GetCredentialAsync(isProxy, host, port, realm, scheme, op.cancellationToken);
			if (getCredentialTask is null)
				return false;

			getCredentialTask.ContinueWith(t =>
			{
				NetworkCredential credential = (t.Status == TaskStatus.RanToCompletion) ? t.Result : null;
				if (credential is null)
					callback.Cancel();
				else
					callback.Continue(credential.UserName, credential.Password);
			}, op.cancellationToken, TaskContinuationOptions.ExecuteSynchronously | TaskContinuationOptions.DenyChildAttach, TaskScheduler.Default);
			return true;
		}

		/// <summary>
		/// Notifies about a download progress.
		/// </summary>
		/// <param name="request">The associated <see cref="CefUrlRequest"/>.</param>
		/// <param name="current">The number of bytes received up to the call.</param>
		/// <param name="total">The expected total size of the response (or -1 if not determined).</param>
		protected internal override void OnDownloadProgress(CefUrlRequest request, long current, long total)
		{
			try
			{
				if (_stream is CefNetMemoryStream mem && mem.Capacity < total)
				{
					mem.Capacity = (int)total;
				}
			}
			catch (Exception e)
			{
				SetException(e);
				request.Cancel();
			}
		}

		/// <summary>
		/// Called when some part of the response is read. This function will not be called if the
		/// <see cref="CefUrlRequestFlags.NoDownloadData"/> flag is set on the request.
		/// </summary>
		/// <param name="request">The associated <see cref="CefUrlRequest"/>.</param>
		/// <param name="data">The pointer to the buffer that contains the current bytes received since the last call.</param>
		/// <param name="dataLength">The size of the data buffer in bytes.</param>
		protected internal override void OnDownloadData(CefUrlRequest request, IntPtr data, long dataLength)
		{
			try
			{
				if (_stream is null)
				{
					_stream = CreateResourceStream((int)dataLength);
					if (_stream is null)
					{
						request.Cancel();
						return;
					}
				}

				if (_stream is CefNetMemoryStream mem)
				{
					long startPos = _stream.Position;
					long endPos = startPos + dataLength;
					if (endPos <= mem.Capacity)
					{
						mem.SetLength(endPos);
						Marshal.Copy(data, mem.GetBuffer(), (int)startPos, (int)dataLength);
						mem.Position = endPos;
						return;
					}
				}

				var buffer = new byte[dataLength];
				Marshal.Copy(data, buffer, 0, buffer.Length);
				_stream.Write(buffer, 0, buffer.Length);
			}
			catch (Exception e)
			{
				SetException(e);
				request.Cancel();
			}
		}

		/// <summary>
		/// Notifies that the request has completed.
		/// </summary>
		/// <param name="request">The associated <see cref="CefUrlRequest"/>.</param>
		/// <remarks>
		/// Use the <see cref="CefUrlRequest.RequestStatus"/> to determine
		/// if the request was successful or not.
		/// </remarks>
		protected internal override void OnRequestComplete(CefUrlRequest request)
		{
			if (_stream != null)
			{
				try
				{
					_stream.Flush();
					if (_stream.CanSeek)
						_stream.Seek(0, SeekOrigin.Begin);
				}
				catch (IOException ioe)
				{
					SetException(ioe);
				}
			}

			_request = request.Request;
			_response = request.Response;
			_requestStatus = request.RequestStatus;
			this.RequestError = request.RequestError;
			this.ResponseWasCached = request.ResponseWasCached();

			IsCompleted = true;
			RequestOperation op = Interlocked.Exchange(ref _activeOperation, null);
			if (op is null || op.continuation is null)
				return;

			ThreadPool.QueueUserWorkItem(cont => ((Action)cont)(), op.continuation);
		}

		private CefNetWebRequest GetAwaiter()
		{
			return this;
		}

		private bool IsCompleted { get; set; }

		private void GetResult()
		{
			Exception exception = Volatile.Read(ref _exception);
			if (exception is not null)
				throw exception;
		}

		void INotifyCompletion.OnCompleted(Action continuation)
		{
			if (IsCompleted)
				continuation();

			RequestOperation op = Volatile.Read(ref _activeOperation);
			if (op is null)
			{
				IsCompleted = true;
				Interlocked.CompareExchange(ref _exception, new InvalidOperationException(), null);
				continuation();
			}
			else
			{
				op.continuation = continuation;
			}
		}

		/// <summary>
		/// Gets the request object used to create this URL request. The returned
		/// object is read-only and should not be modified.
		/// </summary>
		public CefRequest Request
		{
			get
			{
				RequestOperation op = Volatile.Read(ref _activeOperation);
				return op is null ? _request : op.request.Request;
			}
		}

		/// <summary>
		/// Gets the response, or null if no response information is available.
		/// Response information will only be available after the upload has completed.
		/// The returned object is read-only and should not be modified.
		/// </summary>
		public CefResponse Response
		{
			get
			{
				RequestOperation op = Volatile.Read(ref _activeOperation);
				return op is null ? _response : op.request.Response;
			}
		}

		/// <summary>
		/// Gets the request status.
		/// </summary>
		public CefUrlRequestStatus RequestStatus
		{
			get
			{
				RequestOperation op = Volatile.Read(ref _activeOperation);
				return op is null ? _requestStatus : op.request.RequestStatus;
			}
		}

		/// <summary>
		/// Gets the request error if status is UR_CANCELED or UR_FAILED, or 0
		/// otherwise.
		/// </summary>
		public CefErrorCode RequestError { get; private set; }

		/// <summary>
		/// Gets the value which indicates that the response body was served from the cache.
		/// This includes responses for which revalidation was required.
		/// </summary>
		public bool ResponseWasCached { get; private set; }

		/// <summary>
		/// Sends the request to the server as an asynchronous operation.<para/>
		/// For requests originating from the browser process:
		/// <list type="bullet">
		/// <item>
		/// <description>
		/// It may be intercepted by the client via CefResourceRequestHandler or
		/// CefSchemeHandlerFactory;
		/// </description>
		/// </item>
		/// <item>
		/// <description>
		/// POST data may only contain only a single element of type
		/// <see cref="CefPostDataElementType.File"/> or <see cref="CefPostDataElementType.Bytes"/>.
		/// </description>
		/// </item>
		/// </list>
		/// For requests originating from the render process:
		/// <list type="bullet">
		/// <item>
		/// <description>
		/// It cannot be intercepted by the client so only http(s) and blob schemes
		/// are supported.
		/// </description>
		/// </item>
		/// <item>
		/// <description>
		/// POST data may only contain a single element of type <see cref="CefPostDataElementType.Bytes"/>.
		/// </description>
		/// </item>
		/// </list>
		/// </summary>
		/// <param name="request">
		/// The <see cref="CefRequest"/> object to send. It will be marked as read-only after calling
		/// this function.
		/// </param>
		/// <param name="context">
		/// A request context or null, if <paramref name="context"/> is empty the global
		/// request context will be used. For requests originating from the render process
		/// this parameter must be null.
		/// </param>
		/// <param name="cancellationToken">The cancellation token to cancel operation.</param>
		/// <returns>The task object representing the asynchronous operation.</returns>
		/// <remarks>
		/// This operation will not block. The returned <see cref="Task"/> object will complete once
		/// the entire response including content is read.
		/// </remarks>
		public async Task SendAsync(CefRequest request, CefRequestContext context, CancellationToken cancellationToken)
		{
			if (CefNetApplication.ProcessType != ProcessType.Main && context != null)
				throw new ArgumentOutOfRangeException(nameof(context));

			if (Interlocked.CompareExchange(ref _activeOperation, new RequestOperation(), null) != null)
				throw new InvalidOperationException();

			_request = request;
			_response = null;
			_exception = null;
			if (_stream != null)
			{
				_stream.Dispose();
				_stream = null;
			}
			_requestStatus = CefUrlRequestStatus.Unknown;
			this.RequestError = CefErrorCode.None;
			this.ResponseWasCached = false;
			this.IsCompleted = false;

			try
			{
				_activeOperation.request = await CreateUrlRequest(request, context, cancellationToken).ConfigureAwait(false);
				using (cancellationToken.Register(Abort))
				{
					await this;
				}
			}
			finally
			{
				Interlocked.Exchange(ref _activeOperation, null);
			}

			Exception exception = Volatile.Read(ref _exception);
			if (exception is null)
				return;

			ExceptionDispatchInfo.Capture(exception).Throw();
		}

		private Task<CefUrlRequest> CreateUrlRequest(CefRequest request, CefRequestContext context, CancellationToken cancellationToken)
		{
			_activeOperation.cancellationToken = cancellationToken;

			if (CefApi.CurrentlyOn(CefThreadId.IO))
				return Task.FromResult(new CefUrlRequest(request, this, context));

			var tcs = new TaskCompletionSource<CefUrlRequest>();
			CefNetApi.Post(CefThreadId.IO, () =>
			{
				try
				{
					cancellationToken.ThrowIfCancellationRequested();
					tcs.SetResult(new CefUrlRequest(request, this, context));
				}
				catch (Exception e)
				{
					tcs.SetException(e);
				}
			});
			return tcs.Task;
		}

		/// <summary>
		/// Returns a <see cref="Task"/> that can be used to wait for the request to complete.
		/// </summary>
		/// <returns>The task object representing the asynchronous operation.</returns>
		protected async Task GetWaitTask()
		{
			await this;
		}

		/// <summary>
		/// Cancels the request to a resource.
		/// </summary>
		public void Abort()
		{
			Volatile.Write(ref _exception, new OperationCanceledException());
			RequestOperation op = Interlocked.Exchange(ref _activeOperation, null);
			if (op is null)
				return;
			op.request?.Cancel();
			IsCompleted = true;
			if (op.continuation is null)
				return;
			op.continuation();
		}

		/// <summary>
		/// Gets the stream that is used to read the body of the response from the server.
		/// </summary>
		/// <returns>A <see cref="Stream"/> containing the body of the response.</returns>
		public virtual Stream GetResponseStream()
		{
			if (!IsCompleted)
			{
				RequestOperation op = Volatile.Read(ref _activeOperation);
				if (op is null)
					return null;
				GetWaitTask().Wait(op.cancellationToken);
			}
			return _stream;
		}

		/// <summary>
		/// Marks the current request as failed and binds the specified exception to the request.
		/// </summary>
		/// <param name="exception">The exception to bind to the request.</param>
		protected void SetException(Exception exception)
		{
			if (exception is null)
				throw new ArgumentNullException(nameof(exception));
			Volatile.Write(ref _exception, exception);
		}

		/// <summary>
		/// Creates a <see cref="Stream"/> into which downloaded data will be written.
		/// </summary>
		/// <param name="initialCapacity">The size of the initial portion of data to write to the stream.</param>
		/// <returns>The <see cref="Stream"/> in which the response body will be written.</returns>
		protected virtual Stream CreateResourceStream(int initialCapacity)
		{
			return new CefNetMemoryStream(initialCapacity);
		}

	}

}
