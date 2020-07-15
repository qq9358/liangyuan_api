using Egoal.Annotations;
using Egoal.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;

namespace Egoal.Customers.Dto
{
    public class EditGuiderDto : EntityDto<Guid>
    {
        [Display(Name = "姓名")]
        [MustFillIn]
        public string Name { get; set; }

        [Display(Name = "单位名称")]
        [MustFillIn]
        public string CompanyName { get; set; }

        [Display(Name = "手机号码")]
        [MustFillIn]
        [MobileNumber]
        public string Mobile { get; set; }

        public string VerificationCode { get; set; }

        public string CertTypeName { get; set; }
        public string CertNo { get; set; }
        public string Photo { get; set; }

        /// <summary>
        /// 导游证号
        /// </summary>
        public string BusinessLicense { get; set; }
    }
}
