using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Egoal.EntityFrameworkCore;
using Dapper;

namespace Egoal.Tickets
{
    public class TicketSaleDayStatRepository : EfCoreRepositoryBase<TicketSaleDayStat, long>, ITicketSaleDayStatRepository
    {
        public TicketSaleDayStatRepository(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {

        }

        public override async Task<TicketSaleDayStat> InsertOrUpdateAsync(TicketSaleDayStat entity)
        {
            string sql = @"
UPDATE dbo.TM_TicketSaleDayStat SET
TicketNum=TicketNum+@TicketNum,
PersonNum=PersonNum+@PersonNum,
TicMoney=TicMoney+@TicMoney
WHERE TicketTypeID=@TicketTypeId
AND CashierID=@CashierId
AND CashPCID=@CashPcid
AND CDate=@Cdate
AND CTP=@Ctp

IF @@ROWCOUNT=0
BEGIN
	INSERT INTO dbo.TM_TicketSaleDayStat
	(
	    TicketNum,
	    PersonNum,
	    TicMoney,
	    TicketTypeID,
	    CashierID,
	    CashPCID,
	    CDate,
	    CTP
	)
	VALUES
	(   @TicketNum,
	    @PersonNum,
	    @TicMoney,
	    @TicketTypeId,
	    @CashierId,
	    @CashPcid,
	    @Cdate,
	    @Ctp
	)
END
";
            await Connection.ExecuteAsync(sql, entity, Transaction);

            return entity;
        }

        public async Task<int> GetSaleNumAsync(string date)
        {
            string sql = @"
SELECT 
ISNULL(SUM(PersonNum),0) AS TotalNum 
FROM dbo.TM_TicketSaleDayStat WITH(UPDLOCK)
WHERE CDate=@date
";
            return await Connection.ExecuteScalarAsync<int>(sql, new { date }, Transaction);
        }
    }
}
