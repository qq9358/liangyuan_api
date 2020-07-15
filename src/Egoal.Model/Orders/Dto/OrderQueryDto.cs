namespace Egoal.Orders.Dto
{
    public class OrderQueryDto
    {
        public string ListNo { get; set; }
        public string TravelDate { get; set; }
        public int TotalNum { get; set; }
        public decimal TotalMoney { get; set; }
        public string OrderStatusName { get; set; }

        /// <summary>
        /// 入馆时段
        /// </summary>
        public string ChangCiName { get; set; }

        /// <summary>
        /// 联系方式
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 下单日期
        /// </summary>
        public string CTime { get; set; }
    }
}
