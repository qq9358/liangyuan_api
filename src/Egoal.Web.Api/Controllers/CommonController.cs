using Egoal.Barcode;
using Egoal.Common;
using Egoal.Extensions;
using Egoal.IO;
using Egoal.Mvc.ModelBinding;
using Egoal.Settings;
using Egoal.Upload;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.IO;
using System.Threading.Tasks;

namespace Egoal.Web.Api.Controllers
{
    public class CommonController : TmsControllerBase
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ILogger _logger;
        private readonly ParkOptions _parkOptions;
        private readonly ICommonAppService _commonAppService;

        public CommonController(
             IHostingEnvironment hostingEnvironment,
             ILogger<CommonController> logger,
             IOptions<ParkOptions> options,
             ICommonAppService commonAppService)
        {
            _hostingEnvironment = hostingEnvironment;
            _logger = logger;
            _parkOptions = options.Value;
            _commonAppService = commonAppService;
        }

        [HttpPost]
        public void LogError(string message)
        {
            _logger.LogError(message);
        }

        [HttpGet]
        public JsonResult GetEducationComboboxItems()
        {
            var items = _commonAppService.GetEducationComboboxItems();

            return new JsonResult(items);
        }

        [HttpGet]
        public JsonResult GetNationComboboxItems()
        {
            var items = _commonAppService.GetNationComboboxItems();

            return new JsonResult(items);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<JsonResult> GetAgeRangeComboboxItemsAsync()
        {
            var items = await _commonAppService.GetAgeRangeComboboxItemsAsync();

            return new JsonResult(items);
        }

        [HttpGet]
        public async Task<JsonResult> GetTouristOriginTreeAsync()
        {
            var node = await _commonAppService.GetTouristOriginTreeAsync();

            return new JsonResult(node);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<JsonResult> GetCertTypeComboboxItemsAsync()
        {
            var items = await _commonAppService.GetCertTypeComboboxItemsAsync();

            return new JsonResult(items);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<JsonResult> GetChangCiForSaleAsync(string date)
        {
            var result = await _commonAppService.GetChangCiForSaleAsync(date);

            return Json(result);
        }

        [HttpGet]
        public async Task<string> CreateQRCodeAsync(string value)
        {
            return await QRCodeHelper.ToDataURLAsync(value);
        }

        [HttpPost]
        [DisableFormValueModelBinding]
        public async Task<string> UploadImageAsync()
        {
            var webRootPath = _hostingEnvironment.WebRootPath;
            if (webRootPath.IsNullOrEmpty())
            {
                webRootPath = Path.Combine(_hostingEnvironment.ContentRootPath, "wwwroot");
            }

            var fileUrl = "images";
            var directory = Path.Combine(webRootPath, fileUrl);
            DirectoryHelper.CreateIfNotExists(directory);

            var formValueProvider = await UploadHelper.UploadAsync(Request, async (stream, ext) =>
            {
                var fileName = $"{Path.GetRandomFileName()}{ext}";
                fileUrl = fileUrl.UrlCombine(fileName);
                var physicalFileName = Path.Combine(directory, fileName);

                using (var targetStream = System.IO.File.Create(physicalFileName))
                {
                    await stream.CopyToAsync(targetStream);
                }
            });

            return _parkOptions.WebApiUrl.UrlCombine(fileUrl);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task SendVerificationCodeAsync(string address)
        {
            await _commonAppService.SendVerificationCodeAsync(address);
        }
    }
}