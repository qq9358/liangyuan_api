namespace Egoal.WeChat.OAuth
{
    /// <summary>
    /// 小程序获取OpenId
    /// </summary>
    public class Code2SessionResult : ResultBase
    {
        /// <summary>
        /// 用户维一标识
        /// </summary>
        public string openid { get; set; }

        /// <summary>
        /// 会话密钥
        /// </summary>
        public string session_key { get; set; }

        /// <summary>
        /// 开放平台维一标识符
        /// </summary>
        public string unionid { get; set; }
    }
}
