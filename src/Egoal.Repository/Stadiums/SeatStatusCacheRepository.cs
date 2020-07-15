using Dapper;
using Egoal.EntityFrameworkCore;
using Egoal.Tickets.Dto;
using Egoal.UI;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Egoal.Stadiums
{
    public class SeatStatusCacheRepository : EfCoreRepositoryBase<SeatStatusCache, decimal>, ISeatStatusCacheRepository
    {
        public SeatStatusCacheRepository(IServiceProvider serviceProvider)
            : base(serviceProvider)
        { }

        public async Task<int> GetDisabledSeatQuantityAsync(DateTime date, int changCiId, int stadiumId, int lockTime)
        {
            string sql = $@"
SELECT
SUM(x.SaleNum)
FROM
(
	SELECT
	COUNT(*) AS SaleNum
	FROM dbo.TM_TicketSaleSeatStatusCache a WITH(NOLOCK)
	LEFT JOIN dbo.SS_Seat b WITH(NOLOCK) ON a.SeatID=b.ID
	WHERE a.SDate=@date
	AND a.ChangCiID=@changCiId
	AND a.StatusID<>1
	AND a.StatusID<>3
	AND b.StadiumID=@stadiumId
	UNION ALL
	SELECT
	COUNT(*) AS SaleNum
	FROM dbo.TM_TicketSaleSeatStatusCache a WITH(NOLOCK)
	LEFT JOIN dbo.SS_Seat b WITH(NOLOCK) ON a.SeatID=b.ID
	WHERE a.SDate=@date
	AND a.ChangCiID=@changCiId
	AND a.StatusID=3 
	AND a.LockTime>DATEADD(MINUTE,@lockTime,GETDATE())
	AND b.StadiumID=@stadiumId
)x
";
            return await Connection.ExecuteScalarAsync<int>(sql, new { date, changCiId, stadiumId, lockTime }, Transaction);
        }

        public async Task<List<TicketSaleSeatDto>> GetOrderSeatsAsync(string listNo)
        {
            string sql = @"
SELECT
a.TradeID,
a.TicketID,
a.SeatID,
a.SDate,
a.ChangCiID,
c.ID AS GroundId
FROM dbo.TM_TicketSaleSeatStatusCache a WITH(NOLOCK)
LEFT JOIN dbo.SS_Seat b ON a.SeatID=b.ID
LEFT JOIN dbo.VM_Ground c ON b.StadiumID=c.StadiumID
WHERE a.ListNo=@listNo
";
            return (await Connection.QueryAsync<TicketSaleSeatDto>(sql, new { listNo }, Transaction)).ToList();
        }

        public async Task<bool> ReSaleAsync(decimal id, string listNo, SeatStatus seatStatus, DateTime lockTime)
        {
            string sql = @"
UPDATE dbo.TM_TicketSaleSeatStatusCache SET
TradeID=NULL,
TicketID=NULL,
StatusID=6,
LockTime=GETDATE(),
ListNo=@listNo
WHERE ID=@id
AND StatusID=@seatStatus
AND LockTime=@lockTime
";
            return (await Connection.ExecuteAsync(sql, new { id, listNo, seatStatus, lockTime }, Transaction)) > 0;
        }

        public async Task SaleAsync(SeatStatusCache seatStatusCache)
        {
            try
            {
                await InsertAsync(seatStatusCache);
                await Context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw new UserFriendlyException("座位锁定失败，请稍后重试");
            }
        }

        public async Task CancelAsync(string listNo)
        {
            string sql = @"
DELETE dbo.TM_TicketSaleSeatStatusCache WHERE ListNo=@listNo
";
            await Connection.ExecuteAsync(sql, new { listNo }, Transaction);
        }
    }
}
