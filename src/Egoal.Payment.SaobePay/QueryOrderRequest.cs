namespace Egoal.Payment.SaobePay
{
    public class QueryOrderRequest : RequestBase
    {
        public string pay_trace { get; set; }
        public string pay_time { get; set; }
        public string out_trade_no { get; set; }

        protected override string ToUrl()
        {
            return $"{base.ToUrl()}&out_trade_no={out_trade_no}";
        }
    }
}
