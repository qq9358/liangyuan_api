using Egoal.Application.Services.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Egoal.Payment
{
    public interface IPayTypeAppService
    {
        Task EnsureSystemPayTypeAsync();
        Task<List<ComboboxItemDto<int>>> GetPayTypeComboboxItemsAsync();
        Task<List<ComboboxItemDto<int>>> GetNetPayTypeComboboxItemsAsync();
    }
}
