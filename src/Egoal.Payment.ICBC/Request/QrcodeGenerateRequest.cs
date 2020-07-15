namespace Egoal.Payment.IcbcPay.Request
{
    /// <summary>
    /// 二维码生成请求参数
    /// </summary>
    public class QrcodeGenerateRequest
    {
        /// <summary>
        /// 商户线下档案编号（特约商户12位，特约部门15位）
        /// </summary>
        public string mer_id { get; set; }

        /// <summary>
        /// e生活档案编号
        /// </summary>
        public string store_code { get; set; }

        /// <summary>
        /// 商户系统订单号
        /// </summary>
        public string out_trade_no { get; set; }

        /// <summary>
        /// 订单总金额，单位：分
        /// </summary>
        public string order_amt { get; set; }

        /// <summary>
        /// 商户订单生成日期，格式：yyyyMMdd
        /// </summary>
        public string trade_date { get; set; }

        /// <summary>
        /// 商户订单生成时间，格式：HHmmss
        /// </summary>
        public string trade_time { get; set; }

        /// <summary>
        /// 商户附加数据，最多21个汉字字符，原样返回
        /// </summary>
        public string attach { get; set; }

        /// <summary>
        /// 二维码有效期，单位：秒，必须小于24小时
        /// </summary>
        public string pay_expire { get; set; }

        /// <summary>
        /// 商户接收支付成功通知消息URL，当notify_flag为1时必输
        /// </summary>
        public string notify_url { get; set; }

        /// <summary>
        /// 商户订单乱的机器IP
        /// </summary>
        public string tporder_create_ip { get; set; }

        /// <summary>
        /// 扫码后是否需要跳转分行，0：否，1：是；非1按0处理
        /// </summary>
        public string sp_flag { get; set; }

        /// <summary>
        /// 商户是否开户通知接口0-否，1-是；非1按0处理
        /// </summary>
        public string notify_flag { get; set; }
    }
}
