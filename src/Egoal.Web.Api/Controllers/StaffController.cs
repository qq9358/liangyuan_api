using Egoal.Authorization;
using Egoal.Staffs;
using Egoal.Staffs.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Egoal.Web.Api.Controllers
{
    public class StaffController : TmsControllerBase
    {
        private readonly IStaffAppService _staffAppService;

        public StaffController(IStaffAppService staffAppService)
        {
            _staffAppService = staffAppService;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<JsonResult> LoginFromAdminAsync(LoginInput input)
        {
            var result = await _staffAppService.LoginAsync(input, SystemType.后台管理系统);

            return Json(result);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<JsonResult> LoginFromHandsetAsync(LoginInput input)
        {
            var result = await _staffAppService.LoginAsync(input, SystemType.安卓手持机系统);

            return Json(result);
        }

        [HttpPost]
        public async Task EditPasswordAsync(EditPasswordInput input)
        {
            await _staffAppService.EditPasswordAsync(input);
        }

        [HttpGet]
        public async Task<JsonResult> GetCashierComboboxItemsAsync()
        {
            var result = await _staffAppService.GetCashierComboboxItemsAsync();

            return Json(result);
        }

        [HttpGet]
        public async Task<JsonResult> GetSalesManComboboxItemsAsync()
        {
            var result = await _staffAppService.GetSalesManComboboxItemsAsync();

            return Json(result);
        }
    }
}
