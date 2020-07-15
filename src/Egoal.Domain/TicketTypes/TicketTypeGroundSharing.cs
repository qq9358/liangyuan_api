using Egoal.Domain.Entities;

namespace Egoal.TicketTypes
{
    public class TicketTypeGroundSharing : Entity
    {
        public int TicketTypeId { get; set; }
        public int GroundId { get; set; }
        public int? DateTypeId { get; set; }
        public decimal SharingRate { get; set; }

        public virtual TicketType TicketType { get; set; }
    }
}
