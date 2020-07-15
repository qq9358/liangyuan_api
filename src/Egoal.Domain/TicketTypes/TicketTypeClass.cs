using Egoal.Domain.Entities;

namespace Egoal.TicketTypes
{
    public class TicketTypeClass : Entity
    {
        public string Name { get; set; }
        public string SortCode { get; set; }
    }
}
