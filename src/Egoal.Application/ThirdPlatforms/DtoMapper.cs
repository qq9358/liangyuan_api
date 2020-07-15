using Egoal.AutoMapper;
using Egoal.ThirdPlatforms.Dto;

namespace Egoal.ThirdPlatforms
{
    public class DtoMapper : IAutoMap
    {
        public void CreateMappings()
        {
            CustomMapper.AutoMap<ThirdPlatform, ThirdPlatformDto>();
        }
    }
}
