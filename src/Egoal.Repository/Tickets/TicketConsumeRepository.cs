using Dapper;
using Egoal.Application.Services.Dto;
using Egoal.EntityFrameworkCore;
using Egoal.Extensions;
using Egoal.Tickets.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Egoal.Tickets
{
    public class TicketConsumeRepository : EfCoreRepositoryBase<TicketConsume, long>, ITicketConsumeRepository
    {
        public TicketConsumeRepository(IServiceProvider serviceProvider)
            : base(serviceProvider)
        { }

        public async Task<PagedResultDto<TicketConsumeListDto>> QueryTicketConsumesAsync(QueryTicketConsumeInput input)
        {
            var where = new StringBuilder();
            where.AppendWhereIf(input.StartCheckTime.HasValue, "a.ConsumeTime>=@StartCheckTime");
            where.AppendWhereIf(input.EndCheckTime.HasValue, "a.ConsumeTime<=@EndCheckTime");
            where.AppendWhereIf(input.StartConsumeTime.HasValue, "a.LastNoticeTime>=@StartConsumeTime");
            where.AppendWhereIf(input.EndConsumeTime.HasValue, "a.LastNoticeTime<=@EndConsumeTime");
            where.AppendWhereIf(input.StartTravelDate.HasValue, "b.STime>=@StartTravelDate");
            where.AppendWhereIf(input.EndTravelDate.HasValue, "b.STime<=@EndTravelDate");
            where.AppendWhereIf(!input.ListNo.IsNullOrEmpty(), "b.ListNo=@ListNo OR b.OrderListNo=@ListNo OR a.ThirdPartyPlatformOrderID=@ListNo");
            where.AppendWhereIf(!input.TicketCode.IsNullOrEmpty(), "a.CardNo=@TicketCode");
            where.AppendWhereIf(input.TicketTypeId.HasValue, "a.TicketTypeId=@TicketTypeId");
            where.AppendWhereIf(input.CustomerId.HasValue, "b.CustomerID=@CustomerId");
            where.AppendWhereIf(input.ConsumeType.HasValue, "a.ConsumeType=@ConsumeType");
            where.AppendWhereIf(input.HasNoticed.HasValue, "a.HasNoticed=@HasNoticed");

            string sql = $@"
SELECT
a.Id,
a.TicketTypeId,
a.CardNo AS TicketCode,
a.Price,
a.ConsumeNum,
b.PersonNum AS TotalNum,
a.ConsumeType,
b.CustomerName,
b.ListNo,
a.ThirdPartyPlatformOrderID AS ThirdOrderId,
a.ConsumeTime,
a.LastNoticeTime,
ROW_NUMBER() OVER(ORDER BY a.Id DESC) AS RowNum
FROM dbo.TM_TicketConsume a WITH(NOLOCK)
LEFT JOIN dbo.TM_TicketSale b WITH(NOLOCK) ON b.ID=a.TicketId
{where}
";
            string pagedSql = $@"
SELECT
*
FROM
(
    {sql}
)x
WHERE x.RowNum BETWEEN @StartRowNum AND @EndRowNum
";
            string countSql = $@"
SELECT COUNT(*)
FROM dbo.TM_TicketConsume a WITH(NOLOCK)
LEFT JOIN dbo.TM_TicketSale b WITH(NOLOCK) ON b.ID=a.TicketId
{where}
";
            if (input.ShouldPage)
            {
                int count = await Connection.ExecuteScalarAsync<int>(countSql, input, Transaction);
                var items = await Connection.QueryAsync<TicketConsumeListDto>(pagedSql, input, Transaction);

                return new PagedResultDto<TicketConsumeListDto>(count, items.ToList());
            }
            else
            {
                var items = await Connection.QueryAsync<TicketConsumeListDto>(sql, input, Transaction);

                return new PagedResultDto<TicketConsumeListDto>(items.Count(), items.ToList());
            }
        }

        public async Task<List<StatTicketConsumeListDto>> StatTicketConsumeAsync(StatTicketConsumeInput input)
        {
            var where = new StringBuilder();
            where.AppendWhereIf(input.StartCheckTime.HasValue, "a.ConsumeTime>=@StartCheckTime");
            where.AppendWhereIf(input.EndCheckTime.HasValue, "a.ConsumeTime<=@EndCheckTime");
            where.AppendWhereIf(input.StartConsumeTime.HasValue, "a.LastNoticeTime>=@StartConsumeTime");
            where.AppendWhereIf(input.EndConsumeTime.HasValue, "a.LastNoticeTime<=@EndConsumeTime");
            where.AppendWhereIf(input.TicketTypeId.HasValue, "a.TicketTypeId=@TicketTypeId");
            where.AppendWhereIf(input.CustomerId.HasValue, "b.CustomerID=@CustomerId");
            where.AppendWhereIf(input.ConsumeType.HasValue, "a.ConsumeType=@ConsumeType");

            string sql = $@"
SELECT
b.CustomerID,
a.TicketTypeId,
a.Price,
SUM(a.ConsumeNum) AS CheckNum,
SUM(CASE WHEN a.HasNoticed=1 THEN a.ConsumeNum ELSE 0 END) AS ConsumeNum
FROM dbo.TM_TicketConsume a WITH(NOLOCK)
LEFT JOIN dbo.TM_TicketSale b WITH(NOLOCK) ON b.ID=a.TicketId
{where}
GROUP BY b.CustomerID,a.TicketTypeId,a.Price
ORDER BY b.CustomerID
";
            return (await Connection.QueryAsync<StatTicketConsumeListDto>(sql, input, Transaction)).ToList();
        }
    }
}
