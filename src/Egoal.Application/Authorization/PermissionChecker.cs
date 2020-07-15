using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Egoal.Authorization
{
    public class PermissionChecker : IPermissionChecker
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IMemoryCache _memoryCache;

        public PermissionChecker(
            IRoleRepository roleRepository,
            IMemoryCache memoryCache)
        {
            _roleRepository = roleRepository;
            _memoryCache = memoryCache;
        }

        public async Task<bool> IsGrantedAsync(int roleId, string permission)
        {
            //var permissions = await GetPermissionsAsync(roleId);

            //return permissions.Any(p => p.Equals(permission, StringComparison.OrdinalIgnoreCase));

            return await _roleRepository.IsGrantedAsync(roleId, permission);
        }

        private async Task<List<string>> GetPermissionsAsync(int roleId)
        {
            return await _memoryCache.GetOrCreateAsync($"Permission:Role:{roleId}", entry =>
            {
                entry.SlidingExpiration = TimeSpan.FromMinutes(10);

                return _roleRepository.GetPermissionsAsync(roleId, null);
            });
        }
    }
}
