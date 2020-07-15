using Egoal.Domain.Repositories;
using Egoal.Tickets.Dto;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Egoal.Tickets
{
    public interface ITicketSaleSeatRepository : IRepository<TicketSaleSeat, long>
    {
        Task<List<TicketSaleSeatDto>> GetTicketSeatsAsync(GetTicketSeatsInput input);
        Task<DataTable> StatGroundChangCiSaleAsync(StatGroundChangCiSaleInput input);
    }
}
