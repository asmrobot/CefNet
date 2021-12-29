using CefNet.CApi;

namespace CefNet
{
	public unsafe partial class CefPostDataElement
	{
		/// <summary>
		/// Create a new CefPostDataElement object.
		/// </summary>
		public CefPostDataElement()
			: this(CefNativeApi.cef_post_data_element_create())
		{

		}
	}
}
