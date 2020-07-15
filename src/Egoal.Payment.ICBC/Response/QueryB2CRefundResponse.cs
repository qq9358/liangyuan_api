using Egoal.Extensions;
using System;

namespace Egoal.Payment.IcbcPay.Response
{
    public class QueryB2CRefundResponse : IcbcPayResponse
    {
        public string emallRejectId { get; set; }
        public string status { get; set; }
        public string serialNo { get; set; }
        public string bankRem { get; set; }
        public string rejectCashAmt { get; set; }
        public string rejectBankYHAmt { get; set; }
        public string rejectShopYHAmt { get; set; }

        public QueryRefundOutput ToQueryRefundOutput()
        {
            QueryRefundOutput queryRefundOutput = new QueryRefundOutput();
            queryRefundOutput.RefundListNo = emallRejectId;
            queryRefundOutput.RefundId = serialNo;
            if (!rejectCashAmt.IsNullOrEmpty())
            {
                queryRefundOutput.RefundFee = rejectCashAmt.To<decimal>() / 100;
            }
            queryRefundOutput.RefundStatus = status;
            queryRefundOutput.RefundTime = DateTime.Now;
            queryRefundOutput.ErrorMessage = return_msg;
            queryRefundOutput.Success = status == "1";
            queryRefundOutput.ShouldRetry = status == "3";
            queryRefundOutput.IsExist = true;

            return queryRefundOutput;
        }
    }
}
