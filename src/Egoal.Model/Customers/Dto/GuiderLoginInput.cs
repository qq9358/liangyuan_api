using Egoal.Annotations;
using Egoal.Extensions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Egoal.Customers.Dto
{
    public class GuiderLoginInput : IValidatableObject
    {
        [Display(Name = "用户名")]
        [MustFillIn]
        public string UserName { get; set; }

        public string Password { get; set; }
        public string VerificationCode { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Password.IsNullOrEmpty() && VerificationCode.IsNullOrEmpty())
            {
                yield return new ValidationResult("请输入验证码或密码");
            }
        }
    }
}
