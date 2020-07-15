using System;
using System.Collections.Generic;
using System.Linq;

namespace Egoal.Payment.Alipay
{
    public class RefundResponse : AlipayResponse
    {
        public string trade_no { get; set; }
        public string out_trade_no { get; set; }
        public string buyer_logon_id { get; set; }
        public string fund_change { get; set; }
        public decimal refund_fee { get; set; }
        public string refund_currency { get; set; }
        public DateTime gmt_refund_pay { get; set; }
        public List<TradeFundBill> refund_detail_item_list { get; set; }
        public string store_name { get; set; }
        public string buyer_user_id { get; set; }
        public PresetPayToolInfo refund_preset_paytool_list { get; set; }
        public string refund_settlement_id { get; set; }
        public string present_refund_buyer_amount { get; set; }
        public string present_refund_discount_amount { get; set; }
        public string present_refund_mdiscount_amount { get; set; }

        public RefundOutput ToRefundOutput()
        {
            var output = new RefundOutput();
            output.ListNo = out_trade_no;
            output.RefundId = refund_settlement_id;
            output.RefundFee = refund_fee;
            output.Success = code == "10000" && fund_change == "Y";
            output.ShouldRetry = ShouldRetry();
            output.ErrorMessage = sub_msg ?? msg;

            return output;
        }

        private bool ShouldRetry()
        {
            var retryCodes = new[] { "ACQ.SYSTEM_ERROR", "ACQ.SELLER_BALANCE_NOT_ENOUGH" };

            return retryCodes.Contains(sub_code?.ToUpper());
        }
    }
}
