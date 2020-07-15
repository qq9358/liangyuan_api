using Egoal.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Egoal.Tickets
{
    public interface ITicketSaleDayStatRepository : IRepository<TicketSaleDayStat, long>
    {
        Task<int> GetSaleNumAsync(string date);
    }
}
