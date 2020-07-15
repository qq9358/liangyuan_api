using Egoal.Orders.Dto;
using Egoal.Tickets.Dto;
using Egoal.TicketTypes;
using System.Threading.Tasks;

namespace Egoal.Orders
{
    public interface IOrderAppService
    {
        Task<CreateOrderOutput> CreateOrderAsync(CreateOrderInput input, SaleChannel saleChannel, OrderType orderType);
        Task PayOrderAsync(string listNo, int payTypeId);
        Task<string> CreateGroupOrderAsync(CreateGroupOrderInput input);
        Task ChangeChangCiAsync(ChangeChangCiInput input);
        Task ChangeQuantityAsync(ChangeQuantityInput input);
        Task CancelByUserAsync(CancelOrderInput input);
        Task CancelOrderAsync(CancelOrderInput input);
        Task ApplyInvoiceAsync(InvoiceInput input);
        Task ApplyRefundAsync(RefundOrderInput input);
    }
}
