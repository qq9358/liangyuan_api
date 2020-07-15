using Egoal.Domain.Entities;

namespace Egoal.Trades
{
    public class TradeType : Entity
    {
        public string Name { get; set; }
        public string SortCode { get; set; }
        public int? TradeTypeTypeId { get; set; }
        public decimal? TicPrice { get; set; }
        public bool? SaleFlag { get; set; }
        public bool? StatFlag { get; set; }
        public bool? PrintInvoiceFlag { get; set; }
    }
}
