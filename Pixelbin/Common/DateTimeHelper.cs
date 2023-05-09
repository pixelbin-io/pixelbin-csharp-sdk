using System;
using TimeZoneConverter;
using static Pixelbin.Common.Constants;

namespace Pixelbin.Common
{
    internal static class DateTimeHelper
    {
        private static TimeZoneInfo timezone = TZConvert.GetTimeZoneInfo(TIMEZONE);

        public static DateTime GetIstNow()
        {
            return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timezone);
        }
    }
}