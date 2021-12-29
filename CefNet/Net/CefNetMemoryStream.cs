using System;
using System.IO;

namespace CefNet.Net
{
	/// <summary>
	/// Creates a stream whose backing store is memory.
	/// </summary>
	public sealed class CefNetMemoryStream : Stream
	{
		private byte[] _buffer;
		private long _length;
		private long _position;

		/// <summary>
		/// Initializes a new instance of the <see cref="CefNetMemoryStream"/> class.
		/// </summary>
		/// <param name="capacity">The initial size of the internal array in bytes.</param>
		public CefNetMemoryStream(int capacity)
		{
			_buffer = new byte[capacity];
		}

		/// <summary>
		/// Gets or sets the number of bytes allocated for this stream.
		/// </summary>
		/// <value>The length of the buffer for the stream.</value>
		public long Capacity
		{
			get
			{
				return _buffer.Length;
			}
			set
			{
				if (value < _length)
					throw new ArgumentOutOfRangeException(nameof(value));

				if (value == _length)
					return;

				var buffer = new byte[value];
				Array.Copy(_buffer, buffer, _length);
				_buffer = buffer;
			}
		}

		/// <inheritdoc />
		public override bool CanRead
		{
			get { return true; }
		}

		/// <inheritdoc />
		public override bool CanSeek
		{
			get { return true; }
		}

		/// <inheritdoc />
		public override bool CanWrite
		{
			get { return true; }
		}

		/// <inheritdoc />
		public override long Length
		{
			get { return _length; }
		}

		/// <inheritdoc />
		public override long Position
		{
			get { return _position;  }
			set { _position = value; }
		}

		/// <inheritdoc />
		public override void Flush() { }

		/// <inheritdoc />
		public override int Read(byte[] buffer, int offset, int count)
		{
			count = Math.Min(count, (int)(_length - _position));
			Array.Copy(_buffer, _position, buffer, offset, count);
			_position += count;
			return count;
		}

		/// <inheritdoc />
		public override long Seek(long offset, SeekOrigin origin)
		{
			if (origin == SeekOrigin.Begin)
			{
				if (offset < 0 || offset > _length)
					throw new ArgumentOutOfRangeException(nameof(offset));
				_position = offset;
				return _position;
			}
			if (origin == SeekOrigin.End)
			{
				if (offset > 0)
					throw new ArgumentOutOfRangeException(nameof(offset));
				long pos = _length + offset;
				if (pos < 0)
					throw new ArgumentOutOfRangeException(nameof(offset));
				_position = pos;
				return _position;
			}
			if (origin == SeekOrigin.Begin)
			{
				long pos = _position + offset;
				if (pos < 0 || pos > _length)
					throw new ArgumentOutOfRangeException(nameof(offset));
				_position = pos;
				return _position;
			}
			throw new ArgumentOutOfRangeException(nameof(origin));
		}

		/// <inheritdoc />
		public override void SetLength(long value)
		{
			if (value < 0)
				throw new ArgumentOutOfRangeException(nameof(value));

			if (value > _buffer.Length)
			{
				var buffer = new byte[value];
				Array.Copy(_buffer, buffer, _buffer.Length);
				_buffer = buffer;
			}
			_length = value;
		}

		/// <inheritdoc />
		public override void Write(byte[] buffer, int offset, int count)
		{
			long target = _position + count;
			if (target > this.Capacity)
				this.Capacity = target;
			Array.Copy(buffer, offset, _buffer, _position, count);
			_position = target;
			_length = target;
		}

		/// <summary>
		/// Returns the array of unsigned bytes from which this stream was created.
		/// </summary>
		/// <returns>
		/// The underlying byte array.
		/// </returns>
		public byte[] GetBuffer()
		{
			return _buffer;
		}

	}
}
