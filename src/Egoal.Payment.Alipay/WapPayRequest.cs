namespace Egoal.Payment.Alipay
{
    public class WapPayRequest
    {
        public string body { get; set; }
        public string subject { get; set; }
        public string out_trade_no { get; set; }
        public string timeout_express { get; set; }
        public string time_expire { get; set; }
        public decimal total_amount { get; set; }
        public string auth_token { get; set; }
        public string product_code { get; set; }
        public string goods_type { get; set; }
        public string passback_params { get; set; }
        public string promo_params { get; set; }
        public string extend_params { get; set; }
        public string enable_pay_channels { get; set; }
        public string disable_pay_channels { get; set; }
        public string store_id { get; set; }
        public string quit_url { get; set; }
        public ExtUserInfo ext_user_info { get; set; }
    }
}
