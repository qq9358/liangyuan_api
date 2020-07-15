using Egoal.Extensions;
using Egoal.Models;
using Egoal.Mvc.Uow;
using Egoal.Payment;
using Egoal.Payment.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Egoal.Web.Api.Controllers
{
    public class PaymentController : TmsControllerBase
    {
        private readonly IPayAppService _payAppService;

        public PaymentController(IPayAppService payAppService)
        {
            _payAppService = payAppService;
        }

        [HttpGet]
        public async Task<JsonResult> GetNetPayOrderAsync(string listNo)
        {
            var result = await _payAppService.GetNetPayOrderAsync(listNo);

            return Json(result);
        }

        [HttpPost]
        public async Task<string> JsApiPayAsync(JsApiPayInput input)
        {
            return await _payAppService.JsApiPayAsync(input);
        }

        [HttpPost]
        [UnitOfWork(IsDisabled = true)]
        public async Task<JsonResult> MicroPayAsync(MicroPayInput payInput)
        {
            var result = await _payAppService.MicroPayAsync(payInput);

            return Json(result);
        }

        [HttpPost]
        public async Task<string> NativePayAsync(NativePayInput input)
        {
            return await _payAppService.NativePayAsync(input);
        }

        [HttpPost]
        public async Task<string> H5PayAsync(H5PayInput input)
        {
            return await _payAppService.H5PayAsync(input);
        }

        [HttpPost]
        public async Task<JsonResult> CashPayAsync(string listNo)
        {
            var result = await _payAppService.CashPayAsync(listNo);

            return Json(result);
        }

        [HttpPost]
        [AllowAnonymous]
        [UnitOfWork(IsDisabled = true)]
        [DontWrapResult]
        public async Task<string> WeChatNotify()
        {
            string args = await Request.Body.ReadAsStringAsync();

            var output = await _payAppService.HandlePayNotifyAsync(args, DefaultPayType.微信支付);

            return output.Data;
        }

        [HttpPost]
        [AllowAnonymous]
        [UnitOfWork(IsDisabled = true)]
        [DontWrapResult]
        public async Task<string> AlipayNotify()
        {
            string args = await Request.Body.ReadAsStringAsync();

            var output = await _payAppService.HandlePayNotifyAsync(args, DefaultPayType.支付宝付款);

            return output.Data;
        }

        [HttpPost]
        [AllowAnonymous]
        [UnitOfWork(IsDisabled = true)]
        [DontWrapResult]
        public async Task<string> SaobeNotify()
        {
            string args = await Request.Body.ReadAsStringAsync();

            var output = await _payAppService.HandlePayNotifyAsync(args, DefaultPayType.扫呗支付);

            return output.Data;
        }

        [HttpPost]
        [AllowAnonymous]
        [UnitOfWork(IsDisabled = true)]
        [DontWrapResult]
        public async Task<string> IcbcPayNotify()
        {
            string args = await Request.Body.ReadAsStringAsync();

            var output = await _payAppService.HandlePayNotifyAsync(args, DefaultPayType.工商银行);

            return output.Data;
        }
    }
}
