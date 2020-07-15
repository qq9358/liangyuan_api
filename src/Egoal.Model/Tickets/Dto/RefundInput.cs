namespace Egoal.Tickets.Dto
{
    public class RefundInput
    {
        public string TicketCode { get; set; }
        public int? CashierId { get; set; }
        public int? CashPcid { get; set; }
        public int? SalePointId { get; set; }
        public int? ParkId { get; set; }
    }
}
