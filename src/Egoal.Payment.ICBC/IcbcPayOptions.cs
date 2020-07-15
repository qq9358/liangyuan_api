namespace Egoal.Payment.IcbcPay
{
    public class IcbcPayOptions
    {
        public string IcbcMerchantPrivateKey { get; set; }
        public string IcbcPublicKey { get; set; }
        public string IcbcAESKey { get; set; }
        public string IcbcAppId { get; set; }
        public string IcbcMerchantId { get; set; }
        public string IcbcEMerchantId { get; set; }
        public string IcbcH5MallCode { get; set; }
        public string IcbcH5AppId { get; set; }
        public string IcbcH5MerchantId { get; set; }
        public string IcbcH5EMerchantId { get; set; }
        public string IcbcH5ClearingAccount { get; set; }
        public string IcbcCAPrivateKeyPath { get; set; }
        public string IcbcCAPrivateKeyPassword { get; set; }
        public string IcbcCAPublicKey { get; set; }
        public string IcbcPayUrl { get; set; } = "https://gw.open.icbc.com.cn";
        public string WebApiUrl { get; set; }
        public string WxAppID { get; set; }
    }
}
