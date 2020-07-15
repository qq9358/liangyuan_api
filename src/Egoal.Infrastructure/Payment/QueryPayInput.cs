using System;

namespace Egoal.Payment
{
    public class QueryPayInput
    {
        public string ListNo { get; set; }
        public string TransactionId { get; set; }
        public string SubPayTypeId { get; set; }
        public OnlinePayTradeType TradeType { get; set; }
        public DateTime PayTime { get; set; }
    }
}
