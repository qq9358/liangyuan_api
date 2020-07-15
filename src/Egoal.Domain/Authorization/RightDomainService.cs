using Egoal.Domain.Repositories;
using Egoal.Domain.Services;
using System;
using System.Threading.Tasks;

namespace Egoal.Authorization
{
    public class RightDomainService : DomainService, IRightDomainService
    {
        private readonly IRepository<Right> _rightRepository;

        public RightDomainService(IRepository<Right> rightRepository)
        {
            _rightRepository = rightRepository;

        }

        public async Task<bool> HasFeatureAsync(Guid feature)
        {
            return await _rightRepository.AnyAsync(r => r.UniqueCode == feature);
        }
    }
}
