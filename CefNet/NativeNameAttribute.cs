using System;
using System.Collections.Generic;
using System.Text;

namespace CefNet
{
	[AttributeUsage(System.AttributeTargets.Method, Inherited = false)]
	internal sealed class NativeNameAttribute : Attribute
	{
		public NativeNameAttribute(string name) { }
	}
}
