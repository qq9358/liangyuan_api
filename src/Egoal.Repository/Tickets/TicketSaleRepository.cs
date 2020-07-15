using Dapper;
using Egoal.Application.Services.Dto;
using Egoal.EntityFrameworkCore;
using Egoal.Extensions;
using Egoal.Scenics;
using Egoal.Tickets.Dto;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Egoal.Tickets
{
    public class TicketSaleRepository : EfCoreRepositoryBase<TicketSale, long>, ITicketSaleRepository
    {
        public TicketSaleRepository(IServiceProvider serviceProvider)
            : base(serviceProvider)
        { }

        public async Task InValidAsync(TicketSale ticketSale)
        {
            string sql = @"
DELETE dbo.TM_TicketGroundCache WHERE TicketID=@TicketID
DELETE dbo.TM_TicketGround WHERE TicketID=@TicketID
";
            await Connection.ExecuteAsync(sql, new { TicketID = ticketSale.Id }, Transaction);
        }

        public async Task RefundAsync(TicketSale ticketSale, int quantity)
        {
            string sql = @"
UPDATE dbo.TM_TicketGroundCache SET SurplusNum=SurplusNum-@quantity WHERE TicketID=@TicketID
UPDATE dbo.TM_TicketGround SET SurplusNum=SurplusNum-@quantity WHERE TicketID=@TicketID
";
            await Connection.ExecuteAsync(sql, new { TicketID = ticketSale.Id, quantity }, Transaction);
        }

        public async Task<int> GetFingerprintQuantityAsync(long ticketId)
        {
            string sql = @"
SELECT
COUNT(*)
FROM dbo.TM_TicketSaleFinger WITH(NOLOCK)
WHERE TicketID=@ticketId
";
            return await Connection.ExecuteScalarAsync<int>(sql, new { ticketId }, Transaction);
        }

        public async Task<DateTime> GetFacePhotoBindTimeAsync(long ticketId)
        {
            string sql = @"
SELECT TOP 1
CTime
FROM dbo.TM_TicketSalePhoto WITH(NOLOCK)
WHERE TicketID=@ticketId
AND PhotoTemplate IS NOT NULL
";
            return await Connection.ExecuteScalarAsync<DateTime>(sql, new { ticketId }, Transaction);
        }

        public async Task<decimal> GetOrderRealMoneyAsync(string listNo)
        {
            string sql = @"
SELECT
ISNULL(SUM(a.ReaMoney),0) + ISNULL(SUM(b.ReaMoney),0)
FROM dbo.TM_TicketSale a WITH(NOLOCK)
LEFT JOIN dbo.TM_TicketSale b WITH(NOLOCK) ON b.ReturnTicketID=a.ID
WHERE a.ListNo=@listNo
";
            return await Connection.QueryFirstOrDefaultAsync<decimal>(sql, new { listNo }, Transaction);
        }

        public async Task<PagedResultDto<TicketSaleListDto>> QueryTicketSalesAsync(QueryTicketSaleInput input)
        {
            StringBuilder where = new StringBuilder();
            where.AppendWhere("a.CTime>=@StartSaleTime");
            where.AppendWhere("a.CTime<=@EndSaleTime");
            where.AppendWhere("a.CommitFlag=1");
            where.AppendWhereIf(!input.TicketCode.IsNullOrEmpty(), "a.TicketCode=@TicketCode");
            where.AppendWhereIf(!input.CardNo.IsNullOrEmpty(), "a.CardNo=@CardNo");
            where.AppendWhereIf(!input.ListNo.IsNullOrEmpty(), "a.ListNo=@ListNo");
            where.AppendWhereIf(input.TicketStatusId.HasValue, "a.TicketStatusID=@TicketStatusId");
            where.AppendWhereIf(input.IsExpired.HasValue, $"a.ETime{(input.IsExpired == true ? "<" : ">=")}@Now");
            where.AppendWhereIf(input.TicketTypeTypeId.HasValue, "a.TicketTypeTypeID=@TicketTypeTypeId");
            where.AppendWhereIf(input.TicketTypeId.HasValue, "a.TicketTypeID=@TicketTypeId");
            where.AppendWhereIf(input.CustomerId.HasValue, "a.CustomerID=@CustomerId");
            where.AppendWhereIf(input.MemberId.HasValue, "a.MemberID=@MemberId");
            where.AppendWhereIf(input.ParkId.HasValue, "a.ParkID=@ParkId");
            where.AppendWhereIf(input.SalePointId.HasValue, "a.SalePointID=@SalePointId");
            where.AppendWhereIf(input.CashierId.HasValue, "a.CashierID=@CashierId");
            where.AppendWhereIf(input.CashpcId.HasValue, "a.CashpcID=@CashpcId");
            where.AppendWhereIf(!input.OrderListNo.IsNullOrEmpty(), "a.OrderListNo=@OrderListNo");
            where.AppendWhereIf(!input.ThirdListNo.IsNullOrEmpty(), "b.ThirdPartyPlatformOrderID=@ThirdListNo");
            where.AppendWhereIf(input.TradeSource.HasValue, "b.TradeSource=@TradeSource");
            where.AppendWhereIf(input.PayTypeId.HasValue, "b.PayTypeID=@PayTypeId");
            where.AppendWhereIf(input.SalesManId.HasValue, "a.SalesManID=@SalesManId");
            where.AppendWhereIf(!input.CertNo.IsNullOrEmpty(), "EXISTS(SELECT TOP 1 1 FROM dbo.TM_TicketSaleBuyer WITH(NOLOCK) WHERE TicketID=a.ID AND CertNo=@CertNo)");
            where.AppendWhereIf(input.HasFaceImage.HasValue, "a.PhotoBindFlag=@HasFaceImage");
            if (input.HasFingerprint.HasValue)
            {
                where.AppendWhere($"a.FingerStatusID={(input.HasFingerprint.Value ? "2" : "1")}");
            }

            string sql = $@"
SELECT
a.ID AS Id,
a.ListNo,
a.TicketCode,
a.CardNo,
a.FingerStatusID,
a.PhotoBindFlag,
a.ValidFlag,
a.TicketStatusID,
a.TicketStatusName,
a.TicketTypeID,
a.TicketTypeName,
a.DiscountTypeName,
a.DiscountRate,
a.TicPrice,
a.ReaPrice AS RealPrice,
a.TicMoney,
a.ReaMoney AS RealMoney,
a.PayTypeName,
a.PersonNum,
a.TotalNum,
a.STime,
a.ETime,
a.CashierId,
a.CashierName,
a.CashPCName,
a.SalePointName,
a.SalesmanName,
a.MemberID,
a.MemberName,
a.CustomerID,
a.CustomerName,
a.GuiderID,
a.GuiderName,
a.ReturnTypeName,
a.ReturnRate,
a.OrderListNo,
d.Name as CertTypeName,
a.CertNo,
a.CTime,
a.ParkName,
a.Memo,
b.TradeSource,
b.ThirdPartyPlatformOrderID,
ROW_NUMBER() OVER(ORDER BY a.CTime DESC) AS RowNum
FROM dbo.TM_TicketSale a WITH(NOLOCK)
LEFT JOIN dbo.TM_Trade b WITH(NOLOCK) ON b.ID=a.TradeID
left join TM_TicketSaleBuyer c on (c.TicketID=a.ID or c.TicketID=a.ReturnTicketID)
left join SM_CertType d on d.ID=c.CertTypeID
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
FROM dbo.TM_TicketSale a WITH(NOLOCK)
LEFT JOIN dbo.TM_Trade b WITH(NOLOCK) ON b.ID=a.TradeID
{where}
";
            if (input.ShouldPage)
            {
                int count = await Connection.ExecuteScalarAsync<int>(countSql, input, Transaction);
                var tickets = await Connection.QueryAsync<TicketSaleListDto>(pagedSql, input, Transaction);

                return new PagedResultDto<TicketSaleListDto>(count, tickets.ToList());
            }
            else
            {
                var tickets = await Connection.QueryAsync<TicketSaleListDto>(sql, input, Transaction);

                return new PagedResultDto<TicketSaleListDto>(tickets.Count(), tickets.ToList());
            }
        }

        public async Task<DataTable> StatAsync(StatTicketSaleInput input)
        {
            var statColumns = new[] { "a.CDate", "a.CWeek", "a.CMonth", "a.CQuarter", "a.CYear", "a.TicketTypeID", "b.TradeSource" };
            var statColumn = statColumns[(int)input.StatType - 1];

            StringBuilder where = new StringBuilder();
            where.AppendWhere("a.CTime>=@StartCTime");
            where.AppendWhere("a.CTime<=@EndCTime");
            where.AppendWhere("a.CommitFlag=1");
            where.AppendWhere("a.StatFlag=1");
            where.AppendWhereIf(input.TicketTypeId.HasValue, "a.TicketTypeID=@TicketTypeId");

            string sql = $@"
SELECT
{statColumn},
SUM(CASE WHEN a.TicketStatusID=11 THEN 0 ELSE a.TicketNum END) AS SaleNum,
SUM(CASE WHEN a.TicketStatusID=11 THEN 0 ELSE a.PersonNum END) AS SalePersonNum,
SUM(CASE WHEN a.TicketStatusID=11 THEN 0 ELSE a.ReaMoney END) AS SaleMoney,
SUM(CASE WHEN a.TicketStatusID=11 THEN ABS(a.TicketNum) ELSE 0 END) AS ReturnNum,
SUM(CASE WHEN a.TicketStatusID=11 THEN ABS(a.PersonNum) ELSE 0 END) AS ReturnPersonNum,
SUM(CASE WHEN a.TicketStatusID=11 THEN ABS(a.ReaMoney) ELSE 0 END) AS ReturnMoney,
SUM(CASE WHEN a.TicketStatusID=11 THEN -ABS(a.TicketNum) ELSE ABS(a.TicketNum) END) AS RealNum,
SUM(CASE WHEN a.TicketStatusID=11 THEN -ABS(a.PersonNum) ELSE ABS(a.PersonNum) END) AS RealPersonNum,
SUM(CASE WHEN a.TicketStatusID=11 THEN -ABS(a.ReaMoney) ELSE ABS(a.ReaMoney) END) AS RealMoney
FROM dbo.TM_TicketSale a WITH(NOLOCK)
LEFT JOIN dbo.TM_Trade b WITH(NOLOCK) ON a.TradeID=b.ID
{where}
GROUP BY {statColumn}
";
            var reader = await Connection.ExecuteReaderAsync(sql, input, Transaction);
            var dataTable = new DataTable();
            dataTable.Load(reader);

            return dataTable;
        }

        /// <summary>
        /// 收银员销售汇总
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<DataTable> StatCashierSaleAsync(StatCashierSaleInput input)
        {
            StringBuilder where = new StringBuilder();
            where.AppendWhere("CTime>=@StartCTime");
            where.AppendWhere("CTime<=@EndCTime");
            where.AppendWhereIf(input.CashierId.HasValue, "CashierID=@CashierId");
            where.AppendWhereIf(input.SalePointId.HasValue, "SalePointID=@SalePointId");

            string sql = $@"
SELECT
CashierID,
TicketTypeName,
1 AS TicketTypeTypeID,
ReaPrice AS RealPrice,
SUM(CASE WHEN TicketStatusID=11 THEN 0 ELSE TicketNum END) AS SaleNum,
SUM(CASE WHEN TicketStatusID=11 THEN 0 ELSE ABS(ReaMoney)+ABS(ISNULL(YaJin,0)) END) AS SaleMoney,
SUM(CASE WHEN TicketStatusID=11 THEN ABS(TicketNum) ELSE 0 END) AS ReturnNum,
SUM(CASE WHEN TicketStatusID=11 THEN ABS(ReaMoney)+ABS(ISNULL(YaJin,0)) ELSE 0 END) AS ReturnMoney,
SUM(CASE WHEN TicketStatusID=11 THEN -ABS(ReaMoney)-ABS(ISNULL(YaJin,0)) ELSE ABS(ReaMoney)+ABS(ISNULL(YaJin,0)) END) AS RealMoney
FROM dbo.TM_TicketSale WITH(NOLOCK)
{where}
AND CommitFlag=1
AND StatFlag=1
GROUP BY CashierID,TicketTypeName,ReaPrice
UNION ALL
SELECT
CashierID,
TicketTypeName + '售卡' AS TicketTypeName,
8 AS TicketTypeTypeID,
NULL AS RealPrice,
SUM(CASE WHEN CzkRechargeTypeID = 1 THEN 1 ELSE 0 END) AS SaleNum,
SUM(CASE WHEN CzkRechargeTypeID = 1 THEN RealPrice+YaJin ELSE 0 END) AS SaleMoney,
SUM(CASE WHEN CzkRechargeTypeID IN (3,6) THEN 1 ELSE 0 END) AS ReturnNum,
SUM(CASE WHEN CzkRechargeTypeID IN (3,6) THEN ABS(RechargeCardMoney)+ABS(YaJin) ELSE 0 END) AS ReturnMoney,
SUM(CASE WHEN CzkRechargeTypeID = 1 THEN RealPrice+YaJin ELSE 0 END)-SUM(CASE WHEN CzkRechargeTypeID IN (3,6) THEN ABS(RechargeCardMoney)+ABS(YaJin) ELSE 0 END) AS RealMoney
FROM dbo.MM_CzkDetail WITH(NOLOCK)
{where}
AND CommitFlag=1
AND StatFlag=1
AND CzkRechargeTypeID IN (1,3,6)
GROUP BY CashierID,TicketTypeName
UNION ALL
SELECT
CashierID,
'充值' AS TicketTypeName,
9 AS TicketTypeTypeID,
NULL AS RealPrice,
SUM(1) AS SaleNum,
SUM(RechargeCardMoney) AS SaleMoney,
NULL AS ReturnNum,
NULL AS ReturnMoney,
SUM(RechargeCardMoney) AS RealMoney
FROM dbo.MM_CzkDetail WITH(NOLOCK)
{where}
AND CommitFlag=1
AND StatFlag=1
AND CzkRechargeTypeID=2
GROUP BY CashierID
";
            var reader = await Connection.ExecuteReaderAsync(sql, input, Transaction);
            var dataTable = new DataTable();
            dataTable.Load(reader);

            return dataTable;
        }

        public async Task<DataTable> StatByTradeSourceAsync(StatTicketSaleByTradeSourceInput input)
        {
            string statType = string.Empty;
            if (input.StatType == "1")
            {
                statType = "a.CDate,";
            }
            else if (input.StatType == "2")
            {
                statType = "a.CMonth,";
            }
            else if (input.StatType == "3")
            {
                statType = "a.CYear,";
            }
            string statTypeColumn = statType.IsNullOrEmpty() ? string.Empty : $"{statType.TrimEnd(',')} AS StatType,";

            StringBuilder where = new StringBuilder();
            where.AppendWhere("a.CTime>=@StartCTime");
            where.AppendWhere("a.CTime<=@EndCTime");
            where.AppendWhere("a.CommitFlag=1");
            where.AppendWhere("a.StatFlag=1");
            where.AppendWhereIf(input.TicketTypeId.HasValue, "a.TicketTypeID=@TicketTypeId");
            where.AppendWhereIf(input.CashierId.HasValue, "a.CashierID=@CashierId");
            where.AppendWhereIf(input.SalePointId.HasValue, "a.SalePointID=@SalePointId");
            where.AppendWhereIf(input.TradeSource.HasValue, "b.TradeSource=@TradeSource");
            where.AppendWhereIf(
                input.TicketTypeSearchGroupId.HasValue,
                "EXISTS(SELECT TOP 1 1 FROM dbo.TM_TicketTypeSearchGroupDetail WHERE TicketTypeID=a.TicketTypeID AND TicketTypeSearchGroupID=@TicketTypeSearchGroupId)");

            string sql = $@"
SELECT
{statTypeColumn}
ISNULL(b.TradeSource,1) AS TradeSource,
a.TicketTypeName,
a.ReaPrice,
SUM(a.TicketNum) AS TicketNum,
SUM(a.PersonNum) AS PersonNum,
SUM(a.ReaMoney) AS ReaMoney
FROM dbo.TM_TicketSale a WITH(NOLOCK)
LEFT JOIN dbo.TM_Trade b WITH(NOLOCK) ON a.TradeID=b.ID
{where}
GROUP BY {statType}b.TradeSource,a.TicketTypeName,a.ReaPrice
";
            var reader = await Connection.ExecuteReaderAsync(sql, input, Transaction);
            var dataTable = new DataTable();
            dataTable.Load(reader);

            return dataTable;
        }

        public async Task<DataTable> StatByTicketTypeClassAsync(StatTicketSaleByTicketTypeClassInput input)
        {
            StringBuilder where = new StringBuilder();
            where.AppendWhere("a.CTime>=@StartCTime");
            where.AppendWhere("a.CTime<=@EndCTime");
            where.AppendWhere("a.CommitFlag=1");
            where.AppendWhere("a.StatFlag=1");
            where.AppendWhereIf(input.TicketTypeId.HasValue, "a.TicketTypeID=@TicketTypeId");
            where.AppendWhereIf(input.TicketTypeClassId.HasValue, "b.TicketTypeClassID=@TicketTypeClassId");

            string sql = $@"
SELECT
b.TicketTypeClassID,
a.TicketTypeID,
a.ReaPrice AS RealPrice,
SUM(CASE WHEN a.TicketStatusID=11 THEN 0 ELSE a.TicketNum END) AS SaleNum,
SUM(CASE WHEN a.TicketStatusID=11 THEN 0 ELSE a.PersonNum END) AS SalePersonNum,
SUM(CASE WHEN a.TicketStatusID=11 THEN 0 ELSE a.ReaMoney END) AS SaleMoney,
SUM(CASE WHEN a.TicketStatusID=11 THEN ABS(a.TicketNum) ELSE 0 END) AS ReturnNum,
SUM(CASE WHEN a.TicketStatusID=11 THEN ABS(a.PersonNum) ELSE 0 END) AS ReturnPersonNum,
SUM(CASE WHEN a.TicketStatusID=11 THEN ABS(a.ReaMoney) ELSE 0 END) AS ReturnMoney,
SUM(CASE WHEN a.TicketStatusID=11 THEN -ABS(a.TicketNum) ELSE ABS(a.TicketNum) END) AS RealNum,
SUM(CASE WHEN a.TicketStatusID=11 THEN -ABS(a.PersonNum) ELSE ABS(a.PersonNum) END) AS RealPersonNum,
SUM(CASE WHEN a.TicketStatusID=11 THEN -ABS(a.ReaMoney) ELSE ABS(a.ReaMoney) END) AS RealMoney
FROM dbo.TM_TicketSale a WITH(NOLOCK)
LEFT JOIN dbo.TM_TicketTypeClassDetail b WITH(NOLOCK) ON b.TicketTypeID=a.TicketTypeID
{where}
GROUP BY b.TicketTypeClassID,a.TicketTypeID,a.ReaPrice
";
            var reader = await Connection.ExecuteReaderAsync(sql, input, Transaction);
            var dataTable = new DataTable();
            dataTable.Load(reader);

            return dataTable;
        }

        public async Task<DataTable> StatByPayTypeAsync(StatTicketSaleByPayTypeInput input, IEnumerable<ComboboxItemDto<int>> payTypes)
        {
            StringBuilder where = new StringBuilder();
            where.AppendWhere("CTime>=@StartCTime");
            where.AppendWhere("CTime<=@EndCTime");
            where.AppendWhere("CommitFlag=1");
            where.AppendWhere("StatFlag=1");
            where.AppendWhereIf(input.TicketTypeId.HasValue, "TicketTypeID=@TicketTypeId");

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
	TicketTypeID,
	PayTypeID,
	SUM(ReaMoney) AS TotalMoney
	FROM dbo.TM_TicketSale WITH(NOLOCK)
    {where}
	GROUP BY TicketTypeID,PayTypeID
)x
PIVOT(SUM(x.TotalMoney) FOR x.PayTypeID IN ({payTypeColumns.ToString().TrimEnd(',')})) AS p
";
            var reader = await Connection.ExecuteReaderAsync(sql, input, Transaction);
            var dataTable = new DataTable();
            dataTable.Load(reader);

            return dataTable;
        }

        public async Task<DataTable> StatBySalePointAsync(StatTicketSaleBySalePointInput input)
        {
            StringBuilder where = new StringBuilder();
            where.AppendWhere("a.CTime>=@StartCTime");
            where.AppendWhere("a.CTime<=@EndCTime");
            where.AppendWhere("a.CommitFlag=1");
            where.AppendWhere("a.StatFlag=1");
            where.AppendWhereIf(input.ParkId.HasValue, "a.ParkID=@ParkId");
            where.AppendWhereIf(input.SalePointId.HasValue, "a.SalePointID=@SalePointId");
            where.AppendWhereIf(input.TicketTypeId.HasValue, "a.TicketTypeID=@TicketTypeId");

            string sql = $@"
SELECT
a.ParkID,
a.SalePointID,
a.TicketTypeID,
a.ReaPrice,
SUM(CASE WHEN a.TicketStatusID=11 THEN 0 ELSE a.TicketNum END) AS SaleNum,
SUM(CASE WHEN a.TicketStatusID=11 THEN 0 ELSE a.PersonNum END) AS SalePersonNum,
SUM(CASE WHEN a.TicketStatusID=11 THEN 0 ELSE a.ReaMoney END) AS SaleMoney,
SUM(CASE WHEN a.TicketStatusID=11 THEN ABS(a.TicketNum) ELSE 0 END) AS ReturnNum,
SUM(CASE WHEN a.TicketStatusID=11 THEN ABS(a.PersonNum) ELSE 0 END) AS ReturnPersonNum,
SUM(CASE WHEN a.TicketStatusID=11 THEN ABS(a.ReaMoney) ELSE 0 END) AS ReturnMoney,
SUM(CASE WHEN a.TicketStatusID=11 THEN -ABS(a.TicketNum) ELSE ABS(a.TicketNum) END) AS RealNum,
SUM(CASE WHEN a.TicketStatusID=11 THEN -ABS(a.PersonNum) ELSE ABS(a.PersonNum) END) AS RealPersonNum,
SUM(CASE WHEN a.TicketStatusID=11 THEN -ABS(a.ReaMoney) ELSE ABS(a.ReaMoney) END) AS RealMoney
FROM dbo.TM_TicketSale a WITH(NOLOCK)
{where}
GROUP BY a.ParkID,a.SalePointID,a.TicketTypeID,a.ReaPrice
";
            var reader = await Connection.ExecuteReaderAsync(sql, input, Transaction);
            var dataTable = new DataTable();
            dataTable.Load(reader);

            return dataTable;
        }

        public async Task<DataTable> StatGroundSharingAsync(StatGroundSharingInput input)
        {
            StringBuilder where = new StringBuilder();
            where.AppendWhere("b.CTime>=@StartCTime");
            where.AppendWhere("b.CTime<=@EndCTime");
            where.AppendWhere("b.CommitFlag=1");
            where.AppendWhere("b.StatFlag=1");
            where.AppendWhere($"b.SalePointID<>{DefaultSalePoint.分销平台}");
            where.AppendWhereIf(input.SalePointId.HasValue, "b.SalePointID=@SalePointId");
            where.AppendWhereIf(input.TicketTypeId.HasValue, "b.TicketTypeID=@TicketTypeId");
            where.AppendWhereIf(input.GroundId.HasValue, "a.GroundID=@GroundId");

            string sql = $@"
SELECT
a.GroundID,
b.SalePointID,
b.TicketTypeID,
b.ReaPrice,
a.SharingRate,
SUM(b.PersonNum) AS PersonNum,
SUM(a.SharingMoney) AS SharingMoney
FROM dbo.TM_TicketSaleGroundSharing a WITH(NOLOCK)
LEFT JOIN dbo.TM_TicketSale b WITH(NOLOCK) ON a.TicketID=b.ID
{where}
GROUP BY a.GroundID,b.SalePointID,b.TicketTypeID,b.ReaPrice,a.SharingRate
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

            StringBuilder ticketWhere = new StringBuilder(where.ToString());
            ticketWhere.AppendWhere("a.TicketTypeTypeID<>9");

            StringBuilder tradeWhere = new StringBuilder(where.ToString());
            tradeWhere.AppendWhere("a.TotalMoney<>0");
            tradeWhere.AppendWhere("a.TradeTypeTypeID NOT IN(3,6)");
            tradeWhere.AppendWhere("a.TradeTypeID NOT IN(10,11,21,41)");

            string payTypeColumn = input.StatTicketByPayType ? "c.PayTypeID," : string.Empty;
            string payTypeJoin = input.StatTicketByPayType ? "LEFT JOIN dbo.TM_PayDetail c WITH(NOLOCK) ON a.TradeID=c.TradeID" : string.Empty;

            string sql = $@"
SELECT
{payTypeColumn}
b.TradeTypeID,
a.TicketTypeID,
a.ReaPrice AS RealPrice,
SUM(CASE WHEN a.TicketStatusID=11 THEN 0 ELSE a.PersonNum END) AS SaleNum,
SUM(CASE WHEN a.TicketStatusID=11 THEN ABS(a.PersonNum) ELSE 0 END) AS ReturnNum,
SUM(CASE WHEN a.TicketStatusID=11 THEN -ABS(a.PersonNum) ELSE a.PersonNum END) AS RealNum,
SUM(CASE WHEN a.TicketStatusID=11 THEN -ABS(a.TicketNum) ELSE a.TicketNum END) AS RealTicketNum,
SUM(a.ReaMoney) AS RealMoney
FROM dbo.TM_TicketSale a WITH(NOLOCK)
LEFT JOIN dbo.TM_Trade b WITH(NOLOCK) ON a.TradeID=b.ID
{payTypeJoin}
{ticketWhere}
GROUP BY {payTypeColumn}b.TradeTypeID,a.TicketTypeID,a.ReaPrice
UNION ALL
SELECT
{payTypeColumn}
a.TradeTypeID,
NULL AS TicketTypeID,
ABS(a.TotalMoney) AS RealPrice,
SUM(CASE WHEN a.TotalMoney>=0 THEN 1 ELSE 0 END) AS SaleNum,
SUM(CASE WHEN a.TotalMoney<0 THEN 1 ELSE 0 END) AS ReturnNum,
SUM(CASE WHEN a.TotalMoney<0 THEN -1 ELSE 1 END) AS RealNum,
NULL AS RealTicketNum,
SUM(a.TotalMoney) AS RealMoney
FROM dbo.TM_TradeDetail a WITH(NOLOCK)
LEFT JOIN dbo.TM_Trade b WITH(NOLOCK) ON a.TradeID=b.ID
{payTypeJoin}
{tradeWhere}
GROUP BY {payTypeColumn}a.TradeTypeID,a.TotalMoney
";
            var reader = await Connection.ExecuteReaderAsync(sql, input, Transaction);
            var dataTable = new DataTable();
            dataTable.Load(reader);

            return dataTable;
        }
    }
}
