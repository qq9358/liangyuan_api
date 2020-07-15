using Dapper;
using Egoal.EntityFrameworkCore;
using Egoal.Tickets.Dto;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using Egoal.Extensions;

namespace Egoal.Tickets
{
    public class TicketSaleSeatRepository : EfCoreRepositoryBase<TicketSaleSeat, long>, ITicketSaleSeatRepository
    {
        public TicketSaleSeatRepository(IServiceProvider serviceProvider)
            : base(serviceProvider)
        { }

        public async Task<List<TicketSaleSeatDto>> GetTicketSeatsAsync(GetTicketSeatsInput input)
        {
            StringBuilder where = new StringBuilder();
            where.AppendWhereIf(!input.ListNo.IsNullOrEmpty(), "d.OrderListNo=@ListNo");
            where.AppendWhereIf(input.TicketId.HasValue, "d.ID=@TicketId");

            string sql = $@"
SELECT
a.TradeID,
a.TicketID,
a.SeatID,
a.SDate,
a.ChangCiID,
c.ID AS GroundId
FROM dbo.TM_TicketSaleSeat a WITH(NOLOCK)
LEFT JOIN dbo.SS_Seat b ON a.SeatID=b.ID
LEFT JOIN dbo.VM_Ground c ON b.StadiumID=c.StadiumID
LEFT JOIN dbo.TM_TicketSale d WITH(NOLOCK) ON a.TicketID=d.ID
{where}
";
            return (await Connection.QueryAsync<TicketSaleSeatDto>(sql, input, Transaction)).ToList();
        }

        public async Task<DataTable> StatGroundChangCiSaleAsync(StatGroundChangCiSaleInput input)
        {
            string sql = $@"
SELECT
a.GroundID,
a.ChangCiID,
c.STime,
c.ETime,
MAX(b.SeatNum) AS TotalNum,
SUM(d.SaleNum) AS SaleNum,
0 AS SurplusNum
FROM dbo.TM_WeekChangCiPlan a WITH(NOLOCK)
LEFT JOIN dbo.VM_Ground b WITH(NOLOCK) ON a.GroundID=b.ID
LEFT JOIN dbo.TM_ChangCi c WITH(NOLOCK) ON a.ChangCiID=c.ID
LEFT JOIN
(
	SELECT * FROM dbo.TM_GroundDateChangCiSaleNum WITH(NOLOCK) WHERE Date=@TravelDate
)d
ON a.GroundID=d.GroundID AND a.ChangCiID=d.ChangCiID
WHERE a.Week=@Week AND b.ChangCiSaleFlag=1 AND b.SeatSaleFlag=0
GROUP BY a.GroundID,a.ChangCiID,c.STime,c.ETime
UNION ALL
SELECT
a.GroundID,
a.ChangCiID,
c.STime,
c.ETime,
COUNT(DISTINCT e.ID) AS TotalNum,
COUNT(DISTINCT f.ID) AS SaleNum,
0 AS SurplusNum
FROM dbo.TM_WeekChangCiPlan a WITH(NOLOCK)
LEFT JOIN dbo.VM_Ground b WITH(NOLOCK) ON a.GroundID=b.ID
LEFT JOIN dbo.TM_ChangCi c WITH(NOLOCK) ON a.ChangCiID=c.ID
LEFT JOIN dbo.SS_Stadium d WITH(NOLOCK) ON b.StadiumID=d.ID
LEFT JOIN dbo.SS_Seat e WITH(NOLOCK) ON d.ID=e.StadiumID
LEFT JOIN
(
	SELECT * FROM dbo.TM_TicketSaleSeat WITH(NOLOCK) WHERE SDate=@TravelDate
)f
ON e.ID=f.SeatID AND a.ChangCiID=f.ChangCiID
WHERE a.Week=@Week AND b.SeatSaleFlag=1 AND e.RegionID>0
GROUP BY a.GroundID,a.ChangCiID,c.STime,c.ETime
ORDER BY a.GroundID,c.STime
";
            var reader = await Connection.ExecuteReaderAsync(sql, input, Transaction);
            var dataTable = new DataTable();
            dataTable.Load(reader);

            return dataTable;
        }
    }
}
