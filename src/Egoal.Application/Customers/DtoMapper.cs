using Egoal.AutoMapper;
using Egoal.Customers.Dto;
using Egoal.Members;

namespace Egoal.Customers
{
    public class DtoMapper : IAutoMap
    {
        public void CreateMappings()
        {
            CustomMapper.Bind<RegistGuiderInput, Member>();
            CustomMapper.AutoMap<EditGuiderDto, Member>();
        }
    }
}
