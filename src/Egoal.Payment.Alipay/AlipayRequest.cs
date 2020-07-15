namespace Egoal.Payment.Alipay
{
    public class AlipayRequest
    {
        public string app_id { get; set; }
        public string method { get; set; }
        public string format { get; set; } = "json";
        public string charset { get; set; } = "utf-8";
        public string sign_type { get; set; }
        public string sign { get; set; }
        public string timestamp { get; set; }
        public string version { get; set; } = "1.0";
        public string notify_url { get; set; }
        public string return_url { get; set; }
        public string app_auth_token { get; set; }
        public string biz_content { get; set; }
    }
}
