using Egoal.Domain.Entities;
using System;

namespace Egoal.Tickets
{
    public class TicketSaleStock : Entity
    {
        public int TicketTypeId { get; set; }
        public int? CustomerTypeId { get; set; }
        public Guid? CustomerId { get; set; }
        public DateTime TravelDate { get; set; }
        public int SaleNum { get; set; }
    }
}
