using System;

namespace CefNet.Internal
{
	internal sealed class CefCompletionCallbackImpl : CefCompletionCallback
	{
		private readonly Func<int, bool> _complete;

		public CefCompletionCallbackImpl(Func<int, bool> complete)
		{
			_complete = complete;
		}

		protected internal override void OnComplete()
		{
			_complete(0);
		}
	}
}
