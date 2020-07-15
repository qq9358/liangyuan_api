using System;

namespace Egoal.Trades.Dto
{
    public class StatTradeByPayTypeInput
    {
        public DateTime StartCTime { get; set; }
        public DateTime EndCTime { get; set; }
        public int? TradeTypeId { get; set; }
    }
}
