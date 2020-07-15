using Egoal.Annotations;
using System.ComponentModel.DataAnnotations;

namespace Egoal.Scenics.Dto
{
    public class RegistPcInput
    {
        [Display(Name = "主机名称")]
        [MustFillIn]
        [MaximumLength(50)]
        public string Name { get; set; }

        [Display(Name = "IP地址")]
        [IPAddress]
        public string Ip { get; set; }

        [Display(Name = "Mac地址")]
        [MacAddress]
        public string Mac { get; set; }

        [Display(Name = "设备识别码")]
        [MustFillIn]
        [MaximumLength(50)]
        public string IdentifyCode { get; set; }
    }
}
