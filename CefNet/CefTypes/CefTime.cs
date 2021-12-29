using System;
using System.Collections.Generic;
using System.Text;

namespace CefNet
{
	public partial struct CefTime
	{
		public static CefTime FromDateTime(DateTime t)
		{
			t =  t.ToUniversalTime();
			return new CefTime
			{
				DayOfMonth = t.Day,
				DayOfWeek = (int)t.DayOfWeek,
				Hour = t.Hour,
				Millisecond = t.Millisecond,
				Minute = t.Minute,
				Month = t.Month,
				Second = t.Second,
				Year = t.Year
			};
		}

		public DateTime ToDateTime()
		{
			if (Year >= 1 && Year <= 9999 && Month >= 1 && Month <= 12 && DayOfMonth >= 1
				&& Hour >= 0 && Hour < 24 && Minute >= 0 && Minute < 60 && Second >= 0 && Second < 60)
			{
				return new DateTime(Year, Month, DayOfMonth, Hour, Minute, Second, DateTimeKind.Utc);
			}
			return default;
		}
	}
}
