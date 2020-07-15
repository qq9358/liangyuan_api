namespace Egoal.Payment.IcbcPay.Request
{
    /// <summary>
    /// 扩展信息对象
    /// </summary>
    public class Custom
    {
        /// <summary>
        /// 联名校验标志，手机银行订单必输0，不校验
        /// </summary>
        public string verify_join_flag { get; set; }

        /// <summary>
        /// 语言版本，默认中文版，目前只支持中文版，取值："zh-CN"
        /// </summary>
        public string language { get; set; }
    }
}
