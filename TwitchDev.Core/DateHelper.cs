using System;

namespace TwitchDev.Core
{
    public static class DateHelper
    {
        public static double ToUnixTimestamp(this DateTime value)
        {
            return Convert.ToDouble(value.Subtract(new DateTime(1970, 1, 1)).TotalSeconds);
        }
    }
}
