using Egoal.Annotations;
using System.ComponentModel.DataAnnotations;

namespace Egoal.Customers.Dto
{
    public class ChangePasswordInput
    {
        [Display(Name = "原密码")]
        [MustFillIn]
        [MinimumLength(6)]
        [MaximumLength(12)]
        public string OriginalPassword { get; set; }

        [Display(Name = "新密码")]
        [MustFillIn]
        [MinimumLength(6)]
        [MaximumLength(12)]
        public string NewPassword { get; set; }
    }
}
