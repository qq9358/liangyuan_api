using Egoal.Application.Services;
using Egoal.Authorization;
using Egoal.Cryptography;
using Egoal.Domain.Repositories;
using Egoal.Extensions;
using Egoal.Logging;
using Egoal.Scenics;
using Egoal.Settings.Dto;
using Egoal.WeChat;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Egoal.Settings
{
    public class SettingAppService : ApplicationService, ISettingAppService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger _logger;
        private readonly IRepository<Setting, string> _settingRepository;

        public SettingAppService(
            IServiceProvider serviceProvider,
            ILogger<SettingAppService> logger,
            IRepository<Setting, string> settingRepository)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _settingRepository = settingRepository;
        }

        public async Task ConfigOptionsAsync()
        {
            ConfigJwtOptions();
            await ConfigWeChatOptionsAsync();
            await ConfigScenicOptionsAsync();

            await EnsureOrderSettingsExistsAsync();
            await EnsureShortMessageSettingsExistsAsync();
            await EnsureEmailSettingsExistsAsync();
            await EnsureIcbcSettingsExistsAsync();
        }

        private void ConfigJwtOptions()
        {
            var tokenService = _serviceProvider.GetRequiredService<ITokenService>();
            tokenService.ConfigJwtOptions();
        }

        private async Task ConfigWeChatOptionsAsync()
        {
            var weChatOptions = _serviceProvider.GetRequiredService<IOptions<WeChatOptions>>().Value;
            var parkOptions = _serviceProvider.GetRequiredService<IOptions<ParkOptions>>().Value;

            weChatOptions.NotifyUrl = parkOptions.WebApiUrl?.UrlCombine("Payment");

            await EnsureSettingExistsAsync("WxSubscribeUrl", "微信公众号地址");
            await EnsureSettingExistsAsync("WxMenuUrl", "微信购票菜单地址");
            await EnsureSettingExistsAsync("WxCertStr", "微信证书字符串", "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");

            try
            {
                if (weChatOptions.WxCertStr.IsNullOrEmpty())
                {
                    weChatOptions.SslCert = new X509Certificate2(weChatOptions.WxCertPath, weChatOptions.WxCertPassword);
                }
                else
                {
                    weChatOptions.SslCert = new X509Certificate2(Convert.FromBase64String(weChatOptions.WxCertStr), weChatOptions.WxCertPassword);
                }
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);

                weChatOptions.SslCert = LoadCertFromStore(new[] { "O=Tenpay.com", "O=Tencent" });
            }
        }

        private async Task EnsureOrderSettingsExistsAsync()
        {
            await EnsureSettingExistsAsync("GroupMaxBuyQuantityPerOrder", "团队订单每单最大购票数量", "0");
        }

        private async Task EnsureShortMessageSettingsExistsAsync()
        {
            await EnsureSettingExistsAsync("ShortMessageUserName", "短信用户名");
            await EnsureSettingExistsAsync("ShortMessagePassword", "短信密码");
        }

        private async Task EnsureEmailSettingsExistsAsync()
        {
            await EnsureSettingExistsAsync("SmtpHost", "SMTP服务器");
            await EnsureSettingExistsAsync("SmtpPort", "SMTP端口", "465");
            await EnsureSettingExistsAsync("SmtpUserName", "SMTP用户名");
            await EnsureSettingExistsAsync("SmtpPassword", "SMTP密码");
            await EnsureSettingExistsAsync("SmtpUseSSL", "SMTP使用SSL", "True");
            await EnsureSettingExistsAsync("EmailDefaultFromAddress", "默认发件邮箱地址");
            await EnsureSettingExistsAsync("EmailDefaultFromDisplayName", "默认发件人");
        }

        private async Task EnsureIcbcSettingsExistsAsync()
        {
            await EnsureSettingExistsAsync("IcbcMerchantPrivateKey", "工商银行商户私玥");
            await EnsureSettingExistsAsync("IcbcPublicKey", "工商银行公钥");
            await EnsureSettingExistsAsync("IcbcAESKey", "工商银行AESKey");
            await EnsureSettingExistsAsync("IcbcAppId", "工商银行AppId");
            await EnsureSettingExistsAsync("IcbcMerchantId", "工商银行商户编号");
            await EnsureSettingExistsAsync("IcbcEMerchantId", "工商银行e生活商户编号");
            await EnsureSettingExistsAsync("IcbcH5MallCode", "工商银行H5商城代码");
            await EnsureSettingExistsAsync("IcbcH5AppId", "工商银行H5AppId");
            await EnsureSettingExistsAsync("IcbcH5MerchantId", "工商银行H5商户编号");
            await EnsureSettingExistsAsync("IcbcH5EMerchantId", "工商银行H5e生活商户编号");
            await EnsureSettingExistsAsync("IcbcH5ClearingAccount", "工商银行H5清算账号");
            await EnsureSettingExistsAsync("IcbcCAPrivateKeyPath", "工商银行CA私玥路径");
            await EnsureSettingExistsAsync("IcbcCAPrivateKeyPassword", "工商银行CA私玥密码");
            await EnsureSettingExistsAsync("IcbcCAPublicKey", "工商银行CA公钥");
            await EnsureSettingExistsAsync("IcbcPayUrl", "工商银行支付地址", "https://gw.open.icbc.com.cn");
        }

        private async Task EnsureSettingExistsAsync(string name, string caption, string defaultValue = "")
        {
            if (!await _settingRepository.AnyAsync(s => s.Id == name))
            {
                var setting = new Setting
                {
                    Id = name,
                    Caption = caption
                };

                if (!defaultValue.IsNullOrEmpty())
                {
                    setting.Value = defaultValue;
                }

                await _settingRepository.InsertAsync(setting);
            }
        }

        private X509Certificate2 LoadCertFromStore(string[] issuers)
        {
            try
            {
                X509Store store = new X509Store("MY", StoreLocation.CurrentUser);
                store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
                foreach (var cert in store.Certificates)
                {
                    if (issuers.Any(issuer => cert.Issuer.Contains(issuer)))
                    {
                        return cert;
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);

                return null;
            }
        }

        private async Task ConfigScenicOptionsAsync()
        {
            var options = _serviceProvider.GetRequiredService<IOptions<ScenicOptions>>().Value;

            var parkRepository = _serviceProvider.GetRequiredService<IRepository<Park>>();
            options.IsMultiPark = await parkRepository.CountAsync() > 1;

            var scenicRepository = _serviceProvider.GetRequiredService<IRepository<Scenic>>();
            var scenic = await scenicRepository.FirstOrDefaultAsync(s => s.Id > 0);
            if (scenic != null)
            {
                options.ScenicName = scenic.ScenicName;
                options.ParkOpenTime = scenic.OpenTime;
                options.ParkCloseTime = scenic.CloseTime;
            }
            else
            {
                var constantRepository = _serviceProvider.GetRequiredService<IRepository<Constant, string>>();
                options.ScenicName = (await constantRepository.FirstOrDefaultAsync("CompanyName"))?.Value;
            }
        }

        public async Task<OrderNoticeDto> GetOrderNoticeAsync()
        {
            var constantRepository = _serviceProvider.GetRequiredService<IRepository<Constant, string>>();

            var output = new OrderNoticeDto();
            output.ScenicName = (await constantRepository.FirstOrDefaultAsync("CompanyName"))?.Value;
            output.TimeNotice = (await constantRepository.FirstOrDefaultAsync("开馆时间"))?.Value;
            output.OrderNotice = (await constantRepository.FirstOrDefaultAsync("预约须知"))?.Value;
            output.ContactNotice = (await constantRepository.FirstOrDefaultAsync("联系信息"))?.Value;

            return output;
        }

        public async Task<Dictionary<string, string>> GetSettingsFromWeChatAsync()
        {
            var settings = new Dictionary<string, string>();

            var distributionUrl = await _settingRepository.GetAll()
                .Where(s => s.Id == "App.General.DistributionUrl")
                .Select(s => s.Value)
                .FirstOrDefaultAsync();
            settings.Add("DistributionUrl", distributionUrl);

            var wxSubscribeUrl = await _settingRepository.GetAll()
                .Where(s => s.Id == "WxSubscribeUrl")
                .Select(s => s.Value)
                .FirstOrDefaultAsync();
            settings.Add("WxSubscribeUrl", wxSubscribeUrl);

            var wxMenuUrl = await _settingRepository.GetAll()
                .Where(s => s.Id == "WxMenuUrl")
                .Select(s => s.Value)
                .FirstOrDefaultAsync();
            settings.Add("WxMenuUrl", wxMenuUrl);

            return settings;
        }

        public WxJsApiSignature GetWxJsApiSignature(string url)
        {
            var options = _serviceProvider.GetRequiredService<IOptions<WeChatOptions>>().Value;

            var signature = new WxJsApiSignature();
            signature.AppId = options.WxAppID;
            signature.NonceStr = Guid.NewGuid().ToString().Replace("-", string.Empty);
            signature.Timestamp = DateTime.Now.ToUnixTimestamp().To<long>();

            var args = new SortedDictionary<string, string>();
            args.Add("noncestr", signature.NonceStr);
            args.Add("timestamp", signature.Timestamp.ToString());
            args.Add("jsapi_ticket", options.JsApiTicket);
            args.Add("url", url);

            var stringArgs = new StringBuilder();
            foreach (var pair in args)
            {
                stringArgs.Append(pair.Key).Append("=").Append(pair.Value).Append("&");
            }

            signature.Signature = SHAHelper.SHA1Encrypt(stringArgs.ToString().TrimEnd('&')).ToLower();

            return signature;
        }

        public string GetWxLoginUrl(string url)
        {
            var options = _serviceProvider.GetRequiredService<IOptions<WeChatOptions>>().Value;

            return $"https://open.weixin.qq.com/connect/oauth2/authorize?appid={options.WxAppID}&redirect_uri={url.UrlEncode()}&response_type=code&scope=snsapi_userinfo&state=123#wechat_redirect";
        }

        /// <summary>
        /// 获取系统参数
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<string> GetSysPara(string name)
        {
            Setting setting = await _settingRepository.GetAll()
                .Where(s => s.Id == name)
                .FirstOrDefaultAsync();
            if (setting != null)
            {
                return setting.Value;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获取用户重试次数和锁定时间
        /// </summary>
        /// <returns></returns>
        public async Task<LoginLockParaOutput> GetLoginLockPara()
        {
            LoginLockParaOutput loginLockParaOutput = new LoginLockParaOutput();
            int lockStaffMaxLoginErrorTimes = 0;
            int.TryParse(await GetSysPara("LockStaffMaxLoginErrorTimes"), out lockStaffMaxLoginErrorTimes);
            loginLockParaOutput.LockStaffMaxLoginErrorTimes = lockStaffMaxLoginErrorTimes;
            int lockStaffMinutes = 0;
            int.TryParse(await GetSysPara("LockStaffMinutes"), out lockStaffMinutes);
            loginLockParaOutput.LockStaffMinutes = lockStaffMinutes;
            return loginLockParaOutput;
        }
    }
}
