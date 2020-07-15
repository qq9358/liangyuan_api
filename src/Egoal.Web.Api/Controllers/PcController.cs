using Egoal.Scenics;
using Egoal.Scenics.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Egoal.Web.Api.Controllers
{
    public class PcController : TmsControllerBase
    {
        private readonly IPcAppService _pcAppService;

        public PcController(IPcAppService pcAppService)
        {
            _pcAppService = pcAppService;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<JsonResult> RegistHandsetAsync(RegistPcInput input)
        {
            var result = await _pcAppService.RegistHandsetAsync(input);

            return Json(result);
        }

        [HttpPut]
        public async Task<JsonResult> ChangeLocationAsync(ChangePcLocationInput input)
        {
            var result = await _pcAppService.ChangeLocationAsync(input);

            return Json(result);
        }

        [HttpGet]
        public async Task<JsonResult> GetCashPcComboboxItemsAsync()
        {
            var result = await _pcAppService.GetCashPcComboboxItemsAsync();

            return Json(result);
        }
    }
}
