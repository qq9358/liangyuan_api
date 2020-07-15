using System;

namespace Egoal.Thirdparties.BigData.Dto
{
    public class QueryTicketCheckInput : InputBase
    {
        public DateTime start_date { get; set; }
        public DateTime end_date { get; set; }
        public int page_size { get { return 50; } }
        public int page_index { get; set; }

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
            if (page_index <= 0)
            {
                throw new TmsException("page_index不正确");
            }
        }
    }
}
