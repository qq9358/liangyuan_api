namespace Egoal.Payment.SaobePay
{
    public class RefundRequest : RequestBase
    {
        public string refund_fee { get; set; }
        public string out_trade_no { get; set; }
        public string pay_trace { get; set; }
        public string pay_time { get; set; }
        public string auth_code { get; set; }

        protected override string ToUrl()
        {
            return $"{base.ToUrl()}&refund_fee={refund_fee}&out_trade_no={out_trade_no}";
        }
    }
}
