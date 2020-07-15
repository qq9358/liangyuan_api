using Egoal.Annotations;
using System.ComponentModel.DataAnnotations;

namespace Egoal.Payment.Dto
{
    public class MicroPayInput
    {
        [Display(Name = "订单号码")]
        [MustFillIn]
        public string ListNo { get; set; }

        [Display(Name = "付款方式")]
        public int PayTypeId { get; set; }

        [Display(Name = "授权码")]
        [MustFillIn]
        public string AuthCode { get; set; }
    }
}
