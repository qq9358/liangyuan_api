using Egoal.Annotations;
using System.ComponentModel.DataAnnotations;

namespace Egoal.Payment.Dto
{
    public class NativePayInput
    {
        [Display(Name = "订单号码")]
        [MustFillIn]
        public string ListNo { get; set; }

        [Display(Name = "付款方式")]
        public int PayTypeId { get; set; }
    }
}
