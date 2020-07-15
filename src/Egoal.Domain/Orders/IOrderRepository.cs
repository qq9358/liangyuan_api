using Egoal.Application.Services.Dto;
using Egoal.Domain.Repositories;
using Egoal.Orders.Dto;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Egoal.Orders
{
    public interface IOrderRepository : IRepository<Order, string>
    {
        Task<bool> HasExchangedAsync(string listNo);
        Task<DateTime?> GetOrderCheckInTimeAsync(string listNo);
        Task<DateTime?> GetOrderCheckOutTimeAsync(string listNo);
        Task<bool> CancelExplainerAsync(string listNo);
        Task<List<OrderForExplainListDto>> GetOrdersForExplainAsync(string travelDate, string customerName);
        Task<PagedResultDto<OrderListDto>> GetOrdersAsync(GetOrdersInput input);
        Task<PagedResultDto<ExplainerOrderListDto>> GetExplainerOrdersAsync(GetExplainerOrdersInput input);
        Task<DataTable> StatOrderByCustomerAsync(StatOrderByCustomerInput input);

        /// <summary>
        /// 查询日期类型名称
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        Task<string> GetTMDateTypeName(string date);
    }
}
