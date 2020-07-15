using Dapper;
using Egoal.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Egoal.Orders
{
    public class OrderStatRepository : EfCoreRepositoryBase<OrderStat>, IOrderStatRepository
    {
        public OrderStatRepository(IServiceProvider serviceProvider)
            : base(serviceProvider)
        { }

        public async override Task<OrderStat> InsertOrUpdateAsync(OrderStat entity)
        {
            string sql = @"
UPDATE dbo.OM_OrderStat SET
OrderNum=OrderNum+@OrderNum
WHERE CDate=@Cdate
AND OrderPlanType=@OrderPlanType

IF @@ROWCOUNT=0
BEGIN
    INSERT INTO dbo.OM_OrderStat
    (
        CDate,
        OrderNum,
        OrderPlanType
    )
    VALUES
    (   @Cdate,
        @OrderNum,
        @OrderPlanType
    )
END
";
            await Connection.ExecuteAsync(sql, entity, Transaction);

            return entity;
        }

        public Task<int> GetOrderQuantityAsync(string date)
        {
            string sql = @"
SELECT
ISNULL(SUM(OrderNum),0) AS Quantity
FROM dbo.OM_OrderStat WITH(UPDLOCK)
WHERE CDate=@date
";
            return Connection.ExecuteScalarAsync<int>(sql, new { date }, Transaction);
        }
    }
}
