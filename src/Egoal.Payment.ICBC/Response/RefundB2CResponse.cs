using Egoal.Extensions;
using System;

namespace Egoal.Payment.IcbcPay.Response
{
    public class RefundB2CResponse : IcbcPayResponse
    {
        public string emallRejectId { get; set; }
        public string status { get; set; }
        public string serialNo { get; set; }
        public string bankRem { get; set; }
        public string rejectCashAmt { get; set; }
        public string rejectBankYHAmt { get; set; }
        public string rejectShopYHAmt { get; set; }

        public RefundOutput ToRefundOutput()
        {
            RefundOutput refundOutput = new RefundOutput();
            refundOutput.RefundListNo = emallRejectId;
            refundOutput.RefundId = serialNo;
            if (!rejectCashAmt.IsNullOrEmpty())
            {
                refundOutput.RefundFee = rejectCashAmt.To<decimal>() / 100;
            }
            refundOutput.RefundTime = DateTime.Now;
            refundOutput.Success = status == "1";
            refundOutput.ShouldRetry = return_code == 89105 || status.IsIn("2", "4");
            refundOutput.ErrorMessage = return_msg;

            return refundOutput;
        }
    }
}
