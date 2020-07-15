using Egoal.Scenics;
using Egoal.Scenics.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Egoal.Web.Api.Controllers
{
    public class GateController : TmsControllerBase
    {
        private readonly IGateAppService _gateAppService;

        public GateController(IGateAppService gateAppService)
        {
            _gateAppService = gateAppService;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<JsonResult> RegistHandsetAsync(RegistGateInput input)
        {
            var output = await _gateAppService.GetOrRegistGateAsync(input, GateType.安卓手持机);

            return Json(output);
        }

        [HttpPost]
        public async Task<JsonResult> RegistMobileAsync(RegistGateInput input)
        {
            var result = await _gateAppService.GetOrRegistGateAsync(input, GateType.手机);

            return Json(result);
        }

        [HttpPut]
        public async Task ChangeLocationAsync(ChangeGateLocationInput input)
        {
            await _gateAppService.ChangeLocationAsync(input);
        }

        [HttpGet]
        public async Task<JsonResult> GetGateComboboxItemsAsync(int? gateGroupId)
        {
            var result = await _gateAppService.GetGateComboboxItemsAsync(gateGroupId);

            return Json(result);
        }
    }
}
