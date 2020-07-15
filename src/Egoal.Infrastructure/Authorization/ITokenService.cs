using System;
using System.Security.Claims;

namespace Egoal.Authorization
{
    public interface ITokenService
    {
        string CreateToken(ClaimsIdentity claimsIdentity, TimeSpan? validTime = null);
        void ConfigJwtOptions();
    }
}
