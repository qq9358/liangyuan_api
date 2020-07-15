using System;

namespace Egoal.Payment
{
    public class QueryRefundInput
    {
        public string ListNo { get; set; }
        public string TransactionId { get; set; }
        public string RefundListNo { get; set; }
        public string RefundId { get; set; }
        public string SubPayTypeId { get; set; }
        public OnlinePayTradeType TradeType { get; set; }
        public decimal RefundFee { get; set; }
        public DateTime PayTime { get; set; }
    }
}
