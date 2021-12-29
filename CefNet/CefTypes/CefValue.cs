using CefNet.CApi;

namespace CefNet
{
	public unsafe partial class CefValue
	{
		/// <summary>
		/// Creates a new object.
		/// </summary>
		public CefValue()
			: this(CefNativeApi.cef_value_create())
		{

		}

		public bool SetBinary(byte[] buffer)
		{
			using (var v = new CefBinaryValue(buffer))
			{
				return SetBinary(v);
			}
		}
	}
}
