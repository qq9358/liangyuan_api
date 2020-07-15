using System.Collections.Generic;

namespace Egoal.Payment.Alipay
{
    public class PrecreateRequest
    {
        public string out_trade_no { get; set; }
        public string seller_id { get; set; }
        public decimal total_amount { get; set; }
        public decimal? discountable_amount { get; set; }
        public string subject { get; set; }
        public List<GoodInfo> goods_detail { get; set; }
        public string body { get; set; }
        public string product_code { get; set; }
        public string operator_id { get; set; }
        public string store_id { get; set; }
        public string disable_pay_channels { get; set; }
        public string enable_pay_channels { get; set; }
        public string terminal_id { get; set; }
        public ExtendParams extend_params { get; set; }
        public string timeout_express { get; set; }
        public SettleInfo settle_info { get; set; }
        public string merchant_order_no { get; set; }
        public BusinessParams business_params { get; set; }
        public string qr_code_timeout_express { get; set; }
    }
}
