namespace Egoal.Payment.SaobePay
{
    public class SaobePayOptions
    {
        public const string DateTimeFormat = "yyyyMMddHHmmss";

        public string SaoBeMerchantNo { get; set; }
        public string SaoBeTerminalId { get; set; }
        public string SaoBeTerminalRegisted { get; set; }
        public string SaoBeAccessToken { get; set; }
        public string SaoBeDomainUrl { get; set; } = "http://test.lcsw.cn:8045/lcsw";
        public string WebApiUrl { get; set; }
    }
}
