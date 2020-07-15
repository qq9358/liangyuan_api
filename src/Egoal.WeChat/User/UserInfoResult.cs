namespace Egoal.WeChat.User
{
    public class UserInfoResult : ResultBase
    {
        /// <summary>
        /// 用户是否订阅该公众号标识，值为0时，代表此用户没有关注该公众号，拉取不到其余信息。
        /// </summary>
        public string subscribe { get; set; }

        /// <summary>
        /// 用户的标识，对当前公众号唯一
        /// </summary>
        public string openid { get; set; }

        /// <summary>
        /// 用户的昵称
        /// </summary>
        public string nickname { get; set; }

        /// <summary>
        /// 用户的性别，值为1时是男性，值为2时是女性，值为0时是未知
        /// </summary>
        public string sex { get; set; }

        /// <summary>
        /// 用户所在城市
        /// </summary>
        public string city { get; set; }

        /// <summary>
        /// 用户所在国家
        /// </summary>
        public string country { get; set; }

        /// <summary>
        /// 用户所在省份
        /// </summary>
        public string province { get; set; }

        /// <summary>
        /// 用户的语言，简体中文为zh_CN
        /// </summary>
        public string language { get; set; }

        /// <summary>
        /// 用户头像
        /// </summary>
        public string headimgurl { get; set; }

        /// <summary>
        /// 用户关注时间，为时间戳。如果用户曾多次关注，则取最后关注时间
        /// </summary>
        public string subscribe_time { get; set; }

        /// <summary>
        /// 只有在用户将公众号绑定到微信开放平台帐号后，才会出现该字段
        /// </summary>
        public string unionid { get; set; }

        /// <summary>
        /// 公众号运营者对粉丝的备注，公众号运营者可在微信公众平台用户管理界面对粉丝添加备注
        /// </summary>
        public string remark { get; set; }

        /// <summary>
        /// 用户所在的分组ID（兼容旧的用户分组接口）
        /// </summary>
        public string groupid { get; set; }

        /// <summary>
        /// 用户被打上的标签ID列表
        /// </summary>
        public string[] tagid_list { get; set; }

        /// <summary>
        /// 返回用户关注的渠道来源
        /// </summary>
        public string subscribe_scene { get; set; }

        /// <summary>
        /// 二维码扫码场景（开发者自定义）
        /// </summary>
        public string qr_scene { get; set; }

        /// <summary>
        /// 二维码扫码场景描述（开发者自定义）
        /// </summary>
        public string qr_scene_str { get; set; }

        public string Address
        {
            get { return $"{country}{province}{city}"; }
        }
    }
}
