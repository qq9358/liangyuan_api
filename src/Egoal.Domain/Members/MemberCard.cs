using Egoal.Domain.Entities.Auditing;
using Egoal.Tickets;
using System;

namespace Egoal.Members
{
    public class MemberCard : CreationAuditedEntity
    {
        public Guid? MemberId { get; set; }
        public Guid? MemberAccountId { get; set; }
        public Guid? TradeId { get; set; }
        public string ListNo { get; set; }
        public long? TicketId { get; set; }
        public string TicketCode { get; set; }
        public string CardNo { get; set; }
        public string Stime { get; set; }
        public string Etime { get; set; }
        public int? TicketTypeId { get; set; }
        public string TicketTypeName { get; set; }
        public int? TicketStatusId { get; set; }
        public string TicketStatusName { get; set; }
        public bool? CardValidFlag { get; set; }
        public string CardValidFlagName { get; set; }
        public bool? PrincipalCard { get; set; }
        public bool? CommitFlag { get; set; } = true;
        public bool IsElectronicTicket { get; set; }
        public bool? AviateFlag { get; set; }

        public void Renew(string etime, int ticketStatus, string ticketStatusName)
        {
            Etime = etime;
            TicketStatusId = ticketStatus;
            TicketStatusName = ticketStatusName;
            CardValidFlag = true;
            CardValidFlagName = "有效";
        }
    }
}
