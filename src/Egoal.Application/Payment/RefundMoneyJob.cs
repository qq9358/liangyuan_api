using Egoal.BackgroundJobs;
using Egoal.Dependency;
using Egoal.Domain.Uow;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Egoal.Payment
{
    public class RefundMoneyJob : IBackgroundJob, IScopedDependency
    {
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IPayAppService _payAppService;

        public RefundMoneyJob(
            IUnitOfWorkManager unitOfWorkManager,
            IPayAppService payAppService)
        {
            _unitOfWorkManager = unitOfWorkManager;
            _payAppService = payAppService;
        }

        public async Task ExecuteAsync(string args, CancellationToken stoppingToken)
        {
            using (var uow = _unitOfWorkManager.Begin())
            {
                await _payAppService.RefundAsync(Convert.ToInt64(args));

                await uow.CompleteAsync();
            }
        }
    }
}
