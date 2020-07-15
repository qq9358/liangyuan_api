using Dapper;
using Egoal.Application.Services.Dto;
using Egoal.EntityFrameworkCore;
using Egoal.Orders.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using Egoal.Extensions;
using System.Data;

namespace Egoal.Orders
{
    public class OrderRepository : EfCoreRepositoryBase<Order, string>, IOrderRepository
    {
        public OrderRepository(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {

        }

        public async Task<bool> HasExchangedAsync(string listNo)
        {
            string sql = @"
SELECT TOP 1 1
FROM dbo.TM_TicketSale WITH(NOLOCK)
WHERE OrderListNo=@listNo
AND (HasExchanged=1 OR PrintNum>0)
";
            return await Connection.ExecuteScalarAsync<string>(sql, new { listNo }, Transaction) == "1";
        }

        public async Task<DateTime?> GetOrderCheckInTimeAsync(string listNo)
        {
            string sql = @"
SELECT TOP 1
a.CTime
FROM dbo.TM_TicketCheck a WITH(NOLOCK)
JOIN dbo.TM_TicketSale b WITH(NOLOCK) ON b.ID=a.TicketID
WHERE a.InOutFlag=1
AND b.OrderListNo=@listNo
";
            return await Connection.ExecuteScalarAsync<DateTime?>(sql, new { listNo }, Transaction);
        }

        public async Task<DateTime?> GetOrderCheckOutTimeAsync(string listNo)
        {
            string sql = @"
SELECT TOP 1
a.CTime
FROM dbo.TM_TicketCheck a WITH(NOLOCK)
JOIN dbo.TM_TicketSale b WITH(NOLOCK) ON b.ID=a.TicketID
WHERE a.InOutFlag=0
AND b.OrderListNo=@listNo
ORDER BY a.CTime DESC
";
            return await Connection.ExecuteScalarAsync<DateTime?>(sql, new { listNo }, Transaction);
        }

        public async Task<bool> CancelExplainerAsync(string listNo)
        {
            string sql = @"
UPDATE dbo.OM_Order SET 
ExplainerId=NULL,
ExplainerTimeId=NULL
WHERE ListNo=@listNo
";
            return (await Connection.ExecuteAsync(sql, new { listNo }, Transaction)) > 0;
        }

        public async Task<List<OrderForExplainListDto>> GetOrdersForExplainAsync(string travelDate, string customerName)
        {
            StringBuilder where = new StringBuilder();
            where.AppendWhere("a.ETime=@travelDate")
                .AppendWhere("a.CustomerID IS NOT NULL")
                .AppendWhere("a.ExplainerTimeId IS NOT NULL")
                .AppendWhereIf(!customerName.IsNullOrEmpty(), $"a.CustomerName LIKE '%{customerName}%'");

            string sql = $@"
SELECT
a.ListNo,
a.ETime AS TravelDate,
a.CustomerName,
a.TotalNum,
a.KeYuanTypeID,
a.KeYuanAreaID,
a.JidiaoName,
a.ExplainerId,
a.ExplainerTimeId,
a.Memo,
b.BeginTime,
b.CompleteTime
FROM dbo.OM_Order a
LEFT JOIN dbo.RM_ExplainerWorkRecord b ON b.ListNo=a.ListNo
LEFT JOIN dbo.RM_ExplainerTimeslot c ON c.Id=a.ExplainerTimeId
{where}
ORDER BY b.CompleteTime,b.BeginTime,c.BeginTime,a.CTime
";
            return (await Connection.QueryAsync<OrderForExplainListDto>(sql, new { travelDate }, Transaction)).ToList();
        }

        public async Task<PagedResultDto<OrderListDto>> GetOrdersAsync(GetOrdersInput input)
        {
            StringBuilder where = new StringBuilder();
            where.AppendWhereIf(input.StartCTime.HasValue, "CTime>=@StartCTime");
            where.AppendWhereIf(input.EndCTime.HasValue, "CTime<=@EndCTime");
            where.AppendWhereIf(!input.StartTravelDate.IsNullOrEmpty(), "ETime>=@StartTravelDate");
            where.AppendWhereIf(!input.EndTravelDate.IsNullOrEmpty(), "ETime<=@EndTravelDate");
            where.AppendWhereIf(input.CustomerId.HasValue, "CustomerID=@CustomerId");
            where.AppendWhereIf(!input.ListNo.IsNullOrEmpty(), "(ListNo=@ListNo OR ThirdPartyPlatformOrderID=@ListNo)");
            where.AppendWhereIf(input.OrderStatus.HasValue, "OrderStatusID=@OrderStatus");
            where.AppendWhereIf(input.ConsumeStatus.HasValue, "ConsumeStatus=@ConsumeStatus");
            where.AppendWhereIf(input.RefundStatus.HasValue, "RefundStatus=@RefundStatus");
            where.AppendWhereIf(input.OrderType.HasValue, "OrderTypeID=@OrderType");
            where.AppendWhereIf(!input.ContactName.IsNullOrEmpty(), "YdrName=@ContactName");
            where.AppendWhereIf(!input.ContactMobile.IsNullOrEmpty(), "Mobile=@ContactMobile");
            where.AppendWhereIf(!input.Remark.IsNullOrEmpty(), $"Memo LIKE '{input.Remark}%'");
            if (input.HasCustomer.HasValue)
            {
                if (input.HasCustomer.Value)
                {
                    where.AppendWhere("CustomerID IS NOT NULL");
                }
                else
                {
                    where.AppendWhere("CustomerID IS NULL");
                }
            }

            string sql = $@"
SELECT
ListNo,
OrderTypeID,
OrderStatusID,
ConsumeStatus,
RefundStatus,
ETime AS TravelDate,
PayFlag,
TotalMoney,
TotalNum,
CollectNum,
UsedNum,
ReturnNum,
SurplusNum,
MemberName,
CustomerName,
GuiderName,
ExplainerId,
ExplainerTimeId,
YdrName,
Mobile,
JidiaoName,
JidiaoMobile,
ThirdPartyPlatformOrderID AS ThirdListNo,
Memo,
CTime,
ROW_NUMBER() OVER (ORDER BY CTime DESC) AS RowNum
FROM dbo.OM_Order WITH(NOLOCK)
{where.ToString()}
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
SELECT COUNT(*) FROM dbo.OM_Order WITH(NOLOCK) {where.ToString()}
";
            if (input.ShouldPage)
            {
                int count = await Connection.ExecuteScalarAsync<int>(countSql, input, Transaction);
                var orders = await Connection.QueryAsync<OrderListDto>(pagedSql, input, Transaction);

                return new PagedResultDto<OrderListDto>(count, orders.ToList());
            }
            else
            {
                var orders = await Connection.QueryAsync<OrderListDto>(sql, input, Transaction);

                return new PagedResultDto<OrderListDto>(orders.Count(), orders.ToList());
            }
        }

        public async Task<PagedResultDto<ExplainerOrderListDto>> GetExplainerOrdersAsync(GetExplainerOrdersInput input)
        {
            StringBuilder where = new StringBuilder();
            where.AppendWhere("a.CTime BETWEEN @StartCTime AND @EndCTime");
            where.AppendWhere("a.ExplainerTimeId IS NOT NULL");
            where.AppendWhereIf(input.CustomerId.HasValue, "a.CustomerID=@CustomerId");
            where.AppendWhereIf(input.TimeslotId.HasValue, "a.ExplainerTimeId=@TimeslotId");
            where.AppendWhereIf(input.ExplainerId.HasValue, "a.ExplainerId=@ExplainerId");

            string sql = $@"
SELECT
a.ListNo,
a.ETime AS TravelDate,
a.ExplainerTimeId AS TimeslotId,
a.ExplainerId,
a.CustomerName,
a.TotalNum,
b.BeginTime,
b.CompleteTime,
ROW_NUMBER() OVER(ORDER BY a.CTime DESC) AS RowNum
FROM dbo.OM_Order a WITH(NOLOCK)
LEFT JOIN dbo.RM_ExplainerWorkRecord b WITH(NOLOCK) ON b.ListNo=a.ListNo
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
SELECT COUNT(*) FROM dbo.OM_Order a WITH(NOLOCK) {where}
";
            if (input.ShouldPage)
            {
                int count = await Connection.ExecuteScalarAsync<int>(countSql, input, Transaction);
                var orders = await Connection.QueryAsync<ExplainerOrderListDto>(pagedSql, input, Transaction);

                return new PagedResultDto<ExplainerOrderListDto>(count, orders.ToList());
            }
            else
            {
                var orders = await Connection.QueryAsync<ExplainerOrderListDto>(sql, input, Transaction);

                return new PagedResultDto<ExplainerOrderListDto>(orders.Count(), orders.ToList());
            }
        }

        public async Task<DataTable> StatOrderByCustomerAsync(StatOrderByCustomerInput input)
        {
            StringBuilder where = new StringBuilder();
            where.AppendWhere("a.CTime>=@StartCTime");
            where.AppendWhere("a.CTime<=@EndCTime");
            where.AppendWhere("a.CustomerID IS NOT NULL");
            where.AppendWhere("b.Name IS NOT NULL");

            string sql = $@"
SELECT
ISNULL(c.Name,'未设置') AS 团体客户类型,
b.Name AS 团体客户,
SUM(a.TotalNum) AS 数量
FROM dbo.OM_Order a WITH(NOLOCK)
LEFT JOIN dbo.MM_Member b WITH(NOLOCK) ON b.ID=a.CustomerID
LEFT JOIN dbo.CM_CustomerType c ON c.ID=b.MemberTypeID
{where}
GROUP BY c.Name,b.Name
";
            var reader = await Connection.ExecuteReaderAsync(sql, input, Transaction);
            var dataTable = new DataTable();
            dataTable.Load(reader);

            return dataTable;
        }

        /// <summary>
        /// 查询日期类型名称
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public async Task<string> GetTMDateTypeName(string date)
        {
            string sql = @"
        select dt.Name from TM_Date d
left join TM_DateType dt on d.DateTypeID = dt.ID
where d.Date = @DateNow";
            var reader = (await Connection.QueryAsync<string>(sql, new { DateNow = date }, Transaction)).ToList();
            if(reader!=null && reader.Count > 0)
            {
                return reader[0];
            }
            else
            {
                return "";
            }
        }
    }
}
