using System;

namespace Egoal.Tickets.Dto
{
    public class StatTicketCheckInInput
    {
        public DateTime StartCTime { get; set; }
        public DateTime EndCTime { get; set; }
        public int? ParkId { get; set; }
        public int? GateGroupId { get; set; }
        public int? GateId { get; set; }
        public int? CheckerId { get; set; }
        public int StatType { get; set; }
    }
}
