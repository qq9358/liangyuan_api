using Egoal.BackgroundJobs;
using Egoal.Dependency;
using Egoal.Domain.Repositories;
using Egoal.Domain.Uow;
using Egoal.Extensions;
using Egoal.Messages;
using Egoal.Messages.Dto;
using Egoal.Tickets.Dto;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Egoal.Tickets
{
    public class SendInvoiceMessageJob : IBackgroundJob, IScopedDependency
    {
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IEmailAppService _emailAppService;
        private readonly ITicketSaleQueryAppService _ticketSaleQueryAppService;
        private readonly IShortMessageAppService _shortMessageAppService;
        private readonly IRepository<ReceiveInvoiceEmailDomainBlacklist, long> _emailBlacklistRepository;

        public SendInvoiceMessageJob(
            IUnitOfWorkManager unitOfWorkManager,
            IEmailAppService emailAppService,
            ITicketSaleQueryAppService ticketSaleQueryAppService,
            IShortMessageAppService shortMessageAppService,
            IRepository<ReceiveInvoiceEmailDomainBlacklist, long> emailBlacklistRepository)
        {
            _unitOfWorkManager = unitOfWorkManager;
            _emailAppService = emailAppService;
            _ticketSaleQueryAppService = ticketSaleQueryAppService;
            _shortMessageAppService = shortMessageAppService;
            _emailBlacklistRepository = emailBlacklistRepository;
        }

        public async Task ExecuteAsync(string args, CancellationToken stoppingToken)
        {
            using (var uow = _unitOfWorkManager.Begin())
            {
                var input = args.JsonToObject<SendInvoiceMessageInput>();

                if (!input.Email.IsNullOrEmpty())
                {
                    var emailDomain = input.Email.Split("@")[1];
                    var emailBlacklist = await _emailBlacklistRepository.GetAll().Select(b => b.EmailDomain).ToListAsync();
                    if (emailBlacklist.Any(e => emailDomain.Contains(e)))
                    {
                        input.InvoiceUrl = await _ticketSaleQueryAppService.DownloadInvoiceAsync(new DownloadInvoiceInput { ListNo = input.ListNo });

                        await _emailAppService.SendInvoiceMessageAsync(input);
                    }
                }
                else if (!input.Mobile.IsNullOrEmpty())
                {
                    input.InvoiceUrl = await _ticketSaleQueryAppService.DownloadInvoiceAsync(new DownloadInvoiceInput { ListNo = input.ListNo });
                }

                await uow.CompleteAsync();
            }
        }
    }
}
