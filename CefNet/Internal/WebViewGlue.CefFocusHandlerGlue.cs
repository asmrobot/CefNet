using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace CefNet.Internal
{
	public partial class WebViewGlue
	{
		public void CreateOrDestroyFocusGlue()
		{
			if (AvoidOnTakeFocus()
				&& AvoidOnSetFocus()
				&& AvoidOnGotFocus())
			{
				this.FocusGlue = null;
			}
			else if (this.FocusGlue is null)
			{
				this.FocusGlue = new CefFocusHandlerGlue(this);
			}
		}

		[MethodImpl(MethodImplOptions.ForwardRef)]
		internal extern bool AvoidOnTakeFocus();

		/// <summary>
		/// Called when the browser component is about to loose focus. For instance, if focus
		/// was on the last HTML element and the user pressed the TAB key.
		/// </summary>
		/// <param name="browser"></param>
		/// <param name="next">
		/// The |next| will be true if the browser is giving focus to the next component
		/// and false if the browser is giving focus to the previous component.
		/// </param>
		internal protected virtual void OnTakeFocus(CefBrowser browser, bool next)
		{

		}

		[MethodImpl(MethodImplOptions.ForwardRef)]
		internal extern bool AvoidOnSetFocus();

		/// <summary>
		/// Called when the browser component is requesting focus. Return false to allow
		/// the focus to be set or true to cancel setting the focus.
		/// </summary>
		/// <param name="browser"></param>
		/// <param name="source">
		/// Indicates where the focus request is originating from.
		/// </param>
		/// <returns>Return false to allow the focus to be set or true to cancel setting the focus.</returns>
		internal protected virtual bool OnSetFocus(CefBrowser browser, CefFocusSource source)
		{
			return false;
		}

		[MethodImpl(MethodImplOptions.ForwardRef)]
		internal extern bool AvoidOnGotFocus();

		/// <summary>
		/// Called when the browser component has received focus.
		/// </summary>
		/// <param name="browser"></param>
		internal protected virtual void OnGotFocus(CefBrowser browser)
		{

		}
	}
}
