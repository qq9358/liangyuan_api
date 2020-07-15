namespace Egoal.WeChat.OAuth
{
    public class UserInfoResult : ResultBase
    {
        public string openid { get; set; }
        public string nickname { get; set; }
        /// <summary>
        /// 用户的性别，值为1时是男性，值为2时是女性，值为0时是未知
        /// </summary>
        public string sex { get; set; }
        public string province { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public string headimgurl { get; set; }
        /// <summary>
        /// 用户特权信息，json 数组，如微信沃卡用户为（chinaunicom）
        /// </summary>
        public string[] privilege { get; set; }
        public string unionid { get; set; }

        public string Address
        {
            get { return $"{country}{province}{city}"; }
        }
    }
}
