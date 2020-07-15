using Egoal.Domain.Entities;
using System;

namespace Egoal.Tickets
{
    public class TicketSaleSeat : Entity<long>
    {
        public Guid? TradeId { get; set; }
        public long? TicketId { get; set; }
        public long? SeatId { get; set; }
        public string Sdate { get; set; }
        public int? ChangCiId { get; set; }
        public bool? CommitFlag { get; set; } = true;

        public virtual TicketSale TicketSale { get; set; }
    }
}
