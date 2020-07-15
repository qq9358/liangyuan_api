using Egoal.Application.Services.Dto;
using System;

namespace Egoal.Members.Dto
{
    public class MemberDto : EntityDto<Guid>
    {
        public string Name { get; set; }
        public string HeadImgUrl { get; set; }
        public string OpenId { get; set; }
        public string Mobile { get; set; }
        public Guid? CustomerId { get; set; }
        public string CustomerName { get; set; }
        public bool HasBindCustomer { get; set; }
        public bool HasElectronicMemberCard { get; set; }
        public bool? IsWeChatSubscribed { get; set; }
        public bool LocalTicketEnrollFace { get; set; }
        public bool IsRegisted { get; set; }
        public Guid GuiderId { get; set; }
        public string CompanyName { get; set; }
    }
}
