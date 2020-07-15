using Egoal.Dependency;
using System.Threading.Tasks;

namespace Egoal.Authorization
{
    public interface IPermissionChecker : IScopedDependency
    {
        Task<bool> IsGrantedAsync(int roleId, string permission);
    }
}
