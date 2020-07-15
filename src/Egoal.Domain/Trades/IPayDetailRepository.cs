using Egoal.Application.Services.Dto;
using Egoal.Domain.Repositories;
using Egoal.Tickets.Dto;
using Egoal.Trades.Dto;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Egoal.Trades
{
    public interface IPayDetailRepository : IRepository<PayDetail, Guid>
    {
        Task<DataTable> StatJbAsync(StatJbInput input);
        Task<DataTable> StatAsync(StatPayDetailInput input, IEnumerable<ComboboxItemDto<int>> payTypes);
    }
}
