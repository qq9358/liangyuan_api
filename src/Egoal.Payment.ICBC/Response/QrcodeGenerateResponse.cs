namespace Egoal.Payment.IcbcPay.Response
{
    /// <summary>
    /// 二维码生成响应参数
    /// </summary>
    public class QrcodeGenerateResponse: IcbcPayResponse
    {
        /// <summary>
        /// 订单二维码字符串信息
        /// </summary>
        public string qrcode { get; set; }

        /// <summary>
        /// 商户附加数据，原样返回
        /// </summary>
        public string attach { get; set; }
    }
}
