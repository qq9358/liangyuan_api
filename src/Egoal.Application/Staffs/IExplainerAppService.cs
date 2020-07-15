using Egoal.Application.Services.Dto;
using Egoal.Dto;
using Egoal.Staffs.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Egoal.Staffs
{
    public interface IExplainerAppService
    {
        Task<List<VantPickerItem<int>>> GetSchedulingsAsync(GetSchedulingInput input);
        Task<List<NameValue>> GetReservedSchedulingComboxItemsAsync(GetSchedulingInput input);
        Task<List<ComboboxItemDto>> GetExplainTimeslotComboboxItemsAsync();
        Task<List<ComboboxItemDto>> GetExplainerComboboxItemsAsync();
        Task BeginExplainAsync(ExplainInput input);
        Task CompleteExplainAsync(ExplainInput input);
    }
}
