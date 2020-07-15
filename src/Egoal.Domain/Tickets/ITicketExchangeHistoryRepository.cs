using Egoal.Domain.Repositories;
using Egoal.Tickets.Dto;
using System.Data;
using System.Threading.Tasks;

namespace Egoal.Tickets
{
    public interface ITicketExchangeHistoryRepository : IRepository<TicketExchangeHistory, long>
    {
        Task<DataTable> StatJbAsync(StatJbInput input);
    }
}
