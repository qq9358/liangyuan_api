namespace Egoal.Payment.IcbcPay.Request
{
    public class QueryB2CRefundRequest
    {
        public string functionID { get; set; }
        public string o2oFlag { get; set; }
        public string onLine_merID { get; set; }
        public string offLine_merID { get; set; }
        public string channel_merID { get; set; }
        public string orderNum { get; set; }
        public string emallRejectId { get; set; }
        public string serialNo { get; set; }
        public string rejectType { get; set; }
    }
}
