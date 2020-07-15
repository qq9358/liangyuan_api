using Egoal.Application.Services.Dto;
using Egoal.Domain.Repositories;
using Egoal.Trades.Dto;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Egoal.Trades
{
    public interface ITradeRepository : IRepository<Trade, Guid>
    {
        Task<List<ComboboxItemDto<int>>> GetTradeTypeTypeComboboxItemsAsync();
        Task<List<ComboboxItemDto<int>>> GetTradeTypeComboboxItemsAsync(int? tradeTypeTypeId);
        Task<PagedResultDto<TradeListDto>> QueryTradesAsync(QueryTradeInput input);
        Task<DataTable> StatByPayTypeAsync(StatTradeByPayTypeInput input, IEnumerable<ComboboxItemDto<int>> payTypes);
        Task<DataTable> StatAsync(StatTradeInput input);
    }
}
