using Egoal.Authorization;
using Egoal.Extensions;
using Egoal.Mvc.Authorization;
using Egoal.ThirdPlatforms;
using Egoal.ThirdPlatforms.Dto;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Egoal.Web.Api.Controllers
{
    public class ThirdPlatformController : TmsControllerBase
    {
        private readonly IThirdPlatformAppService _thirdPlatformAppService;

        public ThirdPlatformController(IThirdPlatformAppService thirdPlatformAppService)
        {
            _thirdPlatformAppService = thirdPlatformAppService;
        }

        [HttpPost]
        [PermissionFilter(Permissions.TMSAdmin_ThirdPlatformSetting)]
        public async Task CreateAsync(ThirdPlatformDto input)
        {
            await _thirdPlatformAppService.CreateAsync(input);
        }

        [HttpPut]
        [PermissionFilter(Permissions.TMSAdmin_ThirdPlatformSetting)]
        public async Task UpdateAsync(ThirdPlatformDto input)
        {
            await _thirdPlatformAppService.UpdateAsync(input);
        }

        [HttpDelete]
        [PermissionFilter(Permissions.TMSAdmin_ThirdPlatformSetting)]
        public async Task DeleteAsync(string id)
        {
            await _thirdPlatformAppService.DeleteAsync(id);
        }

        [HttpGet]
        [PermissionFilter(Permissions.TMSAdmin_ThirdPlatformSetting)]
        public async Task<JsonResult> GetThirdPlatformsAsync(string uid)
        {
            var result = await _thirdPlatformAppService.GetThirdPlatformsAsync(uid);

            return Json(result);
        }

        [HttpGet]
        [PermissionFilter(Permissions.TMSAdmin_ThirdPlatformSetting)]
        public async Task<JsonResult> GetThirdPlatformForEditAsync(string id)
        {
            var result = await _thirdPlatformAppService.GetThirdPlatformForEditAsync(id);

            return Json(result);
        }

        [HttpGet]
        public JsonResult GetPlatformTypeComboboxItems()
        {
            var items = typeof(PlatformType).ToComboboxItems();

            return Json(items);
        }
    }
}
