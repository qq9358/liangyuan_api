using Egoal.Extensions;
using Egoal.TicketTypes;
using Egoal.Trades;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Egoal.Thirdparties.BigData.Dto
{
    public class TicketSaleListDto
    {
        public string list_no { get; set; }

        [JsonIgnore]
        public string OrderListNo { get; set; }

        [JsonIgnore]
        public TradeSource TradeSource { get; set; }
        public string trade_source { get; set; }

        public string ota { get; set; }

        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime? sale_time { get; set; }

        public string sale_type { get; set; }
        public string group_name { get; set; }
        public string travel_date { get; set; }

        [JsonIgnore]
        public TicketTypeType? TicketTypeTypeId { get; set; }
        public string product_type { get; set; }

        public string product_name { get; set; }
        public long ticket_id { get; set; }
        public string ticket_code { get; set; }
        public int? quantity { get; set; }
        public decimal? price { get; set; }
        public decimal? total_money { get; set; }
        public string status { get; set; }
        public string end_time { get; set; }

        [JsonIgnore]
        public int? ParkId { get; set; }
        public string park_name { get; set; }

        [JsonIgnore]
        public int? AreaId { get; set; }

        [JsonIgnore]
        public bool HasTourist { get; set; }

        public List<TouristListDto> tourists { get; set; }
    }
}
