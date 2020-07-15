namespace Egoal.Payment
{
    public enum NetPayType
    {
        /// <summary>
        /// 微信扫码支付
        /// </summary>
        WeiXinScanQRCodePay = 1,
        /// <summary>
        /// 微信刷卡支付
        /// </summary>
        WeiXinScanCardPay = 2,
        /// <summary>
        /// 支付宝刷卡支付
        /// </summary>
        AliBarcodePay = 3,
        /// <summary>
        /// 支付宝扫码支付
        /// </summary>
        AliScanQRCodePay = 4,

        /// <summary>
        /// 第三方微信扫码支付
        /// </summary>
        ThirdPayScanQRCodePay = 5,

        /// <summary>
        /// 第三方平台退款
        /// </summary>
        ThirdPartyPlatRefund = 6,

        /// <summary>
        /// 威富通刷卡支付
        /// </summary>
        WFTScanCardPay = 7,

        /// <summary>
        /// 通联收银宝刷卡支付
        /// </summary>
        AllinScanCardPay = 8,

        /// <summary>
        /// 威富通扫码支付--自带前置版
        /// </summary>
        WFTScanCardPayDLL = 9,

        /// <summary>
        /// 微信公众号支付
        /// </summary>
        WeiXinJsApiPay = 10,

        /// <summary>
        /// 威富通公众号支付
        /// </summary>
        WFTJsApiPay = 11,

        /// <summary>
        /// 扫呗公众号支付
        /// </summary>
        SaoBeJsApiPay = 12,

        /// <summary>
        /// 扫呗刷卡支付
        /// </summary>
        SaoBeMicroPay = 13,

        /// <summary>
        /// 扫呗扫码支付
        /// </summary>
        SaoBeNativePay = 14,

        /// <summary>
        /// 收钱吧支付
        /// </summary>
        ShouQianBaPay = 15,

        /// <summary>
        /// 兴业银行支付
        /// </summary>
        XingYeBankPay = 16,

        /// <summary>
        /// 青海农商银行
        /// </summary>
        QingHaiRuralCommercialBankPay = 17,

        /// <summary>
        /// 工商银行公众号支付
        /// </summary>
        IcbcJsApiPay = 18,

        /// <summary>
        /// 工商银行刷卡支付
        /// </summary>
        IcbcMicroPay = 19,

        /// <summary>
        /// 工商银行扫码支付
        /// </summary>
        IcbcNativePay = 20,

        /// <summary>
        /// 工商银行手机网页支付
        /// </summary>
        IcbcH5Pay = 21
    }
}
