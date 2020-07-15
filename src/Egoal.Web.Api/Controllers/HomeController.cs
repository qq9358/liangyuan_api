using Egoal.Models;
using Egoal.Mvc.Uow;
using Egoal.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Egoal.Web.Api.Controllers
{
    [Route("/")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly ISettingAppService _settingAppService;

        public HomeController(ISettingAppService settingAppService)
        {
            _settingAppService = settingAppService;
        }

        // GET: api/Home
        [HttpGet]
        [AllowAnonymous]
        [DontWrapResult]
        [UnitOfWork]
        public async Task<string> Get()
        {
            try
            {
                var options = await _settingAppService.GetOrderNoticeAsync();

                return $"{options.ScenicName}WebApi接口V5.5.5";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
