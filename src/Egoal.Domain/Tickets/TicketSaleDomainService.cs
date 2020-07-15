using Egoal.Caches;
using Egoal.Common;
using Egoal.Cryptography;
using Egoal.Domain.Repositories;
using Egoal.Domain.Services;
using Egoal.Events.Bus;
using Egoal.Extensions;
using Egoal.Localization;
using Egoal.Scenics.Dto;
using Egoal.Tickets.Dto;
using Egoal.TicketTypes;
using Egoal.Trades;
using Egoal.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Egoal.Tickets
{
    public class TicketSaleDomainService : DomainService, ITicketSaleDomainService
    {
        private readonly IEventBus _eventBus;
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly TicketSaleOptions _ticketSaleOptions;
        private readonly ExpirationDateCalculator _expirationDateCalculator;
        private readonly ITicketSaleRepository _ticketSaleRepository;
        private readonly ITicketTypeRepository _ticketTypeRepository;
        private readonly ITicketSaleBuyerRepository _ticketSaleBuyerRepository;
        private readonly IRepository<TicketGroundCache, long> _ticketGroundCacheRepository;
        private readonly IRepository<TicketGround, long> _ticketGroundRepository;
        private readonly ITicketCheckRepository _ticketCheckRepository;
        private readonly IRepository<TicketConsume, long> _ticketConsumeRepository;
        private readonly ITradeRepository _tradeRepository;
        private readonly INameCacheService _nameCacheService;
        private readonly IRepository<ChangCi> _changCiRepository;
        private readonly IRepository<InvoiceInfo> _invoiceRepository;

        public TicketSaleDomainService(
            IEventBus eventBus,
            IStringLocalizer<SharedResource> localizer,
            IOptions<TicketSaleOptions> ticketSaleOptions,
            ExpirationDateCalculator expirationDateCalculator,
            ITicketSaleRepository ticketSaleRepository,
            ITicketTypeRepository ticketTypeRepository,
            ITicketSaleBuyerRepository ticketSaleBuyerRepository,
            IRepository<TicketGroundCache, long> ticketGroundCacheRepository,
            IRepository<TicketGround, long> ticketGroundRepository,
            ITicketCheckRepository ticketCheckRepository,
            IRepository<TicketConsume, long> ticketConsumeRepository,
            ITradeRepository tradeRepository,
            INameCacheService nameCacheService,
            IRepository<ChangCi> changCiRepository,
            IRepository<InvoiceInfo> invoiceRepository)
        {
            _eventBus = eventBus;
            _localizer = localizer;
            _ticketSaleOptions = ticketSaleOptions.Value;
            _expirationDateCalculator = expirationDateCalculator;
            _ticketSaleRepository = ticketSaleRepository;
            _ticketTypeRepository = ticketTypeRepository;
            _ticketSaleBuyerRepository = ticketSaleBuyerRepository;
            _ticketGroundCacheRepository = ticketGroundCacheRepository;
            _ticketGroundRepository = ticketGroundRepository;
            _ticketCheckRepository = ticketCheckRepository;
            _ticketConsumeRepository = ticketConsumeRepository;
            _tradeRepository = tradeRepository;
            _nameCacheService = nameCacheService;
            _changCiRepository = changCiRepository;
            _invoiceRepository = invoiceRepository;
        }

        public async Task ValidateCertNoAsync(IEnumerable<string> certNos, DateTime travelDate)
        {
            var repetitiveCertNos = certNos.GroupBy(c => c).Where(g => g.Count() > 1);
            if (repetitiveCertNos.Count() > 0)
            {
                string certNo = repetitiveCertNos.First().Key;
                throw new UserFriendlyException(_localizer.GetString("CertRepeat", certNo.Length > 26 ? DES3Helper.Decrypt(certNo) : certNo));
            }

            if (_ticketSaleOptions.CertTicketSaleDaysRange > 0 && _ticketSaleOptions.CertTicketSaleNum > 0)
            {
                int range = _ticketSaleOptions.CertTicketSaleDaysRange - 1;
                DateTime startTime = travelDate.AddDays(-range);
                DateTime endTime = travelDate.AddDays(range);
                foreach (var certNo in certNos)
                {
                    int buyQuantity = await _ticketSaleBuyerRepository.GetCertNoBuyQuantity(certNo, startTime, endTime);
                    if (buyQuantity + 1 > _ticketSaleOptions.CertTicketSaleNum)
                    {
                        throw new UserFriendlyException(_localizer.GetString("CertBuyLimit", certNo.Length > 24 ? DES3Helper.Decrypt(certNo) : certNo));
                    }
                }
            }
        }

        public async Task<bool> ShouldInValidAsync(TicketSale ticketSale, int surplusQuantity, int refundQuantity)
        {
            if (surplusQuantity > 0)
            {
                return false;
            }

            if (ticketSale.PersonNum > 1)
            {
                var maxGroundSurplusNum = await GetMaxGroundCacheSurplusNumAsync(ticketSale);
                if (maxGroundSurplusNum.HasValue)
                {
                    if (maxGroundSurplusNum.Value > ticketSale.GetCheckNum() * refundQuantity)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public async Task<int> GetConsumeNumAsync(TicketSale ticketSale)
        {
            var consumeNums = await _ticketConsumeRepository.GetAll()
                .Where(c => c.TicketId == ticketSale.Id)
                .Select(c => c.ConsumeNum)
                .ToListAsync();

            return consumeNums.Sum();
        }

        public async Task<int> GetRealNumAsync(TicketSale ticketSale)
        {
            return ticketSale.PersonNum.Value - await GetRefundNumAsync(ticketSale);
        }

        public async Task<int> GetRefundNumAsync(TicketSale ticketSale)
        {
            var refundNums = await _ticketSaleRepository.GetAll()
                .Where(t => t.ReturnTicketId == ticketSale.Id)
                .Select(t => t.PersonNum)
                .ToListAsync();

            var refundNum = refundNums.Sum();

            return refundNum.HasValue ? Math.Abs(refundNum.Value) : 0;
        }

        public async Task<int> GetSurplusNumAsync(TicketSale ticketSale)
        {
            if (ticketSale.TicketStatusId == TicketStatus.作废)
            {
                return 0;
            }

            int checkNum = ticketSale.GetCheckNum();

            var isActive = await IsActiveAsync(ticketSale);
            if (!isActive)
            {
                return ticketSale.SurplusNum.Value / checkNum;
            }

            var minGroundCacheSurplusNum = await _ticketGroundCacheRepository.GetAll()
                .Where(t => t.TicketId == ticketSale.Id)
                .Select(t => t.SurplusNum)
                .MinAsync();
            if (minGroundCacheSurplusNum.HasValue)
            {
                return minGroundCacheSurplusNum.Value / checkNum;
            }

            var minGroundSurplusNum = await _ticketGroundRepository.GetAll()
                .Where(t => t.TicketId == ticketSale.Id)
                .Select(t => t.SurplusNum)
                .MinAsync();
            if (minGroundSurplusNum.HasValue)
            {
                return minGroundSurplusNum.Value / checkNum;
            }

            return 0;
        }

        public async Task<bool> IsUsableAsync(TicketSale ticketSale)
        {
            if (ticketSale.TicketStatusId == TicketStatus.作废)
            {
                return false;
            }

            if (ticketSale.Etime.To<DateTime>() < DateTime.Now)
            {
                return false;
            }

            if (ticketSale.TicketType == null)
            {
                ticketSale.TicketType = await _ticketTypeRepository.GetAll()
                    .AsNoTracking()
                    .FirstOrDefaultAsync(t => t.Id == ticketSale.TicketTypeId.Value);
            }

            var checkType = ticketSale.CheckTypeId ?? ticketSale.TicketType.CheckTypeId;
            if (checkType.IsCheckByNum())
            {
                var maxGroundSurplusNum = await GetMaxGroundCacheSurplusNumAsync(ticketSale);
                if (!maxGroundSurplusNum.HasValue || maxGroundSurplusNum.Value <= 0)
                {
                    return false;
                }
            }

            return true;
        }

        private async Task<int?> GetMaxGroundCacheSurplusNumAsync(TicketSale ticketSale)
        {
            var maxSurplusNum = await _ticketGroundCacheRepository.GetAll()
                .Where(t => t.TicketId == ticketSale.Id)
                .Select(t => t.SurplusNum)
                .MaxAsync();

            return maxSurplusNum;
        }

        public async Task<bool> AllowRefundAsync(TicketSale ticketSale)
        {
            if (ticketSale.TicketStatusId == TicketStatus.作废)
            {
                return false;
            }

            if (!ticketSale.InvoiceNo.IsNullOrEmpty()) return false;

            if (ticketSale.HasExchanged)
            {
                return false;
            }

            if (ticketSale.CheckTypeId == CheckType.有效期票 && ticketSale.TicketStatusId != TicketStatus.已售)
            {
                return false;
            }

            if (ticketSale.TicketType == null)
            {
                ticketSale.TicketType = await _ticketTypeRepository.GetAll()
                    .AsNoTracking()
                    .FirstOrDefaultAsync(t => t.Id == ticketSale.TicketTypeId.Value);
            }

            var checkType = ticketSale.CheckTypeId ?? ticketSale.TicketType.CheckTypeId;
            if (checkType.IsCheckByNum())
            {
                if (ticketSale.SurplusNum <= 0)
                {
                    return false;
                }
            }

            if (!ticketSale.TicketType.AllowRefund)
            {
                return false;
            }

            if (ticketSale.TicketType.AllowExpiredRefund == false && ticketSale.Etime.To<DateTime>() < DateTime.Now)
            {
                return false;
            }

            if (ticketSale.TicketType.AllowExpiredRefund == true && ticketSale.TicketType.AllowExpiredRefundMaxDays > 0)
            {
                if (ticketSale.Etime.To<DateTime>().AddDays(ticketSale.TicketType.AllowExpiredRefundMaxDays.Value) < DateTime.Now)
                {
                    return false;
                }
            }

            return true;
        }

        public async Task<bool> IsActiveAsync(TicketSale ticketSale)
        {
            if (ticketSale.TicketStatusId == TicketStatus.作废) return true;

            return await _ticketGroundRepository.GetAll().AnyAsync(t => t.TicketId == ticketSale.Id);
        }

        public async Task ActiveAsync(TicketSale ticketSale, IEnumerable<GroundChangCiDto> groundChangCis = null)
        {
            if (ticketSale.TicketCode.IsNullOrEmpty())
            {
                throw new TmsException("票号未绑定");
            }

            if (ticketSale.CardNo.IsNullOrEmpty())
            {
                throw new TmsException("卡号未绑定");
            }

            if (ticketSale.TicketType == null || ticketSale.TicketType.TicketTypeGrounds.IsNullOrEmpty())
            {
                ticketSale.TicketType = await _ticketTypeRepository.GetAll()
                    .AsNoTracking()
                    .Include(t => t.TicketTypeGrounds)
                    .Where(t => t.Id == ticketSale.TicketTypeId)
                    .FirstOrDefaultAsync();
            }

            foreach (var ticketTypeGround in ticketSale.TicketType.TicketTypeGrounds)
            {
                var ticketGround = ticketSale.MapToTicketGround();
                ticketGround.GroundId = ticketTypeGround.GroundId;
                ticketGround.GroundPrice = ticketTypeGround.GroundPrice;
                ticketSale.TicketGrounds.Add(ticketGround);

                var ticketGroundCache = ticketGround.MapToTicketGroundCache();
                ticketGroundCache.GroundId = ticketTypeGround.GroundId;
                ticketGroundCache.GroundPrice = ticketTypeGround.GroundPrice;
                ticketSale.TicketGroundCaches.Add(ticketGroundCache);

                if (!groundChangCis.IsNullOrEmpty())
                {
                    ticketGround.ChangCiId = groundChangCis.FirstOrDefault(g => g.GroundId == ticketTypeGround.GroundId)?.ChangCiId;
                    if (ticketGround.ChangCiId.HasValue)
                    {
                        var changCi = await _changCiRepository.FirstOrDefaultAsync(ticketGround.ChangCiId.Value);
                        ticketGround.Stime = $"{ticketGround.Stime.Substring(0, 10)} {changCi.Stime}:00";
                        ticketGround.Etime = $"{ticketGround.Stime.Substring(0, 10)} {changCi.Etime}:59";
                    }

                    if (!ticketSale.TicketSaleSeats.IsNullOrEmpty())
                    {
                        ticketGround.SeatId = (int?)ticketSale.TicketSaleSeats.FirstOrDefault(s => s.ChangCiId == ticketGround.ChangCiId)?.SeatId;
                    }

                    ticketGroundCache.ChangCiId = ticketGround.ChangCiId;
                    ticketGroundCache.Stime = ticketGround.Stime;
                    ticketGroundCache.Etime = ticketGround.Etime;
                    ticketGroundCache.SeatId = ticketGround.SeatId;
                }
            }

            if (!groundChangCis.IsNullOrEmpty())
            {
                var maxEtime = ticketSale.TicketGroundCaches.Max(g => g.Etime.To<DateTime>());
                if (ticketSale.Etime.To<DateTime>() < maxEtime)
                {
                    ticketSale.Etime = maxEtime.ToDateTimeString();
                }
            }
        }

        public async Task ChangeChangCiAsync(TicketSale ticketSale, int changCiId)
        {
            var changCi = await _changCiRepository.GetAll().AsNoTracking().FirstOrDefaultAsync(c => c.Id == changCiId);
            UserFriendlyCheck.NotNull(changCi, $"场次：{changCiId}不存在");

            ticketSale.ChangCiId = changCiId;
            ticketSale.Stime = $"{ticketSale.Sdate} {changCi.Stime}:00";
            ticketSale.Etime = $"{ticketSale.Sdate} {changCi.Etime}:59";

            var ticketGroundCaches = await _ticketGroundCacheRepository.GetAllListAsync(t => t.TicketId == ticketSale.Id);
            foreach (var ticketGroundCache in ticketGroundCaches)
            {
                ticketGroundCache.ChangCiId = changCiId;
                ticketGroundCache.Stime = ticketSale.Stime;
                ticketGroundCache.Etime = ticketSale.Etime;
            }

            var ticketGrounds = await _ticketGroundRepository.GetAllListAsync(t => t.TicketId == ticketSale.Id);
            foreach (var ticketGround in ticketGrounds)
            {
                ticketGround.ChangCiId = changCiId;
                ticketGround.Stime = ticketSale.Stime;
                ticketGround.Etime = ticketSale.Etime;
            }
        }

        public async Task<TicketSale> RenewAsync(long ticketId)
        {
            var query = _ticketSaleRepository.GetAllIncluding(t => t.TicketGroundCaches, t => t.TicketGrounds).Where(t => t.Id == ticketId);
            var ticketSale = await _ticketSaleRepository.FirstOrDefaultAsync(query);
            if (ticketSale == null)
            {
                throw new UserFriendlyException($"TicketID{ticketId}不存在");
            }

            var etime = (await _expirationDateCalculator.GetEndDelayTimeAsync(DateTime.Now.Date, ticketSale.TicketTypeId.Value)).ToDateTimeString();

            ticketSale.Renew(etime);

            return ticketSale;
        }

        public async Task InValidAsync(TicketSale ticketSale)
        {
            ticketSale.InValid();

            await _ticketSaleRepository.InValidAsync(ticketSale);
        }

        public async Task ConsumeAsync(TicketSale ticketSale, ConsumeTicketInput input)
        {
            int realNum = await GetRealNumAsync(ticketSale);
            int consumeNum = Math.Min(realNum, input.ConsumeNum);

            var ticketType = ticketSale.TicketType ?? await _ticketTypeRepository.GetAll().AsNoTracking().FirstOrDefaultAsync(t => t.Id == ticketSale.TicketTypeId.Value);

            var ticketGroundCaches = await _ticketGroundCacheRepository.GetAllListAsync(g => g.TicketId == ticketSale.Id);
            var consumeTicketGroundCaches = input.GroundId.HasValue ? ticketGroundCaches.Where(g => g.GroundId == input.GroundId).ToList() : ticketGroundCaches;
            foreach (var ticketGroundCache in consumeTicketGroundCaches)
            {
                int groundConsumeNum = Math.Min(ticketGroundCache.SurplusNum.Value, consumeNum);

                ticketGroundCache.Consume(groundConsumeNum, input.GateId);

                var ticketGround = await _ticketGroundRepository.FirstOrDefaultAsync(g => g.TicketId == ticketSale.Id && g.GroundId == ticketGroundCache.GroundId);
                ticketGround.Consume(groundConsumeNum, input.GateId);

                var ticketCheck = ticketSale.MapToTicketCheck();
                ticketCheck.GroundId = ticketGroundCache.GroundId;
                ticketCheck.GroundName = _nameCacheService.GetGroundName(ticketCheck.GroundId);
                ticketCheck.GateGroupId = input.GateGroupId;
                ticketCheck.GateGroupName = _nameCacheService.GetGateGroupName(ticketCheck.GateGroupId);
                ticketCheck.GroundPrice = ticketGroundCache.GroundPrice;
                ticketCheck.GateId = input.GateId;
                ticketCheck.GateName = _nameCacheService.GetGateName(ticketCheck.GateId);
                ticketCheck.InOutFlag = true;
                ticketCheck.InOutFlagName = "入";
                ticketCheck.CheckTypeId = ticketGroundCache.CheckTypeId;
                ticketCheck.CheckTypeName = ticketCheck.CheckTypeId?.ToString();
                ticketCheck.SurplusNum = ticketGroundCache.SurplusNum;
                ticketCheck.CheckNum = groundConsumeNum;
                ticketCheck.RecycleFlag = ticketType.RecycleFlag;
                ticketCheck.RecycleFlagName = ticketCheck.RecycleFlag == true ? "是" : "否";
                ticketCheck.CheckerId = input.CheckerId;
                ticketCheck.CheckerName = _nameCacheService.GetStaffName(ticketCheck.CheckerId);
                await _ticketCheckRepository.InsertAndGetIdAsync(ticketCheck);
            }

            int totalConsumeNum = await GetConsumeNumAsync(ticketSale);
            int surplusNum = realNum - totalConsumeNum;
            if (surplusNum > 0)
            {
                var checkType = ticketSale.CheckTypeId ?? ticketType.CheckTypeId;
                var isCheckByNum = checkType.IsCheckByNum();
                bool shouldConsume = true;
                if (isCheckByNum)
                {
                    int maxGroundTotalConsumeNum = ticketGroundCaches.Max(g => realNum * ticketSale.GetCheckNum() - g.SurplusNum).Value;
                    shouldConsume = maxGroundTotalConsumeNum > totalConsumeNum;
                }
                if (shouldConsume)
                {
                    consumeNum = Math.Min(surplusNum, consumeNum);

                    var trade = await _tradeRepository.GetAll()
                        .Where(t => t.Id == ticketSale.TradeId)
                        .Select(s => new { s.TradeSource, s.ThirdPartyPlatformId, s.ThirdPartyPlatformOrderId })
                        .FirstOrDefaultAsync();

                    var ticketConsume = new TicketConsume();
                    ticketConsume.TradeId = ticketSale.TradeId;
                    ticketConsume.TicketId = ticketSale.Id;
                    ticketConsume.CardNo = ticketSale.CardNo;
                    ticketConsume.CertNo = ticketSale.CertNo;
                    ticketConsume.TicketTypeId = ticketSale.TicketTypeId.Value;
                    ticketConsume.TicketTypeName = ticketSale.TicketTypeName;
                    ticketConsume.Price = ticketSale.ReaPrice.Value;
                    ticketConsume.ConsumeNum = consumeNum;
                    ticketConsume.ConsumeTime = DateTime.Now;
                    ticketConsume.ConsumeType = input.ConsumeType;
                    ticketConsume.NeedNotice = trade.TradeSource.IsIn(TradeSource.第三方, TradeSource.分销平台);
                    ticketConsume.ThirdPartyPlatformId = trade.ThirdPartyPlatformId;
                    ticketConsume.ThirdPartyPlatformOrderId = trade.ThirdPartyPlatformOrderId;
                    if (!ticketConsume.NeedNotice)
                    {
                        ticketConsume.HasNoticed = true;
                        ticketConsume.LastNoticeTime = ticketConsume.ConsumeTime;
                    }
                    await _ticketConsumeRepository.InsertAndGetIdAsync(ticketConsume);

                    bool validFlag = isCheckByNum ? ticketGroundCaches.Any(g => g.SurplusNum > 0) : ticketSale.Etime.To<DateTime>() > DateTime.Now;
                    ticketSale.Consume(consumeNum, isCheckByNum, validFlag);

                    var eventData = new TicketConsumingEventData();
                    eventData.ListNo = ticketSale.ListNo;
                    eventData.TotalConsumeNum = totalConsumeNum + consumeNum;
                    eventData.OrderListNo = ticketSale.OrderListNo;
                    eventData.OrderDetailId = ticketSale.OrderDetailId;
                    eventData.TicketConsume = ticketConsume;
                    await _eventBus.TriggerAsync(eventData);
                }
            }
        }

        public async Task CheckOutAsync(TicketSale ticketSale, CheckOutTicketInput input)
        {
            var ticketType = ticketSale.TicketType ?? await _ticketTypeRepository.FirstOrDefaultAsync(ticketSale.TicketTypeId.Value);

            var query = _ticketGroundCacheRepository.GetAll()
                .Where(g => g.TicketId == ticketSale.Id)
                .WhereIf(input.GroundId.HasValue, g => g.GroundId == input.GroundId);
            var checkOutTicketGroundCaches = await _ticketGroundCacheRepository.ToListAsync(query);
            foreach (var ticketGroundCache in checkOutTicketGroundCaches)
            {
                ticketGroundCache.CheckOut(input.CheckNum, input.GateId);

                var ticketGround = await _ticketGroundRepository.FirstOrDefaultAsync(g => g.TicketId == ticketSale.Id && g.GroundId == ticketGroundCache.GroundId);
                ticketGround.CheckOut(input.CheckNum, input.GateId);

                var ticketCheck = ticketSale.MapToTicketCheck();
                ticketCheck.GroundId = ticketGroundCache.GroundId;
                ticketCheck.GroundName = _nameCacheService.GetGroundName(ticketGroundCache.GroundId);
                ticketCheck.GateGroupId = input.GateGroupId;
                ticketCheck.GateGroupName = input.GateGroupId.HasValue ? _nameCacheService.GetGateGroupName(input.GateGroupId.Value) : string.Empty;
                ticketCheck.GroundPrice = ticketGroundCache.GroundPrice;
                ticketCheck.GateId = input.GateId;
                ticketCheck.GateName = input.GateId.HasValue ? _nameCacheService.GetGateName(input.GateId.Value) : string.Empty;
                ticketCheck.InOutFlag = false;
                ticketCheck.InOutFlagName = "出";
                ticketCheck.CheckTypeId = ticketGroundCache.CheckTypeId;
                ticketCheck.CheckTypeName = ticketCheck.CheckTypeId?.ToString();
                ticketCheck.SurplusNum = ticketGroundCache.SurplusNum;
                ticketCheck.CheckNum = input.CheckNum;
                ticketCheck.RecycleFlag = ticketType.RecycleFlag;
                ticketCheck.RecycleFlagName = ticketCheck.RecycleFlag == true ? "是" : "否";
                ticketCheck.CheckerId = input.CheckerId;
                await _ticketCheckRepository.InsertAsync(ticketCheck);
            }
        }

        public async Task<DateTime?> GetLastCheckInTimeAsync(long id, bool isChecking = false)
        {
            var checkTimes = await _ticketCheckRepository.GetAll()
                .Where(t => t.TicketId == id && t.InOutFlag == true)
                .OrderByDescending(t => t.Ctime)
                .Select(t => t.Ctime)
                .Take(2)
                .ToListAsync();

            if (isChecking)
            {
                return checkTimes.Count == 2 ? checkTimes[1] : null;
            }
            else
            {
                return checkTimes.FirstOrDefault();
            }
        }

        public async Task InvoiceAsync(List<TicketSale> ticketSales, InvoiceInfo invoice)
        {
            foreach (var ticketSale in ticketSales)
            {
                ticketSale.InvoiceCode = invoice.FPDM;
                ticketSale.InvoiceNo = invoice.FPHM;
            }

            await _invoiceRepository.InsertAsync(invoice);
        }
    }
}
