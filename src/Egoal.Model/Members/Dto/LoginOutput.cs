using System.Collections.Generic;

namespace Egoal.Members.Dto
{
    public class LoginOutput
    {
        public LoginOutput()
        {
            Permissions = new List<string>();
        }

        public MemberDto Member { get; set; }
        public List<string> Permissions { get; set; }
        public string Token { get; set; }
    }
}
