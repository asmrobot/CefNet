using CefNet.CApi;

namespace CefNet
{
	public unsafe partial class CefWaitableEvent
	{
		/// <summary>
		/// Create a new waitable event.
		/// </summary>
		/// <param name="automaticReset">
		/// If |automaticReset| is True then the event state is automatically reset
		/// to un-signaled after a single waiting thread has been released; otherwise,
		/// the state remains signaled until reset() is called manually.
		/// </param>
		/// <param name="initiallySignaled">
		/// If |initiallySignaled| is True then the event will start in the signaled state.
		/// </param>
		public CefWaitableEvent(bool automaticReset, bool initiallySignaled)
			: this(CefNativeApi.cef_waitable_event_create(automaticReset ? 1 : 0, initiallySignaled ? 1 : 0))
		{

		}

	}
}
