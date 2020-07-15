using Egoal.Application.Services.Dto;
using Egoal.Extensions;
using Egoal.Trades;
using System;

namespace Egoal.Tickets.Dto
{
    public class QueryTicketSaleInput : PagedInputDto
    {
        public string StartSaleTime { get; set; }
        public string EndSaleTime { get; set; }
        public string TicketCode { get; set; }
        public string CardNo { get; set; }
        public string ListNo { get; set; }
        public TicketStatus? TicketStatusId { get; set; }
        public bool? IsExpired { get; set; }
        public int? TicketTypeTypeId { get; set; }
        public int? TicketTypeId { get; set; }
        public int? PayTypeId { get; set; }
        public Guid? CustomerId { get; set; }
        public Guid? MemberId { get; set; }
        public int? ParkId { get; set; }
        public int? SalePointId { get; set; }
        public int? CashierId { get; set; }
        public int? CashpcId { get; set; }
        public string OrderListNo { get; set; }
        public string ThirdListNo { get; set; }
        public TradeSource? TradeSource { get; set; }
        public int? SalesManId { get; set; }
        public string CertNo { get; set; }
        public bool? HasFingerprint { get; set; }
        public bool? HasFaceImage { get; set; }
        public string Now { get; set; } = DateTime.Now.ToDateTimeString();
    }
}
