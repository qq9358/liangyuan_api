namespace Egoal.WeChat.OAuth
{
    public class AccessTokenResult : ResultBase
    {
        public string access_token { get; set; }
        public int expires_in { get; set; }
    }
}
