namespace Egoal.Orders.Dto
{
    public class OrderSimpleListDto
    {
        public string ListNo { get; set; }
        public string TravelDate { get; set; }
        public int TotalNum { get; set; }
        public decimal TotalMoney { get; set; }
        public bool IsFree { get; set; }
        public string OrderStatusName { get; set; }
    }
}
