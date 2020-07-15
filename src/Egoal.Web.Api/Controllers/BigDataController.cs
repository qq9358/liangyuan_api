using Egoal.Models;
using Egoal.Mvc.Uow;
using Egoal.Thirdparties.BigData;
using Egoal.Thirdparties.BigData.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Data;
using System.Threading.Tasks;

namespace Egoal.Web.Api.Controllers
{
    public class BigDataController : TmsControllerBase
    {
        private readonly IBigDataAppService _bigDataAppService;

        public BigDataController(IBigDataAppService bigDataAppService)
        {
            _bigDataAppService = bigDataAppService;
        }

        [HttpPost]
        [AllowAnonymous]
        [DontWrapResult]
        [UnitOfWork(IsolationLevel.ReadUncommitted)]
        public async Task<JsonResult> Api(RequestDto request)
        {
            ResponseDto response = null;
            Exception exception = null;

            try
            {
                response = await _bigDataAppService.ProcessRequestAsync(request);
            }
            catch (Exception ex)
            {
                exception = ex;
                response = new ResponseDto();
                var exceptionType = ex.GetType().Name;
                if (exceptionType.ToLower() == "exception")
                {
                    response.message = "操作失败请稍后重试";
                }
                else
                {
                    response.message = ex.Message;
                }
            }

            await _bigDataAppService.AddApiLogAsync(request, exception);

            return Json(response);
        }
    }
}
