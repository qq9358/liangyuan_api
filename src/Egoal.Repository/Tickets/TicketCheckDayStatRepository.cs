using Dapper;
using Egoal.EntityFrameworkCore;
using Egoal.Tickets.Dto;
using System;
using System.Data;
using System.Threading.Tasks;
using System.Text;
using Egoal.Extensions;

namespace Egoal.Tickets
{
    public class TicketCheckDayStatRepository : EfCoreRepositoryBase<TicketCheckDayStat, long>, ITicketCheckDayStatRepository
    {
        public TicketCheckDayStatRepository(IServiceProvider serviceProvider)
            : base(serviceProvider)
        { }

        public override async Task<TicketCheckDayStat> InsertOrUpdateAsync(TicketCheckDayStat entity)
        {
            string sql = @"
UPDATE dbo.TM_TicketCheckDayStat SET
CheckNum=CheckNum+@CheckNum
WHERE GroundId=@GroundId
AND GateGroupId=@GateGroupId
AND GateId=@GateId
AND InOutFlag=@InOutFlag
AND CheckerId=@CheckerId
AND CDate=@Cdate
AND CTP=@Ctp

IF @@ROWCOUNT=0
BEGIN
	INSERT INTO dbo.TM_TicketCheckDayStat
	(
	    CheckNum,
	    GroundId,
	    GateGroupId,
	    GateId,
	    InOutFlag,
	    CheckerId,
	    CDate,
	    CTP
	)
	VALUES
	(   @CheckNum,
	    @GroundId,
	    @GateGroupId,
	    @GateId,
	    @InOutFlag,
	    @CheckerId,
	    @Cdate,
	    @Ctp
	)
END
";
            await Connection.ExecuteAsync(sql, entity, Transaction);

            return entity;
        }

        public async Task<DataTable> GetStadiumTicketCheckOverviewAsync(string startDate, string endDate)
        {
            string sql = @"
SELECT
*
FROM
(
	SELECT
	b.Name AS GateName,
	CASE WHEN a.InOutFlag=1 THEN 'CheckIn' ELSE 'CheckOut' END AS InOutFlag,
	SUM(a.CheckNum) AS CheckNum
	FROM dbo.TM_TicketCheckDayStat a WITH(NOLOCK)
	LEFT JOIN dbo.VM_Gate b ON b.ID=a.GateID
	WHERE a.CDate>=@startDate
    AND a.CDate<=@endDate
	AND b.GateTypeID=5
	AND b.InOutFlag=1
	GROUP BY b.Name,a.InOutFlag
)x
PIVOT(SUM(x.CheckNum) FOR x.InOutFlag IN ([CheckIn],[CheckOut])) AS p
";
            var reader = await Connection.ExecuteReaderAsync(sql, new { startDate, endDate }, Transaction);
            var dataTable = new DataTable();
            dataTable.Load(reader);

            return dataTable;
        }

        public async Task<DataTable> StatStadiumTicketCheckInAsync(StatTicketCheckInInput input)
        {
            string gateSql = "SELECT [Name] FROM dbo.VM_Gate WHERE GateTypeID=5 AND InOutFlag=1";
            var gates = await Connection.QueryAsync<string>(gateSql, null, Transaction);
            StringBuilder gateColumns = new StringBuilder();
            foreach (var gate in gates)
            {
                gateColumns.Append("[").Append(gate).Append("],");
            }

            int statTypeLength = 10;
            string statTypeName = "日期";
            if (input.StatType == 2)
            {
                statTypeLength = 7;
                statTypeName = "月份";
            }
            else if (input.StatType == 3)
            {
                statTypeLength = 4;
                statTypeName = "年份";
            }

            string sql = $@"
SELECT
*
FROM
(
	SELECT
	x.StatType AS {statTypeName},
	x.GateName,
	SUM(x.CheckNum) AS CheckNum
	FROM
	(
		SELECT
		CONVERT(VARCHAR({statTypeLength}),a.CDate,120) AS StatType,
		b.Name AS GateName,
		a.CheckNum
		FROM dbo.TM_TicketCheckDayStat a WITH(NOLOCK)
		LEFT JOIN dbo.VM_Gate b ON b.ID=a.GateID
		WHERE a.CDate>=@StartCTime
		AND a.CDate<=@EndCTime
		AND a.InOutFlag=1
		AND b.GateTypeID=5
		AND b.InOutFlag=1
	)x
	GROUP BY x.StatType,x.GateName
)y PIVOT(SUM(y.CheckNum) FOR y.GateName IN ({gateColumns.ToString().TrimEnd(',')})) AS p
";
            var reader = await Connection.ExecuteReaderAsync(sql, input, Transaction);
            var dataTable = new DataTable();
            dataTable.Load(reader);

            return dataTable;
        }

        /// <summary>
        /// 检票年度对比统计
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<DataTable> StatTicketCheckInYearOverYearComparisonAsync(StatTicketCheckInInput input)
        {
            StringBuilder yearColumns = new StringBuilder();
            for (int i = input.StartCTime.Year; i <= input.EndCTime.Year; i++)
            {
                yearColumns.Append('[').Append(i).Append("],");
            }

            string sql = $@"
SELECT
*
FROM
(
	SELECT
	x.Year,
	x.Month AS 月份,
	SUM(x.CheckNum) AS CheckNum
	FROM
	(
		SELECT
		CONVERT(VARCHAR(4),a.CDate,120) AS [Year],
		SUBSTRING(a.CDate,6,2) AS [Month],
		a.CheckNum
		FROM dbo.TM_TicketCheckDayStat a WITH(NOLOCK)
		LEFT JOIN dbo.VM_Gate b ON b.ID=a.GateID
		WHERE a.CDate>=@StartCTime
		AND a.CDate<=@EndCTime
		AND a.InOutFlag=1
		AND b.GateTypeID<>5 
	)x
	GROUP BY x.Year,x.Month
)y PIVOT(SUM(y.CheckNum) FOR y.Year IN ({yearColumns.ToString().TrimEnd(',')})) AS p
";
            var reader = await Connection.ExecuteReaderAsync(sql, input, Transaction);
            var dataTable = new DataTable();
            dataTable.Load(reader);

            return dataTable;
        }

        public async Task<int> GetScenicCheckInQuantityAsync(string startDate, string endDate)
        {
            string sql = @"
SELECT 
SUM(a.CheckNum)
FROM TM_TicketCheckDayStat a WITH(NOLOCK)
LEFT JOIN dbo.VM_Gate b ON b.ID=a.GateID
WHERE a.CDate>=@startDate
AND a.CDate<=@endDate
AND a.InOutFlag=1
AND b.GateTypeID<>5 
";
            int quantity = await Connection.ExecuteScalarAsync<int>(sql, new { startDate, endDate }, Transaction);

            return quantity;
        }

        public async Task<int> GetScenicCheckOutQuantityAsync(string startDate, string endDate)
        {
            string sql = @"
SELECT
SUM(x.CheckNum)
FROM
(
	SELECT 
	SUM(a.CheckNum) AS CheckNum
	FROM TM_TicketCheckDayStat a WITH(NOLOCK)
	LEFT JOIN dbo.VM_Gate b ON b.ID=a.GateID
    WHERE a.CDate>=@startDate
    AND a.CDate<=@endDate
    AND a.InOutFlag=0
	AND b.GateTypeID=5
	AND b.InOutFlag=0
	UNION ALL
	SELECT
	SUM(a.CheckNum) AS CheckNum
	FROM dbo.TM_TicketCheckDayStat a WITH(NOLOCK)
	LEFT JOIN dbo.VM_Gate b ON b.ID=a.GateID
	WHERE a.CDate>=@startDate
    AND a.CDate<=@endDate
    AND a.InOutFlag=0
	AND b.GateTypeID<>5
)x
";
            int quantity = await Connection.ExecuteScalarAsync<int>(sql, new { startDate, endDate }, Transaction);

            return quantity;
        }

        public async Task<DataTable> StatTicketCheckInAverageByTimeslotAsync(StatTicketCheckInInput input)
        {
            StringBuilder where = new StringBuilder();
            where.AppendWhere("a.CDate>=@StartCTime");
            where.AppendWhere("a.CDate<=@EndCTime");
            where.AppendWhere("a.InOutFlag=1");
            where.AppendWhere("b.GateTypeID<>5");

            string sql = $@"
SELECT
y.CTP AS 时间段,
AVG(y.CheckNum) AS 数量
FROM
(
	SELECT
	x.CDate,
	x.CTP,
	SUM(x.CheckNum) AS CheckNum
	FROM
	(
		SELECT
		a.CDate,
		CASE WHEN a.CTP<'08' THEN '8点前' WHEN a.CTP>'19' THEN '20点后' ELSE a.CTP END AS CTP,
		a.CheckNum
		FROM dbo.TM_TicketCheckDayStat a WITH(NOLOCK)
		LEFT JOIN dbo.VM_Gate b WITH(NOLOCK) ON b.ID=a.GateID
		{where}
	)x
	GROUP BY x.CDate,x.CTP
)y
GROUP BY y.CTP
";
            var reader = await Connection.ExecuteReaderAsync(sql, input, Transaction);
            var dataTable = new DataTable();
            dataTable.Load(reader);

            if (!dataTable.IsNullOrEmpty())
            {
                DataRow row = dataTable.Rows[dataTable.Rows.Count - 1];
                if (row["时间段"].ToString() == "8点前")
                {
                    DataRow newRow = dataTable.NewRow();
                    newRow["时间段"] = row["时间段"];
                    newRow["数量"] = row["数量"];
                    dataTable.Rows.Remove(row);
                    dataTable.Rows.InsertAt(newRow, 0);
                }
            }

            return dataTable;
        }

        public async Task<DataTable> StatTicketCheckInAverageByDateAsync(StatTicketCheckInInput input)
        {
            StringBuilder where = new StringBuilder();
            where.AppendWhere("a.CDate>=@StartCTime");
            where.AppendWhere("a.CDate<=@EndCTime");
            where.AppendWhere("a.InOutFlag=1");
            where.AppendWhere("b.GateTypeID<>5");

            string sql = $@"
SELECT
x.CDate AS 日期,
AVG(x.CheckNum) AS 数量
FROM
(
	SELECT
	SUBSTRING(a.CDate,9,2) AS CDate,
	SUM(a.CheckNum) AS CheckNum
	FROM dbo.TM_TicketCheckDayStat a WITH(NOLOCK)
	LEFT JOIN dbo.VM_Gate b WITH(NOLOCK) ON b.ID=a.GateID
	{where}
	GROUP BY a.CDate
)x
GROUP BY x.CDate
";
            var reader = await Connection.ExecuteReaderAsync(sql, input, Transaction);
            var dataTable = new DataTable();
            dataTable.Load(reader);

            return dataTable;
        }

        public async Task<DataTable> StatTicketCheckInAverageByMonthAsync(StatTicketCheckInInput input)
        {
            StringBuilder where = new StringBuilder();
            where.AppendWhere("a.CDate>=@StartCTime");
            where.AppendWhere("a.CDate<=@EndCTime");
            where.AppendWhere("a.InOutFlag=1");
            where.AppendWhere("b.GateTypeID<>5");

            string sql = $@"
SELECT
y.CMonth AS 月份,
AVG(y.CheckNum) AS 数量
FROM
(
	SELECT
	SUBSTRING(x.CMonth,6,2) AS CMonth,
	SUM(x.CheckNum) AS CheckNum
	FROM
	(
		SELECT
		SUBSTRING(a.CDate,1,7) AS CMonth,
		a.CheckNum
		FROM dbo.TM_TicketCheckDayStat a WITH(NOLOCK)
		LEFT JOIN dbo.VM_Gate b WITH(NOLOCK) ON b.ID=a.GateID
		{where}
	)x
	GROUP BY x.CMonth
)y
GROUP BY y.CMonth
";
            var reader = await Connection.ExecuteReaderAsync(sql, input, Transaction);
            var dataTable = new DataTable();
            dataTable.Load(reader);

            return dataTable;
        }
    }
}
