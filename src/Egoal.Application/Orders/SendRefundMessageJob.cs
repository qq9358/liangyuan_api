using Egoal.BackgroundJobs;
using Egoal.Dependency;
using Egoal.Domain.Uow;
using Egoal.Extensions;
using Egoal.Messages;
using Egoal.Orders.Dto;
using System.Threading;
using System.Threading.Tasks;

namespace Egoal.Orders
{
    public class SendRefundMessageJob : IBackgroundJob, IScopedDependency
    {
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IShortMessageAppService _shortMessageAppService;

        public SendRefundMessageJob(
            IUnitOfWorkManager unitOfWorkManager,
            IShortMessageAppService shortMessageAppService)
        {
            _unitOfWorkManager = unitOfWorkManager;
            _shortMessageAppService = shortMessageAppService;
        }

        public async Task ExecuteAsync(string args, CancellationToken stoppingToken)
        {
            using (var uow = _unitOfWorkManager.Begin())
            {
                var message = args.JsonToObject<SendRefundMessageArgs>();

                await _shortMessageAppService.SendRefundMessageAsync(message.Mobile, message.ETime, message.RefundReason);

                await uow.CompleteAsync();
            }
        }
    }
}
