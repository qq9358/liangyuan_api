namespace Egoal.Payment.IcbcPay.Request
{
    public class QueryB2CRequest
    {
        public string consumer_id { get; set; }
        public string merid { get; set; }
        public string o2o_merid { get; set; }
        public string orderid { get; set; }
        public string tdate { get; set; }
        public string secorder_flag { get; set; }
        public string service_id { get; set; }
        public string order_status { get; set; }
        public string orderid_type { get; set; }
    }
}
