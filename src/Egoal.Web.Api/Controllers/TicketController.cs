using Egoal.Application.Services.Dto;
using Egoal.Authorization;
using Egoal.Extensions;
using Egoal.Models;
using Egoal.Mvc.Authorization;
using Egoal.Orders;
using Egoal.Tickets;
using Egoal.Tickets.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Egoal.Web.Api.Controllers
{
    public class TicketController : TmsControllerBase
    {
        private readonly Runtime.Session.ISession _session;
        private readonly ITicketSaleQueryAppService _ticketSaleQueryAppService;
        private readonly ITicketSaleAppService _ticketSaleAppService;

        public TicketController(
            Runtime.Session.ISession session,
            ITicketSaleQueryAppService ticketSaleQueryAppService,
            ITicketSaleAppService ticketSaleAppService)
        {
            _session = session;
            _ticketSaleQueryAppService = ticketSaleQueryAppService;
            _ticketSaleAppService = ticketSaleAppService;
        }

        [HttpPost]
        [PermissionFilter(Permissions.Handset_CheckTicket)]
        public async Task<JsonResult> CheckTicketAsync(CheckTicketInput input)
        {
            input.ConsumeType = ConsumeType.安卓手持机检票;

            var result = await _ticketSaleAppService.CheckTicketAsync(input);

            return Json(result);
        }

        [HttpPost]
        [PermissionFilter(Permissions.TMSWeChat_CheckTicket)]
        public async Task<JsonResult> CheckTicketFromMobileAsync(CheckTicketInput input)
        {
            input.ConsumeType = ConsumeType.手机检票;

            var result = await _ticketSaleAppService.CheckTicketAsync(input);

            return Json(result);
        }

        [HttpPost]
        [PermissionFilter(Permissions.Handset_RefundTicket)]
        public async Task RefundFromHandsetAsync(RefundInput input)
        {
            await _ticketSaleAppService.RefundAsync(input, RefundChannel.安卓手持机);
        }

        [HttpPost]
        public async Task InvoiceAsync(InvoiceInput input)
        {
            await _ticketSaleAppService.InvoiceAsync(input);
        }

        [HttpPost]
        public async Task RePrintAsync(PrintTicketInput input)
        {
            await _ticketSaleAppService.RePrintAsync(input);
        }

        [HttpPost]
        public async Task PrintAsync(PrintTicketInput input)
        {
            await _ticketSaleAppService.PrintAsync(input);
        }

        [HttpGet]
        public JsonResult GetConsumeTypeComboboxItems()
        {
            var items = typeof(ConsumeType).ToComboboxItems();

            return Json(items);
        }

        [HttpGet]
        public async Task<decimal> GetOrderRealMoneyAsync(string listNo)
        {
            return await _ticketSaleQueryAppService.GetOrderRealMoneyAsync(listNo);
        }

        [HttpPost]
        public async Task<string> DownloadInvoiceAsync(DownloadInvoiceInput input)
        {
            return await _ticketSaleQueryAppService.DownloadInvoiceAsync(input);
        }

        [HttpPost]
        public async Task<JsonResult> GetMemberTicketsForMobileAsync(GetMemberTicketsInput input)
        {
            var tickets = await _ticketSaleQueryAppService.GetMemberTicketsForMobileAsync(input);

            return new JsonResult(tickets);
        }

        [HttpPost]
        [PermissionFilter(Permissions.TMSAdmin_SearchTicketSale)]
        public async Task<JsonResult> QueryTicketSalesAsync(QueryTicketSaleInput input)
        {
            var result = await _ticketSaleQueryAppService.QueryTicketSalesAsync(input);

            return Json(result);
        }

        [HttpPost]
        [DontWrapResult]
        [PermissionFilter(Permissions.TMSAdmin_SearchTicketSale)]
        public async Task<FileContentResult> QueryTicketSalesToExcelAsync(QueryTicketSaleInput input)
        {
            var fileContents = await _ticketSaleQueryAppService.QueryTicketSalesToExcelAsync(input);

            return Excel(fileContents);
        }

        [HttpGet]
        public async Task<JsonResult> GetTicketFullInfoAsync(string ticketCode)
        {
            var result = await _ticketSaleQueryAppService.GetTicketFullInfoAsync(ticketCode);

            return Json(result);
        }

        [HttpPost]
        public async Task<JsonResult> GetLastCheckTicketInfoAsync(GetLastCheckTicketInfoInput input)
        {
            var result = await _ticketSaleQueryAppService.GetLastCheckTicketInfoAsync(input);

            return Json(result);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<JsonResult> StatTouristByAgeRangeAsync(StatTouristByAgeRangeInput input)
        {
            var result = await _ticketSaleQueryAppService.StatTouristByAgeRangeAsync(input);

            return new JsonResult(result);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<JsonResult> StatTouristByAreaAsync(StatTouristByAreaInput input)
        {
            var result = await _ticketSaleQueryAppService.StatTouristByAreaAsync(input);

            return new JsonResult(result);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<JsonResult> StatTouristBySexAsync(StatTouristBySexInput input)
        {
            var result = await _ticketSaleQueryAppService.StatTouristBySexAsync(input);

            return new JsonResult(result);
        }

        [HttpPost]
        [PermissionFilter(Permissions.TMSAdmin_SearchTicketCheck)]
        public async Task<JsonResult> QueryTicketChecksAsync(QueryTicketCheckInput input)
        {
            var result = await _ticketSaleQueryAppService.QueryTicketChecksAsync(input);

            return Json(result);
        }

        [HttpPost]
        [AllowAnonymous]
        [PermissionFilter(Permissions.TMSAdmin_StatTicketCheckIn)]
        public async Task<JsonResult> StatTicketCheckInAsync(StatTicketCheckInInput input)
        {
            var result = await _ticketSaleQueryAppService.StatTicketCheckInAsync(input);

            return new JsonResult(result);
        }

        [HttpPost]
        [PermissionFilter(Permissions.TMSAdmin_StatTicketCheckByPark)]
        public async Task<JsonResult> StatTicketCheckInByDateAndParkAsync(StatTicketCheckInInput input)
        {
            var result = await _ticketSaleQueryAppService.StatTicketCheckInByDateAndParkAsync(input);

            return Json(result);
        }

        [HttpPost]
        [PermissionFilter(Permissions.TMSAdmin_StatTicketCheckByGateGroup)]
        public async Task<JsonResult> StatTicketCheckInByGateGroupAsync(StatTicketCheckInInput input)
        {
            var result = await _ticketSaleQueryAppService.StatTicketCheckInByGateGroupAsync(input);

            return Json(result);
        }

        [HttpPost]
        [PermissionFilter(Permissions.TMSWeChat_TicketCheckStat)]
        public async Task<JsonResult> StatTicketCheckInByGroundAndGateGroupAsync(StatTicketCheckInInput input)
        {
            var result = await _ticketSaleQueryAppService.StatTicketCheckInByGroundAndGateGroupAsync(input);

            return Json(result);
        }

        [HttpPost]
        [PermissionFilter(Permissions.TMSWeChat_TicketCheckStat)]
        public async Task<JsonResult> StatTicketCheckByGroundAndTimeAsync(StatTicketCheckInInput input)
        {
            var result = await _ticketSaleQueryAppService.StatTicketCheckByGroundAndTimeAsync(input);

            return Json(result);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<JsonResult> StatStadiumTicketCheckInAsync(StatTicketCheckInInput input)
        {
            var result = await _ticketSaleQueryAppService.StatStadiumTicketCheckInAsync(input);

            return new JsonResult(result);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<JsonResult> StatTicketCheckByTradeSourceAsync(StatTicketCheckInInput input)
        {
            var result = await _ticketSaleQueryAppService.StatTicketCheckByTradeSourceAsync(input);

            return new JsonResult(result);
        }

        [HttpPost]
        public async Task<JsonResult> StatTicketCheckInByTicketTypeAsync(StatTicketCheckInInput input)
        {
            var result = await _ticketSaleQueryAppService.StatTicketCheckInByTicketTypeAsync(input);

            return Json(result);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<JsonResult> StatTicketCheckInYearOverYearComparisonAsync(StatTicketCheckInInput input)
        {
            var result = await _ticketSaleQueryAppService.StatTicketCheckInYearOverYearComparisonAsync(input);

            return new JsonResult(result);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<JsonResult> GetTicketCheckOverviewAsync(GetTicketCheckOverviewInput input)
        {
            var result = await _ticketSaleQueryAppService.GetTicketCheckOverviewAsync(input);

            return new JsonResult(result);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<JsonResult> GetStadiumTicketCheckOverviewAsync(GetTicketCheckOverviewInput input)
        {
            var result = await _ticketSaleQueryAppService.GetStadiumTicketCheckOverviewAsync(input);

            return new JsonResult(result);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<int> GetScenicCheckInQuantityAsync(GetTicketCheckOverviewInput input)
        {
            return await _ticketSaleQueryAppService.GetScenicCheckInQuantityAsync(input);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<JsonResult> StatTicketCheckInAverageAsync(StatTicketCheckInInput input)
        {
            var result = await _ticketSaleQueryAppService.StatTicketCheckInAverageAsync(input);

            return new JsonResult(result);
        }

        [HttpGet]
        public JsonResult GetTicketStatusComboboxItems()
        {
            var items = _ticketSaleQueryAppService.GetTicketStatusComboboxItems();

            return Json(items);
        }

        [HttpPost]
        [PermissionFilter(Permissions.TMSWeChat_TicketSaleStat, Permissions.TMSAdmin_SearchTicketSale)]
        public async Task<JsonResult> StatTicketSaleAsync(StatTicketSaleInput input)
        {
            var result = await _ticketSaleQueryAppService.StatTicketSaleAsync(input);

            return Json(result);
        }

        [HttpPost]
        [PermissionFilter(Permissions.TMSAdmin_StatCashierSale)]
        public async Task<JsonResult> StatCashierSaleAsync(StatCashierSaleInput input)
        {
            var result = await _ticketSaleQueryAppService.StatCashierSaleAsync(input);

            return Json(result);
        }

        [HttpPost]
        [PermissionFilter(Permissions.TMSAdmin_StatTicketSaleByTradeSource)]
        public async Task<JsonResult> StatTicketSaleByTradeSourceAsync(StatTicketSaleByTradeSourceInput input)
        {
            var result = await _ticketSaleQueryAppService.StatTicketSaleByTradeSourceAsync(input);

            return Json(result);
        }

        [HttpPost]
        [PermissionFilter(Permissions.TMSAdmin_SearchReprintLog)]
        public async Task<JsonResult> QueryReprintLogAsync(QueryReprintLogInput input)
        {
            var result = await _ticketSaleQueryAppService.QueryReprintLogsAsync(input);

            return Json(result);
        }

        [HttpPost]
        [PermissionFilter(Permissions.TMSAdmin_SearchExchangeHistory)]
        public async Task<JsonResult> QueryExchangeHistoryAsync(QueryExchangeHistoryInput input)
        {
            var result = await _ticketSaleQueryAppService.QueryExchangeHistorysAsync(input);

            return Json(result);
        }

        [HttpPost]
        [PermissionFilter(Permissions.TMSAdmin_StatShift)]
        public async Task<JsonResult> StatExchangeHistoryJbAsync(StatJbInput input)
        {
            var result = await _ticketSaleQueryAppService.StatExchangeHistoryJbAsync(input);

            return Json(result);
        }

        [HttpPost]
        [PermissionFilter(Permissions.TMSAdmin_StatTicketSaleByTicketTypeClass)]
        public async Task<JsonResult> StatTicketSaleByTicketTypeClassAsync(StatTicketSaleByTicketTypeClassInput input)
        {
            var result = await _ticketSaleQueryAppService.StatTicketSaleByTicketTypeClassAsync(input);

            return Json(result);
        }

        [HttpPost]
        [DontWrapResult]
        [PermissionFilter(Permissions.TMSAdmin_StatTicketSaleByPayType)]
        public async Task<FileContentResult> StatTicketSaleByPayTypeToExcelAsync(StatTicketSaleByPayTypeInput input)
        {
            var fileContents = await _ticketSaleQueryAppService.StatTicketSaleByPayTypeToExcelAsync(input);

            return Excel(fileContents);
        }

        [HttpPost]
        [PermissionFilter(Permissions.TMSAdmin_StatTicketSaleByPayType)]
        public async Task<JsonResult> StatTicketSaleByPayTypeAsync(StatTicketSaleByPayTypeInput input)
        {
            var result = await _ticketSaleQueryAppService.StatTicketSaleByPayTypeAsync(input);

            return Json(result);
        }

        [HttpPost]
        [PermissionFilter(Permissions.TMSAdmin_StatTicketSaleBySalePoint)]
        public async Task<JsonResult> StatTicketSaleBySalePointAsync(StatTicketSaleBySalePointInput input)
        {
            var result = await _ticketSaleQueryAppService.StatTicketSaleBySalePointAsync(input);

            return Json(result);
        }

        [HttpPost]
        [PermissionFilter(Permissions.TMSAdmin_StatTicketSaleGroundSharing)]
        public async Task<JsonResult> StatTicketSaleGroundSharingAsync(StatGroundSharingInput input)
        {
            var result = await _ticketSaleQueryAppService.StatTicketSaleGroundSharingAsync(input);

            return Json(result);
        }

        [HttpPost]
        [PermissionFilter(Permissions.TMSAdmin_StatShift)]
        public async Task<JsonResult> StatTicketSaleJbAsync(StatJbInput input)
        {
            var result = await _ticketSaleQueryAppService.StatTicketSaleJbAsync(input);

            return Json(result);
        }

        [HttpPost]
        [PermissionFilter(Permissions.TMSAdmin_StatGroundChangCiSale)]
        public async Task<FileContentResult> StatGroundChangCiSaleToExcelAsync(StatGroundChangCiSaleInput input)
        {
            var fileContents = await _ticketSaleQueryAppService.StatGroundChangCiSaleToExcelAsync(input);

            return Excel(fileContents);
        }

        [HttpPost]
        [PermissionFilter(Permissions.TMSAdmin_StatGroundChangCiSale)]
        public async Task<JsonResult> StatGroundChangCiSaleAsync(StatGroundChangCiSaleInput input)
        {
            var result = await _ticketSaleQueryAppService.StatGroundChangCiSaleAsync(input);

            return Json(result);
        }

        [HttpPost]
        [DontWrapResult]
        [PermissionFilter(Permissions.TMSAdmin_SearchTicketConsume)]
        public async Task<FileContentResult> QueryTicketConsumesToExcelAsync(QueryTicketConsumeInput input)
        {
            var fileContents = await _ticketSaleQueryAppService.QueryTicketConsumesToExcelAsync(input);

            return Excel(fileContents);
        }

        [HttpPost]
        [PermissionFilter(Permissions.TMSAdmin_SearchTicketConsume)]
        public async Task<JsonResult> QueryTicketConsumesAsync(QueryTicketConsumeInput input)
        {
            var result = await _ticketSaleQueryAppService.QueryTicketConsumesAsync(input);

            return Json(result);
        }

        [HttpPost]
        [DontWrapResult]
        [PermissionFilter(Permissions.TMSAdmin_StatTicketConsume)]
        public async Task<FileContentResult> StatTicketConsumeToExcelAsync(StatTicketConsumeInput input)
        {
            var fileContents = await _ticketSaleQueryAppService.StatTicketConsumeToExcelAsync(input);

            return Excel(fileContents);
        }

        [HttpPost]
        [PermissionFilter(Permissions.TMSAdmin_StatTicketConsume)]
        public async Task<JsonResult> StatTicketConsumeAsync(StatTicketConsumeInput input)
        {
            var result = await _ticketSaleQueryAppService.StatTicketConsumeAsync(input);

            return Json(result);
        }
    }
}
