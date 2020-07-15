namespace Egoal.Payment.Alipay
{
    public class TradeFundBill
    {
        public string fund_channel { get; set; }
        public string bank_code { get; set; }
        public decimal amount { get; set; }
        public decimal? real_amount { get; set; }
        public string fund_type { get; set; }
    }
}
