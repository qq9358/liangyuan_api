using Egoal.Application.Services.Dto;
using Egoal.Extensions;
using Newtonsoft.Json;
using System;

namespace Egoal.Tickets.Dto
{
    public class TicketReprintLogListDto : EntityDto<long>
    {
        public long? TicketId { get; set; }

        [JsonIgnore]
        public int? TicketTypeId { get; set; }
        public string TicketTypeName { get; set; }
        public string TicketCode { get; set; }
        public string CardNo { get; set; }

        [JsonIgnore]
        public int? CashierId { get; set; }
        public string CashierName { get; set; }

        [JsonIgnore]
        public int? CashPcid { get; set; }
        public string CashPcname { get; set; }

        [JsonIgnore]
        public int? SalePointId { get; set; }
        public string SalePointName { get; set; }

        [JsonIgnore]
        public int? ParkId { get; set; }
        public string ParkName { get; set; }

        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime? Ctime { get; set; }
        public int RowNum { get; set; }
    }
}
