using Dapper;
using Egoal.Application.Services.Dto;
using Egoal.EntityFrameworkCore;
using Egoal.Extensions;
using Egoal.Trades.Dto;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Egoal.Trades
{
    public class TradeRepository : EfCoreRepositoryBase<Trade, Guid>, ITradeRepository
    {
        public TradeRepository(IServiceProvider serviceProvider)
            : base(serviceProvider)
        { }

        public async Task<List<ComboboxItemDto<int>>> GetTradeTypeTypeComboboxItemsAsync()
        {
            string sql = @"
SELECT
ID AS Value,
Name AS DisplayText
FROM dbo.TM_TradeTypeType
ORDER BY SortCode
";
            return (await Connection.QueryAsync<ComboboxItemDto<int>>(sql, null, Transaction)).ToList();
        }

        public async Task<List<ComboboxItemDto<int>>> GetTradeTypeComboboxItemsAsync(int? tradeTypeTypeId)
        {
            StringBuilder where = new StringBuilder();
            where.AppendWhereIf(tradeTypeTypeId.HasValue, "TradeTypeTypeID=@tradeTypeTypeId");

            string sql = $@"
SELECT
ID AS Value,
Name AS DisplayText
FROM dbo.TM_TradeType
{where}
ORDER BY SortCode
";
            return (await Connection.QueryAsync<ComboboxItemDto<int>>(sql, new { tradeTypeTypeId }, Transaction)).ToList();
        }

        public async Task<PagedResultDto<TradeListDto>> QueryTradesAsync(QueryTradeInput input)
        {
            StringBuilder where = new StringBuilder();
            where.AppendWhere("a.CTime>=@StartSaleTime");
            where.AppendWhere("a.CTime<=@EndSaleTime");
            where.AppendWhere("a.CommitFlag=1");
            where.AppendWhereIf(input.TradeTypeTypeId.HasValue, "a.TradeTypeTypeID=@TradeTypeTypeId");
            where.AppendWhereIf(input.TradeTypeId.HasValue, "a.TradeTypeID=@TradeTypeId");
            where.AppendWhereIf(!input.ListNo.IsNullOrEmpty(), "a.ListNo=@ListNo");
            where.AppendWhereIf(!input.ThirdPartyPlatformOrderId.IsNullOrEmpty(), "b.ThirdPartyPlatformOrderID=@ThirdPartyPlatformOrderId");
            where.AppendWhereIf(input.TradeSource.HasValue, "b.TradeSource=@TradeSource");
            where.AppendWhereIf(input.MemberId.HasValue, "b.MemberID=@MemberId");
            where.AppendWhereIf(input.CustomerId.HasValue, "b.CustomerID=@CustomerId");
            where.AppendWhereIf(input.GuiderId.HasValue, "b.GuiderID=@GuiderId");
            where.AppendWhereIf(input.PayTypeId.HasValue, "b.PayTypeId=@PayTypeId");
            where.AppendWhereIf(input.CashierId.HasValue, "b.CashierId=@CashierId");
            where.AppendWhereIf(input.CashPcid.HasValue, "b.CashPcid=@CashPcid");
            where.AppendWhereIf(input.SalePointId.HasValue, "b.SalePointId=@SalePointId");
            where.AppendWhereIf(input.ParkId.HasValue, "b.ParkId=@ParkId");
            where.AppendWhereIf(!input.IncludeWareTrade, "a.TradeTypeTypeID<>6");

            string sql = $@"
SELECT
a.ID,
a.TradeID,
a.ListNo,
a.TradeTypeName,
a.TotalMoney,
b.PayTypeName,
b.CashierID,
b.CashierName,
b.CashPCName,
b.SalePointName,
b.SalesmanName,
b.ParkName,
b.MemberID,
b.MemberName,
b.CustomerID,
b.CustomerName,
b.GuiderID,
b.GuiderName,
a.InvoiceCode,
a.InvoiceNo,
b.AreaName,
b.Memo,
a.CTime,
ROW_NUMBER() OVER(ORDER BY a.CTime DESC) AS RowNum
FROM dbo.TM_TradeDetail a WITH(NOLOCK)
LEFT JOIN dbo.TM_Trade b WITH(NOLOCK) ON b.ID=a.TradeID
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
SELECT
COUNT(*)
FROM dbo.TM_TradeDetail a WITH(NOLOCK)
LEFT JOIN dbo.TM_Trade b WITH(NOLOCK) ON b.ID=a.TradeID
{where}
";
            if (input.ShouldPage)
            {
                int count = await Connection.ExecuteScalarAsync<int>(countSql, input, Transaction);
                var trades = await Connection.QueryAsync<TradeListDto>(pagedSql, input, Transaction);

                return new PagedResultDto<TradeListDto>(count, trades.ToList());
            }
            else
            {
                var trades = await Connection.QueryAsync<TradeListDto>(sql, input, Transaction);

                return new PagedResultDto<TradeListDto>(trades.Count(), trades.ToList());
            }
        }

        public async Task<DataTable> StatAsync(StatTradeInput input)
        {
            var statColumns = new[] { "a.CDate", "a.CWeek", "a.CMonth", "a.CQuarter", "a.CYear", "" };
            var statColumn = statColumns[(int)input.StatType - 1];
            if (!statColumn.IsNullOrEmpty())
            {
                statColumn = $"{statColumn},";
            }

            StringBuilder where = new StringBuilder();
            where.AppendWhere("b.CTime>=@StartCTime");
            where.AppendWhere("b.CTime<=@EndCTime");
            where.AppendWhere("b.CommitFlag=1");
            where.AppendWhere("b.StatFlag=1");

            string sql = $@"
SELECT
{statColumn}
a.TradeTypeID,
SUM(a.TotalMoney) AS TotalMoney
FROM dbo.TM_TradeDetail a WITH(NOLOCK)
LEFT JOIN dbo.TM_Trade b WITH(NOLOCK) ON b.ID=a.TradeID
{where}
GROUP BY {statColumn}a.TradeTypeID
";
            var reader = await Connection.ExecuteReaderAsync(sql, input, Transaction);
            var dataTable = new DataTable();
            dataTable.Load(reader);

            return dataTable;
        }

        public async Task<DataTable> StatByPayTypeAsync(StatTradeByPayTypeInput input, IEnumerable<ComboboxItemDto<int>> payTypes)
        {
            StringBuilder where = new StringBuilder();
            where.AppendWhere("a.CTime>=@StartCTime");
            where.AppendWhere("a.CTime<=@EndCTime");
            where.AppendWhere("a.CommitFlag=1");
            where.AppendWhere("a.StatFlag=1");
            where.AppendWhereIf(input.TradeTypeId.HasValue, "a.TradeTypeID=@TradeTypeId");

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
	a.TradeTypeID,
	b.PayTypeID,
	SUM(a.TotalMoney) AS TotalMoney
	FROM dbo.TM_TradeDetail a WITH(NOLOCK)
	LEFT JOIN dbo.TM_Trade b WITH(NOLOCK) ON a.TradeID=b.ID
	{where}
	GROUP BY a.TradeTypeID,b.PayTypeID
)x
PIVOT(SUM(x.TotalMoney) FOR x.PayTypeID IN ({payTypeColumns.ToString().TrimEnd(',')})) AS p
";
            var reader = await Connection.ExecuteReaderAsync(sql, input, Transaction);
            var dataTable = new DataTable();
            dataTable.Load(reader);

            return dataTable;
        }
    }
}
