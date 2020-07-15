using Egoal.Annotations;
using System.ComponentModel.DataAnnotations;

namespace Egoal.Payment.Dto
{
    public class H5PayInput
    {
        [Display(Name = "订单号码")]
        [MustFillIn]
        public string ListNo { get; set; }

        [Display(Name = "付款方式")]
        public int PayTypeId { get; set; }

        public string ReturnUrl { get; set; }
        public string WapName { get; set; }
    }
}
