using Egoal.Domain.Entities;

namespace Egoal.TicketTypes
{
    public class TicketTypeGateGroup : Entity
    {
        public int TicketTypeId { get; set; }
        public int GateGroupId { get; set; }
    }
}
