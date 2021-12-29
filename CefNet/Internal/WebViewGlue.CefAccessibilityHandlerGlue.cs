using System;
using System.Collections.Generic;
using System.Text;
using CefNet.WinApi;

namespace CefNet.Internal
{
	public partial class WebViewGlue
	{
		internal bool AvoidOnAccessibilityTreeChange()
		{
			return false;
		}

		internal void OnAccessibilityTreeChange(CefValue value)
		{

		}

		internal bool AvoidOnAccessibilityLocationChange()
		{
			return false;
		}

		internal void OnAccessibilityLocationChange(CefValue value)
		{

		}

	}
}
