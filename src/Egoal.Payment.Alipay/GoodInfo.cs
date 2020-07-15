namespace Egoal.Payment.Alipay
{
    public class GoodInfo
    {
        public string goods_id { get; set; }
        public string alipay_goods_id { get; set; }
        public string goods_name { get; set; }
        public int quantity { get; set; }
        public decimal price { get; set; }
        public string goods_category { get; set; }
        public string categories_tree { get; set; }
        public string body { get; set; }
        public string show_url { get; set; }
    }
}
