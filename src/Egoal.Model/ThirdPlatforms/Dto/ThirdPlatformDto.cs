using Egoal.Annotations;
using Egoal.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;

namespace Egoal.ThirdPlatforms.Dto
{
    public class ThirdPlatformDto : EntityDto<string>
    {
        [Display(Name = "名称")]
        [MustFillIn]
        public string Name { get; set; }

        [Display(Name = "用户名")]
        [MustFillIn]
        public string Uid { get; set; }

        [Display(Name = "密码")]
        [MustFillIn]
        public string Pwd { get; set; }

        public string OrderCheckUrl { get; set; }

        public PlatformType PlatformType { get; set; }

        public string PlatformTypeName { get; set; }

        public DateTime? CTime { get; set; }
    }
}
