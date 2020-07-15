using Egoal.Application.Services.Dto;
using Egoal.Members;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Egoal.Customers.Dto
{
    public class CustomerListDto : EntityDto<Guid>
    {
        public string Uid { get; set; }
        public string Name { get; set; }
        public string Mobile { get; set; }
        [JsonIgnore]
        public int? CertTypeId { get; set; }
        public string CertTypeName { get; set; }
        public string CertNo { get; set; }
        [JsonIgnore]
        public int? CustomerTypeId { get; set; }
        public string CustomerTypeName { get; set; }
        [JsonIgnore]
        public MemberStatus? StatusId { get; set; }
        public string StatusName { get; set; }
        public string BusinessLicense { get; set; }
        public string LegalPerson { get; set; }
        public string Email { get; set; }
        public string Tel { get; set; }
        public string Linkman { get; set; }
        public Guid? MemberId { get; set; }
        public string MemberName { get; set; }
        public DateTime CTime { get; set; }

        public bool ShouldAudit { get; set; }

        /// <summary>
        /// 导游图片
        /// </summary>
        public byte[] Photo { get; set; }

        /// <summary>
        /// 图片路径
        /// </summary>
        public string PhotoSrc { get; set; }
    }
}
