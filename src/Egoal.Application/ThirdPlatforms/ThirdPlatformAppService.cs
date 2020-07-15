using Egoal.Application.Services;
using Egoal.AutoMapper;
using Egoal.Domain.Repositories;
using Egoal.Extensions;
using Egoal.ThirdPlatforms.Dto;
using Egoal.UI;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Egoal.ThirdPlatforms
{
    public class ThirdPlatformAppService : ApplicationService, IThirdPlatformAppService
    {
        private readonly IRepository<ThirdPlatform, string> _thirdPlatformRepository;

        public ThirdPlatformAppService(IRepository<ThirdPlatform, string> thirdPlatformRepository)
        {
            _thirdPlatformRepository = thirdPlatformRepository;
        }

        public async Task CreateAsync(ThirdPlatformDto input)
        {
            if (await _thirdPlatformRepository.AnyAsync(t => t.Uid == input.Uid))
            {
                throw new UserFriendlyException($"用户名{input.Uid}已存在");
            }

            var thirdPlatform = input.MapTo<ThirdPlatform>();
            thirdPlatform.Id = input.Uid;

            await _thirdPlatformRepository.InsertAsync(thirdPlatform);
        }

        public async Task UpdateAsync(ThirdPlatformDto input)
        {
            if (await _thirdPlatformRepository.AnyAsync(t => t.Uid == input.Uid && t.Id != input.Id))
            {
                throw new UserFriendlyException($"用户名{input.Uid}已存在");
            }

            var thirdPlatform = await _thirdPlatformRepository.FirstOrDefaultAsync(input.Id);
            input.MapTo(thirdPlatform);
        }

        public async Task DeleteAsync(string id)
        {
            await _thirdPlatformRepository.DeleteAsync(id);
        }

        public async Task<List<ThirdPlatformDto>> GetThirdPlatformsAsync(string uid)
        {
            var query = _thirdPlatformRepository.GetAll()
                .WhereIf(!uid.IsNullOrEmpty(), t => t.Uid.Contains(uid));

            var thirdPlatforms = await _thirdPlatformRepository.ToListAsync(query);

            var items = thirdPlatforms.MapTo<ThirdPlatformDto>();
            foreach (var item in items)
            {
                item.PlatformTypeName = item.PlatformType.ToString();
            }

            return items;
        }

        public async Task<ThirdPlatformDto> GetThirdPlatformForEditAsync(string id)
        {
            var thirdPlatform = await _thirdPlatformRepository.FirstOrDefaultAsync(id);

            return thirdPlatform.MapTo<ThirdPlatformDto>();
        }
    }
}
