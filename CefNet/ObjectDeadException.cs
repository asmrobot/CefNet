using System;

namespace CefNet
{
	public sealed class ObjectDeadException
		: Exception
	{
		public ObjectDeadException()
			: base("Can't access dead object.")
		{

		}

		public ObjectDeadException(string message)
			: base(message)
		{

		}

	}
}
