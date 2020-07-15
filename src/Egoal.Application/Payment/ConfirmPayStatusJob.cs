using Egoal.BackgroundJobs;
using Egoal.Dependency;
using Egoal.Extensions;
using Egoal.Payment.Dto;
using System.Threading;
using System.Threading.Tasks;

namespace Egoal.Payment
{
    public class ConfirmPayStatusJob : IBackgroundJob, IScopedDependency
    {
        private readonly IPayAppService _payAppService;

        public ConfirmPayStatusJob(IPayAppService payAppService)
        {
            _payAppService = payAppService;
        }

        public async Task ExecuteAsync(string args, CancellationToken stoppingToken)
        {
            var input = args.JsonToObject<ConfirmPayStatusJobArgs>();
            await _payAppService.ConfirmPayStatusAsync(input);
        }
    }
}
