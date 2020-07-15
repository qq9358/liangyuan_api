namespace Egoal.TicketTypes.Dto
{
    public class GetTicketTypesForSaleInput
    {
        public string SaleDate { get; set; }
        public SaleChannel SaleChannel { get; set; }
        public string KeyWord { get; set; }
        public bool? PublicSaleFlag { get; set; }
        public int? GroundId { get; set; }
        public int? StaffId { get; set; }
        public int? SalePointId { get; set; }
        public int? ParkId { get; set; }
    }
}
