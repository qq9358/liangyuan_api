using System;

namespace Egoal.Trades.Dto
{
    public class StatTradeInput
    {
        public DateTime StartCTime { get; set; }
        public DateTime EndCTime { get; set; }
        public TradeStatType StatType { get; set; }
    }

    public enum TradeStatType
    {
        日期 = 1,
        星期 = 2,
        月份 = 3,
        季度 = 4,
        年份 = 5,
        交易类型 = 6
    }
}
