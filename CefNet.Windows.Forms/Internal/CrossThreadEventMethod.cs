using System;
using System.Threading;

namespace CefNet.Internal
{
	public sealed class CrossThreadEventMethod<TEventArgs>
		where TEventArgs: class
	{
		private readonly SendOrPostCallback _invoke;
		private readonly Action<TEventArgs> _raiseEventAction;
		private readonly TEventArgs _eventArgs;

		public CrossThreadEventMethod(Action<TEventArgs> raiseEventAction, TEventArgs eventArgs)
		{
			_invoke = new SendOrPostCallback(InvokeImpl);
			_raiseEventAction = raiseEventAction;
			_eventArgs = eventArgs;
		}

		public SendOrPostCallback Invoke
		{
			get { return _invoke; }
		}

		private void InvokeImpl(object nullState)
		{
			_raiseEventAction(_eventArgs);
		}

	}
}
