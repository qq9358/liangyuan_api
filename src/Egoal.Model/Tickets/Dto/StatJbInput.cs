using System;

namespace Egoal.Tickets.Dto
{
    public class StatJbInput
    {
        public DateTime StartCTime { get; set; }
        public DateTime EndCTime { get; set; }
        public int? ParkId { get; set; }
        public int? SalePointId { get; set; }
        public int? CashierId { get; set; }
        public bool? HasShift { get; set; }
        public bool StatTicketByPayType { get; set; }
        public bool IncludeWareDetail { get; set; }
    }
}
