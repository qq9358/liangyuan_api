using System;

namespace Egoal.Trades.Dto
{
    public class StatPayDetailInput
    {
        public DateTime StartCTime { get; set; }
        public DateTime EndCTime { get; set; }
        public PayDetailStatType StatType { get; set; }
    }

    public enum PayDetailStatType
    {
        按日期 = 1,
        按景点 = 2,
        按售票点 = 3,
        按收银员 = 4
    }
}
