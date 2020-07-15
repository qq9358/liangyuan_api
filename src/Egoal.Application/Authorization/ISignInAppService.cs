using System.Collections.Generic;
using System.Security.Claims;

namespace Egoal.Authorization
{
    public interface ISignInAppService
    {
        string CreateToken(List<Claim> claims);
    }
}
