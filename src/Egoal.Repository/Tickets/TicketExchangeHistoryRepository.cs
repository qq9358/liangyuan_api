using Dapper;
using Egoal.EntityFrameworkCore;
using Egoal.Tickets.Dto;
using System;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Egoal.Tickets
{
    public class TicketExchangeHistoryRepository : EfCoreRepositoryBase<TicketExchangeHistory, long>, ITicketExchangeHistoryRepository
    {
        public TicketExchangeHistoryRepository(IServiceProvider serviceProvider)
            : base(serviceProvider)
        { }

        public async Task<DataTable> StatJbAsync(StatJbInput input)
        {
            StringBuilder where = new StringBuilder();
            where.AppendWhere("a.CTime>=@StartCTime");
            where.AppendWhere("a.CTime<=@EndCTime");
            where.AppendWhereIf(input.ParkId.HasValue, "b.ParkID=@ParkId");
            where.AppendWhereIf(input.SalePointId.HasValue, "a.SalePointID=@SalePointId");
            where.AppendWhereIf(input.CashierId.HasValue, "a.CashierID=@CashierId");

            string sql = $@"
SELECT
a.TicketTypeID,
a.TKID,
COUNT(*) AS TotalNum
FROM dbo.TM_TicketExchangeHistory a WITH(NOLOCK)
LEFT JOIN dbo.RM_SalePoint b ON a.SalePointID=b.ID
{where}
GROUP BY a.TicketTypeID,a.TKID
";
            var reader = await Connection.ExecuteReaderAsync(sql, input, Transaction);
            var dataTable = new DataTable();
            dataTable.Load(reader);

            return dataTable;
        }
    }
}
