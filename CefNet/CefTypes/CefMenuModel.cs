using System;
using System.Runtime.CompilerServices;
using CefNet.CApi;

namespace CefNet
{
	public unsafe partial class CefMenuModel
	{
		/// <summary>
		/// Create a new MenuModel with the specified |delegate|.
		/// </summary>
		public CefMenuModel(CefMenuModelDelegate @delegate)
			: this(CefNativeApi.cef_menu_model_create((@delegate ?? throw new ArgumentNullException("delegate")).GetNativeInstance()))
		{

		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool AddItem(CefMenuId id, string label)
		{
			return AddItem((int)id, label);
		}

	}
}
