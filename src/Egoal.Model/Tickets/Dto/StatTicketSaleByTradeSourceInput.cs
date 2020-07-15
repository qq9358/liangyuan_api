using Egoal.Trades;
using System;

namespace Egoal.Tickets.Dto
{
    public class StatTicketSaleByTradeSourceInput
    {
        public DateTime StartCTime { get; set; }
        public DateTime EndCTime { get; set; }
        public int? TicketTypeId { get; set; }
        public int? TicketTypeSearchGroupId { get; set; }
        public TradeSource? TradeSource { get; set; }
        public int? CashierId { get; set; }
        public int? SalePointId { get; set; }
        public string StatType { get; set; }
    }
}
