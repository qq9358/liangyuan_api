namespace Egoal.Payment.IcbcPay.Request
{
    /// <summary>
    /// 二维码被扫支付请求参数
    /// </summary>
    public class PayRequest
    {
        /// <summary>
        /// 商户扫描上送，客户的付款码
        /// </summary>
        public string qr_code { get; set; }

        /// <summary>
        /// 商户线下档案编号（特约商户12位，特约部门15位）
        /// </summary>
        public string mer_id { get; set; }

        /// <summary>
        /// 商户系统订单号
        /// </summary>
        public string out_trade_no { get; set; }

        /// <summary>
        /// 订单金额，单位：分
        /// </summary>
        public string order_amt { get; set; }

        /// <summary>
        /// 交易日期，格式：YYYYMMDD
        /// </summary>
        public string trade_date { get; set; }

        /// <summary>
        /// 交易时间，格式：HHMMSS
        /// </summary>
        public string trade_time { get; set; }
    }
}
