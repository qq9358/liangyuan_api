using Egoal.Domain.Repositories;
using System.Threading.Tasks;

namespace Egoal.Payment
{
    public interface INetPayOrderRepository : IRepository<NetPayOrder, long>
    {
        Task<bool> SetPayTypeAsync(long id, int payTypeId, OnlinePayTradeType onlinePayTradeType, NetPayType? netPayTypeId = null, string netPayTypeName = null);
        Task<bool> SetPayArgsAsync(long id, string payArgs);
        Task<bool> SetJsApiPayArgsAsync(long id, string jsApiPayArgs);
    }
}
