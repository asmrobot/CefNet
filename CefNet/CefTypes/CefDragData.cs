using CefNet.CApi;

namespace CefNet
{
	public unsafe partial class CefDragData
	{
		/// <summary>
		/// Creates a new CefDragData object.
		/// </summary>
		public CefDragData()
			: this(CefNativeApi.cef_drag_data_create())
		{

		}
	}
}
