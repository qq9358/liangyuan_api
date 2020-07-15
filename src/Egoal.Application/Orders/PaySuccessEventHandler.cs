using Egoal.Dependency;
using Egoal.Events.Bus.Handlers;
using Egoal.Payment;
using System.Threading.Tasks;

namespace Egoal.Orders
{
    public class PaySuccessEventHandler : IAsyncEventHandler<PaySuccessEventData>, IScopedDependency
    {
        private readonly IOrderAppService _orderAppService;

        public PaySuccessEventHandler(IOrderAppService orderAppService)
        {
            _orderAppService = orderAppService;
        }

        public async Task HandleEventAsync(PaySuccessEventData eventData)
        {
            if (eventData.Attach != NetPayAttach.BuyTicket)
            {
                return;
            }

            await _orderAppService.PayOrderAsync(eventData.ListNo, eventData.PayTypeId);
        }
    }
}
