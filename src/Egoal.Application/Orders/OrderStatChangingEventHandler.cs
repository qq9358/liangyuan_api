using Egoal.BackgroundJobs;
using Egoal.Dependency;
using Egoal.Events.Bus.Handlers;
using Egoal.Extensions;
using System.Threading.Tasks;

namespace Egoal.Orders
{
    public class OrderStatChangingEventHandler : IAsyncEventHandler<OrderStatChangingEventData>, IScopedDependency
    {
        private readonly IBackgroundJobService _backgroundJobService;

        public OrderStatChangingEventHandler(IBackgroundJobService backgroundJobService)
        {
            _backgroundJobService = backgroundJobService;
        }

        public async Task HandleEventAsync(OrderStatChangingEventData eventData)
        {
            await _backgroundJobService.EnqueueAsync<UpdateOrderStatJob>(eventData.OrderStat.ToJson());
        }
    }
}
