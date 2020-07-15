using Egoal.Domain.Entities;
using System;

namespace Egoal.Tickets
{
    public class TicketConsume : Entity<long>
    {
        public Guid? TradeId { get; set; }
        public long TicketId { get; set; }
        public string CardNo { get; set; }
        public string CertNo { get; set; }
        public int TicketTypeId { get; set; }
        public string TicketTypeName { get; set; }
        public decimal Price { get; set; }
        public int ConsumeNum { get; set; }
        public DateTime ConsumeTime { get; set; }
        public ConsumeType ConsumeType { get; set; }
        public bool NeedNotice { get; set; }
        public string ThirdPartyPlatformId { get; set; }
        public string ThirdPartyPlatformOrderId { get; set; }
        public bool HasNoticed { get; set; }
        public DateTime? LastNoticeTime { get; set; }
        public Guid TicketConsumeGuid { get; set; } = Guid.NewGuid();
    }
}
