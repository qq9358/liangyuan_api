using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Egoal.Runtime.Session
{
    public class AspNetCorePrincipalAccessor : DefaultPrincipalAccessor
    {
        public override ClaimsPrincipal Principal => _httpContextAccessor.HttpContext?.User ?? base.Principal;

        private readonly IHttpContextAccessor _httpContextAccessor;

        public AspNetCorePrincipalAccessor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
    }
}
