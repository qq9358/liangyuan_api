using Egoal.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Egoal.Staffs
{
    public interface IExplainerTimeslotSchedulingRepository : IRepository<ExplainerTimeslotScheduling>
    {
        Task<List<ExplainerTimeslotScheduling>> GetSchedulingsAsync(string date, string time);
        Task<bool> BookPublicTimeslotAsync(string date, int timeslotId);
        Task CancelPublicTimeslotAsync(string date, int timeslotId);
        Task<bool> BookReservedTimeslotAsync(string date, int timeslotId);
        Task CancelReservedTimeslotAsync(string date, int timeslotId);
    }
}
