using Egoal.Application.Services.Dto;
using Egoal.Extensions;
using Egoal.TicketTypes;
using Newtonsoft.Json;
using System;

namespace Egoal.Tickets.Dto
{
    public class TicketExchangeHistoryListDto : EntityDto<long>
    {
        public string OrderListNo { get; set; }

        [JsonIgnore]
        public int? TicketTypeId { get; set; }
        public string TicketTypeName { get; set; }
        public string OldTicketCode { get; set; }
        public string OldCardNo { get; set; }
        public string NewTicketCode { get; set; }
        public string NewCardNo { get; set; }
        [JsonIgnore]
        public TicketKind? Tkid { get; set; }
        public string Tkname { get; set; }

        [JsonIgnore]
        public int? CashierId { get; set; }
        public string CashierName { get; set; }

        [JsonIgnore]
        public int? SalePointId { get; set; }
        public string SalePointName { get; set; }

        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime? Ctime { get; set; }
        public int RowNum { get; set; }
    }
}
