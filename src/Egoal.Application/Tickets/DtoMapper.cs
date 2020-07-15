using Egoal.AutoMapper;
using Egoal.Tickets.Dto;

namespace Egoal.Tickets
{
    public class DtoMapper : IAutoMap
    {
        public void CreateMappings()
        {
            CustomMapper.Bind<RefundTicketInput, RefundTicketEventData>();
            CustomMapper.Bind<RefundTicketItem, RefundTicketEventData.RefundTicketItem>();
        }
    }
}
