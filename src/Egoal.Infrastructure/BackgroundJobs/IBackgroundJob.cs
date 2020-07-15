using System.Threading;
using System.Threading.Tasks;

namespace Egoal.BackgroundJobs
{
    public interface IBackgroundJob
    {
        Task ExecuteAsync(string args, CancellationToken stoppingToken);
    }
}
