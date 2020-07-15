using Egoal.Domain.Entities;

namespace Egoal.TicketTypes
{
    public class TicketTypeGround : Entity
    {
        public int TicketTypeId { get; set; }
        public int GroundId { get; set; }
        public int? CheckTypeId { get; set; }
        public decimal? GroundPrice { get; set; }
        public bool? Preferred { get; set; }
        public int TotalNum { get; set; }

        public virtual TicketType TicketType { get; set; }
    }
}
