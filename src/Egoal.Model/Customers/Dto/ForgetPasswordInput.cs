using Egoal.Annotations;
using System.ComponentModel.DataAnnotations;

namespace Egoal.Customers.Dto
{
    public class ForgetPasswordInput
    {
        [Display(Name = "新密码")]
        [MustFillIn]
        [MinimumLength(6)]
        [MaximumLength(12)]
        public string NewPassword { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        public string VerificationCode { get; set; }

        /// <summary>
        /// 导游用户名
        /// </summary>
        public string UserName { get; set; }
    }
}
