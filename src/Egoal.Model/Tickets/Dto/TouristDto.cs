using Egoal.Annotations;
using System.ComponentModel.DataAnnotations;

namespace Egoal.Tickets.Dto
{
    public class TouristDto
    {
        [Display(Name = "类型")]
        [MustFillIn]
        public string Type { get; set; }

        [Display(Name = "姓名")]
        [MustFillIn]
        public string Name { get; set; }

        public int? CertType { get; set; }

        [Display(Name = "身份证")]
        public string CertNo { get; set; }

        public string Birthday { get; set; }

        /// <summary>
        /// 证件类型名称
        /// </summary>
        public string CertTypeName { get; set; }
    }
}
