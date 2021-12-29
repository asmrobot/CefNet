using System;

namespace CefNet
{
	/// <summary>
	/// Provides data for a frame event.
	/// </summary>
	public class FrameEventArgs : EventArgs
	{
		/// <summary>
		/// Initializes new instance of the <see cref="FrameEventArgs"/> class.
		/// </summary>
		/// <param name="frame">The frame.</param>
		public FrameEventArgs(CefFrame frame)
		{
			this.Frame = frame;
		}

		/// <summary>
		/// Initializes new instance of the <see cref="FrameEventArgs"/> class.
		/// </summary>
		/// <param name="frame">The frame.</param>
		/// <param name="reattached">true if the <paramref name="frame"/> has been reattached.</param>
		public FrameEventArgs(CefFrame frame, bool reattached)
		{
			this.Frame = frame;
			this.IsReattached = reattached;
		}

		/// <summary>
		/// The frame.
		/// </summary>
		/// <value>The <see cref="CefFrame"/> object.</value>
		public CefFrame Frame { get; }

		/// <summary>
		/// Gets a value that indicates whether the frame has been reattached.
		/// </summary>
		/// <value>true if the <see cref="Frame"/> has been reattached.</value>
		public bool IsReattached { get; }
	}
}
