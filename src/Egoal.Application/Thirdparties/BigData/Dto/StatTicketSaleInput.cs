using System;

namespace Egoal.Thirdparties.BigData.Dto
{
    public class StatTicketSaleInput : InputBase
    {
        public DateTime start_date { get; set; }
        public DateTime end_date { get; set; }
        public int stat_type { get; set; }

        public override void Validate()
        {
            if (start_date > end_date)
            {
                throw new TmsException("开始时间不能大于截止时间");
            }
            if ((end_date - start_date).TotalDays > MaxDateRange)
            {
                throw new TmsException($"时间跨度不能超过{MaxDateRange}天");
            }
        }
    }
}
