using Egoal.Application.Services.Dto;
using Egoal.Orders.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Egoal.Orders
{
    public interface IOrderQueryAppService
    {
        Task<PagedResultDto<OrderSimpleListDto>> GetMemberOrdersForMobileAsync(GetMemberOrdersForMobileInput input);

        /// <summary>
        /// 查询订单列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<OrderInfoDto>> GetMemberOrdersForQueryAsync(GetMemberOrdersForQueryInput input);

        Task<PagedResultDto<OrderQueryDto>> GetMemberOrdersForQueryAsync2(GetMemberOrdersForQueryInput input);

        Task<OrderSimpleListDto> GetMemberOrderForMobileAsync(string listNo);
        Task<OrderInfoDto> GetLastOrderFullInfoAsync(GetLastOrderInput input);
        Task<OrderInfoDto> GetOrderFullInfoAsync(GetOrderInfoInput input);
        Task<GroupOrderDto> GetGroupOrderAsync(string listNo);
        Task<List<RefundApplyDto>> GetRefundApplysWithStatusDetailAsync(string listNo);
        Task<OrderOptionsDto> GetOrderOptionsAsync(string travelDate);

        /// <summary>
        /// 获取购票日期附带票状态
        /// </summary>
        /// <returns></returns>
        Task<OrderOptionsTicketDto> GetOrderOptionsTicketAsync(string travelDate);

        Task<List<OrderForExplainListDto>> GetOrdersForExplainAsync(GetOrdersForExplainInput input);
        Task<List<OrderForConsumeListDto>> GetGroupOrdersForConsumeAsync(GetGroupOrdersForConsumeInput input);
        Task<byte[]> GetOrdersToExcelAsync(GetOrdersInput input);
        Task<PagedResultDto<OrderListDto>> GetOrdersAsync(GetOrdersInput input);
        Task<PagedResultDto<ExplainerOrderListDto>> GetExplainerOrdersAsync(GetExplainerOrdersInput input);
        Task<DynamicColumnResultDto> StatOrderByCustomerAsync(StatOrderByCustomerInput input);

        /// <summary>
        /// 获取完整订单状态下拉列表
        /// </summary>
        /// <returns></returns>
        List<ComboboxItemDto<int>> GetOrderStatusComboboxItemDtos();

        /// <summary>
        /// 获取完整订单状态下拉列表
        /// </summary>
        /// <returns></returns>
        List<ComboboxItemDto<int>> GetCompleteOrderStatusComboboxItemDtos();

        Task<bool> GetIfInvoiceAsync(string listNo);
    }
}
