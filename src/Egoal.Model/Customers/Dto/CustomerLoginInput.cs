using Egoal.Annotations;
using System.ComponentModel.DataAnnotations;

namespace Egoal.Customers.Dto
{
    public class CustomerLoginInput
    {
        [Display(Name = "用户名")]
        [MustFillIn]
        public string UserName { get; set; }

        [Display(Name = "密码")]
        [MustFillIn]
        public string Password { get; set; }

        public bool ShouldBindMember { get; set; }
    }
}
