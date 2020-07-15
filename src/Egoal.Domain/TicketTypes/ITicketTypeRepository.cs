using Egoal.Application.Services.Dto;
using Egoal.Domain.Repositories;
using Egoal.TicketTypes.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Egoal.TicketTypes
{
    public interface ITicketTypeRepository: IRepository<TicketType>
    {
        Task<IEnumerable<TicketType>> GetTicketTypesForSaleAsync(GetTicketTypesForSaleInput input);
        Task<bool> HasSpecifiedCheckGroundAsync(int ticketTypeId);
        Task<bool> HasGrantedToGroundAsync(int ticketTypeId, int groundId);
        Task<bool> HasGrantedToStaffAsync(int ticketTypeId, int staffId);
        Task<bool> HasGrantedToSalePointAsync(int ticketTypeId, int salePointId);
        Task<bool> HasGrantedToParkAsync(int ticketTypeId, int parkId);
        Task<List<ComboboxItemDto<int>>> GetTicketTypeTypeComboboxItemsAsync();
        Task<List<TicketTypeDailyPriceDto>> GetPriceAsync(int ticketTypeId, string startDate, string endDate);
    }
}
