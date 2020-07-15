using Egoal.Domain.Entities;
using Egoal.Payment;
using System;

namespace Egoal.Orders
{
    public class RefundOrderApply : Entity<long>
    {
        public string RefundListNo { get; set; }
        public string ListNo { get; set; }
        public int RefundQuantity { get; set; }
        public decimal RefundMoney { get; set; }
        public string Details { get; set; }
        public string Reason { get; set; }
        public RefundApplyStatus Status { get; set; } = RefundApplyStatus.退款中;
        public string ResultMessage { get; set; }
        public DateTime? HandleTime { get; set; }
        public int? CashierId { get; set; }
        public int? CashPcid { get; set; }
        public int? SalePointId { get; set; }
        public int? ParkId { get; set; }
        public DateTime Ctime { get; set; } = DateTime.Now;
    }
}
