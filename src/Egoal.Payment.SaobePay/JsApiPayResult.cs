namespace Egoal.Payment.SaobePay
{
    public class JsApiPayResult : ResultBase
    {
        public string total_fee { get; set; }
        public string out_trade_no { get; set; }
        public string appId { get; set; }
        public string timeStamp { get; set; }
        public string nonceStr { get; set; }
        public string package_str { get; set; }
        public string signType { get; set; }
        public string paySign { get; set; }
        public string ali_trade_no { get; set; }
        public string token_id { get; set; }

        protected override string ToUrl()
        {
            return $"{base.ToUrl()}&total_fee={total_fee}&out_trade_no={out_trade_no}";
        }
    }
}
