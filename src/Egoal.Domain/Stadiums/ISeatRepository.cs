using Egoal.Domain.Repositories;
using Egoal.Stadiums.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Egoal.Stadiums
{
    public interface ISeatRepository : IRepository<Seat, long>
    {
        Task<List<SeatForSaleDto>> GetSeatForSaleAsync(SeatingInput input);
    }
}
