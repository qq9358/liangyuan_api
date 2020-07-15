using System;

namespace Egoal.Tickets.Dto
{
    public class StatTicketSaleBySalePointInput
    {
        public DateTime StartCTime { get; set; }
        public DateTime EndCTime { get; set; }
        public int? ParkId { get; set; }
        public int? SalePointId { get; set; }
        public int? TicketTypeId { get; set; }
    }
}
