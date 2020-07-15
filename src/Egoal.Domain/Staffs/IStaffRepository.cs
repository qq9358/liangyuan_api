using Egoal.Application.Services.Dto;
using Egoal.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Egoal.Staffs
{
    public interface IStaffRepository : IRepository<Staff>
    {
        Task<int?> GetRoleIdAsync(int id);
        Task<List<ComboboxItemDto>> GetExplainerComboboxItemsAsync();
        Task<List<ComboboxItemDto<int>>> GetCashierComboboxItemsAsync();
        Task<List<ComboboxItemDto<int>>> GetSalesManComboboxItemsAsync();
    }
}
