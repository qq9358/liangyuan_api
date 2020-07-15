using Egoal.Domain.Entities;
using Egoal.Extensions;
using System;
using System.Collections.Generic;

namespace Egoal.Trades
{
    public class Trade : Entity<Guid>
    {
        public Trade() : this(DateTime.Now)
        { }

        public Trade(DateTime ctime)
        {
            Id = Guid.NewGuid();

            TradeDetails = new List<TradeDetail>();
            PayDetails = new List<PayDetail>();

            Ctime = ctime;
            Cdate = ctime.ToDateString();
            Cweek = ctime.ToWeekString();
            Cmonth = ctime.ToMonthString();
            Cquarter = ctime.ToQuarterString();
            Cyear = ctime.ToYearString();
            Ctp = ctime.ToHourString();
        }

        public string ListNo { get; set; }
        public TradeTypeType? TradeTypeTypeId { get; set; }
        public int? TradeTypeId { get; set; }
        public string TradeTypeName { get; set; }
        public TradeSource TradeSource { get; set; }
        public decimal TotalMoney { get; set; }
        public decimal CanReturnMoney { get; set; }
        public Guid? ReturnTradeId { get; set; }
        public decimal? YaJin { get; set; }
        public decimal? YaJinWb { get; set; }
        public int? PayTypeId { get; set; }
        public string PayTypeName { get; set; }
        public bool PayFlag { get; set; }
        public bool? CzkFlag { get; set; }
        public long? CzkId { get; set; }
        public string CzkTicketCode { get; set; }
        public string CzkCardNo { get; set; }
        public string CzkOwner { get; set; }
        public string CzkOwnerTel { get; set; }
        public int? CurrencyId { get; set; } = 1;
        public string CurrencyName { get; set; } = "人民币";
        public decimal? CurrencyRate { get; set; } = 1M;
        public decimal? WbTotalMoney { get; set; }
        public int? CashierId { get; set; }
        public string CashierName { get; set; }
        public int? CashPcid { get; set; }
        public string CashPcname { get; set; }
        public int? SalePointId { get; set; }
        public string SalePointName { get; set; }
        public int? SalesmanId { get; set; }
        public string SalesmanName { get; set; }
        public int? ApproverId { get; set; }
        public string ApproverName { get; set; }
        public string Mobile { get; set; }
        public Guid? MemberId { get; set; }
        public string MemberName { get; set; }
        public Guid? CustomerId { get; set; }
        public string CustomerName { get; set; }
        public Guid? GuiderId { get; set; }
        public string GuiderName { get; set; }
        public Guid? ManagerId { get; set; }
        public string ManagerName { get; set; }
        public int? AreaId { get; set; }
        public string AreaName { get; set; }
        public int? KeYuanTypeId { get; set; }
        public string KeYuanTypeName { get; set; }
        public long? TicketId { get; set; }
        public string CardNo { get; set; }
        public string OrderListNo { get; set; }
        public string ThirdPartyPlatformOrderId { get; set; }
        public string ThirdPartyPlatformId { get; set; }
        public string Ota { get; set; }
        public string Memo { get; set; }
        public bool? StatFlag { get; set; }
        public bool? BdFlag { get; set; } = false;
        public string Cdate { get; set; }
        public string Cweek { get; set; }
        public string Cmonth { get; set; }
        public string Cquarter { get; set; }
        public string Cyear { get; set; }
        public string Ctp { get; set; }
        public DateTime? Ctime { get; set; }
        public DateTime? Ltime { get; set; } = DateTime.Now;
        public long? Bid { get; set; }
        public bool? CommitFlag { get; set; } = true;
        public bool? ShiftFlag { get; set; } = false;
        public DateTime? ShiftTime { get; set; }
        public int? ParkId { get; set; }
        public string ParkName { get; set; }
        public int? ShopId { get; set; }
        public string ShopName { get; set; }
        public string InvoiceCode { get; set; }
        public string InvoiceNo { get; set; }
        public bool IsUpload { get; set; }

        public virtual ICollection<TradeDetail> TradeDetails { get; set; }
        public virtual ICollection<PayDetail> PayDetails { get; set; }

        public void SetTotalMoney(decimal totalMoney)
        {
            TotalMoney = CanReturnMoney = totalMoney;
            WbTotalMoney = totalMoney * CurrencyRate;
        }

        public void Pay(int payTypeId, string payTypeName)
        {
            PayFlag = true;
            PayTypeId = payTypeId;
            PayTypeName = payTypeName;

            var payDetail = this.MapToPayDetail();
            PayDetails.Add(payDetail);
        }
    }
}
