using Egoal.Annotations;
using Egoal.Members;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Egoal.Customers.Dto
{
    public class RegistGuiderInput : IValidatableObject
    {
        [Display(Name = "姓名")]
        [MustFillIn]
        public string Name { get; set; }

        [Display(Name = "证件类型")]
        [MustFillIn]
        public int CertTypeId { get; set; }

        [Display(Name = "证件号码")]
        [MustFillIn]
        public string CertNo { get; set; }

        [Display(Name = "导游证号")]
        public string BusinessLicense { get; set; }

        public string Photo { get; set; }

        [Display(Name = "单位名称")]
        [MustFillIn]
        public string CompanyName { get; set; }

        [Display(Name = "手机号码")]
        [MustFillIn]
        [MobileNumber]
        public string Mobile { get; set; }

        [Display(Name = "验证码")]
        [MustFillIn]
        public string VerificationCode { get; set; }

        [Display(Name = "电子邮箱")]
        [Email]
        public string Email { get; set; }

        [Display(Name = "密码")]
        [MustFillIn]
        [MinimumLength(6)]
        [MaximumLength(12)]
        public string Pwd { get; set; }

        public string SourceType { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (CertTypeId == DefaultCertType.二代身份证)
            {
                IdentityCardNoAttribute identityCardNo = new IdentityCardNoAttribute();
                if (!identityCardNo.IsValid(CertNo))
                {
                    yield return new ValidationResult("证件号码格式不正确");
                }
            }
        }
    }
}
