using Egoal.Dependency;
using Egoal.Events.Bus.Handlers;
using Egoal.Payment;
using System.Threading.Tasks;

namespace Egoal.Orders
{
    public class PayTimeoutEventHandler : IAsyncEventHandler<PayTimeoutEventData>, IScopedDependency
    {
        private readonly IOrderAppService _orderAppService;

        public PayTimeoutEventHandler(IOrderAppService orderAppService)
        {
            _orderAppService = orderAppService;
        }

        public async Task HandleEventAsync(PayTimeoutEventData eventData)
        {
            if (eventData.Attach != NetPayAttach.BuyTicket)
            {
                return;
            }

            await _orderAppService.CancelOrderAsync(new Dto.CancelOrderInput { ListNo = eventData.ListNo });
        }
    }
}
