using Egoal.Domain.Entities;
using Egoal.TicketTypes;
using System;

namespace Egoal.Tickets
{
    public class TicketExchangeHistory : Entity<long>
    {
        public Guid? TradeId { get; set; }
        public string OrderListNo { get; set; }
        public int? TicketTypeId { get; set; }
        public string TicketTypeName { get; set; }
        public long? OldTicketId { get; set; }
        public string OldTicketCode { get; set; }
        public string OldCardNo { get; set; }
        public long? NewTicketId { get; set; }
        public string NewTicketCode { get; set; }
        public string NewCardNo { get; set; }
        public TicketKind? Tkid { get; set; }
        public string Tkname { get; set; }
        public int? CashierId { get; set; }
        public string CashierName { get; set; }
        public int? SalePointId { get; set; }
        public string SalePointName { get; set; }
        public string ThirdPartyPlatformName { get; set; }
        public DateTime? Ctime { get; set; }
    }
}
