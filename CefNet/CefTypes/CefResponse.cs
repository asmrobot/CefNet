using CefNet.CApi;

namespace CefNet
{
	public unsafe partial class CefResponse
	{
		/// <summary>
		/// Creates a new CefResponse object.
		/// </summary>
		public CefResponse()
			: this(CefNativeApi.cef_response_create())
		{

		}
	}
}
