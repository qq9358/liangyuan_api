using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace Egoal.Authorization
{
    public class TokenOptions
    {
        public TokenOptions()
        {
            IssuerSigningKey = "D9B8502B-CF4F-437D-A497-E361EF6212EF";
        }

        public string ValidAudience { get; set; } = "api";

        public string ValidIssuer { get; set; } = "http://www.egoal.com.cn";

        public string IssuerSigningKey
        {
            get { return _IssuerSigningKey; }
            set
            {
                _IssuerSigningKey = value;

                SecurityKey = _IssuerSigningKey == null ? null : new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_IssuerSigningKey));
            }
        }
        private string _IssuerSigningKey;

        public SecurityKey SecurityKey { get; private set; }

        /// <summary>
        /// 登录令牌有效时长
        /// </summary>
        public TimeSpan TokenValidTime { get; set; } = TimeSpan.FromMinutes(30);
    }
}
