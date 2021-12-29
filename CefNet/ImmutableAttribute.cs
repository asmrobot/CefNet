using System;
using System.Collections.Generic;
using System.Text;

namespace CefNet
{
	[AttributeUsage(System.AttributeTargets.Parameter, Inherited = false)]
	internal sealed class ImmutableAttribute : Attribute
	{
	}
}
