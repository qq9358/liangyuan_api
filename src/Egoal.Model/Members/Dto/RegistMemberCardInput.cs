using Egoal.Annotations;

namespace Egoal.Members.Dto
{
    public class RegistMemberCardInput
    {
        [MustFillIn]
        public string Name { get; set; }

        [MustFillIn]
        [MobileNumber]
        public string Mobile { get; set; }

        [MustFillIn]
        [IdentityCardNo]
        public string IdCard { get; set; }

        public string Sex { get; set; }

        public string Nation { get; set; }
        public string Education { get; set; }
        public string Address { get; set; }
    }
}
