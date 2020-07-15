namespace Egoal.Payment.IcbcPay.Response
{
    /// <summary>
    /// H5移动在线支付响应参数
    /// </summary>
    public class H5PayResponse : IcbcPayResponse
    {
        /// <summary>
        /// 线上支付交易，没有则送空
        /// </summary>
        public string cust_id { get; set; }

        /// <summary>
        /// 屏蔽后的银行卡号
        /// </summary>
        public string card_no { get; set; }

        /// <summary>
        /// 订单总金额，一笔订单一个，以分为单位。不可以为零，必需符合金额标准
        /// </summary>
        public string total_amt { get; set; }

        /// <summary>
        /// 积分抵扣金额，单位：分。（线上支付交易不支持积分抵扣送0）
        /// </summary>
        public string point_amt { get; set; }

        /// <summary>
        /// 电子券抵扣金额，单位：分。（线上支付交易不支持积分抵扣送0）
        /// </summary>
        public string ecoupon_amt { get; set; }

        /// <summary>
        /// 商户立减金额（商户部分），单位：分
        /// </summary>
        public string mer_disc_amt { get; set; }

        /// <summary>
        /// 优惠券金额，单位：分。（线上支付交易不支持优惠券送0）
        /// </summary>
        public string coupon_amt { get; set; }

        /// <summary>
        /// 银行补贴金额，单位：分
        /// </summary>
        public string bank_disc_amt { get; set; }

        /// <summary>
        /// 用户实际扣减金额，单位：分。（没有优惠扣减则送0）
        /// </summary>
        public string payment_amt { get; set; }

        /// <summary>
        /// 商户系统订单号，原样返回
        /// </summary>
        public string out_trade_no { get; set; }

        /// <summary>
        /// 行内系统订单号（特约商户27位，特约部门30位）线上支付不需要送
        /// </summary>
        public string order_id { get; set; }

        /// <summary>
        /// 支付完成时间，格式为yyyyMMdd
        /// </summary>
        public string pay_time { get; set; }

        /// <summary>
        /// 总优惠金额，其值=优惠立减金额（商户部分）+银行补贴金额，单位：分。（没有优惠金额则送0）
        /// </summary>
        public string total_disc_amt { get; set; }

        /// <summary>
        /// 商户线下档案编号（特约商户12位，特约部门15位）。线上支付该字段送空
        /// </summary>
        public string mer_id { get; set; }

        /// <summary>
        /// 生产二维码时商户上送的附件数据，原样返回
        /// </summary>
        public string attch { get; set; }
    }
}
