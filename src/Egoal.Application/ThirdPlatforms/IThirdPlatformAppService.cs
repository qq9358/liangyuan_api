using Egoal.ThirdPlatforms.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Egoal.ThirdPlatforms
{
    public interface IThirdPlatformAppService
    {
        Task CreateAsync(ThirdPlatformDto input);
        Task UpdateAsync(ThirdPlatformDto input);
        Task DeleteAsync(string id);
        Task<List<ThirdPlatformDto>> GetThirdPlatformsAsync(string uid);
        Task<ThirdPlatformDto> GetThirdPlatformForEditAsync(string id);
    }
}
