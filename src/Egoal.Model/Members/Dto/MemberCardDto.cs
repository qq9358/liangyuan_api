using Egoal.Application.Services.Dto;

namespace Egoal.Members.Dto
{
    public class MemberCardDto : EntityDto
    {
        public string TicketTypeName { get; set; }
        public string Etime { get; set; }
        public string TicketCode { get; set; }
        public string MemberName { get; set; }
        public string Mobile { get; set; }
        public string IdCard { get; set; }
        public string Sex { get; set; }
        public string Nation { get; set; }
        public string Education { get; set; }
        public string Address { get; set; }
        public bool IsExpired { get; set; }
        public int Days { get; set; }
    }
}
