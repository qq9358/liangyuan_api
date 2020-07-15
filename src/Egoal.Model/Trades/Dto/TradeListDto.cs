using Egoal.Application.Services.Dto;
using Newtonsoft.Json;
using System;

namespace Egoal.Trades.Dto
{
    public class TradeListDto : EntityDto<Guid>
    {
        public Guid TradeId { get; set; }
        public string ListNo { get; set; }
        public string TradeTypeName { get; set; }
        public decimal TotalMoney { get; set; }
        public string PayTypeName { get; set; }
        [JsonIgnore]
        public int? CashierId { get; set; }
        public string CashierName { get; set; }
        public string CashPcname { get; set; }
        public string SalePointName { get; set; }
        public string SalesmanName { get; set; }
        public string ParkName { get; set; }
        [JsonIgnore]
        public Guid? MemberId { get; set; }
        public string MemberName { get; set; }
        [JsonIgnore]
        public Guid? CustomerId { get; set; }
        public string CustomerName { get; set; }
        [JsonIgnore]
        public Guid? GuiderId { get; set; }
        public string GuiderName { get; set; }
        public string InvoiceCode { get; set; }
        public string InvoiceNo { get; set; }
        public string AreaName { get; set; }
        public string Memo { get; set; }
        public string Ctime { get; set; }
        public string RowNum { get; set; }
    }
}
