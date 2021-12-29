using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CefNet
{
	/// <summary>
	/// Encapsulates a method that converts an UTF8-encoded JSON dictionary value to an object of type <see cref="T"/>.
	/// </summary>
	/// <typeparam name="T">The type of object to return.</typeparam>
	/// <param name="buffer">
	/// The pointer to a buffer containing an UTF8-encoded JSON dictionary value.
	/// This pointer is only valid for the scope of this callback and should be copied if necessary.
	/// </param>
	/// <param name="size">The size of the <paramref name="buffer"/> .</param>
	/// <returns>An object whose type is <see cref="T"/> and whose value is equivalent to the data in the buffer.</returns>
	public delegate T ConvertUtf8BufferToTypeDelegate<T>(IntPtr buffer, int size) where T: class;

	internal delegate object ConvertUtf8BufferToObjectDelegate(IntPtr buffer, int size);

	internal sealed class DevToolsCallCompletionSource : TaskCompletionSource<DevToolsMethodResult>
	{
		private readonly ConvertUtf8BufferToObjectDelegate _convertDelegate;

		public DevToolsCallCompletionSource(ConvertUtf8BufferToObjectDelegate convertDelegate)
#if !NET45
			: base(TaskCreationOptions.RunContinuationsAsynchronously)
#endif
		{
			_convertDelegate = convertDelegate;
		}

		public void SaveResult(int messageId, bool success, IntPtr response, int resultSize)
		{
			object resultObj;
			if (success && response != IntPtr.Zero)
			{
				try
				{
					if (_convertDelegate != null)
					{
						resultObj = _convertDelegate(response, resultSize);
					}
					else
					{
						var buffer = new byte[resultSize];
						Marshal.Copy(response, buffer, 0, resultSize);
						resultObj = buffer;
					}
				}
				catch (Exception e)
				{
					SetException(e);
					return;
				}
			}
			else
			{
				resultObj = null;
			}
			SetResult(new DevToolsMethodResult(messageId, resultObj, success));
		}

#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private new void SetResult(DevToolsMethodResult result)
		{
			ThreadPool.QueueUserWorkItem(SetResultCallback, result);
		}

		private void SetResultCallback(object result)
		{
			base.SetResult((DevToolsMethodResult)result);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private new void SetException(Exception exception)
		{
			ThreadPool.QueueUserWorkItem(SetExceptionCallback, exception);
		}

		private void SetExceptionCallback(object exception)
		{
			base.SetException((Exception)exception);
		}
#endif

		internal static unsafe object ConvertUtf8BufferToJsonString(IntPtr buffer, int size)
		{
			return new string((sbyte*)buffer, 0, size, Encoding.UTF8);
		}

	}
}
