namespace Egoal.Payment.SaobePay
{
    public class QueryRefundRequest : RequestBase
    {
        public string out_refund_no { get; set; }
        public string pay_trace { get; set; }
        public string pay_time { get; set; }

        protected override string ToUrl()
        {
            return $"{base.ToUrl()}&out_refund_no={out_refund_no}";
        }
    }
}
