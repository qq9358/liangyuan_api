namespace Egoal.Orders.Dto
{
    public class CreateOrderOutput
    {
        public string ListNo { get; set; }
        public bool ShouldPay { get; set; } = true;

        /// <summary>
        /// 是否需要激活
        /// </summary>
        public bool FirstActiveFlag { get; set; }
    }
}
