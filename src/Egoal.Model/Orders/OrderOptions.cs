namespace Egoal.Orders
{
    public class OrderOptions
    {
        public const int MaxWeChatBookDay = 90;

        /// <summary>
        /// 团队订单每单最大数量
        /// </summary>
        public int GroupMaxBuyQuantityPerOrder { get; set; }

        /// <summary>
        /// 散客订单每单最大成人数量
        /// </summary>
        public int IndividualOrderMaxAdultQuantity { get; set; }

        /// <summary>
        /// 散客订单每单最大儿童数量
        /// </summary>
        public int IndividualOrderMaxChildrenQuantity { get; set; }

        /// <summary>
        /// 每个成人最多携带几个儿童
        /// </summary>
        public int PerAdultMaxChildrenQuantity { get; set; }

        public int WeChatBookDay { get; set; }

        public string WeiXinSaleTime { get; set; }

        public int WeChatMaxBuyQuantityPerDay { get; set; }

        public int WeChatStock { get; set; }

        public string EncodeIDCardNo { get; set; }
    }
}
