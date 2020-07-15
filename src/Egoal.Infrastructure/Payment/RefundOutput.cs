using System;

namespace Egoal.Payment
{
    public class RefundOutput
    {
        public string ListNo { get; set; }
        public string RefundListNo { get; set; }
        public string RefundId { get; set; }
        public decimal RefundFee { get; set; }
        public DateTime? RefundTime { get; set; }
        public bool Success { get; set; }
        public bool ShouldRetry { get; set; }
        public string ErrorMessage { get; set; }
    }
}
