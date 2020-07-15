using Egoal.BackgroundJobs;
using Egoal.Dependency;
using Egoal.Domain.Uow;
using Egoal.Extensions;
using Egoal.Tickets;
using Egoal.Tickets.Dto;
using Egoal.UI;
using System.Threading;
using System.Threading.Tasks;

namespace Egoal.Orders
{
    public class InvoiceJob : IBackgroundJob, IScopedDependency
    {
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IOrderRepository _orderRepository;
        private readonly ITicketSaleAppService _ticketSaleAppService;

        public InvoiceJob(
            IUnitOfWorkManager unitOfWorkManager,
            IOrderRepository orderRepository,
            ITicketSaleAppService ticketSaleAppService)
        {
            _unitOfWorkManager = unitOfWorkManager;
            _orderRepository = orderRepository;
            _ticketSaleAppService = ticketSaleAppService;
        }

        public async Task ExecuteAsync(string args, CancellationToken stoppingToken)
        {
            using (var uow = _unitOfWorkManager.Begin())
            {
                var input = args.JsonToObject<InvoiceInput>();

                var order = await _orderRepository.FirstOrDefaultAsync(input.ListNo);
                if (order.InvoiceStatus != InvoiceStatus.开票中) return;

                try
                {
                    await _ticketSaleAppService.InvoiceAsync(input);

                    order.InvoiceStatus = InvoiceStatus.已开票;
                }
                catch (UserFriendlyException)
                {
                    order.InvoiceStatus = InvoiceStatus.未开票;
                }

                await uow.CompleteAsync();
            }
        }
    }
}
