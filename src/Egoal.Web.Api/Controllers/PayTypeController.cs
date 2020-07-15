using Egoal.Payment;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Egoal.Web.Api.Controllers
{
    public class PayTypeController : TmsControllerBase
    {
        private readonly IPayTypeAppService _payTypeAppService;

        public PayTypeController(IPayTypeAppService payTypeAppService)
        {
            _payTypeAppService = payTypeAppService;
        }

        [HttpGet]
        public async Task<JsonResult> GetPayTypeComboboxItemsAsync()
        {
            var result = await _payTypeAppService.GetPayTypeComboboxItemsAsync();

            return Json(result);
        }

        [HttpGet]
        public async Task<JsonResult> GetNetPayTypeComboboxItemsAsync()
        {
            var result = await _payTypeAppService.GetNetPayTypeComboboxItemsAsync();

            return Json(result);
        }
    }
}
