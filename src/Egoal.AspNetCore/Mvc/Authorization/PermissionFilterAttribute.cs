using Egoal.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Egoal.Mvc.Authorization
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class PermissionFilterAttribute : Attribute, IAsyncAuthorizationFilter
    {
        public string[] Permissions { get; }

        public PermissionFilterAttribute(params string[] permissions)
        {
            Permissions = permissions;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var authorizationService = context.HttpContext.RequestServices.GetRequiredService<IAuthorizationService>();
            var authorizationResult = await authorizationService.AuthorizeAsync(context.HttpContext.User, Permissions, new PermissionRequirement());
            if (!authorizationResult.Succeeded)
            {
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;

                context.Result = new ObjectResult(new AjaxResponse(new ErrorInfo("没有权限进行此操作")));
            }
        }
    }
}
