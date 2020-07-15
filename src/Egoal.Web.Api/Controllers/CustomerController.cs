using Egoal.Application.Services.Dto;
using Egoal.Customers;
using Egoal.Customers.Dto;
using Egoal.Extensions;
using Egoal.Members;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Egoal.Web.Api.Controllers
{
    public class CustomerController : TmsControllerBase
    {
        private readonly ICustomerAppService _customerAppService;
        private readonly IHostingEnvironment _hostingEnvironment;

        public CustomerController(
            ICustomerAppService customerAppService, IHostingEnvironment hostingEnvironment)
        {
            _customerAppService = customerAppService;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpPost]
        public async Task RegistFromMobileAsync(MobileRegistInput input)
        {
            await _customerAppService.RegistFromMobileAsync(input);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task CreateAsync(EditDto input)
        {
            await _customerAppService.CreateAsync(input);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<JsonResult> GetForEditAsync(Guid id)
        {
            var editDto = await _customerAppService.GetForEditAsync(id);

            return new JsonResult(editDto);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task EditAsync(EditDto input)
        {
            await _customerAppService.EditAsync(input);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task AuditAsync(AuditInput input)
        {
            await _customerAppService.AuditAsync(input);
        }

        [HttpPost]
        public async Task ChangePasswordAsync(ChangePasswordInput input)
        {
            await _customerAppService.ChangePasswordAsync(input);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task DeleteAsync(EntityDto<Guid> input)
        {
            await _customerAppService.DeleteAsync(input.Id);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task UnBindMemberAsync(UnBindMemberInput input)
        {
            await _customerAppService.UnBindMemberAsync(input);
        }

        [HttpPost]
        public async Task<JsonResult> LoginFromWeChatAsync(CustomerLoginInput input)
        {
            var result = await _customerAppService.LoginFromWeChatAsync(input);

            return Json(result);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<JsonResult> GetCustomersAsync(GetCustomersInput input)
        {
            var result = await _customerAppService.GetCustomersAsync(input);
            string filePath = _hostingEnvironment.WebRootPath + "/Img/";
            foreach (CustomerListDto customer in result.Items)
            {
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                string fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".jpg";
                using (FileStream fs = new FileStream(filePath + fileName, FileMode.Create, FileAccess.Write))
                {
                    fs.Write(customer.Photo, 0, customer.Photo.Length);
                    fs.Close();
                }
                customer.PhotoSrc = Request.Scheme + "://" + Request.Host + "/Img/" + fileName;
            }
            return new JsonResult(result);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<JsonResult> GetCustomerTypeComboboxItemsAsync()
        {
            var items = await _customerAppService.GetCustomerTypeComboboxItemsAsync();

            return new JsonResult(items);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<JsonResult> GetCustomerComboboxItemsAsync()
        {
            var items = await _customerAppService.GetCustomerComboboxItemsAsync();

            return new JsonResult(items);
        }

        [HttpGet]
        [AllowAnonymous]
        public JsonResult GetCustomerStatusComboboxItemsAsync()
        {
            var items = typeof(MemberStatus).ToComboboxItems();

            return new JsonResult(items);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task RegistGuiderFromMobileAsync(RegistGuiderInput input)
        {
            await _customerAppService.RegistGuiderFromMobileAsync(input);
        }

        [HttpGet]
        public async Task<JsonResult> GetGuiderForEditAsync(Guid id)
        {
            var result = await _customerAppService.GetGuiderForEditAsync(id);

            return Json(result);
        }

        [HttpPost]
        public async Task EditGuiderAsync(EditGuiderDto input)
        {
            await _customerAppService.EditGuiderAsync(input);
        }

        [HttpPost]
        public async Task ChangeGuiderPasswordAsync(ChangePasswordInput input)
        {
            await _customerAppService.ChangeGuiderPasswordAsync(input);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<JsonResult> GuiderLoginAsync(GuiderLoginInput input)
        {
            var result = await _customerAppService.GuiderLoginAsync(input);

            return Json(result);
        }

        [HttpPost]
        [AllowAnonymous]
        /// <summary>
        /// 忘了密码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task ForgetGuidePasswordAsync(ForgetPasswordInput input)
        {
            await _customerAppService.ForgetGuidePasswordAsync(input);
        }

        [HttpPost]
        public async Task<JsonResult> GuiderLogoutAsync()
        {
            var result = await _customerAppService.GuiderLogoutAsync();

            return Json(result);
        }

        [HttpPost]
        [AllowAnonymous]
        /// <summary>
        /// 判断导游用户名是否存在
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public async Task<JsonResult> GetGuideAsync(string userName)
        {
            Member guide = await _customerAppService.GetGuideAsync(userName);
            return Json(guide.AreaName);
        }

        [HttpPost]
        [AllowAnonymous]
        /// <summary>
        /// 注册时获取验证码
        /// </summary>
        /// <param name="phoneNum"></param>
        /// <returns></returns>
        public async Task RegisterSendVerificationCodeAsync(string phoneNum)
        {
            await _customerAppService.RegisterSendVerificationCodeAsync(phoneNum);
        }
    }
}
