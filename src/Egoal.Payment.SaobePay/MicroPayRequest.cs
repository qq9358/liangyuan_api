namespace Egoal.Payment.SaobePay
{
    public class MicroPayRequest : RequestBase
    {
        public string auth_no { get; set; }
        public string total_fee { get; set; }
        public string order_body { get; set; }
        public string attach { get; set; }
        public string goods_detail { get; set; }

        protected override string ToUrl()
        {
            return $"{base.ToUrl()}&auth_no={auth_no}&total_fee={total_fee}";
        }
    }
}
