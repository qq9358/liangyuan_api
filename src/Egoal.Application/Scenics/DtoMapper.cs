using Egoal.AutoMapper;
using Egoal.Scenics.Dto;

namespace Egoal.Scenics
{
    public class DtoMapper : IAutoMap
    {
        public void CreateMappings()
        {
            CustomMapper.AutoMap<Scenic, ScenicDto>();
            CustomMapper.Bind<RegistGateInput, Gate>();
            CustomMapper.Bind<ChangeGateLocationInput, Gate>();
            CustomMapper.Bind<RegistPcInput, Pc>();
            CustomMapper.Bind<ChangePcLocationInput, Pc>();
        }
    }
}
