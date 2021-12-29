using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace CefNet.Internal
{
	internal sealed class LimitedReadOnlyStream : Stream
	{
		private readonly Stream _stream;
		private int _limit;

		public LimitedReadOnlyStream(Stream sourceStream, int limit)
		{
			_stream = sourceStream;
			_limit = limit;
		}

		public override bool CanRead => _stream.CanRead;

		public override bool CanSeek => false;

		public override bool CanWrite => false;

		public override long Length
		{
			get { return _stream.Position + _limit; }
		}

		public override long Position
		{
			get
			{
				throw new NotSupportedException();
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		public int AvailableLimit
		{
			get { return _limit; }
		}

		public override void Flush()
		{
			_stream.Flush();
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			count = Math.Min(count, _limit);
			if (count == 0)
				return 0;
			count = _stream.Read(buffer, offset, count);
			_limit = _limit - count;
			return count;
		}

		public override async Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
		{
			count = Math.Min(count, _limit);
			count = await _stream.ReadAsync(buffer, offset, count, cancellationToken).ConfigureAwait(false);
			_limit = _limit - count;
			return count;
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException();
		}

		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException();
		}

		public override void Close() { }

	}
}
