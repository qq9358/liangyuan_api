using System.Collections.Generic;

namespace Egoal.Orders.Dto
{
    public class RefundApplyDto
    {
        public RefundApplyDto()
        {
            StatusDetails = new List<RefundApplyStatusDetail>();
        }

        public string RefundListNo { get; set; }
        public string RefundStatusName { get; set; }
        public string RefundTimeDescription { get; set; }
        public int RefundQuantity { get; set; }
        public decimal RefundMoney { get; set; }
        public string RefundId { get; set; }
        public string RefundRecvAccount { get; set; }
        public List<RefundApplyStatusDetail> StatusDetails { get; set; }
    }

    public class RefundApplyStatusDetail
    {
        public string Title { get; set; }
        public List<string> Details { get; set; }
    }
}
