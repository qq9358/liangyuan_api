using Egoal.Application.Services.Dto;
using System;

namespace Egoal.Tickets.Dto
{
    public class GetMemberTicketsInput : PagedInputDto
    {
        public Guid MemberId { get; set; }
        public Guid? CustomerId { get; set; }
    }
}
