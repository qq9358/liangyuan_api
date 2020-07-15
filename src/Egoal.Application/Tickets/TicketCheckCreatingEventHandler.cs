using Egoal.Dependency;
using Egoal.Events.Bus.Entities;
using Egoal.Events.Bus.Handlers;
using System.Threading.Tasks;

namespace Egoal.Tickets
{
    public class TicketCheckCreatingEventHandler : IAsyncEventHandler<EntityCreatingEventData<TicketCheck>>, IScopedDependency
    {
        private readonly ITicketCheckDayStatRepository _ticketCheckDayStatRepository;

        public TicketCheckCreatingEventHandler(ITicketCheckDayStatRepository ticketCheckDayStatRepository)
        {
            _ticketCheckDayStatRepository = ticketCheckDayStatRepository;
        }

        public async Task HandleEventAsync(EntityCreatingEventData<TicketCheck> eventData)
        {
            await AddTicketCheckDayStatAsync(eventData.Entity);
        }

        private async Task AddTicketCheckDayStatAsync(TicketCheck ticketCheck)
        {
            var ticketCheckDayStat = new TicketCheckDayStat();
            ticketCheckDayStat.CheckNum = ticketCheck.CheckNum ?? 0;
            ticketCheckDayStat.GroundId = ticketCheck.GroundId ?? 0;
            ticketCheckDayStat.GateGroupId = ticketCheck.GateGroupId ?? 0;
            ticketCheckDayStat.GateId = ticketCheck.GateId ?? 0;
            ticketCheckDayStat.InOutFlag = ticketCheck.InOutFlag ?? false;
            ticketCheckDayStat.CheckerId = ticketCheck.CheckerId ?? 0;
            ticketCheckDayStat.Cdate = ticketCheck.Cdate;
            ticketCheckDayStat.Ctp = ticketCheck.Ctp;
            await _ticketCheckDayStatRepository.InsertOrUpdateAsync(ticketCheckDayStat);
        }
    }
}
