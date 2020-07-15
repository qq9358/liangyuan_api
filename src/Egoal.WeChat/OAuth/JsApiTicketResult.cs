namespace Egoal.WeChat.OAuth
{
    public class JsApiTicketResult : ResultBase
    {
        public string ticket { get; set; }
        public int expires_in { get; set; }
    }
}
