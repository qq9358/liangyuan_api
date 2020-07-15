namespace Egoal.Payment.IcbcPay
{
    public class IcbcPayRequest
    {
        public string app_id { get; set; }
        public string msg_id { get; set; }
        public string format { get; set; } = "json";
        public string charset { get; set; } = "UTF-8";
        public string encrypt_type { get; set; }
        public string sign_type { get; set; } = "RSA2";
        public string sign { get; set; }
        public string timestamp { get; set; }
        public string ca { get; set; }
        public string biz_content { get; set; }
    }
}
