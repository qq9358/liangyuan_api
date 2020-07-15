namespace Egoal.Orders.Dto
{
    public class GroupOrderDto
    {
        public string TravelDate { get; set; }
        public int? ChangCiId { get; set; }
        public string ChangCiName { get; set; }
        public int TotalNum { get; set; }
        public decimal TotalMoney { get; set; }
    }
}
