namespace Egoal.Payment.IcbcPay.Request
{
    /// <summary>
    /// 订单对象
    /// </summary>
    public class OrderInfo
    {
        /// <summary>
        /// 订单日期，格式为：yyyyMMddHHmmss要求在银行系统当然时间的前1小时和后12小时范围内，否则判定交易时间非法
        /// </summary>
        public string order_date { get; set; }

        /// <summary>
        /// 订单号，客户支付后商户网站产生的一个唯一的订单号，该订单号应该在相当长的时间内不重复。工行通过订单号加订单日期来唯一确认一笔订单的重复性
        /// </summary>
        public string order_id { get; set; }

        /// <summary>
        /// 金额，分期付款期数客户支付订单的总金额，一笔订单一个，以分为单位。不可以为零，必需符合金额标准
        /// </summary>
        public string amount { get; set; }

        /// <summary>
        /// 分期付款数，每笔订单一个；取值：1、3、6、9、12、18、24；1代表全额付款，必须为上述值，否则订单校验不通过。B2CPay日前仅支持全额付款
        /// </summary>
        public string installment_times { get; set; }

        /// <summary>
        /// 币种，用来区分一笔支付的币种，目前工行只支持使用人民币（001）支付
        /// </summary>
        public string cur_type { get; set; }

        /// <summary>
        /// 商户代码，唯一确定一个商户的代码，由商户在工行开户时，由工行告知商户
        /// </summary>
        public string mer_id { get; set; }

        /// <summary>
        /// 商户账号，商户入账账号，只能交易时指定。（商户付给银行手续费的账户，可以在开户的时候指定，也可以用交易指定方式；用交易指定方式则使用此商户账号
        /// </summary>
        public string mer_acct { get; set; }
    }
}
