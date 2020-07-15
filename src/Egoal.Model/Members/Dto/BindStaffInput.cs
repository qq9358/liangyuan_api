using Egoal.Annotations;
using System.ComponentModel.DataAnnotations;

namespace Egoal.Members.Dto
{
    public class BindStaffInput
    {
        [Display(Name = "用户名")]
        [MustFillIn]
        public string UserName { get; set; }

        [Display(Name = "密码")]
        [MustFillIn]
        public string Password { get; set; }
    }
}
