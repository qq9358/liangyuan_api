using Egoal.Application.Services.Dto;
using Egoal.Tickets.Dto;
using Egoal.Trades.Dto;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Egoal.Trades
{
    public interface ITradeAppService
    {
        Task SaleTicketAsync(SaleTicketInput input);
        Task RefundTicketAsync(RefundTicketInput input);
        Task<List<ComboboxItemDto<int>>> GetTradeTypeTypeComboboxItemsAsync();
        Task<List<ComboboxItemDto<int>>> GetTradeTypeComboboxItemsAsync(int? tradeTypeTypeId);
        Task<PagedResultDto<TradeListDto>> QueryTradesAsync(QueryTradeInput input);
        Task<byte[]> StatPayDetailToExcelAsync(StatPayDetailInput input);
        Task<DynamicColumnResultDto> StatPayDetailAsync(StatPayDetailInput input);
        Task<DataTable> StatPayDetailJbAsync(StatJbInput input);
        Task<DynamicColumnResultDto> StatTradeAsync(StatTradeInput input);
        Task<byte[]> StatTradeByPayTypeToExcelAsync(StatTradeByPayTypeInput input);
        Task<DynamicColumnResultDto> StatTradeByPayTypeAsync(StatTradeByPayTypeInput input);
    }
}
