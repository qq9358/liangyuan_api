namespace Egoal.Payment.Alipay
{
    public class RefundRoyaltyResult
    {
        public decimal refund_amount { get; set; }
        public string royalty_type { get; set; }
        public string result_code { get; set; }
        public string trans_out { get; set; }
        public string trans_out_email { get; set; }
        public string trans_in { get; set; }
        public string trans_in_email { get; set; }
    }
}
