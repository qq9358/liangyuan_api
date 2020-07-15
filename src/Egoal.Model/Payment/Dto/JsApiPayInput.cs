using Egoal.Annotations;
using System.ComponentModel.DataAnnotations;

namespace Egoal.Payment.Dto
{
    public class JsApiPayInput
    {
        [Display(Name = "订单号码")]
        [MustFillIn]
        public string ListNo { get; set; }

        public string ReturnUrl { get; set; }
    }
}
