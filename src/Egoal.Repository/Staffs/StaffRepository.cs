using Dapper;
using Egoal.Application.Services.Dto;
using Egoal.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Egoal.Staffs
{
    public class StaffRepository : EfCoreRepositoryBase<Staff>, IStaffRepository
    {
        public StaffRepository(IServiceProvider serviceProvider)
            : base(serviceProvider)
        { }

        public async Task<int?> GetRoleIdAsync(int id)
        {
            string sql = @"SELECT TOP 1 RoleID FROM dbo.RM_StaffRole WHERE StaffID=@id";

            return await Connection.ExecuteScalarAsync<int?>(sql, new { id }, Transaction);
        }

        public async Task<List<ComboboxItemDto>> GetExplainerComboboxItemsAsync()
        {
            string sql = @"
SELECT
a.StaffID AS Value,
b.Name AS DisplayText
FROM dbo.RM_StaffExplainer a
JOIN dbo.RM_Staff b ON b.ID=a.StaffID
";
            return (await Connection.QueryAsync<ComboboxItemDto>(sql, null, Transaction)).ToList();
        }

        public async Task<List<ComboboxItemDto<int>>> GetCashierComboboxItemsAsync()
        {
            string sql = @"
SELECT
a.StaffID AS Value,
b.Name AS DisplayText
FROM dbo.RM_StaffCashier a
JOIN dbo.RM_Staff b ON b.ID=a.StaffID
";
            return (await Connection.QueryAsync<ComboboxItemDto<int>>(sql, null, Transaction)).ToList();
        }

        public async Task<List<ComboboxItemDto<int>>> GetSalesManComboboxItemsAsync()
        {
            string sql = @"
SELECT
a.StaffID AS Value,
b.Name AS DisplayText
FROM dbo.RM_StaffSalesman a
JOIN dbo.RM_Staff b ON b.ID=a.StaffID
";
            return (await Connection.QueryAsync<ComboboxItemDto<int>>(sql, null, Transaction)).ToList();
        }
    }
}
