using Egoal.BackgroundJobs;
using Egoal.Customers.Dto;
using Egoal.Dependency;
using Egoal.Domain.Uow;
using Egoal.Extensions;
using Egoal.WeChat.Message;
using System.Threading;
using System.Threading.Tasks;

namespace Egoal.Customers
{
    public class SendAuditMessageJob : IBackgroundJob, IScopedDependency
    {
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly MessageService _messageService;

        public SendAuditMessageJob(
            IUnitOfWorkManager unitOfWorkManager,
            MessageService messageService)
        {
            _unitOfWorkManager = unitOfWorkManager;
            _messageService = messageService;
        }

        public async Task ExecuteAsync(string args, CancellationToken stoppingToken)
        {
            using (var uow = _unitOfWorkManager.Begin())
            {
                var input = args.JsonToObject<SendAuditMessageInput>();
                await _messageService.SendAuditMessageAsync(input.OpenId, input.Title, input.UserName, input.Mobile, input.Date, input.Remark);

                await uow.CompleteAsync();
            }
        }
    }
}
