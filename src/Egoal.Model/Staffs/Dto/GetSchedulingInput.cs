using Egoal.Annotations;
using System.ComponentModel.DataAnnotations;

namespace Egoal.Staffs.Dto
{
    public class GetSchedulingInput
    {
        [Display(Name = "参观日期")]
        [MustFillIn]
        public string Date { get; set; }
    }
}
