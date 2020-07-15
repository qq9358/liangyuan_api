using Egoal.Annotations;
using System.ComponentModel.DataAnnotations;

namespace Egoal.Scenics.Dto
{
    public class RegistGateInput
    {
        [Display(Name = "通道名称")]
        [MustFillIn]
        [MaximumLength(50)]
        public string Name { get; set; }

        [Display(Name = "IP地址")]
        [IPAddress]
        public string TcpIp { get; set; }

        [Display(Name = "Mac地址")]
        [MacAddress]
        public string TcpMac { get; set; }

        [Display(Name = "设备识别码")]
        [MustFillIn]
        [MaximumLength(50)]
        public string IdentifyCode { get; set; }
    }
}
