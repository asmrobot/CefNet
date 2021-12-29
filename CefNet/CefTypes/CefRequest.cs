using CefNet.CApi;

namespace CefNet
{
	public unsafe partial class CefRequest
	{
		/// <summary>
		/// Create a new CefRequest object.
		/// </summary>
		public CefRequest()
			: this(CefNativeApi.cef_request_create())
		{

		}
	}
}
