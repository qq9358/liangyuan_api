using Egoal.Annotations;
using System.ComponentModel.DataAnnotations;

namespace Egoal.Members.Dto
{
    public class H5LoginInput
    {
        [Display(Name = "用户名")]
        [MustFillIn]
        public string UserName { get; set; }

        [Display(Name = "验证码")]
        [MustFillIn]
        public string VerificationCode { get; set; }
    }
}
