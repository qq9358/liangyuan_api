namespace Egoal.Payment.IcbcPay.Response
{
    /// <summary>
    /// 针对该笔支付订单所有退款（/冲正）
    /// </summary>
    public class RefundDetail
    {
        /// <summary>
        /// 商户系统生成的退款编号
        /// </summary>
        public string reject_no { get; set; }

        /// <summary>
        /// 行内生成的退款编号
        /// </summary>
        public string icbc_reject_sq { get; set; }

        /// <summary>
        /// 退款状态码，0：退款可疑，1：退款成功，2：退款失败
        /// </summary>
        public string reject_status { get; set; }

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
    }
}
