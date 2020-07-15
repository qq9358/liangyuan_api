namespace Egoal.Payment.WeChatPay
{
    public class UnifiedOrderResult : ResultBase
    {

        /// <summary>
        /// 设备号
        /// </summary>
        public string device_info { get; set; }

        /// <summary>
        /// 交易类型
        /// </summary>
        public string trade_type { get; set; }

        /// <summary>
        /// 预支付交易会话标识
        /// </summary>
        public string prepay_id { get; set; }

        /// <summary>
        /// 二维码链接
        /// </summary>
        public string code_url { get; set; }

        /// <summary>
        /// 支付跳转链接
        /// </summary>
        public string mweb_url { get; set; }
    }
}
