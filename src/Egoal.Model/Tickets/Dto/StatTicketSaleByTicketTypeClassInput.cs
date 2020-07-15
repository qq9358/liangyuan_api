using System;

namespace Egoal.Tickets.Dto
{
    public class StatTicketSaleByTicketTypeClassInput
    {
        public DateTime StartCTime { get; set; }
        public DateTime EndCTime { get; set; }
        public int? TicketTypeId { get; set; }
        public int? TicketTypeClassId { get; set; }
    }
}
