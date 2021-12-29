using CefNet.WinApi;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace CefNet.Internal
{
	public partial class WebViewGlue
	{
		public void CreateOrDestroyKeyboardGlue()
		{
			if (AvoidOnPreKeyEvent() && AvoidOnKeyEvent())
				this.KeyboardGlue = null;
			else if(this.KeyboardGlue is null)
				this.KeyboardGlue = new CefKeyboardHandlerGlue(this);
		}

		[MethodImpl(MethodImplOptions.ForwardRef)]
		internal extern bool AvoidOnPreKeyEvent();

		/// <summary>
		/// Called before a keyboard event is sent to the renderer.
		/// </summary>
		/// <param name="browser"></param>
		/// <param name="event">contains information about the keyboard event.</param>
		/// <param name="osEvent">the operating system event message, if any.</param>
		/// <param name="isKeyboardShortcut">
		/// If the event will be handled in <see cref="OnKeyEvent"/> as a keyboard shortcut
		/// set <paramref name="isKeyboardShortcut"/> to 1 and return false.
		/// </param>
		/// <returns>Return true if the event was handled or false otherwise.</returns>
		internal protected virtual bool OnPreKeyEvent(CefBrowser browser, CefKeyEvent @event, CefEventHandle osEvent, ref int isKeyboardShortcut)
		{
			return false;
		}

		[MethodImpl(MethodImplOptions.ForwardRef)]
		internal extern bool AvoidOnKeyEvent();

		/// <summary>
		/// Called after the renderer and JavaScript in the page has had a chance to handle the event. 
		/// </summary>
		/// <param name="browser"></param>
		/// <param name="event">Contains information about the keyboard event.</param>
		/// <param name="osEvent">The operating system event message, if any.</param>
		/// <returns>Return true if the keyboard event was handled or false otherwise.</returns>
		internal protected virtual bool OnKeyEvent(CefBrowser browser, CefKeyEvent @event, CefEventHandle osEvent)
		{
			return false;
		}
	}
}
