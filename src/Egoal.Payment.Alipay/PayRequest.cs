using System.Collections.Generic;

namespace Egoal.Payment.Alipay
{
    public class PayRequest
    {
        public string out_trade_no { get; set; }
        public string scene { get; set; }
        public string auth_code { get; set; }
        public string product_code { get; set; }
        public string subject { get; set; }
        public string buyer_id { get; set; }
        public string seller_id { get; set; }
        public decimal total_amount { get; set; }
        public string trans_currency { get; set; }
        public string settle_currency { get; set; }
        public decimal? discountable_amount { get; set; }
        public string body { get; set; }
        public List<GoodInfo> goods_detail { get; set; }
        public string operator_id { get; set; }
        public string store_id { get; set; }
        public string terminal_id { get; set; }
        public ExtendParams extend_params { get; set; }
        public string timeout_express { get; set; }
        public string auth_confirm_mode { get; set; }
        public string terminal_params { get; set; }
        public PromoParam promo_params { get; set; }
        public string advance_payment_type { get; set; }
    }
}
