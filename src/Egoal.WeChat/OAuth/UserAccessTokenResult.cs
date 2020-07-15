namespace Egoal.WeChat.OAuth
{
    public class UserAccessTokenResult : AccessTokenResult
    {
        public string refresh_token { get; set; }
        public string openid { get; set; }
        public string scope { get; set; }
    }
}
