using Egoal.Application.Services.Dto;
using System;

namespace Egoal.Trades.Dto
{
    public class QueryTradeInput : PagedInputDto
    {
        public string StartSaleTime { get; set; }
        public string EndSaleTime { get; set; }
        public TradeTypeType? TradeTypeTypeId { get; set; }
        public int? TradeTypeId { get; set; }
        public string ListNo { get; set; }
        public string ThirdPartyPlatformOrderId { get; set; }
        public TradeSource? TradeSource { get; set; }
        public Guid? MemberId { get; set; }
        public Guid? CustomerId { get; set; }
        public Guid? GuiderId { get; set; }
        public int? PayTypeId { get; set; }
        public int? CashierId { get; set; }
        public int? CashPcid { get; set; }
        public int? SalePointId { get; set; }
        public int? ParkId { get; set; }
        public bool IncludeWareTrade { get; set; }
    }
}
