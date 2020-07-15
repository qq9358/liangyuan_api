using Newtonsoft.Json;
using System;

namespace Egoal.Tickets.Dto
{
    public class TicketSaleSeatDto
    {
        public Guid? TradeId { get; set; }
        public long? TicketId { get; set; }
        [JsonIgnore]
        public int GroundId { get; set; }
        public string GroundName { get; set; }
        [JsonIgnore]
        public long? SeatId { get; set; }
        public string SeatName { get; set; }
        public string Sdate { get; set; }
        [JsonIgnore]
        public int? ChangCiId { get; set; }
        public string ChangCiName { get; set; }
        public bool HasBindToTicket { get; set; }
    }
}
