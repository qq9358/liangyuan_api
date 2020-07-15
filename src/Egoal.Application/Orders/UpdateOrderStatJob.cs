using Egoal.BackgroundJobs;
using Egoal.Dependency;
using Egoal.Domain.Uow;
using Egoal.Extensions;
using System.Threading;
using System.Threading.Tasks;

namespace Egoal.Orders
{
    public class UpdateOrderStatJob : IBackgroundJob, IScopedDependency
    {
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IOrderStatRepository _orderStatRepository;

        public UpdateOrderStatJob(
            IUnitOfWorkManager unitOfWorkManager,
            IOrderStatRepository orderStatRepository)
        {
            _unitOfWorkManager = unitOfWorkManager;
            _orderStatRepository = orderStatRepository;
        }

        public async Task ExecuteAsync(string args, CancellationToken stoppingToken)
        {
            using (var uow = _unitOfWorkManager.Begin())
            {
                var orderStat = args.JsonToObject<OrderStat>();
                await _orderStatRepository.InsertOrUpdateAsync(orderStat);

                await uow.CompleteAsync();
            }
        }
    }
}
