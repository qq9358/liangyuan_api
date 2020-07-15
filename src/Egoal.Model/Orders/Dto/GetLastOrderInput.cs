namespace Egoal.Orders.Dto
{
    public class GetLastOrderInput
    {
        public int CashierId { get; set; }
        public int CashPcid { get; set; }
        public int SalePointId { get; set; }
        public int? ParkId { get; set; }
    }
}
