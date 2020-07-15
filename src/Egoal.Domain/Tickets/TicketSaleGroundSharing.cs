using Egoal.Domain.Entities;

namespace Egoal.Tickets
{
    public class TicketSaleGroundSharing : Entity<long>
    {
        public long? TicketId { get; set; }
        public int? GroundId { get; set; }
        public decimal? SharingRate { get; set; }
        public decimal? SharingMoney { get; set; }

        public virtual TicketSale TicketSale { get; set; }
    }
}
