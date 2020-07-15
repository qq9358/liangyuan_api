using Egoal.Authorization;
using Egoal.Mvc.Authorization;
using Egoal.Mvc.Uow;
using Egoal.Scenics;
using Egoal.Scenics.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Egoal.Web.Api.Controllers
{
    public class ScenicController : TmsControllerBase
    {
        private readonly IScenicAppService _scenicAppService;

        public ScenicController(IScenicAppService scenicAppService)
        {
            _scenicAppService = scenicAppService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<JsonResult> GetScenicAsync(string language)
        {
            var result = await _scenicAppService.GetScenicAsync(language);

            return Json(result);
        }

        [HttpPost]
        [UnitOfWork(IsDisabled = true)]
        [PermissionFilter(Permissions.TMSAdmin_ScenicSetting)]
        public async Task EditScenicAsync(ScenicDto input)
        {
            await _scenicAppService.EditScenicAsync(input);
        }

        [HttpGet]
        public JsonResult GetScenicOptions()
        {
            var result = _scenicAppService.GetScenicOptions();

            return Json(result);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<JsonResult> BookGroundChangCiAsync(BookGroundChangCiInput input)
        {
            var result = await _scenicAppService.BookGroundChangCiAsync(input);

            return Json(result);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task CancelGroundChangCiAsync(CancelGroundChangCiInput input)
        {
            await _scenicAppService.CancelGroundChangCiAsync(input);
        }

        [HttpGet]
        public async Task<JsonResult> GetParkComboboxItemsAsync()
        {
            var result = await _scenicAppService.GetParkComboboxItemsAsync();

            return Json(result);
        }

        [HttpGet]
        public async Task<JsonResult> GetGroundComboboxItemsAsync()
        {
            var result = await _scenicAppService.GetGroundComboboxItemsAsync();

            return Json(result);
        }

        [HttpGet]
        public async Task<JsonResult> GetGateGroupComboboxItemsAsync(int? groundId)
        {
            var result = await _scenicAppService.GetGateGroupComboboxItemsAsync(groundId);

            return Json(result);
        }

        [HttpGet]
        public async Task<JsonResult> GetSalePointComboboxItemsAsync(int? parkId)
        {
            var result = await _scenicAppService.GetSalePointComboboxItemsAsync(parkId);

            return Json(result);
        }
    }
}
