using Egoal.Extensions;
using System;

namespace Egoal.Payment.IcbcPay.Response
{
    /// <summary>
    /// 二维码退款响应参数
    /// </summary>
    public class RejectResponse : IcbcPayResponse
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
        /// 用户唯一标识
        /// </summary>
        public string cust_id { get; set; }

        /// <summary>
        /// 商户系统生成的退款编号
        /// </summary>
        public string reject_no { get; set; }

        /// <summary>
        /// 实退金额（现金部分），单位：分
        /// </summary>
        public string real_reject_amt { get; set; }

        /// <summary>
        /// 本次退款总金额，其值=现金退款部分+积分，单位：分
        /// </summary>
        public string reject_amt { get; set; }

        /// <summary>
        /// 积分退款金额，单位：分
        /// </summary>
        public string reject_point { get; set; }

        /// <summary>
        /// 电子券退款金额，单位：分
        /// </summary>
        public string reject_ecoupon { get; set; }

        /// <summary>
        /// 屏蔽后的交易卡号
        /// </summary>
        public string card_no { get; set; }

        /// <summary>
        /// 本次所退优惠立减金额（商户部分），单位：分
        /// </summary>
        public string reject_mer_disc_amt { get; set; }

        /// <summary>
        /// 本次所退银行补贴金额，单位：分
        /// </summary>
        public string reject_bank_disc_amt { get; set; }

        /// <summary>
        /// 本次所退总优惠金额，其值=本次所退优惠立减金额（商户部分）+本次所退银行补贴金额，单位：分
        /// </summary>
        public string reject_total_disc_amt { get; set; }

        public RefundOutput ToRefundOutput()
        {
            RefundOutput refundOutput = new RefundOutput();
            refundOutput.ListNo = out_trade_no;
            refundOutput.RefundListNo = reject_no;
            refundOutput.RefundId = reject_no;
            if (!reject_amt.IsNullOrEmpty())
            {
                refundOutput.RefundFee = reject_amt.To<decimal>() / 100;
            }
            refundOutput.RefundTime = DateTime.Now;
            refundOutput.Success = return_code == 0;
            refundOutput.ShouldRetry = false;
            refundOutput.ErrorMessage = return_msg;

            return refundOutput;
        }
    }
}
