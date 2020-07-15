namespace Egoal.Payment.Alipay
{
    public class VoucherDetail
    {
        public string id { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public decimal amount { get; set; }
        public decimal? merchant_contribute { get; set; }
        public decimal? other_contribute { get; set; }
        public string memo { get; set; }
        public string template_id { get; set; }
        public decimal? purchase_buyer_contribute { get; set; }
        public decimal? purchase_merchant_contribute { get; set; }
        public decimal? purchase_ant_contribute { get; set; }
    }
}
