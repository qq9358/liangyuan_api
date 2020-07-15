namespace Egoal.Payment.Alipay
{
    public class QueryRefundRequest
    {
        public string trade_no { get; set; }
        public string out_trade_no { get; set; }
        public string out_request_no { get; set; }
        public string org_pid { get; set; }
    }
}
