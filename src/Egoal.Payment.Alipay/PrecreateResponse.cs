namespace Egoal.Payment.Alipay
{
    public class PrecreateResponse : AlipayResponse
    {
        public string out_trade_no { get; set; }
        public string qr_code { get; set; }
    }
}
