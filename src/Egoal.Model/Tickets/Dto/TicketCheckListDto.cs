using Egoal.Application.Services.Dto;

namespace Egoal.Tickets.Dto
{
    public class TicketCheckListDto : EntityDto<long>
    {
        public string Ctime { get; set; }
        public string CardNo { get; set; }
        public string GroundName { get; set; }
        public string GateGroupName { get; set; }
        public string GateName { get; set; }
        public string ParkName { get; set; }
        public string TicketTypeName { get; set; }
        public string CheckTypeName { get; set; }
        public int? TotalNum { get; set; }
        public int? SurplusNum { get; set; }
        public int? CheckNum { get; set; }
        public string RecycleFlagName { get; set; }
        public string TicketCode { get; set; }
        public string SaleParkName { get; set; }
        public string ListNo { get; set; }
        public string Stime { get; set; }
        public string Etime { get; set; }
        public string CheckerName { get; set; }
        public string CashierName { get; set; }
        public string CashPcname { get; set; }
        public string SalePointName { get; set; }
        public string InOutFlagName { get; set; }
        public long? UniqueId { get; set; }
        public string GlkOwnerName { get; set; }
        public string FxCardNo { get; set; }
        public string MemberName { get; set; }
        public string CustomerName { get; set; }
        public string GuiderName { get; set; }
        public string RowNum { get; set; }
    }
}
