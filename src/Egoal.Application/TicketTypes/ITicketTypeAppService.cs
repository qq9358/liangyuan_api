using Egoal.TicketTypes.Dto;
using System.Threading.Tasks;

namespace Egoal.TicketTypes
{
    public interface ITicketTypeAppService
    {
        Task CreateDescriptionAsync(TicketTypeDescriptionDto input);
        Task UpdateDescriptionAsync(TicketTypeDescriptionDto input);
        Task DeleteDescriptionAsync(int ticketTypeId);
    }
}
