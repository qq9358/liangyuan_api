using Egoal.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Egoal.Authorization
{
    public class JwtTokenService : ITokenService
    {
        private readonly TokenOptions _tokenOptions;
        private readonly JwtBearerOptions _jwtBearerOptions;

        public JwtTokenService(
            IOptions<TokenOptions> options,
            IOptionsMonitor<JwtBearerOptions> jwtOptions)
        {
            _tokenOptions = options.Value;
            _jwtBearerOptions = jwtOptions.Get(JwtBearerDefaults.AuthenticationScheme);
        }

        public string CreateToken(ClaimsIdentity claimsIdentity, TimeSpan? validTime = null)
        {
            var securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claimsIdentity,
                Audience = _tokenOptions.ValidAudience,
                Issuer = _tokenOptions.ValidIssuer,
                IssuedAt = DateTime.Now,
                Expires = DateTime.Now.Add(validTime.HasValue ? validTime.Value : _tokenOptions.TokenValidTime),
                SigningCredentials = new SigningCredentials(_tokenOptions.SecurityKey, SecurityAlgorithms.HmacSha256Signature)
            };
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var token = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);
            var tokenString = jwtSecurityTokenHandler.WriteToken(token);

            var tokenResult = new TokenResult
            {
                access_token = tokenString,
                expires_in = securityTokenDescriptor.Expires.Value.ToUnixTimestamp().To<int>(),
                token_type = "Bearer"
            };

            return tokenResult.ToJson();
        }

        public void ConfigJwtOptions()
        {
            if (!_tokenOptions.ValidIssuer.IsNullOrEmpty())
            {
                _jwtBearerOptions.TokenValidationParameters.ValidIssuer = _tokenOptions.ValidIssuer;
            }

            if (!_tokenOptions.IssuerSigningKey.IsNullOrEmpty())
            {
                _jwtBearerOptions.TokenValidationParameters.IssuerSigningKey = _tokenOptions.SecurityKey;
            }
        }
    }
}
