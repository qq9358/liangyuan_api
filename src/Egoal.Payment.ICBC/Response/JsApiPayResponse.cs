namespace Egoal.Payment.IcbcPay.Response
{
    public class JsApiPayResponse : IcbcPayResponse
    {
        public string tran_error_code { get; set; }
        public string tran_error_display_msg { get; set; }
        public string prepay_id { get; set; }
        public string return_url { get; set; }
        public string sign_data { get; set; }
    }
}
