using CefNet.CApi;

namespace CefNet
{
	public unsafe partial class CefPrintSettings
	{
		/// <summary>
		/// Creates a new CefPrintSettings object.
		/// </summary>
		public CefPrintSettings()
			: this(CefNativeApi.cef_print_settings_create())
		{

		}
	}
}
