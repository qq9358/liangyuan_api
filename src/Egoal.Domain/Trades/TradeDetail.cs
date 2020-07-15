using Egoal.Domain.Entities;
using System;

namespace Egoal.Trades
{
    public class TradeDetail : Entity<Guid>
    {
        public Guid TradeId { get; set; }
        public string ListNo { get; set; }
        public string InvoiceCode { get; set; }
        public string InvoiceNo { get; set; }
        public TradeTypeType? TradeTypeTypeId { get; set; }
        public int? TradeTypeId { get; set; }
        public string TradeTypeName { get; set; }
        public decimal? TotalMoney { get; set; }
        public decimal? CanReturnMoney { get; set; }
        public int? CurrencyId { get; set; } = 1;
        public string CurrencyName { get; set; } = "人民币";
        public decimal? CurrencyRate { get; set; } = 1M;
        public decimal? WbTotalMoney { get; set; }
        public int? CashierId { get; set; }
        public bool? StatFlag { get; set; }
        public bool? BdFlag { get; set; } = false;
        public string Cdate { get; set; }
        public string Cweek { get; set; }
        public string Cmonth { get; set; }
        public string Cquarter { get; set; }
        public string Cyear { get; set; }
        public string Ctp { get; set; }
        public DateTime? Ctime { get; set; }
        public long? Bid { get; set; }
        public bool? CommitFlag { get; set; } = true;
        public bool? ShiftFlag { get; set; } = false;
        public DateTime? ShiftTime { get; set; }
        public int? ParkId { get; set; }

        public virtual Trade Trade { get; set; }

        public void SetTotalMoney(decimal? totalMoney)
        {
            TotalMoney = CanReturnMoney = totalMoney;
            WbTotalMoney = totalMoney * CurrencyRate;
        }
    }
}
