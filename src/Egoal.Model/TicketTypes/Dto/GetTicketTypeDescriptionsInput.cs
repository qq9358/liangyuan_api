using Egoal.Application.Services.Dto;

namespace Egoal.TicketTypes.Dto
{
    public class GetTicketTypeDescriptionsInput : PagedInputDto
    {
        public int? TicketTypeId { get; set; }
    }
}
