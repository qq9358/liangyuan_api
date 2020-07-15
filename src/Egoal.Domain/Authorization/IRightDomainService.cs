using System;
using System.Threading.Tasks;

namespace Egoal.Authorization
{
    public interface IRightDomainService
    {
        Task<bool> HasFeatureAsync(Guid feature);
    }
}
