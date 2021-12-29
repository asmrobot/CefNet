using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CefNet.Internal;

namespace CefNet.Net
{
	/// <summary>
	/// Represents a HTTP-source that is backed by a <see cref="Stream"/>. 
	/// </summary>
	public sealed class StreamSource : CefResourceHandler
	{
		private sealed class ReadState
		{
			public LimitedReadOnlyStream Stream;
			public CefResourceReadCallback Callback;
			public int BytesToRead;
		}

		private sealed class SkipState
		{
			public byte[] Buffer;
			public CefResourceSkipCallback Callback;
			public long BytesToSkip;
			public long BytesSkipped;
		}

		private readonly Stream _stream;
		private NameValueCollection _headers;
		private CancellationTokenSource _cancellation;
		private int _bufferSize;
		private bool _leaveOpen;

		/// <summary>
		/// Initializes a new instance of the <see cref="StreamSource"/> class.
		/// </summary>
		/// <param name="stream">The source stream.</param>
		/// <param name="mimeType">A <see cref="string"/> value that indicates MIME type.</param>
		/// <param name="charset">The character encoding standard, if any (e.g. utf-8).</param>
		public StreamSource(Stream stream, string mimeType)
			: this(stream, mimeType, null, false)
		{

		}

		/// <summary>
		/// Initializes a new instance of the <see cref="StreamSource"/> class.
		/// </summary>
		/// <param name="stream">The source stream.</param>
		/// <param name="mimeType">A <see cref="string"/> value that indicates MIME type.</param>
		/// <param name="charset">The character encoding standard, if any (e.g. utf-8).</param>
		/// <param name="leaveInnerStreamOpen">
		/// A <see cref="Boolean"/> value that indicates the closure behavior of the source <paramref name="stream"/>
		/// used by the <see cref="StreamSource"/> receiving data. This parameter indicates if the inner stream
		/// is left open.
		/// </param>
		public StreamSource(Stream stream, string mimeType, string charset, bool leaveInnerStreamOpen)
		{
			if (stream is null)
				throw new ArgumentNullException(nameof(stream));
			if (!stream.CanRead)
				throw new ArgumentOutOfRangeException(nameof(stream));
			if (string.IsNullOrWhiteSpace(mimeType))
				throw new ArgumentOutOfRangeException(nameof(mimeType));

			this.MimeType = mimeType;
			this.StatusCode = HttpStatusCode.OK;
			this.BufferSize = -1;
			this.Charset = charset;
			_cancellation = new CancellationTokenSource();
			_leaveOpen = leaveInnerStreamOpen;
			_stream = stream;
		}

		/// <inheritdoc />
		protected override void Dispose(bool disposing)
		{
			if (!_leaveOpen && _stream is not null)
				_stream.Close();
			base.Dispose(disposing);
		}

		/// <summary>
		/// Gets MIME type for the content.
		/// </summary>
		public string MimeType { get; }

		/// <summary>
		/// The character encoding standard.
		/// </summary>
		public string Charset { get; }

		/// <summary>
		/// Gets or sets the status for the response.
		/// </summary>
		public HttpStatusCode StatusCode { get; set; }

		/// <summary>
		/// Gets and sets the buffer size in bytes.
		/// </summary>
		public int BufferSize
		{
			get
			{
				return _bufferSize;
			}
			set
			{
				_bufferSize = value > 0 ? value : 81920;
			}
		}

		/// <summary>
		/// Gets a collection of content headers set on the <see cref="StringSource"/>.
		/// </summary>
		public NameValueCollection Headers
		{
			get
			{
				if (_headers == null)
					_headers = new NameValueCollection();
				return _headers;
			}
		}

		protected internal sealed override void Cancel()
		{
			_cancellation.Cancel();
		}

		protected internal sealed override void GetResponseHeaders(CefResponse response, ref long responseLength, ref string redirectUrl)
		{
			response.Status = (int)this.StatusCode;
			response.MimeType = this.MimeType;
			response.Charset = this.Charset;
			responseLength = _stream.Length;

			if (_headers is not null && _headers.Count > 0)
			{
				using (var map = new CefStringMultimap())
				{
					map.Add(_headers);
					response.SetHeaderMap(map);
				}
			}
		}

		protected internal sealed override bool Open(CefRequest request, ref int handleRequest, CefCallback callback)
		{
			handleRequest = 1;
			return true;
		}

		protected internal sealed unsafe override bool Read(IntPtr dataOut, int bytesToRead, ref int bytesRead, CefResourceReadCallback callback)
		{
			if (bytesToRead < 0)
			{
				bytesRead = (int)CefErrorCode.Failed;
				return false;
			}

#if NET5_0_OR_GREATER || NETSTANDARD2_1
			if (_stream is MemoryStream || _stream is UnmanagedMemoryStream)
			{
				bytesRead = _stream.Read(new Span<byte>((void*)dataOut, bytesToRead));
				return bytesRead != 0;
			}
			else
#endif
			{
				var ums = new UnmanagedMemoryStream((byte*)dataOut, bytesToRead, bytesToRead, FileAccess.Write);
				var limitedStream = new LimitedReadOnlyStream(_stream, bytesToRead);
				try
				{
					limitedStream.CopyToAsync(ums, this.BufferSize, _cancellation.Token).ContinueWith(InvokeReadCallback, new ReadState
					{
						BytesToRead = bytesToRead,
						Stream = limitedStream,
						Callback = callback,
					}, _cancellation.Token, TaskContinuationOptions.ExecuteSynchronously | TaskContinuationOptions.DenyChildAttach, TaskScheduler.Default);
				}
				catch (AccessViolationException) { throw; }
				catch
				{
					bytesRead = (int)CefErrorCode.Failed;
					return false;
				}
				bytesRead = 0;
			}
			return true;
		}

		private static void InvokeReadCallback(Task readTask, object readState)
		{
			var rs = (ReadState)readState;
			switch (readTask.Status)
			{
				case TaskStatus.RanToCompletion:
					rs.Callback.Continue(rs.BytesToRead - rs.Stream.AvailableLimit);
					return;
				case TaskStatus.Canceled:
					rs.Callback.Continue((int)CefErrorCode.Aborted);
					return;
				default:
					rs.Callback.Continue((int)CefErrorCode.Failed);
					return;
			}
		}

		protected internal sealed override bool Skip(long bytesToSkip, ref long bytesSkipped, CefResourceSkipCallback callback)
		{
			if (bytesToSkip < 0)
			{
				bytesSkipped = (long)CefErrorCode.Failed;
				return false;
			}
			if (_stream.CanSeek)
			{
				long currentPos = _stream.Position;
				bytesSkipped = _stream.Seek(bytesToSkip, SeekOrigin.Current) - currentPos;
				return bytesSkipped > 0;
			}
			var buffer = new byte[BufferSize];
			SkipBytesInternal(Task.FromResult(buffer.Length), new SkipState {
				Buffer = new byte[buffer.Length],
				BytesToSkip = bytesToSkip,
				BytesSkipped = -buffer.Length,
				Callback = callback,
			});
			bytesSkipped = 0;
			return true;
		}

		private void SkipBytesInternal(Task<int> readTask, object skipState)
		{
			var ss = (SkipState)skipState;
			switch (readTask.Status)
			{
				case TaskStatus.RanToCompletion:
					ss.BytesSkipped += readTask.Result;
					if (readTask.Result == 0 || ss.BytesToSkip == 0)
					{
						ss.Callback.Continue(ss.BytesSkipped);
						return;
					}
					int bytesToRead = (int)Math.Min(ss.BytesToSkip, ss.Buffer.Length);
					ss.BytesToSkip = ss.BytesToSkip - bytesToRead;
					try
					{
						_stream.ReadAsync(ss.Buffer, 0, bytesToRead)
							.ContinueWith(SkipBytesInternal, skipState, _cancellation.Token, TaskContinuationOptions.ExecuteSynchronously | TaskContinuationOptions.DenyChildAttach, TaskScheduler.Default);
						return;
					}
					catch (AccessViolationException) { throw; }
					catch { }
					break;
				case TaskStatus.Canceled:
					ss.Callback.Continue((long)CefErrorCode.Aborted);
					return;
			}
			ss.Callback.Continue((long)CefErrorCode.Failed);
		}

		/// <inheritdoc />
		public override string ToString()
		{
			return GetType().Name + "+" + _stream.GetType().Name;
		}
	}

}
