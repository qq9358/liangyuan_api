using Egoal.Domain.Entities;
using System;

namespace Egoal.Payment
{
    public class RefundMoneyApply : Entity<long>
    {
        public string RefundListNo { get; set; }
        public string PayListNo { get; set; }
        public decimal RefundMoney { get; set; }
        public string Reason { get; set; }
        public RefundApplyStatus Status { get; set; } = RefundApplyStatus.退款中;
        public bool ApplySuccess { get; set; }
        public DateTime? ApplySuccessTime { get; set; }
        public string RefundId { get; set; }
        public string RefundRecvAccount { get; set; }
        public DateTime? RefundSuccessTime { get; set; }
        public string ResultMessage { get; set; }
        public DateTime? HandleTime { get; set; }
        public DateTime Ctime { get; set; } = DateTime.Now;
    }
}
