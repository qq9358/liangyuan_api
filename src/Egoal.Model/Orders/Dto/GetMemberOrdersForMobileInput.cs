using Egoal.Application.Services.Dto;
using System;

namespace Egoal.Orders.Dto
{
    public class GetMemberOrdersForMobileInput : PagedInputDto
    {
        public bool? IsUsable { get; set; }
        public bool? IsNotPaid { get; set; }
    }
}
