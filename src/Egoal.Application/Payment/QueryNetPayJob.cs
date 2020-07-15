using Egoal.BackgroundJobs;
using Egoal.Dependency;
using Egoal.Extensions;
using Egoal.Payment.Dto;
using System.Threading;
using System.Threading.Tasks;

namespace Egoal.Payment
{
    public class QueryNetPayJob : IBackgroundJob, IScopedDependency
    {
        private readonly IPayAppService _payAppService;

        public QueryNetPayJob(IPayAppService payAppService)
        {
            _payAppService = payAppService;
        }

        public async Task ExecuteAsync(string args, CancellationToken stoppingToken)
        {
            var input = args.JsonToObject<QueryNetPayJobArgs>();
            await _payAppService.LoopQueryNetPayAsync(input);
        }
    }
}
