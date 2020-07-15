using Egoal.Application.Services.Dto;
using Egoal.Members;
using Egoal.Members.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Egoal.Web.Api.Controllers
{
    public class MemberController : TmsControllerBase
    {
        private readonly IMemberAppService _memberAppService;
        private readonly Runtime.Session.ISession _session;

        public MemberController(
            IMemberAppService memberAppService,
            Runtime.Session.ISession session)
        {
            _memberAppService = memberAppService;
            _session = session;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<JsonResult> LoginFromWeChatAsync(WechatOffiaccountLoginInput input)
        {
            var result = await _memberAppService.LoginFromWechatOffiaccountAsync(input);

            return Json(result);
        }

        /// <summary>
        /// 小程序登录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<JsonResult> LoginFromWechatMiniProgramAsync(WechatMiniProgramLoginInput input)
        {
            var result = await _memberAppService.LoginFromWechatMiniProgramAsync(input);

            return Json(result);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<JsonResult> LoginOrRegistFromH5Async(H5LoginInput input)
        {
            var result = await _memberAppService.LoginOrRegistFromH5Async(input);

            return Json(result);
        }

        [HttpPost]
        public async Task<JsonResult> BindStaffAsync(BindStaffInput input)
        {
            var result = await _memberAppService.BindStaffAsync(input);

            return Json(result);
        }

        [HttpGet]
        public async Task<JsonResult> GetElectronicMemberCardAsync()
        {
            var memberCardDto = await _memberAppService.GetElectronicMemberCardAsync(_session.MemberId.Value);

            return new JsonResult(memberCardDto);
        }

        [HttpPost]
        public async Task RenewMemberCardAsync(EntityDto input)
        {
            await _memberAppService.RenewMemberCardAsync(input.Id);
        }

        [HttpGet]
        public async Task<JsonResult> GetMemberComboboxItemsAsync()
        {
            var result = await _memberAppService.GetMemberComboboxItemsAsync();

            return Json(result);
        }
    }
}