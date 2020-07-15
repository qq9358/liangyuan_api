using Egoal.Domain.Entities;
using System;

namespace Egoal.TicketTypes
{
    public class TicketTypeStock : Entity
    {
        public int TicketTypeId { get; set; }
        public int? CustomerTypeId { get; set; }
        public Guid? CustomerId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Days { get; set; }
        public int Stock { get; set; }
    }
}
