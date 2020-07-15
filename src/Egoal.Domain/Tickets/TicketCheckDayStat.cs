using Egoal.Domain.Entities;

namespace Egoal.Tickets
{
    public class TicketCheckDayStat : Entity<long>
    {
        public int? CheckNum { get; set; } = 0;
        public int? GroundId { get; set; } = 0;
        public int? GateGroupId { get; set; } = 0;
        public int? GateId { get; set; } = 0;
        public bool? InOutFlag { get; set; } = false;
        public int? CheckerId { get; set; } = 0;
        public string Cdate { get; set; }
        public string Ctp { get; set; }
    }
}
