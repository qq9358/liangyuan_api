using Egoal.Application.Services.Dto;
using Egoal.Domain.Repositories;
using Egoal.Tickets.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Egoal.Tickets
{
    public interface ITicketConsumeRepository : IRepository<TicketConsume, long>
    {
        Task<PagedResultDto<TicketConsumeListDto>> QueryTicketConsumesAsync(QueryTicketConsumeInput input);
        Task<List<StatTicketConsumeListDto>> StatTicketConsumeAsync(StatTicketConsumeInput input);
    }
}
