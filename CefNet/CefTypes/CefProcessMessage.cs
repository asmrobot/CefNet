using CefNet.CApi;
using System;

namespace CefNet
{
	public unsafe partial class CefProcessMessage
	{
		/// <summary>
		/// Create a new CefProcessMessage object with the specified name.
		/// </summary>
		public CefProcessMessage(string name)
			: this(Create(name))
		{

		}

		private static cef_process_message_t* Create(string name)
		{
			fixed (char* s = name)
			{
				cef_string_t cstr = new cef_string_t();
				cstr.Base.str = s;
				if (name != null) cstr.Base.length = unchecked((UIntPtr)name.Length);
				return CefNativeApi.cef_process_message_create(&cstr);
			}
		}
	}
}
