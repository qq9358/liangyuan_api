using Egoal.BackgroundJobs;
using Egoal.Dependency;
using Egoal.Domain.Repositories;
using Egoal.Events.Bus.Handlers;
using Egoal.Extensions;
using Egoal.Tickets;
using System;
using System.Threading.Tasks;

namespace Egoal.Orders
{
    public class TicketConsumingEventHandler : IAsyncEventHandler<TicketConsumingEventData>, IScopedDependency
    {
        private readonly IOrderDomainService _orderDomainService;
        private readonly IRepository<AbpBackgroundJob, long> _backgroundJobRepository;

        public TicketConsumingEventHandler(
            IOrderDomainService orderDomainService,
            IRepository<AbpBackgroundJob, long> backgroundJobRepository)
        {
            _orderDomainService = orderDomainService;
            _backgroundJobRepository = backgroundJobRepository;
        }

        public async Task HandleEventAsync(TicketConsumingEventData eventData)
        {
            await ConsumeOrderAsync(eventData);
        }

        private async Task ConsumeOrderAsync(TicketConsumingEventData eventData)
        {
            if (eventData.OrderListNo.IsNullOrEmpty())
            {
                return;
            }

            await _orderDomainService.ConsumeAsync(eventData.OrderListNo, eventData.OrderDetailId.Value, eventData.TicketConsume.ConsumeNum);

            if (eventData.OrderListNo.StartsWith("ds", StringComparison.OrdinalIgnoreCase))
            {
                var job = new AbpBackgroundJob();
                job.JobType = "Egoal.Tms.TmsDsSync.SyncOrderJob, Egoal.Tms.Application, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
                job.JobArgs = new
                {
                    Data = new
                    {
                        TicketCode = eventData.TicketConsume.CardNo,
                        eventData.TotalConsumeNum,
                        eventData.TicketConsume.ConsumeNum,
                        TicketConsumeId = eventData.TicketConsume.Id
                    }.ToJson(),
                    SyncType = 4
                }.ToJson();

                await _backgroundJobRepository.InsertAsync(job);
            }
        }
    }
}
