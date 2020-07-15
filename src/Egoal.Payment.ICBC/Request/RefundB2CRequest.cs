namespace Egoal.Payment.IcbcPay.Request
{
    public class RefundB2CRequest
    {
        public string functionID { get; set; }
        public string o2oFlag { get; set; }
        public string onLine_merID { get; set; }
        public string offLine_merID { get; set; }
        public string channel_merID { get; set; }
        public string payDate { get; set; }
        public string orderNum { get; set; }
        public string emallRejectId { get; set; }
        public string rejectReson { get; set; }
        public string rejectAmt { get; set; }
        public string thirdPayFlag { get; set; }
        public string orderNumType { get; set; }
        public string merattach { get; set; }
    }
}
