using System;
using System.Collections.Generic;
using System.Text;

namespace CefNet.CApi
{
#pragma warning disable CS1591
	public unsafe partial struct cef_time_t
#pragma warning restore CS1591
	{
		/// <summary>
		/// Returns the hash code for this instance.
		/// </summary>
		/// <returns>A 32-bit signed integer hash code.</returns>
		public override int GetHashCode()
		{
			return new DateTime(year, month, day_of_month, hour, minute, second, millisecond, DateTimeKind.Utc).GetHashCode();
		}
	}
}
