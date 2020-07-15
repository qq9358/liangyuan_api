using Egoal.Dependency;
using Egoal.Domain.Repositories;
using Egoal.Events.Bus.Handlers;
using Egoal.Orders;
using Egoal.Scenics.Dto;
using Egoal.Tickets;
using System.Threading.Tasks;

namespace Egoal.Scenics
{
    public class RefundTicketEventHandler : IAsyncEventHandler<RefundTicketEventData>, IScopedDependency
    {
        private readonly IRepository<OrderDetail, long> _orderDetailRepository;
        private readonly IRepository<OrderGroundChangCi, long> _orderGroundChangCiRepository;
        private readonly IScenicAppService _scenicAppService;

        public RefundTicketEventHandler(
            IRepository<OrderDetail, long> orderDetailRepository,
            IRepository<OrderGroundChangCi, long> orderGroundChangCiRepository,
            IScenicAppService scenicAppService)
        {
            _orderDetailRepository = orderDetailRepository;
            _orderGroundChangCiRepository = orderGroundChangCiRepository;
            _scenicAppService = scenicAppService;
        }

        public async Task HandleEventAsync(RefundTicketEventData eventData)
        {
            foreach (var item in eventData.Items)
            {
                var ticketSale = item.OriginalTicketSale;

                if (ticketSale.OrderDetailId.HasValue)
                {
                    var orderDetail = await _orderDetailRepository.FirstOrDefaultAsync(ticketSale.OrderDetailId.Value);

                    if (!orderDetail.HasGroundSeat && !orderDetail.HasGroundChangCi)
                    {
                        continue;
                    }

                    var groundChangCis = await _orderGroundChangCiRepository.GetAllListAsync(o => o.OrderDetailId == orderDetail.Id);
                    foreach (var groundChangCi in groundChangCis)
                    {
                        CancelGroundChangCiInput cancelGroundChangCiInput = new CancelGroundChangCiInput();
                        cancelGroundChangCiInput.HasGroundSeat = orderDetail.HasGroundSeat;
                        cancelGroundChangCiInput.HasGroundChangCi = orderDetail.HasGroundChangCi;
                        cancelGroundChangCiInput.ListNo = ticketSale.OrderListNo;
                        cancelGroundChangCiInput.TicketId = ticketSale.Id;
                        cancelGroundChangCiInput.GroundId = groundChangCi.GroundId;
                        cancelGroundChangCiInput.Date = ticketSale.Sdate;
                        cancelGroundChangCiInput.ChangCiId = groundChangCi.ChangCiId;
                        cancelGroundChangCiInput.Quantity = item.RefundQuantity;

                        await _scenicAppService.CancelGroundChangCiAsync(cancelGroundChangCiInput);
                    }
                }
            }
        }
    }
}
