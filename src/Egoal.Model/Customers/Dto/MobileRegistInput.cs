using Egoal.Annotations;
using System.ComponentModel.DataAnnotations;

namespace Egoal.Customers.Dto
{
    public class MobileRegistInput
    {
        [Display(Name = "用户名")]
        [MustFillIn]
        [MinimumLength(4)]
        [MaximumLength(16)]
        public string UserName { get; set; }

        [Display(Name = "密码")]
        [MustFillIn]
        [MinimumLength(6)]
        [MaximumLength(12)]
        public string Password { get; set; }

        public int CustomerTypeId { get; set; }

        [Display(Name = "机构名称")]
        [MustFillIn]
        public string Name { get; set; }

        [Display(Name = "机构代码")]
        [MustFillIn]
        public string Code { get; set; }

        [Display(Name = "机构照片")]
        [MustFillIn]
        public string Photo { get; set; }

        [Display(Name = "姓名")]
        [MustFillIn]
        public string ContactName { get; set; }

        [Display(Name = "手机号码")]
        [MustFillIn]
        [MobileNumber]
        public string ContactMobile { get; set; }

        [Display(Name = "身份证")]
        [MustFillIn]
        [IdentityCardNo]
        public string ContactCertNo { get; set; }
    }
}
