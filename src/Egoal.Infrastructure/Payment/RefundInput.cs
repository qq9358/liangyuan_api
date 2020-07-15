using System;

namespace Egoal.Payment
{
    public class RefundInput
    {
        public string ListNo { get; set; }
        public string RefundListNo { get; set; }
        public string TransactionId { get; set; }
        public string SubPayTypeId { get; set; }
        public OnlinePayTradeType TradeType { get; set; }
        public decimal TotalFee { get; set; }
        public decimal RefundFee { get; set; }
        public DateTime PayTime { get; set; }
    }
}
