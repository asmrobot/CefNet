using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CefNet.Internal;

namespace CefNet
{
	internal sealed class DevToolsProtocolClient : CefDevToolsMessageObserver
	{
		private readonly IChromiumWebViewPrivate _webview;
		private readonly CefRegistration _registration;
		private Dictionary<int, DevToolsCallCompletionSource> _waitTasks = new Dictionary<int, DevToolsCallCompletionSource>();
		private int _lastMessageId;

		public DevToolsProtocolClient(IChromiumWebViewPrivate webview)
		{
			_webview = webview;
			_registration = ((IChromiumWebView)webview).BrowserObject.Host.AddDevToolsMessageObserver(this);
		}

		private object SyncRoot
		{
			get { return _waitTasks; }
		}

		public void Close()
		{
			_registration.Dispose();
		}

		public int IncrementMessageId()
		{
			int id;
			lock (SyncRoot)
			{
				id = ++_lastMessageId;
			}
			return id;
		}

		public void UpdateLastMessageId(int messageId)
		{
			if (messageId == 0)
				throw new InvalidOperationException();

			lock (SyncRoot)
			{
				if (_lastMessageId < messageId)
					_lastMessageId = messageId;
			}
		}

		/// <summary>
		/// Returns Task.
		/// </summary>
		/// <param name="messageId">Message ID.</param>
		/// <param name="convert">By default, the result is copied to a byte array.</param>
		/// <returns></returns>
		public Task<DevToolsMethodResult> WaitForMessageAsync(int messageId, ConvertUtf8BufferToObjectDelegate convert)
		{
			return RemoveOrAddTaskSource(messageId, convert).Task;
		}

		private DevToolsCallCompletionSource RemoveOrAddTaskSource(int messageId, ConvertUtf8BufferToObjectDelegate convert)
		{
			DevToolsCallCompletionSource m;
			lock (SyncRoot)
			{
				if (!_waitTasks.Remove(messageId, out m))
				{
					m = new DevToolsCallCompletionSource(convert);
					_waitTasks.Add(messageId, m);
				}
			}
			return m;
		}

		protected internal unsafe override void OnDevToolsEvent(CefBrowser browser, string method, IntPtr @params, long paramsSize)
		{
			_webview.RaiseDevToolsEventAvailable(new DevToolsProtocolEventAvailableEventArgs(method, @params != IntPtr.Zero ? new string((sbyte*)@params, 0, (int)paramsSize, Encoding.UTF8) : null));
		}

		protected internal override bool OnDevToolsMessage(CefBrowser browser, IntPtr message, long messageSize)
		{
			return false;
		}

		/// <summary>
		/// Method that will be called after attempted execution of a DevTools protocol
		/// function.
		/// </summary>
		/// <param name="browser">The originating browser instance.</param>
		/// <param name="messageId">The &quot;id&quot; value that identifies the originating function call message.</param>
		/// <param name="success">true, if the function succeeded; otherwise false.</param>
		/// <param name="result">
		/// The UTF8-encoded JSON dictionary value (which may be NULL). <paramref name="result"/>
		/// is only valid for the scope of this callback and should be copied if necessary.See the
		/// <see cref="OnDevToolsMessage"/> documentation for additional details on
		/// <paramref name="result"/> contents.
		/// </param>
		/// <param name="resultSize">The size of the <paramref name="result"/> buffer.</param>
		protected internal override void OnDevToolsMethodResult(CefBrowser browser, int messageId, bool success, IntPtr result, long resultSize)
		{
			RemoveOrAddTaskSource(messageId, null).SaveResult(messageId, success, result, (int)resultSize);
		}

	}
}
