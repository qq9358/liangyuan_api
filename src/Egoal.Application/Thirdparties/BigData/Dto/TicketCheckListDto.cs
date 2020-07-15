using Egoal.Extensions;
using Newtonsoft.Json;
using System;

namespace Egoal.Thirdparties.BigData.Dto
{
    public class TicketCheckListDto
    {
        public string list_no { get; set; }
        public string ticket_code { get; set; }
        public long? ticket_id { get; set; }
        public int? consume_quantity { get; set; }

        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime? consume_time { get; set; }

        [JsonIgnore]
        public int? GroundId { get; set; }
        public string ground_name { get; set; }

        [JsonIgnore]
        public int? ParkId { get; set; }
        public string park_name { get; set; }
    }
}
