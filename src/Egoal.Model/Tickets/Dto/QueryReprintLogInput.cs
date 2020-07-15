using Egoal.Application.Services.Dto;
using System;

namespace Egoal.Tickets.Dto
{
    public class QueryReprintLogInput : PagedInputDto
    {
        public DateTime StartCTime { get; set; }
        public DateTime EndCTime { get; set; }
        public int? TicketTypeId { get; set; }
        public string TicketCode { get; set; }
        public string CardNo { get; set; }
        public int? CashierId { get; set; }
        public int? CashpcId { get; set; }
        public int? SalePointId { get; set; }
    }
}
