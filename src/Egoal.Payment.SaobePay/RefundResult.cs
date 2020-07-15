using Egoal.Extensions;
using System;
using System.Text;

namespace Egoal.Payment.SaobePay
{
    public class RefundResult : ResultBase
    {
        public string refund_fee { get; set; }
        public string end_time { get; set; }
        public string out_trade_no { get; set; }
        public string out_refund_no { get; set; }

        protected override string ToUrl()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(base.ToUrl()).Append("&");
            sb.Append("refund_fee=").AppendWithNull(refund_fee).Append("&");
            sb.Append("end_time=").AppendWithNull(end_time).Append("&");
            sb.Append("out_trade_no=").AppendWithNull(out_trade_no).Append("&");
            sb.Append("out_refund_no=").AppendWithNull(out_refund_no);

            return sb.ToString();
        }

        public RefundOutput ToRefundOutput()
        {
            var output = new RefundOutput();
            output.ListNo = terminal_trace;
            output.RefundId = out_refund_no;
            output.RefundFee = Convert.ToDecimal(refund_fee) / 100;
            output.RefundTime = end_time.ToDateTime(SaobePayOptions.DateTimeFormat);
            output.Success = result_code == "01";
            output.ShouldRetry = return_msg.Contains("支付单处理中");
            output.ErrorMessage = return_msg;

            return output;
        }
    }
}
