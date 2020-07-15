using Dapper;
using Egoal.EntityFrameworkCore;
using Egoal.Extensions;
using Egoal.Tickets.Dto;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Egoal.Tickets
{
    public class TicketSaleBuyerRepository : EfCoreRepositoryBase<TicketSaleBuyer, long>, ITicketSaleBuyerRepository
    {
        public TicketSaleBuyerRepository(IServiceProvider serviceProvider)
            : base(serviceProvider)
        { }

        public async Task<int> GetCertNoBuyQuantity(string certNo, DateTime startTime, DateTime endTime)
        {
            string sql = @"
SELECT COUNT(*) 
FROM dbo.TM_TicketSaleBuyer a WITH(UPDLOCK)
LEFT JOIN dbo.TM_TicketSale b ON b.ID=a.TicketID
WHERE a.CertNo=@certNo
AND b.TicketStatusID<10
AND a.SDate BETWEEN @startTime AND @endTime
";
            return await Connection.ExecuteScalarAsync<int>(sql, new { certNo, startTime, endTime }, Transaction);
        }

        public async Task<DataTable> StatTouristByAgeRangeAsync(StatTouristByAgeRangeInput input)
        {
            string ageRangeSql = "SELECT [Name] FROM dbo.SM_AgeRange";
            var ageRanges = await Connection.QueryAsync<string>(ageRangeSql, null, Transaction);
            StringBuilder ageRangeColumns = new StringBuilder();
            foreach (var name in ageRanges)
            {
                ageRangeColumns.Append("[").Append(name).Append("],");
            }

            string statTypeName = input.StatType == 1 ? "月份" : "年份";
            string statTypeColumn = input.StatType == 1 ? "CMonth" : "CYear";
            string touristStatTypeColumn = input.StatType == 1 ? "CONVERT(VARCHAR(7),a.CTime,120) AS CMonth" : "CONVERT(VARCHAR(4),a.CTime,120) AS CYear";

            StringBuilder sbWhere1 = new StringBuilder();
            sbWhere1.AppendWhereIf(input.StartCTime.HasValue, "a.CTime>=@StartCTime");
            sbWhere1.AppendWhereIf(input.EndCTime.HasValue, "a.CTime<=@EndCTime");

            StringBuilder sbWhere2 = new StringBuilder();
            sbWhere2.AppendWhereIf(input.StartCTime.HasValue, "b.CTime>=@StartCTime");
            sbWhere2.AppendWhereIf(input.EndCTime.HasValue, "b.CTime<=@EndCTime");

            string sql = $@"
SELECT
*
FROM
(
	SELECT
	x.{statTypeColumn} AS {statTypeName},
	x.AgeRange AS 年龄段,
	SUM(x.PersonNum) AS 人数
	FROM
	(
		SELECT
		b.Name AS AgeRange,
		1 AS PersonNum,
		{touristStatTypeColumn}
		FROM dbo.TM_TicketSaleBuyer a WITH(NOLOCK)
		LEFT JOIN dbo.SM_AgeRange b ON a.Age BETWEEN b.StartAge AND b.EndAge
        {sbWhere1.ToString()}
		UNION ALL
        SELECT
        c.Name AS AgeRange,
        a.PersonNum,
        b.{statTypeColumn}
        FROM dbo.OM_OrderAgeRange a WITH(NOLOCK)
        LEFT JOIN dbo.OM_Order b WITH(NOLOCK) ON b.ListNo=a.ListNo
        LEFT JOIN dbo.SM_AgeRange c ON c.Id=a.AgeRangeId
		{sbWhere2.ToString()}
	)x
	GROUP BY x.{statTypeColumn},x.AgeRange
)y
PIVOT(SUM(y.[人数]) FOR y.[年龄段] IN ({ageRangeColumns.ToString().TrimEnd(',')})) AS p
ORDER BY p.{statTypeName}
";
            var reader = await Connection.ExecuteReaderAsync(sql, input, Transaction);
            var dataTable = new DataTable();
            dataTable.Load(reader);

            return dataTable;
        }

        public async Task<DataTable> StatTouristByAreaAsync(StatTouristByAreaInput input)
        {
            StringBuilder columns = new StringBuilder();
            string length = "7";
            if (input.StatType == 1)
            {
                for (var date = new DateTime(input.StartCTime.Year, input.StartCTime.Month, 1); date <= input.EndCTime; date = date.AddMonths(1))
                {
                    columns.Append("[").Append(date.ToString("yyyy-MM")).Append("],");
                }
            }
            else
            {
                length = "4";
                for (var year = input.StartCTime.Year; year <= input.EndCTime.Year; year++)
                {
                    columns.Append("[").Append(year).Append("],");
                }
            }

            StringBuilder where = new StringBuilder();
            where.AppendWhere("a.CTime>=@StartCTime");
            where.AppendWhere("a.CTime<=@EndCTime");
            where.AppendWhereIf(!input.ProvinceName.IsNullOrEmpty(), "b.Name=@ProvinceName");

            StringBuilder where2 = new StringBuilder();
            where2.AppendWhere("a.CustomerID IS NOT NULL")
                .AppendWhere("a.KeYuanAreaID IS NOT NULL")
                .AppendWhere("a.CTime>=@StartCTime")
                .AppendWhere("a.CTime<=@EndCTime")
                .AppendWhereIf(!input.ProvinceName.IsNullOrEmpty(), "b.Name=@ProvinceName");

            string sql = $@"
SELECT
*
FROM
(
	SELECT
	x.AreaName AS 地区,
	x.StatType,
	SUM(x.PersonNum) AS 人数
	FROM
	(
		SELECT
		(CASE WHEN b.Name IS NULL THEN '未登记' ELSE b.Name END) AS AreaName,
		1 AS PersonNum,
		CONVERT(VARCHAR({length}),a.CTime,120) AS StatType
		FROM dbo.TM_TicketSaleBuyer a
		LEFT JOIN dbo.SM_Province b ON b.ID=a.ProvinceID
        {where.ToString()}
        UNION ALL
        SELECT
        (CASE WHEN b.Name IS NULL THEN '未登记' ELSE b.Name END) AS AreaName,
        a.TotalNum AS PersonNum,
        CONVERT(VARCHAR({length}),a.CTime,120) AS StatType
        FROM dbo.OM_Order a WITH(NOLOCK)
        LEFT JOIN dbo.SM_Area b ON b.ID=a.KeYuanAreaID
        {where2}
	)x
	GROUP BY x.AreaName,x.StatType
)y
PIVOT(SUM(y.人数) FOR y.StatType IN ({columns.ToString().TrimEnd(',')})) AS p
";
            var reader = await Connection.ExecuteReaderAsync(sql, input, Transaction);
            var dataTable = new DataTable();
            dataTable.Load(reader);

            return dataTable;
        }

        public async Task<DataTable> StatTouristBySexAsync(StatTouristBySexInput input)
        {
            StringBuilder where = new StringBuilder();
            where.AppendWhere("CTime>=@StartCTime");
            where.AppendWhere("CTime<=@EndCTime");

            string sql = $@"
SELECT
x.Sex AS 性别,
SUM(x.PersonNum) AS 人数
FROM
(
	SELECT
	CASE WHEN Sex IS NULL THEN '未登记' ELSE Sex END AS Sex,
	1 AS PersonNum
	FROM dbo.TM_TicketSaleBuyer WITH(NOLOCK)
    {where}
)x
GROUP BY x.Sex
";
            var reader = await Connection.ExecuteReaderAsync(sql, input, Transaction);
            var dataTable = new DataTable();
            dataTable.Load(reader);

            return dataTable;
        }

        public async Task<List<StatTouristListDto>> StatTouristAsync(StatTouristInput input)
        {
            string areaGroup = input.StatByArea == true ? ",x.Area" : string.Empty;
            string sexGroup = input.StatBySex == true ? ",x.Sex" : string.Empty;
            string nationGroup = input.StatByNation == true ? ",x.Nation" : string.Empty;
            string ageGroup = input.StatByAge == true ? ",x.AgeRange" : string.Empty;

            string sql = $@"
SELECT
x.[Date]
{areaGroup}
{sexGroup}
{nationGroup}
{ageGroup}
,SUM(x.Quantity) AS Quantity
FROM
(
	SELECT
	Sex,
	Nation,
	dbo.FSMGetDatePart(CTime,'d') AS [Date],
	ChinaCityID AS Area,
	dbo.FSMGetAgeRange(Age) AS AgeRange,
	1 AS Quantity
	FROM dbo.TM_TicketSaleBuyer WITH(NOLOCK)
    WHERE CTime>=@StartCTime AND CTime<=@EndCTime
)x
GROUP BY x.[Date]{areaGroup}{sexGroup}{nationGroup}{ageGroup}
";
            return (await Connection.QueryAsync<StatTouristListDto>(sql, new { input.StartCTime, input.EndCTime }, Transaction)).ToList();
        }
    }
}
