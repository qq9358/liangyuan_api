namespace Egoal.Payment.Alipay
{
    public class OpenApiRoyaltyDetailInfoPojo
    {
        public string royalty_type { get; set; }
        public string trans_out { get; set; }
        public string trans_out_type { get; set; }
        public string trans_in_type { get; set; }
        public string trans_in { get; set; }
        public decimal? amount { get; set; }
        public int? amount_percentage { get; set; }
        public string desc { get; set; }
    }
}
