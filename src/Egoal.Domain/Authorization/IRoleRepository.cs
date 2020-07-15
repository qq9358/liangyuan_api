using Egoal.Domain.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Egoal.Authorization
{
    public interface IRoleRepository : IRepository<Role>
    {
        Task<List<string>> GetPermissionsAsync(int roleId, SystemType? systemType);
        Task<bool> IsGrantedAsync(int roleId, string permission);
    }
}
