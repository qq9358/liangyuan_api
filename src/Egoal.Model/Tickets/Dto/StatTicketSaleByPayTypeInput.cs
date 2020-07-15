using System;

namespace Egoal.Tickets.Dto
{
    public class StatTicketSaleByPayTypeInput
    {
        public DateTime StartCTime { get; set; }
        public DateTime EndCTime { get; set; }
        public int? TicketTypeId { get; set; }
    }
}
