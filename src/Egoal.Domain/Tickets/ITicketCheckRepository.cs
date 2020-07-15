using Egoal.Application.Services.Dto;
using Egoal.Domain.Repositories;
using Egoal.Tickets.Dto;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Egoal.Tickets
{
    public interface ITicketCheckRepository : IRepository<TicketCheck, long>
    {
        Task<PagedResultDto<TicketCheckListDto>> QueryTicketChecksAsync(QueryTicketCheckInput input);
        Task<DataTable> StatTicketCheckInByTimeAsync(StatTicketCheckInInput input);
        Task<DataTable> StatTicketCheckInByParkAndTimeAsync(StatTicketCheckInInput input);
        Task<DataTable> StatTicketCheckByGroundAndTimeAsync(StatTicketCheckInInput input);
        Task<DataTable> StatTicketCheckInAsync(StatTicketCheckInInput input);
        Task<DataTable> StatTicketCheckInByParkAsync(StatTicketCheckInInput input);
        Task<DataTable> StatTicketCheckInByDateAndParkAsync(StatTicketCheckInInput input, IEnumerable<ComboboxItemDto<int>> parks);
        Task<DataTable> StatTicketCheckInByGateGroupAsync(StatTicketCheckInInput input);
        Task<DataTable> StatTicketCheckByTradeSourceAsync(StatTicketCheckInInput input);
        Task<DataTable> StatTicketCheckInByTicketTypeAsync(StatTicketCheckInInput input);
        Task<DataTable> StatTicketCheckInByGroundAndGateGroupAsync(StatTicketCheckInInput input);
    }
}
