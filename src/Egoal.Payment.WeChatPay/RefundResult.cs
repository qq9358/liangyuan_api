using System.Linq;

namespace Egoal.Payment.WeChatPay
{
    public class RefundResult : ResultBase
    {
        public string transaction_id { get; set; }
        public string out_trade_no { get; set; }
        public string out_refund_no { get; set; }
        public string refund_id { get; set; }
        public int refund_fee { get; set; }
        public int? settlement_refund_fee { get; set; }
        public int total_fee { get; set; }
        public int? settlement_total_fee { get; set; }
        public string fee_type { get; set; }
        public int cash_fee { get; set; }
        public string cash_fee_type { get; set; }
        public int? cash_refund_fee { get; set; }
        public int? coupon_refund_fee { get; set; }
        public int? coupon_refund_count { get; set; }

        public RefundOutput ToRefundOutput()
        {
            var output = new RefundOutput();
            output.ListNo = out_trade_no;
            output.RefundListNo = out_refund_no;
            output.RefundId = refund_id;
            output.RefundFee = refund_fee / 100M;
            output.Success = result_code == "SUCCESS" || err_code_des == "订单已全额退款" || err_code_des == "已发起退款，请查询退款单确认状态";
            output.ShouldRetry = ShouldRetry();
            output.ErrorMessage = return_code == "SUCCESS" ? err_code_des : return_msg;

            return output;
        }

        private bool ShouldRetry()
        {
            var retryCodes = new[] { "SYSTEMERROR", "BIZERR_NEED_RETRY", "NOTENOUGH", "INVALID_REQ_TOO_MUCH", "FREQUENCY_LIMITED" };

            return retryCodes.Contains(err_code);
        }
    }
}
