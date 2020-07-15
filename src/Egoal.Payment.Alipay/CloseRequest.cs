namespace Egoal.Payment.Alipay
{
    public class CloseRequest
    {
        public string trade_no { get; set; }
        public string out_trade_no { get; set; }
        public string operator_id { get; set; }
    }
}
