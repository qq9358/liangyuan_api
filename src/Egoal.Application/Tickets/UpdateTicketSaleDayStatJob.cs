using Egoal.BackgroundJobs;
using Egoal.Dependency;
using Egoal.Domain.Uow;
using Egoal.Extensions;
using System.Threading;
using System.Threading.Tasks;

namespace Egoal.Tickets
{
    public class UpdateTicketSaleDayStatJob : IBackgroundJob, IScopedDependency
    {
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly ITicketSaleDayStatRepository _ticketSaleDayStatRepository;

        public UpdateTicketSaleDayStatJob(
            IUnitOfWorkManager unitOfWorkManager,
            ITicketSaleDayStatRepository ticketSaleDayStatRepository)
        {
            _unitOfWorkManager = unitOfWorkManager;
            _ticketSaleDayStatRepository = ticketSaleDayStatRepository;
        }

        public async Task ExecuteAsync(string args, CancellationToken stoppingToken)
        {
            using (var uow = _unitOfWorkManager.Begin())
            {
                var dayStat = args.JsonToObject<TicketSaleDayStat>();
                await _ticketSaleDayStatRepository.InsertOrUpdateAsync(dayStat);

                await uow.CompleteAsync();
            }
        }
    }
}
