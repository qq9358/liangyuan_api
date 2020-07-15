using Egoal.Extensions;
using System;

namespace Egoal.Payment.WeChatPay
{
    public class QueryRefundResult : ResultBase
    {
        public int? total_refund_count { get; set; }
        public string transaction_id { get; set; }
        public string out_trade_no { get; set; }
        public int total_fee { get; set; }
        public int? settlement_total_fee { get; set; }
        public string fee_type { get; set; }
        public int cash_fee { get; set; }
        public int refund_count { get; set; }
        public string out_refund_no_0 { get; set; }
        public string refund_id_0 { get; set; }
        public string refund_channel_0 { get; set; }
        public int refund_fee_0 { get; set; }
        public int? settlement_refund_fee_0 { get; set; }
        public int? coupon_refund_fee_0 { get; set; }
        public int? coupon_refund_count_0 { get; set; }
        public string refund_status_0 { get; set; }
        public string refund_account_0 { get; set; }
        public string refund_recv_accout_0 { get; set; }
        public string refund_success_time_0 { get; set; }

        public QueryRefundOutput ToQueryRefundOutput()
        {
            var output = new QueryRefundOutput();
            output.ListNo = out_trade_no;
            output.RefundListNo = out_refund_no_0;
            output.RefundFee = refund_fee_0 / 100M;
            output.RefundStatus = refund_status_0;
            if (!refund_success_time_0.IsNullOrEmpty())
            {
                output.RefundTime = refund_success_time_0.ToDateTime("yyyy-MM-dd HH:mm:ss");
            }
            else
            {
                output.RefundTime = DateTime.Now;
            }
            output.RefundRecvAccount = refund_recv_accout_0;
            output.ErrorMessage = return_code == "SUCCESS" ? err_code_des : return_msg;
            output.Success = refund_status_0 == "SUCCESS";
            output.ShouldRetry = refund_status_0 == "PROCESSING" || err_code == "SYSTEMERROR";
            output.IsExist = err_code != "REFUNDNOTEXIST";

            return output;
        }
    }
}
