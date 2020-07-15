using System.Collections.Generic;

namespace Egoal.Staffs.Dto
{
    public class LoginOutput
    {
        public StaffDto Staff { get; set; }
        public string Token { get; set; }
        public List<string> Permissions { get; set; }
    }
}
