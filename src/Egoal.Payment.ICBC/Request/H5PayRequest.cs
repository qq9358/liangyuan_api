namespace Egoal.Payment.IcbcPay.Request
{
    public class H5PayRequest
    {
        public string app_id { get; set; }
        public string msg_id { get; set; }
        public string charset { get; set; } = "UTF-8";
        public string sign_type { get; set; } = "RSA2";
        public string sign { get; set; }
        public string timestamp { get; set; }
        public string notify_url { get; set; }
        public string ca { get; set; }
        public string interfaceName { get; set; }
        public string interfaceVersion { get; set; }
        public string clientType { get; set; }
        public string biz_content { get; set; }
    }
}
