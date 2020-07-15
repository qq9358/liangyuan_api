using Egoal.Settings.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Egoal.Settings
{
    public interface ISettingAppService
    {
        Task ConfigOptionsAsync();
        Task<OrderNoticeDto> GetOrderNoticeAsync();
        Task<Dictionary<string, string>> GetSettingsFromWeChatAsync();
        WxJsApiSignature GetWxJsApiSignature(string url);
        string GetWxLoginUrl(string url);

        /// <summary>
        /// 获取用户重试次数和锁定时间
        /// </summary>
        /// <returns></returns>
        Task<LoginLockParaOutput> GetLoginLockPara();

        /// <summary>
        /// 获取系统参数
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<string> GetSysPara(string name);
    }
}
