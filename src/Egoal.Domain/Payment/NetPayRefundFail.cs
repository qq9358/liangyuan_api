using Egoal.Domain.Entities;
using System;

namespace Egoal.Payment
{
    public class NetPayRefundFail : Entity<Guid>
    {
        public NetPayType? NetPayTypeId { get; set; }
        public string NetPayTypeName { get; set; }
        public string ListNo { get; set; }
        public string RefundListNo { get; set; }
        public string TotalFee { get; set; }
        public string RefundFee { get; set; }
        public int? StatusId { get; set; }
        public string StatusName { get; set; }
        public string AppId { get; set; }
        public string AppKey { get; set; }
        public int? SalePointId { get; set; }
        public int? CashPcId { get; set; }
        public string EmployeeNo { get; set; }
        public DateTime? Ctime { get; set; } = DateTime.Now;
        public DateTime? RefundTime { get; set; }
        public string Memo { get; set; }
    }
}
