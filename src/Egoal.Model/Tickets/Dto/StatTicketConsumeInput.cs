using System;

namespace Egoal.Tickets.Dto
{
    public class StatTicketConsumeInput
    {
        public DateTime? StartCheckTime { get; set; }
        public DateTime? EndCheckTime { get; set; }
        public DateTime? StartConsumeTime { get; set; }
        public DateTime? EndConsumeTime { get; set; }
        public int? TicketTypeId { get; set; }
        public Guid? CustomerId { get; set; }
        public ConsumeType? ConsumeType { get; set; }
    }
}
