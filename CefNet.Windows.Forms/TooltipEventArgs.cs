using System;

#if MODERNFORMS
namespace CefNet.Modern.Forms
#else
namespace CefNet.Windows.Forms
#endif
{
	/// <summary>
	/// Provides event information for events that occur when a tooltip opens or closes.
	/// </summary>
	public sealed class TooltipEventArgs : EventArgs
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="TooltipEventArgs"/> class.
		/// </summary>
		/// <param name="text">The text for the ToolTip window.</param>
		public TooltipEventArgs(string text)
		{
			this.Text = text;
		}

		/// <summary>
		/// Gets or sets a text for the ToolTip window.
		/// </summary>
		public string Text { get; }
	}
}
