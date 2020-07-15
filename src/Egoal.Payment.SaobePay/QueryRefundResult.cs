using Egoal.Extensions;
using System.Text;

namespace Egoal.Payment.SaobePay
{
    public class QueryRefundResult : ResultBase
    {
        public string refund_fee { get; set; }
        public string end_time { get; set; }
        public string out_refund_no { get; set; }
        public string out_trade_no { get; set; }
        public string trade_state { get; set; }
        public string channel_trade_no { get; set; }
        public string channel_order_no { get; set; }
        public string user_id { get; set; }
        public string attach { get; set; }
        public string pay_trace { get; set; }
        public string pay_time { get; set; }

        protected override string ToUrl()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(base.ToUrl()).Append("&");
            sb.Append("refund_fee=").AppendWithNull(refund_fee).Append("&");
            sb.Append("end_time=").AppendWithNull(end_time).Append("&");
            sb.Append("out_refund_no=").AppendWithNull(out_refund_no).Append("&");
            sb.Append("out_trade_no=").AppendWithNull(out_trade_no);

            return sb.ToString();
        }

        public QueryRefundOutput ToQueryRefundOutput()
        {
            var output = new QueryRefundOutput();
            output.ListNo = out_trade_no;
            output.RefundListNo = out_refund_no;
            output.RefundFee = refund_fee.To<decimal>() / 100M;
            output.RefundStatus = trade_state;
            output.RefundTime = end_time.ToDateTime(SaobePayOptions.DateTimeFormat);
            output.ErrorMessage = return_msg;
            output.Success = trade_state == "SUCCESS";
            output.ShouldRetry = trade_state == "REFUNDING";
            output.IsExist = return_msg != "订单信息不存在！";

            return output;
        }
    }
}
