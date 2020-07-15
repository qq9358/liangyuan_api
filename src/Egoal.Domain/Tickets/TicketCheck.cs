using Egoal.Domain.Entities;
using Egoal.Extensions;
using Egoal.TicketTypes;
using System;

namespace Egoal.Tickets
{
    public class TicketCheck : Entity<long>
    {
        public TicketCheck() : this(DateTime.Now)
        {
        }

        public TicketCheck(DateTime ctime)
        {
            Ctime = ctime;
            Cdate = ctime.ToDateString();
            Cweek = ctime.ToWeekString();
            Cmonth = ctime.ToMonthString();
            Cquarter = ctime.ToQuarterString();
            Cyear = ctime.ToYearString();
            Ctp = ctime.ToHourString();
        }

        public bool? OnLineFlag { get; set; } = true;
        public long? UniqueId { get; set; } = 0;
        public int? GroundId { get; set; }
        public string GroundName { get; set; }
        public int? GroundPlayTypeId { get; set; }
        public string GroundPlayTypeName { get; set; }
        public int? DeptId { get; set; }
        public string DeptName { get; set; }
        public int? GateGroupId { get; set; }
        public string GateGroupName { get; set; }
        public decimal? GroundPrice { get; set; }
        public int? GateId { get; set; }
        public string GateName { get; set; }
        public bool? InOutFlag { get; set; }
        public string InOutFlagName { get; set; }
        public int? SaleParkId { get; set; }
        public string SaleParkName { get; set; }
        public Guid? TradeId { get; set; }
        public string ListNo { get; set; }
        public Guid? TicketSyncCode { get; set; }
        public long? TicketId { get; set; }
        public string TicketCode { get; set; }
        public string CardNo { get; set; }
        public TicketTypeType? TicketTypeTypeId { get; set; }
        public int? TicketTypeId { get; set; }
        public string TicketTypeName { get; set; }
        public CheckType? CheckTypeId { get; set; }
        public string CheckTypeName { get; set; }
        public string Stime { get; set; }
        public string Etime { get; set; }
        public int? TotalNum { get; set; }
        public int? SurplusNum { get; set; }
        public int? CheckNum { get; set; }
        public string ConsumeMinutes { get; set; }
        public bool? RecycleFlag { get; set; }
        public string RecycleFlagName { get; set; }
        public int? CheckerId { get; set; }
        public string CheckerName { get; set; }
        public int? GlkOwnerId { get; set; }
        public string GlkOwnerName { get; set; }
        public string FxTicketCode { get; set; }
        public string FxCardNo { get; set; }
        public int? CashierId { get; set; }
        public string CashierName { get; set; }
        public int? CashPcid { get; set; }
        public string CashPcname { get; set; }
        public int? SalePointId { get; set; }
        public string SalePointName { get; set; }
        public Guid? MemberId { get; set; }
        public string MemberName { get; set; }
        public Guid? CustomerId { get; set; }
        public string CustomerName { get; set; }
        public Guid? GuiderId { get; set; }
        public string GuiderName { get; set; }
        public bool? StatFlag { get; set; }
        public bool? BdFlag { get; set; } = false;
        public bool IsWebCheck { get; set; }
        public string Memo { get; set; }
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
        public int? ParkId { get; set; }
        public string ParkName { get; set; }
        public Guid SyncCode { get; set; } = Guid.NewGuid();
    }
}
