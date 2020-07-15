using Egoal.Extensions;
using System;
using System.Collections.Generic;

namespace Egoal.Tickets.Dto
{
    public class RefundTicketInput
    {
        public RefundTicketInput()
        {
            TradeId = Guid.NewGuid();
            Ctime = DateTime.Now;

            Items = new List<RefundTicketItem>();
        }

        public Guid OriginalTradeId { get; set; }
        public Guid TradeId { get; set; }
        public string RefundListNo { get; set; }
        public string PayListNo { get; set; }
        public int PayTypeId { get; set; }
        public decimal TotalMoney { get; set; }
        public string RefundReason { get; set; }
        public string Cdate { get; set; }
        public string Cweek { get; set; }
        public string Cmonth { get; set; }
        public string Cquarter { get; set; }
        public string Cyear { get; set; }
        public string Ctp { get; set; }
        public DateTime Ctime
        {
            get { return _ctime; }
            set
            {
                _ctime = value;

                Cdate = _ctime.ToDateString();
                Cweek = _ctime.ToWeekString();
                Cmonth = _ctime.ToMonthString();
                Cquarter = _ctime.ToQuarterString();
                Cyear = _ctime.ToYearString();
                Ctp = _ctime.ToHourString();
            }
        }
        private DateTime _ctime;
        public int? CashierId { get; set; }
        public string CashierName { get; set; }
        public int? CashPcid { get; set; }
        public string CashPcname { get; set; }
        public int? SalePointId { get; set; }
        public string SalePointName { get; set; }
        public int? ParkId { get; set; }
        public string ParkName { get; set; }

        public List<RefundTicketItem> Items { get; set; }
    }

    public class RefundTicketItem
    {
        public long TicketId { get; set; }
        public int RefundQuantity { get; set; }

        /// <summary>
        /// 退票之后的剩余数量
        /// </summary>
        public int SurplusQuantityAfterRefund { get; set; }
    }
}
