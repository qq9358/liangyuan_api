using Egoal.Application.Services;
using Egoal.AutoMapper;
using Egoal.BackgroundJobs;
using Egoal.Caches;
using Egoal.Common;
using Egoal.Cryptography;
using Egoal.Domain.Repositories;
using Egoal.DynamicCodes;
using Egoal.Events.Bus;
using Egoal.Extensions;
using Egoal.Invoice;
using Egoal.Members;
using Egoal.Orders;
using Egoal.Orders.Dto;
using Egoal.Runtime.Session;
using Egoal.Scenics;
using Egoal.Tickets.Dto;
using Egoal.TicketTypes;
using Egoal.Trades;
using Egoal.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Egoal.Tickets
{
    public class TicketSaleAppService : ApplicationService, ITicketSaleAppService
    {
        private readonly ISession _session;
        private readonly IEventBus _eventBus;
        private readonly OrderOptions _orderOptions;
        private readonly IDynamicCodeService _dynamicCodeService;
        private readonly INameCacheService _nameCacheService;
        private readonly IBackgroundJobService _backgroundJobService;
        private readonly ExpirationDateCalculator _expirationDateCalculator;
        private readonly ITicketSaleDomainService _ticketSaleDomainService;
        private readonly ITradeAppService _tradeAppService;
        private readonly ITicketTypeDomainService _ticketTypeDomainService;
        private readonly ITicketSaleRepository _ticketSaleRepository;
        private readonly IRepository<TicketGroundCache, long> _ticketGroundCacheRepository;
        private readonly ITicketTypeRepository _ticketTypeRepository;
        private readonly IRepository<Ground> _groundRepository;
        private readonly ITradeRepository _tradeRepository;
        private readonly IRepository<TicketReprintLog, long> _ticketReprintLogRepository;
        private readonly ITicketSaleSeatRepository _ticketSaleSeatRepository;
        private readonly IRepository<ChangCi> _changCiRepository;
        private readonly TicketSaleBuilder _ticketSaleBuilder;
        private readonly IInvoiceService _invoiceService;

        public TicketSaleAppService(
            ISession session,
            IEventBus eventBus,
            IOptions<OrderOptions> orderOptions,
            IDynamicCodeService dynamicCodeService,
            INameCacheService nameCacheService,
            IBackgroundJobService backgroundJobService,
            ExpirationDateCalculator expirationDateCalculator,
            ITicketSaleDomainService ticketSaleDomainService,
            ITradeAppService tradeAppService,
            ITicketTypeDomainService ticketTypeDomainService,
            ITicketSaleRepository ticketSaleRepository,
            IRepository<TicketGroundCache, long> ticketGroundCacheRepository,
            ITicketTypeRepository ticketTypeRepository,
            IRepository<Ground> groundRepository,
            ITradeRepository tradeRepository,
            IRepository<TicketReprintLog, long> ticketReprintLogRepository,
            ITicketSaleSeatRepository ticketSaleSeatRepository,
            IRepository<ChangCi> changCiRepository,
            TicketSaleBuilder ticketSaleBuilder,
            IInvoiceService invoiceService)
        {
            _session = session;
            _eventBus = eventBus;
            _orderOptions = orderOptions.Value;
            _dynamicCodeService = dynamicCodeService;
            _nameCacheService = nameCacheService;
            _backgroundJobService = backgroundJobService;
            _expirationDateCalculator = expirationDateCalculator;
            _ticketSaleDomainService = ticketSaleDomainService;
            _tradeAppService = tradeAppService;
            _ticketTypeDomainService = ticketTypeDomainService;
            _ticketSaleRepository = ticketSaleRepository;
            _ticketGroundCacheRepository = ticketGroundCacheRepository;
            _ticketTypeRepository = ticketTypeRepository;
            _groundRepository = groundRepository;
            _tradeRepository = tradeRepository;
            _ticketReprintLogRepository = ticketReprintLogRepository;
            _ticketSaleSeatRepository = ticketSaleSeatRepository;
            _changCiRepository = changCiRepository;
            _ticketSaleBuilder = ticketSaleBuilder;
            _invoiceService = invoiceService;
        }

        public async Task<List<TicketSale>> SaleAsync(SaleTicketInput input)
        {
            var ticketSales = new List<TicketSale>();

            foreach (var item in input.Items)
            {
                var ticketType = await _ticketTypeRepository.GetAll().AsNoTracking().FirstOrDefaultAsync(t => t.Id == item.TicketTypeId);

                int personNum = 1;
                int ticketNum = item.Quantity;
                if (ticketType.CheckTypeId == CheckType.多人票)
                {
                    personNum = item.Quantity;
                    ticketNum = 1;
                }

                for (int i = 0; i < ticketNum; i++)
                {
                    var ticketSale = input.MapToTicketSale();
                    ticketSale.TicketStatusId = TicketStatus.已售;
                    ticketSale.TicketStatusName = ticketSale.TicketStatusId.ToString();
                    ticketSale.TicketType = ticketType;
                    ticketSale.TicketTypeTypeId = ticketType.TicketTypeTypeId;
                    ticketSale.TicketTypeId = ticketType.Id;
                    ticketSale.TicketTypeName = ticketType.Name;
                    ticketSale.TicketBindTypeId = ticketType.TicketBindTypeId;
                    ticketSale.Tkid = ticketType.Tkid;
                    ticketSale.Tkname = ticketSale.Tkid?.ToString();
                    ticketSale.Ttid = ticketType.Ttid;
                    ticketSale.PersonNum = personNum;
                    if (input.IsExchange)
                    {
                        ticketSale.SetTicMoney(personNum, item.TicPrice);
                        ticketSale.SetReaMoney(personNum, item.RealPrice);
                    }
                    else
                    {
                        var price = await _ticketTypeDomainService.GetPriceAsync(ticketType, input.TravelDate, input.SaleChannel);
                        ticketSale.SetTicMoney(personNum, price);
                        ticketSale.SetReaMoney(personNum, price);
                    }
                    var dailyPrice = await _ticketTypeDomainService.GetPriceAsync(item.TicketTypeId, input.TravelDate.ToDateString());
                    var printPrice = dailyPrice?.PrintPrice;
                    ticketSale.SetPrintMoney(personNum, printPrice ?? ticketType.PrintPrice);
                    ticketSale.TotalNum = ticketSale.SurplusNum = personNum * ticketType.CheckNum;
                    ticketSale.CheckTypeId = ticketType.CheckTypeId;
                    ticketSale.OrderDetailId = item.OrderDetailId;
                    ticketSale.StatFlag = ticketType.StatFlag;

                    await SetCheckTimeAsync(ticketSale, ticketType, input);

                    if (!item.Tourists.IsNullOrEmpty())
                    {
                        if (ticketNum == item.Tourists.Count)
                        {
                            var tourist = item.Tourists[i];

                            BindTicketSaleTourist(ticketSale, tourist);
                            ticketSale.CertTypeId = tourist.CertType;
                            ticketSale.CertTypeName = _nameCacheService.GetCertTypeName(ticketSale.CertTypeId);
                            ticketSale.CertNo = tourist.CertNo;
                        }
                        else
                        {
                            foreach (var tourist in item.Tourists)
                            {
                                BindTicketSaleTourist(ticketSale, tourist);
                            }
                        }
                    }

                    if (item.HasGroundSeat)
                    {
                        if (ticketSale.TicketSaleSeats == null)
                        {
                            ticketSale.TicketSaleSeats = new List<TicketSaleSeat>();
                        }

                        foreach (var groundChangCi in item.GroundChangCis)
                        {
                            var seats = input.Seats.Where(s => !s.HasBindToTicket && s.GroundId == groundChangCi.GroundId && s.ChangCiId == groundChangCi.ChangCiId).Take(personNum);
                            foreach (var seat in seats)
                            {
                                var ticketSaleSeat = new TicketSaleSeat();
                                ticketSaleSeat.TradeId = ticketSale.TradeId;
                                ticketSaleSeat.SeatId = seat.SeatId;
                                ticketSaleSeat.Sdate = seat.Sdate;
                                ticketSaleSeat.ChangCiId = seat.ChangCiId;
                                ticketSale.TicketSaleSeats.Add(ticketSaleSeat);

                                seat.HasBindToTicket = true;
                            }
                        }
                    }

                    if (!item.GroundChangCis.IsNullOrEmpty())
                    {
                        ticketSale.Memo = _nameCacheService.GetChangCiName(item.GroundChangCis.First().ChangCiId);
                    }

                    string ticketCode = string.Empty;
                    if (input.TradeSource == TradeSource.微信)
                    {
                        ticketCode = await _dynamicCodeService.GenerateWxTicketCodeAsync();
                    }
                    else
                    {
                        ticketCode = await _dynamicCodeService.GenerateParkTicketCodeAsync(input.ParkId ?? 1);
                    }
                    ticketSale.BindTicket(ticketCode, ticketCode);

                    if (ticketType.FirstActiveFlag != true)
                    {
                        await _ticketSaleDomainService.ActiveAsync(ticketSale, item.GroundChangCis);
                    }

                    ticketSales.Add(ticketSale);
                }
            }

            foreach (var ticketSale in ticketSales)
            {
                if (input.PayFlag)
                {
                    ticketSale.Pay(input.PayTypeId, input.PayTypeName);
                }
                await _ticketSaleRepository.InsertAndGetIdAsync(ticketSale);
            }

            input.TotalMoney = ticketSales.Sum(t => t.ReaMoney.Value);
            input.StatFlag = ticketSales.Any(t => t.StatFlag == true);
            await _tradeAppService.SaleTicketAsync(input);

            return ticketSales;
        }

        private async Task SetCheckTimeAsync(TicketSale ticketSale, TicketType ticketType, SaleTicketInput input)
        {
            var travelDate = input.TravelDate.ToDateString();

            if (input.ChangCiId.HasValue)
            {
                var changCi = await _changCiRepository.GetAll().AsNoTracking().FirstOrDefaultAsync(c => c.Id == input.ChangCiId.Value);
                if (changCi == null)
                {
                    throw new UserFriendlyException($"场次：{input.ChangCiId}不存在");
                }

                ticketSale.Stime = $"{travelDate} {changCi.Stime}:00";
                ticketSale.Etime = $"{travelDate} {changCi.Etime}:59";
            }
            else
            {
                ticketSale.Stime = _expirationDateCalculator.GetStartValidTime(input.TravelDate, ticketType).ToDateTimeString();
                ticketSale.Etime = _expirationDateCalculator.GetEndValidTime(input.TravelDate, ticketType).ToDateTimeString();
            }

            ticketSale.Sdate = travelDate;
        }

        private void BindTicketSaleTourist(TicketSale ticketSale, TicketTourist tourist)
        {
            var ticketSaleBuyer = ticketSale.MapToTicketSaleBuyer();
            ticketSaleBuyer.BuyerName = tourist.Name;
            if (!tourist.Birthday.IsNullOrEmpty())
            {
                ticketSaleBuyer.Birthday = tourist.Birthday;
            }
            if (!tourist.CertNo.IsNullOrEmpty())
            {
                if (tourist.CertType == DefaultCertType.二代身份证)
                {
                    var certNo = _orderOptions.EncodeIDCardNo == "1" && tourist.CertNo.Length > 18 ? DES3Helper.Decrypt(tourist.CertNo) : tourist.CertNo;
                    ticketSaleBuyer.SetIdCardNo(certNo);
                    ticketSaleBuyer.CertNo = tourist.CertNo;
                }
                else
                {
                    ticketSaleBuyer.CertTypeId = tourist.CertType;
                    ticketSaleBuyer.CertTypeName = _nameCacheService.GetCertTypeName(tourist.CertType);
                    ticketSaleBuyer.CertNo = tourist.CertNo;
                }
            }
            ticketSaleBuyer.SetArea();

            if (ticketSale.TicketSaleBuyers == null)
            {
                ticketSale.TicketSaleBuyers = new List<TicketSaleBuyer>();
            }
            ticketSale.TicketSaleBuyers.Add(ticketSaleBuyer);
        }

        public async Task<DateTime?> ChangeChangCiAsync(ChangeChangCiInput input)
        {
            var ticketSales = await _ticketSaleRepository.GetAllListAsync(t => t.OrderListNo == input.ListNo);
            foreach (var ticketSale in ticketSales)
            {
                await _ticketSaleDomainService.ChangeChangCiAsync(ticketSale, input.ChangCiId);
            }

            if (ticketSales.IsNullOrEmpty())
            {
                return null;
            }

            return ticketSales.Max(t => t.Etime.To<DateTime>());
        }

        public async Task InvoiceAsync(InvoiceInput input)
        {
            var ticketSales = await _ticketSaleRepository.GetAllListAsync(t => t.ListNo == input.ListNo && t.TicketStatusId != TicketStatus.已退);

            if (ticketSales.IsNullOrEmpty()) return;

            if (ticketSales.All(t => !t.InvoiceNo.IsNullOrEmpty())) return;

            var invoiceRequest = await _ticketSaleBuilder.BuildInvoiceRequestAsync(ticketSales, input);

            var response = await _invoiceService.InvoiceAsync(invoiceRequest);
            if (response == null)
            {
                throw new UserFriendlyException("开票失败");
            }

            var invoice = _ticketSaleBuilder.BuildInvoice(input, invoiceRequest, response);

            await _ticketSaleDomainService.InvoiceAsync(ticketSales, invoice);

            await _backgroundJobService.EnqueueAsync<SendInvoiceMessageJob>(_ticketSaleBuilder.BuildInvoiceMessage(invoiceRequest, invoice).ToJson());
        }

        public async Task RefundAsync(RefundInput input, RefundChannel refundChannel)
        {
            var ticketSale = await _ticketSaleRepository.GetAll()
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.TicketCode == input.TicketCode && t.TicketStatusId != TicketStatus.已退);
            if (ticketSale == null)
            {
                throw new UserFriendlyException("无效票");
            }

            if (refundChannel == RefundChannel.安卓手持机)
            {
                if (ticketSale.CashierId != _session.StaffId)
                {
                    throw new UserFriendlyException("只能退自己销售的票");
                }
            }

            if (!await _ticketSaleDomainService.AllowRefundAsync(ticketSale))
            {
                throw new UserFriendlyException("票不可退");
            }

            var surplusNum = await _ticketSaleDomainService.GetSurplusNumAsync(ticketSale);
            if (surplusNum <= 0)
            {
                throw new UserFriendlyException("次数已用完");
            }

            var tradeSource = await _tradeRepository.GetAll()
                .Where(t => t.Id == ticketSale.TradeId)
                .Select(t => t.TradeSource)
                .FirstOrDefaultAsync();

            var refundTicketInput = new RefundTicketInput();
            refundTicketInput.RefundListNo = await _dynamicCodeService.GenerateListNoAsync(tradeSource == TradeSource.本地 ? ListNoType.门票 : ListNoType.门票网上订票);
            refundTicketInput.PayListNo = ticketSale.ListNo;
            refundTicketInput.PayTypeId = ticketSale.PayTypeId.Value;
            refundTicketInput.RefundReason = "景区退票";
            refundTicketInput.CashierId = input.CashierId;
            refundTicketInput.CashierName = _nameCacheService.GetStaffName(refundTicketInput.CashierId);
            refundTicketInput.CashPcid = input.CashPcid;
            refundTicketInput.CashPcname = _nameCacheService.GetPcName(refundTicketInput.CashPcid);
            refundTicketInput.SalePointId = input.SalePointId;
            refundTicketInput.SalePointName = _nameCacheService.GetSalePointName(refundTicketInput.SalePointId);
            refundTicketInput.ParkId = input.ParkId;
            refundTicketInput.ParkName = _nameCacheService.GetParkName(refundTicketInput.ParkId);
            refundTicketInput.OriginalTradeId = ticketSale.TradeId;

            RefundTicketItem refundTicketItem = new RefundTicketItem();
            refundTicketItem.TicketId = ticketSale.Id;
            refundTicketItem.RefundQuantity = surplusNum;
            refundTicketItem.SurplusQuantityAfterRefund = 0;
            refundTicketInput.Items.Add(refundTicketItem);

            await RefundAsync(refundTicketInput);
        }

        public async Task RefundAsync(RefundTicketInput input)
        {
            var originalTicketSales = new List<TicketSale>();
            var ticketSales = new List<TicketSale>();

            foreach (var item in input.Items)
            {
                var originalTicketSale = await _ticketSaleRepository.FirstOrDefaultAsync(item.TicketId);
                originalTicketSales.Add(originalTicketSale);

                int checkNum = originalTicketSale.GetCheckNum();

                var ticketSale = input.MapToTicketSale();
                originalTicketSale.CopyTo(ticketSale);
                ticketSale.ValidFlag = false;
                ticketSale.ValidFlagName = "无效";
                ticketSale.TicketStatusId = TicketStatus.已退;
                ticketSale.TicketStatusName = TicketStatus.已退.ToString();
                ticketSale.TicketNum = -originalTicketSale.TicketNum;
                int personNum = -item.RefundQuantity;
                ticketSale.PersonNum = personNum;
                ticketSale.TotalNum = checkNum * personNum;
                ticketSale.SurplusNum = 0;
                ticketSale.SetTicMoney(personNum, originalTicketSale.TicPrice);
                ticketSale.SetReaMoney(personNum, originalTicketSale.ReaPrice);
                ticketSale.SetPrintMoney(personNum, originalTicketSale.PrintPrice);
                ticketSale.ReturnTicketId = originalTicketSale.Id;
                ticketSale.ReturnRate = 100M;

                ticketSales.Add(ticketSale);

                if (await _ticketSaleDomainService.ShouldInValidAsync(originalTicketSale, item.SurplusQuantityAfterRefund, item.RefundQuantity))
                {
                    await _ticketSaleDomainService.InValidAsync(originalTicketSale);
                }
                else
                {
                    int refundCheckNum = item.RefundQuantity * checkNum;
                    originalTicketSale.SurplusNum -= refundCheckNum;
                    await _ticketSaleRepository.RefundAsync(originalTicketSale, refundCheckNum);
                }

                await DeleteSeatsAsync(originalTicketSale.Id, item.RefundQuantity);

                await _ticketSaleRepository.InsertAsync(ticketSale);
            }

            input.TotalMoney = ticketSales.Sum(t => t.ReaMoney.Value);

            var eventData = input.MapTo<RefundTicketEventData>();
            eventData.Items.Select(i => i.OriginalTicketSale = originalTicketSales.FirstOrDefault(t => t.Id == i.TicketId)).ToArray();
            await _eventBus.TriggerAsync(eventData);

            await _tradeAppService.RefundTicketAsync(input);
        }

        private async Task DeleteSeatsAsync(long ticketId, int quantity)
        {
            var seats = await _ticketSaleSeatRepository.GetAll()
                .Where(s => s.TicketId == ticketId)
                .OrderBy(s => s.SeatId)
                .Take(quantity)
                .ToListAsync();
            foreach (var seat in seats)
            {
                await _ticketSaleSeatRepository.DeleteAsync(seat);
            }
        }

        public async Task<CheckTicketOutput> CheckTicketAsync(CheckTicketInput input)
        {
            var query = _ticketGroundCacheRepository.GetAll()
                .Where(g => g.TicketCode == input.TicketCode || g.CertNo == input.TicketCode)
                .Where(g => g.GroundId == input.GroundId)
                .Where(g => g.CommitFlag == true)
                .OrderByDescending(g => g.Id);
            var ticketGroundCache = await _ticketGroundCacheRepository.FirstOrDefaultAsync(query);

            if (ticketGroundCache == null)
            {
                throw new UserFriendlyException("无效票");
            }

            if (ticketGroundCache.TicketStatusId.IsIn(TicketStatus.挂失, TicketStatus.过期, TicketStatus.作废, TicketStatus.已退))
            {
                throw new UserFriendlyException($"此票{ticketGroundCache.TicketStatusId}");
            }

            if (ticketGroundCache.Etime.To<DateTime>() < DateTime.Now)
            {
                throw new UserFriendlyException("此票已过期");
            }

            var isCheckByNum = ticketGroundCache.CheckTypeId.Value.IsCheckByNum();
            if (isCheckByNum)
            {
                if (ticketGroundCache.SurplusNum <= 0)
                {
                    throw new UserFriendlyException("次数已用完");
                }

                var ground = await _groundRepository.GetAll().AsNoTracking().FirstOrDefaultAsync(g => g.Id == input.GroundId);
                if (ground == null)
                {
                    throw new UserFriendlyException("检票区域未定义");
                }

                if (ground.LastGroundId > 0)
                {
                    if (await _ticketGroundCacheRepository.AnyAsync(t =>
                    t.TicketId == ticketGroundCache.TicketId &&
                    t.GroundId == ground.LastGroundId &&
                    t.CommitFlag == true &&
                    t.SurplusNum <= 0))
                    {
                        throw new UserFriendlyException("次数已用完");
                    }
                }
            }

            if (!await _ticketTypeDomainService.HasGrantedToGateGroupAsync(ticketGroundCache.TicketTypeId.Value, input.GateGroupId))
            {
                throw new UserFriendlyException("通道未授权");
            }

            var ticketType = await _ticketTypeRepository.GetAll().AsNoTracking().FirstOrDefaultAsync(t => t.Id == ticketGroundCache.TicketTypeId.Value);
            if (ticketType == null)
            {
                throw new UserFriendlyException("票类未定义");
            }

            if (ticketGroundCache.IsTodayUsed())
            {
                if (ticketType.MaxCheckNumByDay > 0 && ticketGroundCache.CheckTimesByDay >= ticketType.MaxCheckNumByDay)
                {
                    throw new UserFriendlyException("已达每日最大检票次数");
                }

                if (ticketType.CheckInterval > 0 && ticketGroundCache.LastInCheckTime.Value.AddMinutes(ticketType.CheckInterval.Value) > DateTime.Now)
                {
                    throw new UserFriendlyException("未超检票间隔");
                }
            }

            var startCheckInTime = ticketGroundCache.Stime.To<DateTime>();
            if (ticketType.EarlyIn > 0)
            {
                startCheckInTime = startCheckInTime.AddMinutes(-ticketType.EarlyIn.Value);
            }
            if (startCheckInTime > DateTime.Now)
            {
                throw new UserFriendlyException("未到检票时间");
            }

            if (ticketType.DelayIn > 0)
            {
                var endCheckInTime = ticketGroundCache.Stime.To<DateTime>().AddMinutes(ticketType.DelayIn.Value);
                if (endCheckInTime < DateTime.Now)
                {
                    throw new UserFriendlyException("已过检票时间");
                }
            }

            var ticketSale = await _ticketSaleRepository.FirstOrDefaultAsync(ticketGroundCache.TicketId);
            ticketSale.TicketType = ticketType;

            var consumeInput = new ConsumeTicketInput();
            consumeInput.ConsumeNum = ticketSale.PersonNum.Value;
            consumeInput.ConsumeType = input.ConsumeType;
            consumeInput.GroundId = input.GroundId;
            consumeInput.GateGroupId = input.GateGroupId;
            consumeInput.GateId = input.GateId;
            consumeInput.CheckerId = _session.StaffId;

            await _ticketSaleDomainService.ConsumeAsync(ticketSale, consumeInput);

            var output = new CheckTicketOutput();
            output.CardNo = ticketSale.CardNo;
            output.TicketTypeName = ticketSale.TicketTypeName;
            output.Stime = ticketGroundCache.Stime;
            output.Etime = ticketGroundCache.Etime;
            output.TotalNum = ticketGroundCache.TotalNum;
            output.SurplusNum = ticketGroundCache.SurplusNum;
            output.GroundName = _nameCacheService.GetGroundName(consumeInput.GroundId);
            output.CheckerName = _nameCacheService.GetStaffName(consumeInput.CheckerId);
            output.ShouldPrintAfterCheck = ticketType.ShouldPrintAfterCheck;
            output.CheckTime = DateTime.Now;
            if (!isCheckByNum)
            {
                output.LastCheckInTime = await _ticketSaleDomainService.GetLastCheckInTimeAsync(ticketSale.Id, true);
            }

            return output;
        }

        public async Task RePrintAsync(PrintTicketInput input)
        {
            await PrintAsync(input, true);
        }

        public async Task PrintAsync(PrintTicketInput input, bool isReprint = false)
        {
            var ticketSales = await _ticketSaleRepository.GetAll()
                .WhereIf(!input.ListNo.IsNullOrEmpty(), t => t.ListNo == input.ListNo)
                .WhereIf(!input.TicketCode.IsNullOrEmpty(), t => t.TicketCode == input.TicketCode)
                .Where(t => t.TicketStatusId != TicketStatus.已退)
                .ToListAsync();
            foreach (var ticketSale in ticketSales)
            {
                ticketSale.Print();

                if (!isReprint) continue;

                var rePrintLog = new TicketReprintLog();
                rePrintLog.TicketId = ticketSale.Id;
                rePrintLog.TicketTypeId = ticketSale.TicketTypeId;
                rePrintLog.TicketTypeName = ticketSale.TicketTypeName;
                rePrintLog.TicketCode = ticketSale.TicketCode;
                rePrintLog.CardNo = ticketSale.CardNo;
                rePrintLog.CashierId = _session.StaffId;
                rePrintLog.CashierName = _nameCacheService.GetStaffName(rePrintLog.CashierId);
                rePrintLog.CashPcid = _session.PcId;
                rePrintLog.CashPcname = _nameCacheService.GetPcName(rePrintLog.CashPcid);
                rePrintLog.SalePointId = _session.SalePointId;
                rePrintLog.ParkId = _session.ParkId;
                rePrintLog.ParkName = _nameCacheService.GetParkName(rePrintLog.ParkId);

                await _ticketReprintLogRepository.InsertAsync(rePrintLog);
            }
        }
    }
}
