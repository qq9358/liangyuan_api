using Egoal.Annotations;
using Egoal.Application.Services;
using Egoal.Application.Services.Dto;
using Egoal.Common.Dto;
using Egoal.Cryptography;
using Egoal.Domain.Repositories;
using Egoal.Extensions;
using Egoal.Messages;
using Egoal.Threading.RateLimit;
using Egoal.UI;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Egoal.Common
{
    public class CommonAppService : ApplicationService, ICommonAppService
    {
        private readonly IServiceProvider _serviceProvider;

        public CommonAppService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public List<ComboboxItemDto> GetEducationComboboxItems()
        {
            var items = new List<ComboboxItemDto>
            {
                new ComboboxItemDto{DisplayText="初中",Value="初中"},
                new ComboboxItemDto{DisplayText="高中",Value="高中"},
                new ComboboxItemDto{DisplayText="大专",Value="大专"},
                new ComboboxItemDto{DisplayText="本科",Value="本科"},
                new ComboboxItemDto{DisplayText="硕士",Value="硕士"},
                new ComboboxItemDto{DisplayText="博士",Value="博士"},
                new ComboboxItemDto{DisplayText="其他",Value="其他"}
            };

            return items;
        }

        public List<ComboboxItemDto> GetNationComboboxItems()
        {
            var items = new List<ComboboxItemDto>
            {
                new ComboboxItemDto{DisplayText="汉族",Value="汉族"},
                new ComboboxItemDto{DisplayText="壮族",Value="壮族"},
                new ComboboxItemDto{DisplayText="满族",Value="满族"},
                new ComboboxItemDto{DisplayText="回族",Value="回族"},
                new ComboboxItemDto{DisplayText="苗族",Value="苗族"},
                new ComboboxItemDto{DisplayText="维吾尔族",Value="维吾尔族"},
                new ComboboxItemDto{DisplayText="土家族",Value="土家族"},
                new ComboboxItemDto{DisplayText="彝族",Value="彝族"},
                new ComboboxItemDto{DisplayText="蒙古族",Value="蒙古族"},
                new ComboboxItemDto{DisplayText="藏族",Value="藏族"},
                new ComboboxItemDto{DisplayText="布依族",Value="布依族"},
                new ComboboxItemDto{DisplayText="侗族",Value="侗族"},
                new ComboboxItemDto{DisplayText="瑶族",Value="瑶族"},
                new ComboboxItemDto{DisplayText="朝鲜族",Value="朝鲜族"},
                new ComboboxItemDto{DisplayText="白族",Value="白族"},
                new ComboboxItemDto{DisplayText="哈尼族",Value="哈尼族"},
                new ComboboxItemDto{DisplayText="哈萨克族",Value="哈萨克族"},
                new ComboboxItemDto{DisplayText="黎族",Value="黎族"},
                new ComboboxItemDto{DisplayText="傣族",Value="傣族"},
                new ComboboxItemDto{DisplayText="畲族",Value="畲族"},
                new ComboboxItemDto{DisplayText="傈僳族",Value="傈僳族"},
                new ComboboxItemDto{DisplayText="仡佬族",Value="仡佬族"},
                new ComboboxItemDto{DisplayText="东乡族",Value="东乡族"},
                new ComboboxItemDto{DisplayText="高山族",Value="高山族"},
                new ComboboxItemDto{DisplayText="拉祜族",Value="拉祜族"},
                new ComboboxItemDto{DisplayText="水族",Value="水族"},
                new ComboboxItemDto{DisplayText="佤族",Value="佤族"},
                new ComboboxItemDto{DisplayText="纳西族",Value="纳西族"},
                new ComboboxItemDto{DisplayText="羌族",Value="羌族"},
                new ComboboxItemDto{DisplayText="土族",Value="土族"},
                new ComboboxItemDto{DisplayText="仫佬族",Value="仫佬族"},
                new ComboboxItemDto{DisplayText="锡伯族",Value="锡伯族"},
                new ComboboxItemDto{DisplayText="柯尔克孜族",Value="柯尔克孜族"},
                new ComboboxItemDto{DisplayText="达斡尔族",Value="达斡尔族"},
                new ComboboxItemDto{DisplayText="景颇族",Value="景颇族"},
                new ComboboxItemDto{DisplayText="毛南族",Value="毛南族"},
                new ComboboxItemDto{DisplayText="撒拉族",Value="撒拉族"},
                new ComboboxItemDto{DisplayText="布朗族",Value="布朗族"},
                new ComboboxItemDto{DisplayText="塔吉克族",Value="塔吉克族"},
                new ComboboxItemDto{DisplayText="阿昌族",Value="阿昌族"},
                new ComboboxItemDto{DisplayText="普米族",Value="普米族"},
                new ComboboxItemDto{DisplayText="鄂温克族",Value="鄂温克族"},
                new ComboboxItemDto{DisplayText="怒族",Value="怒族"},
                new ComboboxItemDto{DisplayText="京族",Value="京族"},
                new ComboboxItemDto{DisplayText="基诺族",Value="基诺族"},
                new ComboboxItemDto{DisplayText="德昂族",Value="德昂族"},
                new ComboboxItemDto{DisplayText="保安族",Value="保安族"},
                new ComboboxItemDto{DisplayText="俄罗斯族",Value="俄罗斯族"},
                new ComboboxItemDto{DisplayText="裕固族",Value="裕固族"},
                new ComboboxItemDto{DisplayText="乌孜别克族",Value="乌孜别克族"},
                new ComboboxItemDto{DisplayText="门巴族",Value="门巴族"},
                new ComboboxItemDto{DisplayText="鄂伦春族",Value="鄂伦春族"},
                new ComboboxItemDto{DisplayText="独龙族",Value="独龙族"},
                new ComboboxItemDto{DisplayText="塔塔尔族",Value="塔塔尔族"},
                new ComboboxItemDto{DisplayText="赫哲族",Value="赫哲族"},
                new ComboboxItemDto{DisplayText="珞巴族",Value="珞巴族"},
                new ComboboxItemDto{DisplayText="其他",Value="其他"}
            };

            return items;
        }

        public async Task<List<ComboboxItemDto<int>>> GetAgeRangeComboboxItemsAsync()
        {
            var repository = _serviceProvider.GetRequiredService<IRepository<AgeRange>>();

            var ageRanges = await repository.ToListAsync(repository.GetAll().OrderBy(a => a.StartAge));

            var items = ageRanges.Select(a => new ComboboxItemDto<int> { DisplayText = a.Name, Value = a.Id }).ToList();

            return items;
        }

        public async Task<TreeNodeDto> GetTouristOriginTreeAsync()
        {
            var keYuanRepository = _serviceProvider.GetRequiredService<IRepository<KeYuanType>>();
            var keYuans = await keYuanRepository.ToListAsync(keYuanRepository.GetAll().OrderBy(k => k.SortCode));

            var areaRepository = _serviceProvider.GetRequiredService<IRepository<Area>>();
            var areas = await areaRepository.ToListAsync(areaRepository.GetAll().OrderBy(a => a.SortCode));

            var root = new TreeNodeDto();
            foreach (var keYuan in keYuans)
            {
                var keYuanNode = new TreeNodeDto();
                keYuanNode.DisplayText = keYuan.Name;
                keYuanNode.Value = keYuan.Id.ToString();
                root.Nodes.Add(keYuanNode);

                BuildAreaTree(keYuan.Id, 0, areas, keYuanNode);
            }

            return root;
        }

        private void BuildAreaTree(int keYuanId, int pid, List<Area> areas, TreeNodeDto node)
        {
            var childrenAreas = areas.Where(a => a.KeYuanTypeId == keYuanId && a.Pid == pid).ToList();
            if (childrenAreas.IsNullOrEmpty())
            {
                return;
            }

            foreach (var area in childrenAreas)
            {
                var areaNode = new TreeNodeDto { DisplayText = area.Name, Value = area.Id.ToString() };
                node.Nodes.Add(areaNode);

                BuildAreaTree(keYuanId, area.Id, areas, areaNode);
            }
        }

        public async Task<List<ComboboxItemDto<int>>> GetCertTypeComboboxItemsAsync()
        {
            var repository = _serviceProvider.GetRequiredService<IRepository<CertType>>();

            var query = repository.GetAll()
                .OrderBy(item => item.SortCode)
                .Select(item => new ComboboxItemDto<int>
                {
                    DisplayText = CultureInfo.CurrentCulture.Name == "zh-CN" ? item.Name : item.EnglishName,
                    Value = item.Id
                });

            return await repository.ToListAsync(query);
        }

        public async Task<List<DateChangCiSaleDto>> GetChangCiForSaleAsync(string date)
        {
            var repository = _serviceProvider.GetRequiredService<IChangCiRepository>();

            var now = DateTime.Now;
            var today = now.ToDateString();

            var validChangCis = new List<DateChangCiSaleDto>();

            var changCis = await repository.GetChangCiForSaleAsync(date);
            foreach (var changCi in changCis)
            {
                if (changCi.SurplusNum <= 0) continue;

                if (date == today)
                {
                    var endTime = $"{today} {changCi.ETime}:00".To<DateTime>();
                    if (endTime < now) continue;
                }

                validChangCis.Add(changCi);
            }

            return validChangCis;
        }

        public async Task SendVerificationCodeAsync(string address)
        {
            var key = GetVerificationCodeKey(address);

            double interval = 60;
            var _rateLimiterManager = _serviceProvider.GetRequiredService<IRateLimiterManager>();
            if (!_rateLimiterManager.TryAcquire($"Rate:{key}", 1 / interval, timeout: interval * 10))
            {
                throw new UserFriendlyException("1分钟内只能发送1条验证码");
            }

            var code = RandomHelper.CreateRandomNumber(6);

            MobileNumberAttribute mobile = new MobileNumberAttribute();
            if (mobile.IsValid(address))
            {
                var _shortMessageAppService = _serviceProvider.GetRequiredService<IShortMessageAppService>();
                await _shortMessageAppService.SendVerificationCodeAsync(address, code);
            }
            else
            {
                var _emailAppService = _serviceProvider.GetRequiredService<IEmailAppService>();
                await _emailAppService.SendVerificationCodeAsync(address, code);
            }

            var _memoryCache = _serviceProvider.GetRequiredService<IMemoryCache>();
            _memoryCache.Set(key, code, TimeSpan.FromMinutes(5));
        }

        public void ValidateVerificationCode(string address, string code)
        {
            var _memoryCache = _serviceProvider.GetRequiredService<IMemoryCache>();
            var cacheCode = _memoryCache.Get<string>(GetVerificationCodeKey(address));
            if (cacheCode != code)
            {
                throw new UserFriendlyException("验证码不正确");
            }
        }

        private string GetVerificationCodeKey(string address)
        {
            MobileNumberAttribute mobile = new MobileNumberAttribute();
            EmailAttribute email = new EmailAttribute();

            if (mobile.IsValid(address))
            {
                return $"VerificationCode:M:{address}";
            }
            else if (email.IsValid(address))
            {
                return $"VerificationCode:E:{address}";
            }
            else
            {
                throw new UserFriendlyException("手机号或邮箱地址不正确");
            }
        }

        /// <summary>
        /// 购买成功通知
        /// </summary>
        /// <param name="address"></param>
        /// <param name="monthDay"></param>
        /// <param name="listNo"></param>
        /// <param name="entryTime"></param>
        /// <param name="needActive"></param>
        /// <returns></returns>
        public async Task SendPersonalBookSuccessAsync(string address, string monthDay, string listNo, string entryTime, bool needActive, string qrCodeString)
        {
            MobileNumberAttribute mobileNumberAttribute = new MobileNumberAttribute();
            if (mobileNumberAttribute.IsValid(address))
            {
                var shortMessageAppService = _serviceProvider.GetRequiredService<IShortMessageAppService>();
                await shortMessageAppService.SendPersonalBookSuccessAsync(address, monthDay, listNo, entryTime, needActive, qrCodeString);
            }
            else
            {
                var emailAppService = _serviceProvider.GetRequiredService<IEmailAppService>();
                await emailAppService.SendPersonalBookSuccessAsync(address, monthDay, listNo, entryTime, needActive, qrCodeString);
            }
        }

        /// <summary>
        /// 预约成功通知
        /// </summary>
        /// <param name="address"></param>
        /// <param name="monthDay"></param>
        /// <param name="listNo"></param>
        /// <param name="entryTime"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        public async Task SendGroupBookSuccessAsync(string address, string monthDay, string listNo, string entryTime, int number)
        {
            IShortMessageAppService shortMessageAppService = _serviceProvider.GetRequiredService<IShortMessageAppService>();
            await shortMessageAppService.SendGroupBookSuccessAsync(address, monthDay, listNo, entryTime, number);
        }
    }
}
