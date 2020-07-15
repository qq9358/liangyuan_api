namespace Egoal
{
    public class UserIdentifier : IUserIdentifier
    {
        public int UserId { get; private set; }

        public int? RoleId { get; private set; }

        public UserIdentifier()
        {

        }

        public UserIdentifier(int userId, int? roleId)
        {
            UserId = userId;
            RoleId = roleId;
        }
    }
}
