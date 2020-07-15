using Egoal.Annotations;
using System.ComponentModel.DataAnnotations;

namespace Egoal.Orders.Dto
{
    public class CancelOrderInput
    {
        [Display(Name = "单号")]
        [MustFillIn]
        public string ListNo { get; set; }
    }
}
