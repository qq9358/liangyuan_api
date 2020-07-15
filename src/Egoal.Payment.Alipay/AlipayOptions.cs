namespace Egoal.Payment.Alipay
{
    public class AlipayOptions
    {
        public const string DateTimeFormat = "yyyy-MM-dd HH:mm:ss";

        public string AliAppID { get; set; }
        public string AliPayMerChantPrivateKeyPath { get; set; }
        public string AliPayPublicKeyPath { get; set; }
        public string AliPaySignType { get; set; } = "RSA2";
        public string WebApiUrl { get; set; }
    }
}
