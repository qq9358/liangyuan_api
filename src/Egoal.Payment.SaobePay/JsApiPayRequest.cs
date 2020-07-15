namespace Egoal.Payment.SaobePay
{
    public class JsApiPayRequest : RequestBase
    {
        public string total_fee { get; set; }
        public string open_id { get; set; }
        public string order_body { get; set; }
        public string notify_url { get; set; }
        public string attach { get; set; }
        public string goods_detail { get; set; }

        protected override string ToUrl()
        {
            return $"{base.ToUrl()}&total_fee={total_fee}";
        }
    }
}
