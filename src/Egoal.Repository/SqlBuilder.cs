using Egoal.Extensions;
using System;
using System.Text;

namespace Egoal
{
    public static class SqlBuilder
    {
        public static string BuildTimeStatTypeColumns(string statType, DateTime startTime, DateTime endTime)
        {
            switch (statType.ToUpper())
            {
                case "CWEEK":
                    {
                        return "[星期一],[星期二],[星期三],[星期四],[星期五],[星期六],[星期日]";
                    }
                case "CMONTH":
                    {
                        StringBuilder columns = new StringBuilder();
                        for (var time = new DateTime(startTime.Year, startTime.Month, 1); time <= endTime; time = time.AddMonths(1))
                        {
                            columns.Append("[").Append(time.ToMonthString()).Append("],");
                        }

                        return columns.ToString().TrimEnd(',');
                    }
                case "CQUARTER":
                    {
                        StringBuilder columns = new StringBuilder();
                        for (var time = new DateTime(startTime.Year, startTime.Month, 1); time <= endTime; time = time.AddMonths(3))
                        {
                            columns.Append("[").Append(time.ToQuarterString()).Append("],");
                        }

                        return columns.ToString().TrimEnd(',');
                    }
                case "CYEAR":
                    {
                        StringBuilder columns = new StringBuilder();
                        for (var time = new DateTime(startTime.Year, startTime.Month, 1); time <= endTime; time = time.AddYears(1))
                        {
                            columns.Append("[").Append(time.ToYearString()).Append("],");
                        }

                        return columns.ToString().TrimEnd(',');
                    }
                default:
                    {
                        throw new TmsException("统计类型不支持");
                    }
            }
        }
    }
}
