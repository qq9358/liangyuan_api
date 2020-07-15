using Egoal.Mvc.Uow;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Newtonsoft.Json;

namespace Egoal.Web.Api.Controllers
{
    [Route("/[controller]/[action]")]
    [ApiController]
    [UnitOfWork]
    public class TmsControllerBase : ControllerBase
    {
        protected JsonResult Json(object value)
        {
            return new JsonResult(value);
        }

        protected JsonResult Json(object value, JsonSerializerSettings serializerSettings)
        {
            return new JsonResult(value, serializerSettings);
        }

        protected FileContentResult Excel(byte[] fileContents)
        {
            var contentType = GetContentType(".xlsx");

            return File(fileContents, contentType);
        }

        protected FileContentResult Pdf(byte[] fileContents)
        {
            var contentType = GetContentType(".pdf");

            return File(fileContents, contentType);
        }

        private string GetContentType(string fileExtension)
        {
            var provider = new FileExtensionContentTypeProvider();
            var contentType = provider.Mappings[fileExtension];

            return contentType;
        }
    }
}
