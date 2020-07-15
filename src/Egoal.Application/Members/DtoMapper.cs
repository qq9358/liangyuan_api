using Egoal.AutoMapper;
using Egoal.Members.Dto;

namespace Egoal.Members
{
    public class DtoMapper : IAutoMap
    {
        public void CreateMappings()
        {
            CustomMapper.AutoMap<Member, MemberDto>();
        }
    }
}
