using Egoal.Annotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Egoal.Orders.Dto
{
    public class RefundOrderInput
    {
        public RefundOrderInput()
        {
            Details = new List<RefundOrderDetailDto>();
        }

        [Display(Name = "单号")]
        [MustFillIn]
        public string ListNo { get; set; }

        [Display(Name = "取消原因")]
        [MustFillIn]
        public string Reason { get; set; }

        public List<RefundOrderDetailDto> Details { get; set; }
    }

    public class RefundOrderDetailDto
    {
        public long Id { get; set; }
        public int RefundQuantity { get; set; }
    }
}
