using Egoal.Authorization;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Egoal.Staffs
{
    public interface IStaffDomainService
    {
        Task<int?> GetRoleIdAsync(int id);
        Task<List<string>> GetPermissionsAsync(int id, SystemType? systemType);
        Task<Staff> LoginAsync(string uid, string password);
        void ValidatePassword(Staff staff, string password);
    }
}
