using Egoal.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Egoal.Payment.IcbcPay.Response
{
    /// <summary>
    /// 二维码退款查询响应参数
    /// </summary>
    public class RejectQueryResponse : IcbcPayResponse
    {
        /// <summary>
        /// 商户系统订单号
        /// </summary>
        public string out_trade_no { get; set; }

        /// <summary>
        /// 行内系统订单号（特约商户27位，特约部门30位）
        /// </summary>
        public string order_id { get; set; }

        /// <summary>
        /// 针对该笔支付订单所有退款（/冲正）所组成的数据集合list,每笔退款（/冲正）数据如下所示
        /// </summary>
        public string refund_json_list { get; set; }

        public QueryRefundOutput ToQueryRefundOutput(string refundListNo)
        {
            QueryRefundOutput queryRefundOutput = new QueryRefundOutput();

            var details = refund_json_list.JsonToObject<List<RefundDetail>>();
            var detail = details.Where(r => r.reject_no == refundListNo).First();

            queryRefundOutput.ListNo = out_trade_no;
            queryRefundOutput.RefundListNo = detail.reject_no;
            queryRefundOutput.RefundId = detail.icbc_reject_sq;
            if (!detail.reject_amt.IsNullOrEmpty())
            {
                queryRefundOutput.RefundFee = detail.reject_amt.To<decimal>() / 100;
            }
            queryRefundOutput.RefundStatus = detail.reject_status;
            queryRefundOutput.RefundTime = DateTime.Now;
            queryRefundOutput.ErrorMessage = return_msg;
            queryRefundOutput.Success = detail.reject_status == "1";
            queryRefundOutput.ShouldRetry = return_code < 0;
            queryRefundOutput.IsExist = true;

            return queryRefundOutput;
        }
    }
}
