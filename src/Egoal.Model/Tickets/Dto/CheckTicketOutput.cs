using Egoal.Extensions;
using Newtonsoft.Json;
using System;

namespace Egoal.Tickets.Dto
{
    public class CheckTicketOutput
    {
        public string CardNo { get; set; }

        [JsonIgnore]
        public int? TicketTypeId { get; set; }
        public string TicketTypeName { get; set; }
        public string Stime { get; set; }
        public string Etime { get; set; }
        public int? TotalNum { get; set; }
        public int? SurplusNum { get; set; }

        [JsonIgnore]
        public int? GroundId { get; set; }
        public string GroundName { get; set; }

        [JsonIgnore]
        public int? CheckerId { get; set; }
        public string CheckerName { get; set; }
        public bool ShouldPrintAfterCheck { get; set; }

        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime? CheckTime { get; set; }

        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime? LastCheckInTime { get; set; }
    }
}
