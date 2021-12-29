namespace CefNet.WinApi
{
	/// <summary>
	/// Determines the function's return value if the window does not intersect any display monitor.
	/// </summary>
	public enum MonitorFlag
	{
		/// <summary>Returns NULL.</summary>
		MONITOR_DEFAULTTONULL = 0,
		/// <summary>Returns a handle to the primary display monitor.</summary>
		MONITOR_DEFAULTTOPRIMARY = 1,
		/// <summary>Returns a handle to the display monitor that is nearest to the window.</summary>
		MONITOR_DEFAULTTONEAREST = 2
	}
}
