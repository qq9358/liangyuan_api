using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;

namespace Egoal.Extensions
{
    public static class DateTimeExtensions
    {
        public const string DateFormat = "yyyy-MM-dd";
        public const string DateTimeFormat = "yyyy-MM-dd HH:mm:ss";

        public static DateTime ToDateTime(this string str, string format)
        {
            DateTime.TryParseExact(str, format, Thread.CurrentThread.CurrentCulture, DateTimeStyles.None, out DateTime dateTime);

            return dateTime;
        }

        public static double ToUnixTimestamp(this DateTime target)
        {
            var origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            var diff = target - origin;
            return Math.Floor(diff.TotalSeconds);
        }

        public static DateTime FromUnixTimestamp(this double unixTime)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return epoch.AddSeconds(unixTime);
        }

        public static DateTime ToDayEnd(this DateTime target)
        {
            return target.Date.AddDays(1).AddMilliseconds(-1);
        }

        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            var diff = dt.DayOfWeek - startOfWeek;

            if (diff < 0)
                diff += 7;

            return dt.AddDays(-1 * diff).Date;
        }

        public static IEnumerable<DateTime> DaysOfMonth(int year, int month)
        {
            return Enumerable.Range(0, DateTime.DaysInMonth(year, month))
                .Select(day => new DateTime(year, month, day + 1));
        }

        public static int WeekDayInstanceOfMonth(this DateTime dateTime)
        {
            var y = 0;
            return DaysOfMonth(dateTime.Year, dateTime.Month)
                .Where(date => dateTime.DayOfWeek.Equals(date.DayOfWeek))
                .Select(x => new { n = ++y, date = x })
                .Where(x => x.date.Equals(new DateTime(dateTime.Year, dateTime.Month, dateTime.Day)))
                .Select(x => x.n).FirstOrDefault();
        }

        public static int TotalDaysInMonth(this DateTime dateTime)
        {
            return DaysOfMonth(dateTime.Year, dateTime.Month).Count();
        }

        public static DateTime ToDateTimeUnspecified(this DateTime date)
        {
            if (date.Kind == DateTimeKind.Unspecified)
            {
                return date;
            }

            return new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second, DateTimeKind.Unspecified);
        }

        public static DateTime TrimMilliseconds(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second, date.Kind);
        }

        public static string ToDateTimeString(this DateTime dateTime)
        {
            return dateTime.ToString(DateTimeFormat);
        }

        public static string ToDateString(this DateTime dateTime)
        {
            return dateTime.ToString(DateFormat);
        }

        public static string ToWeekString(this DateTime dateTime)
        {
            string[] weekNames = new[] { "星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六" };

            return weekNames[(int)dateTime.DayOfWeek];
        }

        public static string ToShortWeekString(this DateTime dateTime)
        {
            string[] weekNames = new[] { "周日", "周一", "周二", "周三", "周四", "周五", "周六" };

            return weekNames[(int)dateTime.DayOfWeek];
        }

        public static string ToMonthString(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM");
        }

        public static string ToQuarterString(this DateTime dateTime)
        {
            int quarter = dateTime.Month / 3;
            if (dateTime.Month % 3 != 0)
            {
                quarter++;
            }

            return $"{dateTime.ToString("yyyy")}-{quarter}";
        }

        public static string ToYearString(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy");
        }

        public static string ToHourString(this DateTime dateTime)
        {
            return dateTime.ToString("HH");
        }
    }
}
