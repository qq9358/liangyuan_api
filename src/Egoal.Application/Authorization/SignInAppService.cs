using Egoal.Application.Services;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace Egoal.Authorization
{
    public class SignInAppService : ApplicationService, ISignInAppService
    {
        private readonly ITokenService _tokenService;

        public SignInAppService(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        public string CreateToken(List<Claim> claims)
        {
            var userIdentity = new ClaimsIdentity(claims);

            var token = _tokenService.CreateToken(userIdentity, TimeSpan.FromHours(24));
            return token;
        }
    }
}
