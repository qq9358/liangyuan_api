using Egoal.Application.Services.Dto;
using Egoal.Authorization;
using Egoal.Extensions;
using Egoal.Models;
using Egoal.Mvc.Authorization;
using Egoal.Mvc.Uow;
using Egoal.Orders;
using Egoal.Orders.Dto;
using Egoal.Scenics;
using Egoal.Staffs;
using Egoal.Tickets.Dto;
using Egoal.TicketTypes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Egoal.Web.Api.Controllers
{
    public class OrderController : TmsControllerBase
    {
        private readonly IOrderAppService _orderAppService;
        private readonly IOrderQueryAppService _orderQueryAppService;

        public OrderController(
            IOrderAppService orderAppService,
            IOrderQueryAppService orderQueryAppService)
        {
            _orderAppService = orderAppService;
            _orderQueryAppService = orderQueryAppService;
        }

        [HttpPost]
        public async Task<JsonResult> GetMemberOrdersForMobileAsync(GetMemberOrdersForMobileInput input)
        {
            var orders = await _orderQueryAppService.GetMemberOrdersForMobileAsync(input);

            return Json(orders);
        }

        /// <summary>
        /// 查询订单列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> GetMemberOrdersForQueryAsync(GetMemberOrdersForQueryInput input)
        {
            Application.Services.Dto.PagedResultDto<OrderInfoDto> orders = await _orderQueryAppService.GetMemberOrdersForQueryAsync(input);

            return Json(orders);
        }

        [HttpGet]
        public async Task<JsonResult> GetMemberOrderForMobileAsync(string listNo)
        {
            var result = await _orderQueryAppService.GetMemberOrderForMobileAsync(listNo);

            return Json(result);
        }

        [HttpPost]
        public async Task<JsonResult> GetLastOrderFullInfoAsync(GetLastOrderInput input)
        {
            var result = await _orderQueryAppService.GetLastOrderFullInfoAsync(input);

            return Json(result);
        }

        [HttpPost]
        public async Task<JsonResult> GetOrderFullInfoAsync(GetOrderInfoInput input)
        {
            var order = await _orderQueryAppService.GetOrderFullInfoAsync(input);

            return Json(order);
        }

        [HttpGet]
        public async Task<JsonResult> GetGroupOrderAsync(string listNo)
        {
            var order = await _orderQueryAppService.GetGroupOrderAsync(listNo);

            return Json(order);
        }

        [HttpGet]
        public async Task<JsonResult> GetRefundApplysWithStatusDetailAsync(string listNo)
        {
            var result = await _orderQueryAppService.GetRefundApplysWithStatusDetailAsync(listNo);

            return Json(result);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<JsonResult> GetOrderOptionsAsync(string travelDate)
        {
            var options = await _orderQueryAppService.GetOrderOptionsAsync(travelDate);

            return Json(options);
        }

        [HttpGet]
        [AllowAnonymous]
        /// <summary>
        /// 获取购票日期附带票状态
        /// </summary>
        /// <returns></returns>
        public async Task<JsonResult> GetOrderOptionsTicketAsync(string travelDate)
        {
            OrderOptionsTicketDto orderOptionsTicketDto = await _orderQueryAppService.GetOrderOptionsTicketAsync(travelDate);
            return Json(orderOptionsTicketDto);
        }

        [HttpPost]
        [PermissionFilter(Permissions.TMSWeChat_GroupExplain)]
        public async Task<JsonResult> GetOrdersForExplainAsync(GetOrdersForExplainInput input)
        {
            var orders = await _orderQueryAppService.GetOrdersForExplainAsync(input);

            return Json(orders);
        }

        [HttpPost]
        [PermissionFilter(Permissions.Handset_SaleTicket)]
        public async Task<JsonResult> CreateHandsetOrderAsync(CreateOrderInput input)
        {
            var result = await _orderAppService.CreateOrderAsync(input, SaleChannel.Local, OrderType.手持机订票);

            return Json(result);
        }

        [HttpPost]
        public async Task<JsonResult> CreateWeiXinOrderAsync(CreateOrderInput input)
        {
            input.CashierId = DefaultStaff.微信购票;
            input.CashPcid = DefaultPc.微信购票;
            input.SalePointId = DefaultSalePoint.微信购票;
            input.ParkId = DefaultPark.微信购票;

            var result = await _orderAppService.CreateOrderAsync(input, SaleChannel.Net, OrderType.微信订票);

            return Json(result);
        }

        [HttpPost]
        public async Task<string> CreateGroupOrderAsync(CreateGroupOrderInput input)
        {
            input.OrderType = OrderType.微信订票;

            return await _orderAppService.CreateGroupOrderAsync(input);
        }

        [HttpPost]
        public async Task<JsonResult> CreateWebOrderAsync(CreateOrderInput input)
        {
            var result = await _orderAppService.CreateOrderAsync(input, SaleChannel.Net, OrderType.网上订票);

            return Json(result);
        }

        [HttpPost]
        public async Task<string> CreateWebGroupOrderAsync(CreateGroupOrderInput input)
        {
            input.OrderType = OrderType.网上订票;
            return await _orderAppService.CreateGroupOrderAsync(input);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<string> CreateScenicGroupOrderAsync(CreateGroupOrderInput input)
        {
            input.OrderType = OrderType.电话订票;

            return await _orderAppService.CreateGroupOrderAsync(input);
        }

        [HttpPost]
        public async Task ChangeChangCiAsync(ChangeChangCiInput input)
        {
            await _orderAppService.ChangeChangCiAsync(input);
        }

        [HttpPost]
        public async Task ChangeQuantityAsync(ChangeQuantityInput input)
        {
            await _orderAppService.ChangeQuantityAsync(input);
        }

        [HttpPost]
        [UnitOfWork(IsDisabled = true)]
        public async Task CancelOrderAsync(CancelOrderInput input)
        {
            await _orderAppService.CancelByUserAsync(input);
        }

        [HttpPost]
        public async Task ApplyInvoiceAsync(InvoiceInput input)
        {
            await _orderAppService.ApplyInvoiceAsync(input);
        }

        [HttpPost]
        public async Task ApplyRefundAsync(RefundOrderInput input)
        {
            await _orderAppService.ApplyRefundAsync(input);
        }

        [HttpPost]
        public async Task<JsonResult> GetGroupOrdersForConsumeAsync(GetGroupOrdersForConsumeInput input)
        {
            var orders = await _orderQueryAppService.GetGroupOrdersForConsumeAsync(input);

            return Json(orders);
        }

        [HttpPost]
        [DontWrapResult]
        [PermissionFilter(Permissions.TMSAdmin_OrderManage)]
        public async Task<FileContentResult> GetOrdersToExcelAsync(GetOrdersInput input)
        {
            var fileContents = await _orderQueryAppService.GetOrdersToExcelAsync(input);

            return Excel(fileContents);
        }

        [HttpPost]
        [PermissionFilter(Permissions.TMSAdmin_OrderManage)]
        public async Task<JsonResult> GetOrdersAsync(GetOrdersInput input)
        {
            var result = await _orderQueryAppService.GetOrdersAsync(input);

            return Json(result);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<JsonResult> GetExplainerOrdersAsync(GetExplainerOrdersInput input)
        {
            var result = await _orderQueryAppService.GetExplainerOrdersAsync(input);

            return Json(result);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<JsonResult> StatOrderByCustomerAsync(StatOrderByCustomerInput input)
        {
            var result = await _orderQueryAppService.StatOrderByCustomerAsync(input);

            return Json(result);
        }

        [HttpGet]
        public JsonResult GetOrderStatusComboboxItems()
        {
            return Json(_orderQueryAppService.GetOrderStatusComboboxItemDtos());
        }

        /// <summary>
        /// 获取完整订单状态名称
        /// </summary>
        /// <returns></returns>
        public JsonResult GetCompleteOrderStatusComboboxItems()
        {
            return Json(_orderQueryAppService.GetCompleteOrderStatusComboboxItemDtos());
        }

        [HttpGet]
        public JsonResult GetConsumeStatusComboboxItems()
        {
            var items = typeof(ConsumeStatus).ToComboboxItems();

            return Json(items);
        }

        [HttpGet]
        public JsonResult GetRefundStatusComboboxItems()
        {
            var items = typeof(RefundStatus).ToComboboxItems();

            return Json(items);
        }

        [HttpGet]
        public JsonResult GetOrderTypeComboboxItems()
        {
            var items = typeof(OrderType).ToComboboxItems();

            return Json(items);
        }

        [HttpPost]
        public async Task<JsonResult> GetIfInvoiceAsync(string listNo)
        {
            var result = await _orderQueryAppService.GetIfInvoiceAsync(listNo);
            return Json(result);
        }
    }
}