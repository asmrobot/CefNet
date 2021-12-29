using CefNet.CApi;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace CefNet
{
	public partial struct CefSize
	{
		public CefSize(int width, int height)
		{
			_instance = new cef_size_t { width = width, height = height };
		}

		public override string ToString()
		{
			return "{Width=" + Width.ToString(CultureInfo.InvariantCulture) + ",Height=" + Height.ToString(CultureInfo.InvariantCulture) + "}";
		}
	}
}
