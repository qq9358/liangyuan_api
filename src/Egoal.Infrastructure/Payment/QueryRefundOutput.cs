using System;

namespace Egoal.Payment
{
    public class QueryRefundOutput
    {
        public string ListNo { get; set; }
        public string RefundListNo { get; set; }
        public string RefundId { get; set; }
        public decimal RefundFee { get; set; }
        public string RefundStatus { get; set; }
        public DateTime RefundTime { get; set; }
        public string RefundRecvAccount { get; set; }
        public string ErrorMessage { get; set; }
        public bool Success { get; set; }
        public bool ShouldRetry { get; set; }
        public bool IsExist { get; set; }
    }
}
