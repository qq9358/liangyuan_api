using System;
using System.Collections.Generic;

namespace Egoal.Payment.Alipay
{
    public class QueryRefundResponse : AlipayResponse
    {
        public string trade_no { get; set; }
        public string out_trade_no { get; set; }
        public string out_request_no { get; set; }
        public string refund_reason { get; set; }
        public decimal? total_amount { get; set; }
        public decimal? refund_amount { get; set; }
        public List<RefundRoyaltyResult> refund_royaltys { get; set; }
        public DateTime? gmt_refund_pay { get; set; }
        public List<TradeFundBill> refund_detail_item_list { get; set; }
        public string send_back_fee { get; set; }
        public string refund_settlement_id { get; set; }
        public string present_refund_buyer_amount { get; set; }
        public string present_refund_discount_amount { get; set; }
        public string present_refund_mdiscount_amount { get; set; }

        public QueryRefundOutput ToQueryRefundOutput()
        {
            var output = new QueryRefundOutput();
            output.ListNo = out_trade_no;
            output.RefundListNo = out_request_no;
            output.RefundFee = refund_amount ?? 0;
            output.RefundTime = gmt_refund_pay ?? DateTime.Now;
            output.RefundRecvAccount = "支付宝账户";
            output.ErrorMessage = sub_msg ?? msg;
            output.Success = code == "10000" && refund_amount.HasValue && refund_amount > 0;
            output.ShouldRetry = sub_code?.ToUpper() == "ACQ.SYSTEM_ERROR";
            output.IsExist = sub_code?.ToUpper() != "TRADE_NOT_EXIST";

            return output;
        }
    }
}
