namespace Egoal.Payment.Alipay
{
    public class CloseResponse : AlipayResponse
    {
        public string trade_no { get; set; }
        public string out_trade_no { get; set; }

        public ClosePayOutput ToClosePayOutput()
        {
            var output = new ClosePayOutput();
            output.Success = code == "10000";

            return output;
        }
    }
}
