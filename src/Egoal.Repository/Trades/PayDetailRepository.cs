using Dapper;
using Egoal.Application.Services.Dto;
using Egoal.EntityFrameworkCore;
using Egoal.Tickets.Dto;
using Egoal.Trades.Dto;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Egoal.Trades
{
    public class PayDetailRepository : EfCoreRepositoryBase<PayDetail, Guid>, IPayDetailRepository
    {
        public PayDetailRepository(IServiceProvider serviceProvider)
            : base(serviceProvider)
        { }

        public async Task<DataTable> StatAsync(StatPayDetailInput input, IEnumerable<ComboboxItemDto<int>> payTypes)
        {
            StringBuilder where = new StringBuilder();
            where.AppendWhere("a.CTime>=@StartCTime");
            where.AppendWhere("a.CTime<=@EndCTime");
            where.AppendWhere("a.CommitFlag=1");
            where.AppendWhere("a.StatFlag=1");

            var statTypeColumns = new string[] { "a.CDate", "b.ParkID", "b.SalePointID", "b.CashierID" };
            var statTypeColumn = statTypeColumns[(int)input.StatType - 1];

            StringBuilder payTypeColumns = new StringBuilder();
            foreach (var payType in payTypes)
            {
                payTypeColumns.Append("[").Append(payType.Value).Append("],");
            }

            string sql = $@"
SELECT
*
FROM
(
	SELECT
	{statTypeColumn},
	a.PayTypeID,
	SUM(a.PayMoney) AS PayMoney
	FROM dbo.TM_PayDetail a WITH(NOLOCK)
    LEFT JOIN dbo.TM_Trade b WITH(NOLOCK) ON a.TradeID=b.ID
    {where}
	GROUP BY {statTypeColumn},a.PayTypeID
)x
PIVOT(SUM(x.PayMoney) FOR x.PayTypeID IN ({payTypeColumns.ToString().TrimEnd(',')})) AS p
";
            var reader = await Connection.ExecuteReaderAsync(sql, input, Transaction);
            var dataTable = new DataTable();
            dataTable.Load(reader);

            return dataTable;
        }

        public async Task<DataTable> StatJbAsync(StatJbInput input)
        {
            StringBuilder where = new StringBuilder();
            where.AppendWhere("a.CTime>=@StartCTime");
            where.AppendWhere("a.CTime<=@EndCTime");
            where.AppendWhere("a.CommitFlag=1");
            where.AppendWhere("a.StatFlag=1");
            where.AppendWhereIf(input.ParkId.HasValue, "b.ParkID=@ParkId");
            where.AppendWhereIf(input.SalePointId.HasValue, "b.SalePointID=@SalePointId");
            where.AppendWhereIf(input.CashierId.HasValue, "b.CashierID=@CashierId");
            where.AppendWhereIf(input.HasShift.HasValue, "b.ShiftFlag=@HasShift");

            string sql = $@"
SELECT
(CASE WHEN c.CzkFlag=1 OR c.PayFlag=0 THEN '无实际收款' ELSE '实际收款' END) AS TypeName,
c.Name AS PayTypeName,
SUM(a.PayMoney) AS PayMoney
FROM dbo.TM_PayDetail a WITH(NOLOCK)
LEFT JOIN dbo.TM_Trade b WITH(NOLOCK) ON a.TradeID=b.ID
LEFT JOIN dbo.SM_PayType c WITH(NOLOCK) ON a.PayTypeID=c.ID
{where}
GROUP BY c.Name,c.PayFlag,c.CzkFlag
";
            var reader = await Connection.ExecuteReaderAsync(sql, input, Transaction);
            var dataTable = new DataTable();
            dataTable.Load(reader);

            return dataTable;
        }
    }
}
