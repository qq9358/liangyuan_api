using Egoal.Domain.Entities;

namespace Egoal.TicketTypes
{
    public class TicketTypeClassDetail : Entity
    {
        public int TicketTypeClassId { get; set; }
        public int TicketTypeId { get; set; }
    }
}
