using Egoal.Orders.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Egoal.Orders
{
    public interface IOrderDomainService
    {
        Task CreateAsync(Order order);
        Task ChangeChangCiAsync(Order order, int changCiId);
        Task ChangeQuantityAsync(Order order, int quantity);
        void ApplyInvoice(Order order);
        Task<bool> AllowCancelAsync(string listNo);
        bool AllowInvoice(Order order);
        bool AllowChangeChangCi(Order order);
        bool AllowChangeQuantity(Order order);
        Task CancelAsync(Order order);
        Task BeginExplainAsync(string listNo, int explainerId);
        Task ConsumeAsync(string listNo);
        Task ConsumeAsync(string listNo, long orderDetailId, int consumeNum);
        Task BookChangCiAsync(string travelDate, int changCiId, int quantity, bool cancel = false);
    }
}
