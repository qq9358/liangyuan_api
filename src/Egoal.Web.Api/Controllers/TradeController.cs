using Egoal.Authorization;
using Egoal.Extensions;
using Egoal.Models;
using Egoal.Mvc.Authorization;
using Egoal.Tickets.Dto;
using Egoal.Trades;
using Egoal.Trades.Dto;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Egoal.Web.Api.Controllers
{
    public class TradeController : TmsControllerBase
    {
        private readonly ITradeAppService _tradeAppService;

        public TradeController(ITradeAppService tradeAppService)
        {
            _tradeAppService = tradeAppService;
        }

        [HttpGet]
        public JsonResult GetTradeSourceComboboxItems()
        {
            var items = typeof(TradeSource).ToComboboxItems();

            return Json(items);
        }

        [HttpGet]
        public async Task<JsonResult> GetTradeTypeTypeComboboxItemsAsync()
        {
            var items = await _tradeAppService.GetTradeTypeTypeComboboxItemsAsync();

            return Json(items);
        }

        [HttpGet]
        public async Task<JsonResult> GetTradeTypeComboboxItemsAsync(int? tradeTypeTypeId)
        {
            var items = await _tradeAppService.GetTradeTypeComboboxItemsAsync(tradeTypeTypeId);

            return Json(items);
        }

        [HttpPost]
        [PermissionFilter(Permissions.TMSAdmin_SearchTrade)]
        public async Task<JsonResult> QueryTradesAsync(QueryTradeInput input)
        {
            var result = await _tradeAppService.QueryTradesAsync(input);

            return Json(result);
        }

        [HttpGet]
        public JsonResult GetPayDetailStatTypeComboboxItems()
        {
            var items = typeof(PayDetailStatType).ToComboboxItems();

            return Json(items);
        }

        [HttpPost]
        [DontWrapResult]
        [PermissionFilter(Permissions.TMSAdmin_StatPayDetail)]
        public async Task<FileContentResult> StatPayDetailToExcelAsync(StatPayDetailInput input)
        {
            var fileContents = await _tradeAppService.StatPayDetailToExcelAsync(input);

            return Excel(fileContents);
        }

        [HttpPost]
        [PermissionFilter(Permissions.TMSAdmin_StatPayDetail)]
        public async Task<JsonResult> StatPayDetailAsync(StatPayDetailInput input)
        {
            var result = await _tradeAppService.StatPayDetailAsync(input);

            return Json(result);
        }

        [HttpPost]
        [PermissionFilter(Permissions.TMSAdmin_StatShift, Permissions.TMSWeChat_TradeStat)]
        public async Task<JsonResult> StatPayDetailJbAsync(StatJbInput input)
        {
            var result = await _tradeAppService.StatPayDetailJbAsync(input);

            return Json(result);
        }

        [HttpPost]
        [PermissionFilter(Permissions.TMSWeChat_TradeStat)]
        public async Task<JsonResult> StatTradeAsync(StatTradeInput input)
        {
            var result = await _tradeAppService.StatTradeAsync(input);

            return Json(result);
        }

        [HttpPost]
        [DontWrapResult]
        [PermissionFilter(Permissions.TMSAdmin_StatTradeByPayType)]
        public async Task<FileContentResult> StatTradeByPayTypeToExcelAsync(StatTradeByPayTypeInput input)
        {
            var fileContents = await _tradeAppService.StatTradeByPayTypeToExcelAsync(input);

            return Excel(fileContents);
        }

        [HttpPost]
        [PermissionFilter(Permissions.TMSAdmin_StatTradeByPayType)]
        public async Task<JsonResult> StatTradeByPayTypeAsync(StatTradeByPayTypeInput input)
        {
            var result = await _tradeAppService.StatTradeByPayTypeAsync(input);

            return Json(result);
        }
    }
}
