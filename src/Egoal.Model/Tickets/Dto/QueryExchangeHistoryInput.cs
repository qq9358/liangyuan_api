using Egoal.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Egoal.Tickets.Dto
{
    public class QueryExchangeHistoryInput : PagedInputDto
    {
        public DateTime StartCTime { get; set; }
        public DateTime EndCTime { get; set; }
        public string OrderListNo { get; set; }
        public string OldTicketCode { get; set; }
        public int? TicketTypeId { get; set; }
        public int? CashierId { get; set; }
        public int? SalePointId { get; set; }
    }
}
