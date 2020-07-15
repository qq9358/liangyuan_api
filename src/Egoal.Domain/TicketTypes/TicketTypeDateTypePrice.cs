using Egoal.Domain.Entities;

namespace Egoal.TicketTypes
{
    public class TicketTypeDateTypePrice : Entity
    {
        public int TicketTypeId { get; set; }
        public int DateTypeId { get; set; }
        public decimal TicPrice { get; set; }
        public decimal? NetPrice { get; set; }
        public decimal? PrintPrice { get; set; }
    }
}
