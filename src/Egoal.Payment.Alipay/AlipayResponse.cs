namespace Egoal.Payment.Alipay
{
    public class AlipayResponse
    {
        public string code { get; set; }
        public string msg { get; set; }
        public string sub_code { get; set; }
        public string sub_msg { get; set; }
        public string sign { get; set; }
    }
}
