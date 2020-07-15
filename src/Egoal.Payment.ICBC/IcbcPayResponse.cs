namespace Egoal.Payment.IcbcPay
{
    public abstract class IcbcPayResponse
    {
        public int return_code { get; set; }
        public string return_msg { get; set; }
        public string msg_id { get; set; }
    }
}
