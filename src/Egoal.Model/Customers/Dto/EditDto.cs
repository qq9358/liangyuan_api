using Egoal.Application.Services.Dto;
using Egoal.Members;
using System;

namespace Egoal.Customers.Dto
{
    public class EditDto : EntityDto<Guid>
    {
        public string Uid { get; set; }
        public string Pwd { get; set; }
        public string Name { get; set; }
        public string Zjf { get; set; }
        public string Mobile { get; set; }
        public int? CustomerTypeId { get; set; }
        public int? CertTypeId { get; set; }
        public string CertNo { get; set; }
        public MemberStatus? StatusId { get; set; }
        public string BusinessLicense { get; set; }
        public string LegalPerson { get; set; }
        public string Email { get; set; }
        public string Tel { get; set; }
        public string Linkman { get; set; }
        public string Address { get; set; }
        public string Memo { get; set; }
        public string Photo { get; set; }
        public bool PhotoChanged { get; set; }
    }
}
