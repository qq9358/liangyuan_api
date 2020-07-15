using System;

namespace Egoal.Tickets.Dto
{
    public class StatTicketSaleInput
    {
        public DateTime StartCTime { get; set; }
        public DateTime EndCTime { get; set; }
        public int? TicketTypeId { get; set; }
        public TicketSaleStatType StatType { get; set; }
    }

    public enum TicketSaleStatType
    {
        日期 = 1,
        星期 = 2,
        月份 = 3,
        季度 = 4,
        年份 = 5,
        票类 = 6,
        销售渠道 = 7
    }
}
