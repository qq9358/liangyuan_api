using System;

namespace Egoal.TicketTypes.Dto
{
    public class TicketTypeDailyPriceDto
    {
        public int TicketTypeId { get; set; }
        public DateTime Date { get; set; }
        public decimal TicPrice { get; set; }
        public decimal? NetPrice { get; set; }
        public decimal? PrintPrice { get; set; }
    }
}
