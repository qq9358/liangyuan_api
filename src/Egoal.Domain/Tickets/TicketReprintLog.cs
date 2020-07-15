using Egoal.Domain.Entities;
using System;

namespace Egoal.Tickets
{
    public class TicketReprintLog : Entity<long>
    {
        public long? TicketId { get; set; }
        public int? TicketTypeId { get; set; }
        public string TicketTypeName { get; set; }
        public string TicketCode { get; set; }
        public string CardNo { get; set; }
        public int? CashierId { get; set; }
        public string CashierName { get; set; }
        public int? CashPcid { get; set; }
        public string CashPcname { get; set; }
        public int? SalePointId { get; set; }
        public int? ParkId { get; set; }
        public string ParkName { get; set; }
        public DateTime? Ctime { get; set; } = DateTime.Now;
    }
}
