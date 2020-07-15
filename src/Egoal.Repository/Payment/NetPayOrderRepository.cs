using Dapper;
using Egoal.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Egoal.Extensions;

namespace Egoal.Payment
{
    public class NetPayOrderRepository : EfCoreRepositoryBase<NetPayOrder, long>, INetPayOrderRepository
    {
        public NetPayOrderRepository(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
        }

        public async Task<bool> SetPayTypeAsync(long id, int payTypeId, OnlinePayTradeType onlinePayTradeType, NetPayType? netPayTypeId = null, string netPayTypeName = null)
        {
            string sql = @"
UPDATE dbo.OM_NetPayOrder SET
PayTypeId=@payTypeId,
OnlinePayTradeType=@onlinePayTradeType,
NetPayTypeID=@netPayTypeId,
NetPayTypeName=@netPayTypeName
WHERE ID=@id
";
            var param = new { id, payTypeId, onlinePayTradeType, netPayTypeId, netPayTypeName };
            return (await Connection.ExecuteAsync(sql, param, Transaction)) > 0;
        }

        public async Task<bool> SetPayArgsAsync(long id, string payArgs)
        {
            if (!payArgs.IsJson()) return true;

            string sql = @"
UPDATE dbo.OM_NetPayOrder SET
PayArgs=@payArgs
WHERE ID=@id
";
            return (await Connection.ExecuteAsync(sql, new { id, payArgs }, Transaction)) > 0;
        }

        public async Task<bool> SetJsApiPayArgsAsync(long id, string jsApiPayArgs)
        {
            if (!jsApiPayArgs.IsJson()) return true;

            string sql = @"
UPDATE dbo.OM_NetPayOrder SET
JsApiPayArgs=@jsApiPayArgs
WHERE ID=@id
";
            return (await Connection.ExecuteAsync(sql, new { id, jsApiPayArgs }, Transaction)) > 0;
        }
    }
}
