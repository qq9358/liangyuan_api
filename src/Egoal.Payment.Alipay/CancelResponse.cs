using System;

namespace Egoal.Payment.Alipay
{
    public class CancelResponse : AlipayResponse
    {
        public string trade_no { get; set; }
        public string out_trade_no { get; set; }
        public string retry_flag { get; set; }
        public string action { get; set; }
        public DateTime? gmt_refund_pay { get; set; }
        public string refund_settlement_id { get; set; }

        public ReversePayOutput ToReversePayOutput()
        {
            var output = new ReversePayOutput();
            output.Success = code == "10000";
            output.ShouldRetry = retry_flag == "Y" || sub_code?.ToUpper() == "AQC.SYSTEM_ERROR" || sub_code?.ToUpper() == "ACQ.SELLER_BALANCE_NOT_ENOUGH";
            output.ErrorMessage = sub_msg ?? msg;

            return output;
        }
    }
}
