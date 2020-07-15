using Egoal.Application.Services.Dto;
using Egoal.Authorization;
using Egoal.Staffs.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Egoal.Staffs
{
    public interface IStaffAppService
    {
        Task<LoginOutput> LoginAsync(LoginInput input, SystemType systemType);
        Task EditPasswordAsync(EditPasswordInput input);
        Task<List<ComboboxItemDto<int>>> GetCashierComboboxItemsAsync();
        Task<List<ComboboxItemDto<int>>> GetSalesManComboboxItemsAsync();
    }
}
