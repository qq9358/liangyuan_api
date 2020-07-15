using Egoal.Domain.Entities;
using System;

namespace Egoal.Trades
{
    public class PayDetail : Entity<Guid>
    {
        public Guid TradeId { get; set; }
        public string ListNo { get; set; }
        public decimal PayMoney { get; set; }
        public int? PayTypeId { get; set; }
        public string PayTypeName { get; set; }
        public bool PayFlag { get; set; }
        public bool? CzkFlag { get; set; }
        public long? CzkId { get; set; }
        public string CzkTicketCode { get; set; }
        public string CzkCardNo { get; set; }
        public string CzkOwner { get; set; }
        public string CzkOwnerTel { get; set; }
        public string PayListNo { get; set; }
        public int? CurrencyId { get; set; } = 1;
        public string CurrencyName { get; set; } = "人民币";
        public decimal? CurrencyRate { get; set; } = 1M;
        public decimal? WbPayMoney { get; set; }
        public Guid? CustomerId { get; set; }
        public int? GzStatusId { get; set; }
        public decimal? GzMoney { get; set; }
        public decimal? GzWbMoney { get; set; }
        public bool? StatFlag { get; set; }
        public bool? BdFlag { get; set; } = false;
        public DateTime? Ctime { get; set; }
        public string Cdate { get; set; }
        public int? CashierId { get; set; }
        public long? Bid { get; set; }
        public bool? CommitFlag { get; set; } = true;
        public bool? ShiftFlag { get; set; } = false;
        public DateTime? ShiftTime { get; set; }
        public int? ParkId { get; set; }

        public virtual Trade Trade { get; set; }
    }
}
