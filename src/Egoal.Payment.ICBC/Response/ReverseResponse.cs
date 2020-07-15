namespace Egoal.Payment.IcbcPay.Response
{
    /// <summary>
    /// 二维码冲正响应参数
    /// </summary>
    public class ReverseResponse : IcbcPayResponse
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
        /// 本次退货总金额，其值=现金退款部分+积分，单位：分
        /// </summary>
        public string reject_amt { get; set; }

        /// <summary>
        /// 积分退货金额，单位：分
        /// </summary>
        public string reject_point { get; set; }

        /// <summary>
        /// 电子券退货金额，单位：分
        /// </summary>
        public string reject_ecoupon { get; set; }

        /// <summary>
        /// 屏蔽后的交易卡号
        /// </summary>
        public string card_no { get; set; }

        public ReversePayOutput ToReversePayOutput()
        {
            ReversePayOutput reversePayOutput = new ReversePayOutput();
            reversePayOutput.Success = return_code == 0;
            reversePayOutput.ShouldRetry = return_code < 0;
            reversePayOutput.ErrorMessage = return_msg;

            return reversePayOutput;
        }
    }
}
