using CefNet.CApi;

namespace CefNet
{
	public unsafe partial class CefV8StackTrace
	{
		/// <summary>
		/// Returns the stack trace for the currently active context. 
		/// </summary>
		/// <param name="frameLimit">
		/// The maximum number of frames that will be captured.
		/// </param>
		public CefV8StackTrace GetCurrent(int frameLimit)
		{
			return CefV8StackTrace.Wrap(CefV8StackTrace.Create, CefNativeApi.cef_v8stack_trace_get_current(frameLimit));
		}

	}
}

