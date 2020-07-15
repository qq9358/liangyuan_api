using Egoal.Authorization;
using Egoal.Mvc.Authorization;
using Egoal.Staffs;
using Egoal.Staffs.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Egoal.Web.Api.Controllers
{
    public class ExplainerController : TmsControllerBase
    {
        private readonly IExplainerAppService _explainerAppService;

        public ExplainerController(IExplainerAppService explainerAppService)
        {
            _explainerAppService = explainerAppService;
        }

        [HttpPost]
        public async Task<JsonResult> GetSchedulingsAsync(GetSchedulingInput input)
        {
            var output = await _explainerAppService.GetSchedulingsAsync(input);

            return new JsonResult(output);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<JsonResult> GetReservedSchedulingComboxItemsAsync(GetSchedulingInput input)
        {
            var items = await _explainerAppService.GetReservedSchedulingComboxItemsAsync(input);

            return new JsonResult(items);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<JsonResult> GetExplainTimeslotComboboxItemsAsync()
        {
            var items = await _explainerAppService.GetExplainTimeslotComboboxItemsAsync();

            return new JsonResult(items);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<JsonResult> GetExplainerComboboxItemsAsync()
        {
            var items = await _explainerAppService.GetExplainerComboboxItemsAsync();

            return new JsonResult(items);
        }

        [HttpPost]
        [PermissionFilter(Permissions.TMSWeChat_GroupExplain)]
        public async Task BeginExplainAsync(ExplainInput input)
        {
            await _explainerAppService.BeginExplainAsync(input);
        }

        [HttpPost]
        [PermissionFilter(Permissions.TMSWeChat_GroupExplain)]
        public async Task CompleteExplainAsync(ExplainInput input)
        {
            await _explainerAppService.CompleteExplainAsync(input);
        }
    }
}
