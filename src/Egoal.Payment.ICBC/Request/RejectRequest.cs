namespace Egoal.Payment.IcbcPay.Request
{
    /// <summary>
    /// 二维码退款请求参数
    /// </summary>
    public class RejectRequest
    {
        /// <summary>
        /// 商户线下档案编号（特约商户12位，特约部门15位）
        /// </summary>
        public string mer_id { get; set; }

        /// <summary>
        /// 支付时工行返回的用户唯一标识
        /// </summary>
        public string cust_id { get; set; }

        /// <summary>
        /// 商户系统订单号，商户订单号或行内订单号必须其中一个不为空
        /// </summary>
        public string out_trade_no { get; set; }

        /// <summary>
        /// 完整行内系统订单号（特约商户27位，特约部门30位）或其后15位，商户订单号或行内订单号必须其中一个不为空
        /// </summary>
        public string order_id { get; set; }

        /// <summary>
        /// 商户系统生成的退款编号，每次部分退款需要生成不同的退款编号
        /// </summary>
        public string reject_no { get; set; }

        /// <summary>
        /// 退款金额，单位：分
        /// </summary>
        public string reject_amt { get; set; }

        /// <summary>
        /// 操作人员ID
        /// </summary>
        public string oper_id { get; set; }
    }
}
