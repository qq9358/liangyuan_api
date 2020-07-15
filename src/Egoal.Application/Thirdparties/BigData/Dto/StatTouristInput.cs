using System;

namespace Egoal.Thirdparties.BigData.Dto
{
    public class StatTouristInput : InputBase
    {
        public DateTime start_date { get; set; }
        public DateTime end_date { get; set; }
        public bool? stat_by_area { get; set; }
        public bool? stat_by_sex { get; set; }
        public bool? stat_by_nation { get; set; }
        public bool? stat_by_age { get; set; }

        public override void Validate()
        {
            if (start_date > end_date)
            {
                throw new TmsException("起始日期不能大于截止日期");
            }
            if ((end_date - start_date).TotalDays > MaxDateRange)
            {
                throw new TmsException($"时间跨度不能超过{MaxDateRange}天");
            }
        }
    }
}
