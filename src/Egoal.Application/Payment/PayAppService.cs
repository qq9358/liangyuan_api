using Egoal.Application.Services;
using Egoal.Auditing;
using Egoal.AutoMapper;
using Egoal.BackgroundJobs;
using Egoal.Domain.Repositories;
using Egoal.Domain.Uow;
using Egoal.DynamicCodes;
using Egoal.Events.Bus;
using Egoal.Extensions;
using Egoal.Logging;
using Egoal.Payment.Dto;
using Egoal.Staffs;
using Egoal.Threading.Lock;
using Egoal.Trades;
using Egoal.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace Egoal.Payment
{
    public class PayAppService : ApplicationService, IPayAppService
    {
        private readonly ILogger _logger;
        private readonly INetPayOrderRepository _netPayOrderRepository;
        private readonly IRepository<RefundMoneyApply, long> _refundMoneyApplyRepository;
        private readonly ITradeRepository _tradeRepository;
        private readonly IRepository<NetPayRefundFail, Guid> _netPayRefundFailRepository;
        private readonly IRepository<Staff> _staffRepository;
        private readonly NetPayServiceFactory _netPayServiceFactory;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly PayOptions _payOptions;
        private readonly IEventBus _eventBus;
        private readonly IBackgroundJobService _backgroundJobAppService;
        private readonly IDynamicCodeService _dynamicCodeService;
        private readonly IClientInfoProvider _clientInfoProvider;
        private readonly IDistributedLockFactory _lockFactory;

        public PayAppService(
            ILogger<PayAppService> logger,
            INetPayOrderRepository netPayOrderRepository,
            IRepository<RefundMoneyApply, long> refundMoneyApplyRepository,
            ITradeRepository tradeRepository,
            IRepository<NetPayRefundFail, Guid> netPayRefundFailRepository,
            IRepository<Staff> staffRepository,
            NetPayServiceFactory netPayServiceFactory,
            IUnitOfWorkManager unitOfWorkManager,
            IOptions<PayOptions> options,
            IEventBus eventBus,
            IBackgroundJobService backgroundJobAppService,
            IDynamicCodeService dynamicCodeService,
            IClientInfoProvider clientInfoProvider,
            IDistributedLockFactory lockFactory)
        {
            _logger = logger;
            _netPayOrderRepository = netPayOrderRepository;
            _refundMoneyApplyRepository = refundMoneyApplyRepository;
            _tradeRepository = tradeRepository;
            _netPayRefundFailRepository = netPayRefundFailRepository;
            _staffRepository = staffRepository;
            _netPayServiceFactory = netPayServiceFactory;
            _unitOfWorkManager = unitOfWorkManager;
            _payOptions = options.Value;
            _eventBus = eventBus;
            _backgroundJobAppService = backgroundJobAppService;
            _dynamicCodeService = dynamicCodeService;
            _clientInfoProvider = clientInfoProvider;
            _lockFactory = lockFactory;
        }

        public async Task PrePayAsync(PrePayInput payInput)
        {
            var netPayOrder = new NetPayOrder();
            netPayOrder.ListNo = payInput.ListNo;
            netPayOrder.SubPayTypeId = payInput.SubPayTypeId;
            netPayOrder.TotalFee = payInput.PayMoney;
            netPayOrder.OrderStatusId = NetPayOrderStatus.未支付;
            netPayOrder.OrderStatusName = netPayOrder.OrderStatusId.ToString();

            var netPayInput = payInput.MapTo<NetPayInput>();
            netPayInput.PayStartTime = netPayOrder.Ctime;
            netPayInput.PayExpireTime = netPayInput.PayStartTime.AddMinutes(_payOptions.WechatTimeOutOrderCancelTime);

            netPayOrder.PayArgs = netPayInput.ToJson();

            await _netPayOrderRepository.InsertAsync(netPayOrder);

            var jobArgs = new ConfirmPayStatusJobArgs();
            jobArgs.ListNo = payInput.ListNo;
            jobArgs.Attach = payInput.Attach;

            var delay = TimeSpan.FromMinutes(_payOptions.WechatTimeOutOrderCancelTime).Subtract(TimeSpan.FromSeconds(10));
            await _backgroundJobAppService.EnqueueAsync<ConfirmPayStatusJob>(jobArgs.ToJson(), delay: delay);
        }

        public async Task<NetPayOrderDto> GetNetPayOrderAsync(string listNo)
        {
            var netPayOrder = await _netPayOrderRepository.GetAll().AsNoTracking().FirstOrDefaultAsync(o => o.ListNo == listNo);
            if (netPayOrder == null)
            {
                throw new UserFriendlyException($"订单{listNo}不存在");
            }

            var orderDto = new NetPayOrderDto();
            orderDto.PayMoney = netPayOrder.TotalFee;
            if (!netPayOrder.PayArgs.IsNullOrEmpty())
            {
                var payInput = netPayOrder.PayArgs.JsonToObject<NetPayInput>();
                var timespan = payInput.PayExpireTime - DateTime.Now;
                orderDto.ExpireSeconds = Convert.ToInt64(timespan.TotalSeconds);
            }
            orderDto.PaySuccess = netPayOrder.OrderStatusId == NetPayOrderStatus.支付成功;
            orderDto.IsPaying = netPayOrder.IsPayResultUnknown();

            return orderDto;
        }

        public async Task<string> JsApiPayAsync(JsApiPayInput input)
        {
            var netPayOrder = await _netPayOrderRepository.GetAll().AsNoTracking().FirstOrDefaultAsync(o => o.ListNo == input.ListNo);
            if (netPayOrder == null)
            {
                throw new UserFriendlyException($"订单{input.ListNo}不存在");
            }

            if (!netPayOrder.AllowPay())
            {
                throw new UserFriendlyException($"订单{netPayOrder.OrderStatusName}");
            }

            if (!netPayOrder.JsApiPayArgs.IsNullOrEmpty())
            {
                return netPayOrder.JsApiPayArgs;
            }

            var payTypeId = _payOptions.WxSalePayTypeId;
            var payInput = netPayOrder.PayArgs.JsonToObject<NetPayInput>();
            payInput.ReturnUrl = input.ReturnUrl;
            payInput.ClientIp = _clientInfoProvider.ClientIpAddress;

            using (var uow = _unitOfWorkManager.Begin(TransactionScopeOption.RequiresNew))
            {
                var netPayType = netPayOrder.GetJsApiNetPayType(payTypeId);
                await _netPayOrderRepository.SetPayTypeAsync(netPayOrder.Id, payTypeId, OnlinePayTradeType.OFFIACCOUNT, netPayType, netPayType.ToString());

                var args = new QueryNetPayJobArgs();
                args.ListNo = netPayOrder.ListNo;
                args.PayTypeId = payTypeId;
                args.NetPayTypeId = netPayType;
                args.TradeType = OnlinePayTradeType.OFFIACCOUNT;
                args.Attach = payInput.Attach;
                args.IntervalSecond = 15;
                await _backgroundJobAppService.EnqueueAsync<QueryNetPayJob>(args.ToJson(), delay: TimeSpan.FromSeconds(args.IntervalSecond));

                await uow.CompleteAsync();
            }

            var payService = _netPayServiceFactory.GetPayService(payTypeId);

            var parameters = await payService.JsApiPayAsync(payInput);
            using (var uow = _unitOfWorkManager.Begin(TransactionScopeOption.RequiresNew))
            {
                await _netPayOrderRepository.SetJsApiPayArgsAsync(netPayOrder.Id, parameters);

                await uow.CompleteAsync();
            }

            return parameters;
        }

        public async Task<PayOutput> MicroPayAsync(MicroPayInput payInput)
        {
            using (var locker = await _lockFactory.LockAsync(ListNo.GetLockKey(payInput.ListNo)))
            {
                if (!locker.IsAcquired)
                {
                    throw new TmsException($"订单{payInput.ListNo}锁定失败");
                }

                using (var uow = _unitOfWorkManager.Begin(TransactionScopeOption.RequiresNew))
                {
                    var netPayOrder = await _netPayOrderRepository.FirstOrDefaultAsync(o => o.ListNo == payInput.ListNo);
                    if (netPayOrder == null)
                    {
                        throw new UserFriendlyException($"订单{payInput.ListNo}不存在");
                    }

                    if (!netPayOrder.AllowPay())
                    {
                        throw new UserFriendlyException($"订单{netPayOrder.OrderStatusName}");
                    }

                    var payTypeId = payInput.PayTypeId;
                    var netPayInput = netPayOrder.PayArgs.JsonToObject<NetPayInput>();
                    netPayInput.AuthCode = payInput.AuthCode;
                    netPayInput.ClientIp = _clientInfoProvider.ClientIpAddress;

                    using (var subUow = _unitOfWorkManager.Begin(TransactionScopeOption.RequiresNew))
                    {
                        var netPayType = netPayOrder.GetMicroNetPayType(payTypeId);
                        await _netPayOrderRepository.SetPayTypeAsync(netPayOrder.Id, payTypeId, OnlinePayTradeType.MICROPAY, netPayType, netPayType.ToString());

                        var args = new QueryNetPayJobArgs();
                        args.ListNo = netPayOrder.ListNo;
                        args.PayTypeId = payTypeId;
                        args.NetPayTypeId = netPayType;
                        args.TradeType = OnlinePayTradeType.MICROPAY;
                        args.Attach = netPayInput.Attach;
                        args.IntervalSecond = 5;
                        await _backgroundJobAppService.EnqueueAsync<QueryNetPayJob>(args.ToJson(), delay: TimeSpan.FromSeconds(args.IntervalSecond));

                        await subUow.CompleteAsync();
                    }

                    var payService = _netPayServiceFactory.GetPayService(payTypeId);

                    var result = await payService.MicroPayAsync(netPayInput);
                    if (result.IsPaid)
                    {
                        netPayOrder.TransactionId = result.TransactionId;
                        netPayOrder.SubTransactionId = result.SubTransactionId;
                        netPayOrder.OrderStatusId = NetPayOrderStatus.支付成功;
                        netPayOrder.PayTime = result.PayTime;
                        netPayOrder.BankType = result.BankType;
                        netPayOrder.ClearPayArgs();
                        netPayOrder.ErrorCode = null;

                        var eventData = new PaySuccessEventData();
                        eventData.ListNo = netPayOrder.ListNo;
                        eventData.PayTypeId = payTypeId;
                        eventData.Attach = netPayInput.Attach;
                        await _eventBus.TriggerAsync(eventData);
                    }
                    else if (result.IsPaying)
                    {
                        netPayOrder.OrderStatusId = NetPayOrderStatus.用户支付中;
                    }
                    else
                    {
                        netPayOrder.OrderStatusId = NetPayOrderStatus.支付失败;
                        netPayOrder.ErrorCode = result.ErrorMessage;
                    }
                    netPayOrder.OrderStatusName = netPayOrder.OrderStatusId.ToString();
                    netPayOrder.Mtime = DateTime.Now;

                    await uow.CompleteAsync();

                    var output = new PayOutput();
                    output.Success = result.IsPaid;
                    output.IsPaying = result.IsPaying;
                    output.Message = result.ErrorMessage;

                    return output;
                }
            }
        }

        public async Task<string> NativePayAsync(NativePayInput input)
        {
            var netPayOrder = await _netPayOrderRepository.GetAll().AsNoTracking().FirstOrDefaultAsync(o => o.ListNo == input.ListNo);
            if (netPayOrder == null)
            {
                throw new UserFriendlyException($"订单{input.ListNo}不存在");
            }

            if (!netPayOrder.AllowPay())
            {
                throw new UserFriendlyException($"订单{netPayOrder.OrderStatusName}");
            }

            var payTypeId = input.PayTypeId;
            var payInput = netPayOrder.PayArgs.JsonToObject<NetPayInput>();
            payInput.ClientIp = _clientInfoProvider.ClientIpAddress;

            using (var uow = _unitOfWorkManager.Begin(TransactionScopeOption.RequiresNew))
            {
                var netPayType = netPayOrder.GetNativeNetPayType(payTypeId);
                await _netPayOrderRepository.SetPayTypeAsync(netPayOrder.Id, payTypeId, OnlinePayTradeType.NATIVE, netPayType, netPayType.ToString());

                var args = new QueryNetPayJobArgs();
                args.ListNo = netPayOrder.ListNo;
                args.PayTypeId = payTypeId;
                args.NetPayTypeId = netPayType;
                args.TradeType = OnlinePayTradeType.NATIVE;
                args.Attach = payInput.Attach;
                args.IntervalSecond = 15;
                await _backgroundJobAppService.EnqueueAsync<QueryNetPayJob>(args.ToJson(), delay: TimeSpan.FromSeconds(args.IntervalSecond));

                await uow.CompleteAsync();
            }

            var payService = _netPayServiceFactory.GetPayService(payTypeId);

            var qrcode = await payService.NativePayAsync(payInput);

            return qrcode;
        }

        public async Task<string> H5PayAsync(H5PayInput input)
        {
            var netPayOrder = await _netPayOrderRepository.GetAll().AsNoTracking().FirstOrDefaultAsync(o => o.ListNo == input.ListNo);
            if (netPayOrder == null)
            {
                throw new UserFriendlyException($"订单{input.ListNo}不存在");
            }

            if (!netPayOrder.AllowPay())
            {
                throw new UserFriendlyException($"订单{netPayOrder.OrderStatusName}");
            }

            var payTypeId = input.PayTypeId;
            var payInput = netPayOrder.PayArgs.JsonToObject<NetPayInput>();
            payInput.ReturnUrl = input.ReturnUrl;
            payInput.WapName = input.WapName;
            payInput.ClientIp = _clientInfoProvider.ClientIpAddress;

            using (var uow = _unitOfWorkManager.Begin(TransactionScopeOption.RequiresNew))
            {
                var netPayType = netPayOrder.GetNativeNetPayType(payTypeId);
                await _netPayOrderRepository.SetPayTypeAsync(netPayOrder.Id, payTypeId, OnlinePayTradeType.MWEB, netPayType, netPayType.ToString());
                await _netPayOrderRepository.SetPayArgsAsync(netPayOrder.Id, payInput.ToJson());

                var args = new QueryNetPayJobArgs();
                args.ListNo = netPayOrder.ListNo;
                args.PayTypeId = payTypeId;
                args.NetPayTypeId = netPayType;
                args.TradeType = OnlinePayTradeType.MWEB;
                args.Attach = payInput.Attach;
                args.IntervalSecond = 15;

                await _backgroundJobAppService.EnqueueAsync<QueryNetPayJob>(args.ToJson(), delay: TimeSpan.FromSeconds(args.IntervalSecond));

                await uow.CompleteAsync();
            }

            var payService = _netPayServiceFactory.GetPayService(payTypeId);

            var result = await payService.H5PayAsync(payInput);

            return result;
        }

        public async Task<PayOutput> CashPayAsync(string listNo)
        {
            var netPayOrder = await _netPayOrderRepository.FirstOrDefaultAsync(o => o.ListNo == listNo);
            if (netPayOrder == null)
            {
                throw new UserFriendlyException($"订单{listNo}不存在");
            }

            if (!netPayOrder.AllowPay())
            {
                throw new UserFriendlyException($"订单{netPayOrder.OrderStatusName}");
            }

            var payTypeId = DefaultPayType.现金;
            var netPayInput = netPayOrder.PayArgs.JsonToObject<NetPayInput>();

            netPayOrder.PayTypeId = payTypeId;
            netPayOrder.NetPayTypeId = null;
            netPayOrder.NetPayTypeName = null;
            netPayOrder.OrderStatusId = NetPayOrderStatus.支付成功;
            netPayOrder.OrderStatusName = netPayOrder.OrderStatusId.ToString();
            netPayOrder.PayTime = DateTime.Now;
            netPayOrder.ClearPayArgs();
            netPayOrder.ErrorCode = null;
            netPayOrder.Mtime = DateTime.Now;

            var eventData = new PaySuccessEventData();
            eventData.ListNo = netPayOrder.ListNo;
            eventData.PayTypeId = payTypeId;
            eventData.Attach = netPayInput.Attach;
            await _eventBus.TriggerAsync(eventData);

            return new PayOutput { Success = true };
        }

        public async Task LoopQueryNetPayAsync(QueryNetPayJobArgs input)
        {
            using (var locker = await _lockFactory.LockAsync(ListNo.GetLockKey(input.ListNo)))
            {
                using (var uow = _unitOfWorkManager.Begin(TransactionScopeOption.RequiresNew))
                {
                    if (!locker.IsAcquired)
                    {
                        await _backgroundJobAppService.EnqueueAsync<QueryNetPayJob>(input.ToJson(), delay: TimeSpan.FromSeconds(input.IntervalSecond));

                        await uow.CompleteAsync();

                        return;
                    }

                    var netPayOrder = await _netPayOrderRepository.FirstOrDefaultAsync(o => o.ListNo == input.ListNo);
                    if (netPayOrder == null)
                    {
                        throw new TmsException($"订单{input.ListNo}不存在");
                    }

                    if (!netPayOrder.IsPayResultUnknown())
                    {
                        return;
                    }

                    var queryInput = new QueryPayInput();
                    queryInput.ListNo = netPayOrder.ListNo;
                    queryInput.TransactionId = netPayOrder.TransactionId;
                    queryInput.SubPayTypeId = netPayOrder.SubPayTypeId;
                    queryInput.TradeType = input.TradeType;
                    queryInput.PayTime = netPayOrder.Ctime;
                    var payService = _netPayServiceFactory.GetPayService(input.PayTypeId);
                    var queryResult = await payService.QueryPayAsync(queryInput);
                    if (queryResult.IsPaid)
                    {
                        netPayOrder.PayTypeId = input.PayTypeId;
                        netPayOrder.NetPayTypeId = input.NetPayTypeId;
                        netPayOrder.NetPayTypeName = netPayOrder.NetPayTypeId?.ToString();
                        netPayOrder.TransactionId = queryResult.TransactionId;
                        netPayOrder.SubTransactionId = queryResult.SubTransactionId;
                        netPayOrder.OrderStatusId = NetPayOrderStatus.支付成功;
                        netPayOrder.PayTime = queryResult.PayTime;
                        netPayOrder.BankType = queryResult.BankType;
                        netPayOrder.ClearPayArgs();
                        netPayOrder.ErrorCode = null;

                        var eventData = new PaySuccessEventData();
                        eventData.ListNo = netPayOrder.ListNo;
                        eventData.PayTypeId = input.PayTypeId;
                        eventData.Attach = input.Attach;
                        await _eventBus.TriggerAsync(eventData);
                    }
                    else if (netPayOrder.Ctime.AddMinutes(_payOptions.WechatTimeOutOrderCancelTime) > DateTime.Now)
                    {
                        netPayOrder.OrderStatusId = NetPayOrderStatus.未支付;

                        await _backgroundJobAppService.EnqueueAsync<QueryNetPayJob>(input.ToJson(), delay: TimeSpan.FromSeconds(input.IntervalSecond));
                    }
                    else
                    {
                        netPayOrder.OrderStatusId = NetPayOrderStatus.支付失败;
                        netPayOrder.ErrorCode = queryResult.ErrorMessage;
                    }
                    netPayOrder.OrderStatusName = netPayOrder.OrderStatusId.ToString();
                    netPayOrder.Mtime = DateTime.Now;

                    await uow.CompleteAsync();
                }
            }
        }

        public async Task ConfirmPayStatusAsync(ConfirmPayStatusJobArgs input)
        {
            using (var locker = await _lockFactory.LockAsync(ListNo.GetLockKey(input.ListNo)))
            {
                if (!locker.IsAcquired)
                {
                    throw new TmsException($"订单{input.ListNo}锁定失败");
                }

                using (var uow = _unitOfWorkManager.Begin(TransactionScopeOption.RequiresNew))
                {
                    var netPayOrder = await _netPayOrderRepository.FirstOrDefaultAsync(o => o.ListNo == input.ListNo);
                    if (netPayOrder == null)
                    {
                        throw new TmsException($"订单{input.ListNo}不存在");
                    }

                    if (netPayOrder.OrderStatusId.IsIn(NetPayOrderStatus.支付成功, NetPayOrderStatus.已关闭))
                    {
                        return;
                    }

                    if (!netPayOrder.IsNetPay() || netPayOrder.OrderStatusId == NetPayOrderStatus.支付失败)
                    {
                        var eventData = new PayTimeoutEventData();
                        eventData.ListNo = netPayOrder.ListNo;
                        eventData.Attach = input.Attach;
                        await _eventBus.TriggerAsync(eventData);

                        await uow.CompleteAsync();

                        return;
                    }

                    var queryInput = new QueryPayInput();
                    queryInput.ListNo = netPayOrder.ListNo;
                    queryInput.TransactionId = netPayOrder.TransactionId;
                    queryInput.SubPayTypeId = netPayOrder.SubPayTypeId;
                    queryInput.TradeType = netPayOrder.OnlinePayTradeType.Value;
                    queryInput.PayTime = netPayOrder.Ctime;
                    var payService = _netPayServiceFactory.GetPayService(netPayOrder.PayTypeId.Value);
                    var queryResult = await payService.QueryPayAsync(queryInput);
                    if (queryResult.IsPaid)
                    {
                        netPayOrder.TransactionId = queryResult.TransactionId;
                        netPayOrder.SubTransactionId = queryResult.SubTransactionId;
                        netPayOrder.OrderStatusId = NetPayOrderStatus.支付成功;
                        netPayOrder.OrderStatusName = netPayOrder.OrderStatusId.ToString();
                        netPayOrder.PayTime = queryResult.PayTime;
                        netPayOrder.BankType = queryResult.BankType;
                        netPayOrder.ClearPayArgs();
                        netPayOrder.ErrorCode = null;
                        netPayOrder.Mtime = DateTime.Now;

                        var eventData = new PaySuccessEventData();
                        eventData.ListNo = input.ListNo;
                        eventData.PayTypeId = netPayOrder.PayTypeId.Value;
                        eventData.Attach = input.Attach;
                        await _eventBus.TriggerAsync(eventData);
                    }
                    else if (queryResult.IsPaying)
                    {
                        netPayOrder.OrderStatusId = NetPayOrderStatus.未支付;
                    }
                    else
                    {
                        netPayOrder.ErrorCode = queryResult.ErrorMessage;

                        var eventData = new PayTimeoutEventData();
                        eventData.ListNo = netPayOrder.ListNo;
                        eventData.Attach = input.Attach; ;
                        await _eventBus.TriggerAsync(eventData);
                    }

                    await uow.CompleteAsync();
                }
            }
        }

        public async Task ClosePayAsync(string listNo)
        {
            var netPayOrder = await _netPayOrderRepository.FirstOrDefaultAsync(o => o.ListNo == listNo);
            if (netPayOrder == null)
            {
                throw new UserFriendlyException($"订单{listNo}不存在");
            }

            netPayOrder.Close();

            if (!netPayOrder.IsNetPay()) return;

            await CloseNetPayAsync(netPayOrder);
        }

        private async Task CloseNetPayAsync(NetPayOrder netPayOrder)
        {
            var payService = _netPayServiceFactory.GetPayService(netPayOrder.PayTypeId.Value);

            if (netPayOrder.IsMicroPay())
            {
                var reverseInput = new ReversePayInput();
                reverseInput.ListNo = netPayOrder.ListNo;
                reverseInput.TransactionId = netPayOrder.TransactionId;
                reverseInput.SubPayTypeId = netPayOrder.SubPayTypeId;
                reverseInput.PayTime = netPayOrder.Ctime;

                var reverseResult = await payService.ReversePayAsync(reverseInput);
                if (reverseResult.ShouldRetry)
                {
                    throw new RetryJobException(reverseResult.ErrorMessage);
                }
            }
            else
            {
                var closeInput = new ClosePayInput();
                closeInput.ListNo = netPayOrder.ListNo;
                closeInput.TransactionId = netPayOrder.TransactionId;
                closeInput.PayTime = netPayOrder.Ctime;

                var closeResult = await payService.ClosePayAsync(closeInput);
                if (!closeResult.Success && closeResult.IsPaid)
                {
                    throw new RetryJobException($"订单{netPayOrder.ListNo}关闭失败，订单已支付");
                }
            }
        }

        public async Task<NotifyOutput> HandlePayNotifyAsync(string data, int payTypeId)
        {
            var payService = _netPayServiceFactory.GetPayService(payTypeId);
            NetPayInput netPayInput = null;

            try
            {
                var input = payService.Notify(data);

                using (var locker = await _lockFactory.LockAsync(ListNo.GetLockKey(input.ListNo)))
                {
                    if (!locker.IsAcquired)
                    {
                        throw new TmsException($"订单{input.ListNo}锁定失败");
                    }

                    using (var uow = _unitOfWorkManager.Begin(TransactionScopeOption.RequiresNew))
                    {
                        var netPayOrder = await _netPayOrderRepository.FirstOrDefaultAsync(o => o.ListNo == input.ListNo);
                        netPayInput = netPayOrder.PayArgs.JsonToObject<NetPayInput>();
                        netPayInput.TradeType = netPayOrder.OnlinePayTradeType.Value;

                        if (netPayOrder?.OrderStatusId == NetPayOrderStatus.支付成功)
                        {
                            return payService.GenerateNotifyResponse(true, input: netPayInput);
                        }

                        if (ValidatePayNotify(input, netPayOrder, out string message))
                        {
                            netPayOrder.PayTypeId = payTypeId;
                            netPayOrder.TransactionId = input.TransactionId;
                            netPayOrder.SubTransactionId = input.SubTransactionId;
                            netPayOrder.OrderStatusId = NetPayOrderStatus.支付成功;
                            netPayOrder.OrderStatusName = netPayOrder.OrderStatusId.ToString();
                            netPayOrder.PayTime = input.PayTime;
                            netPayOrder.BankType = input.BankType;
                            netPayOrder.ClearPayArgs();
                            netPayOrder.ErrorCode = null;
                            netPayOrder.Mtime = DateTime.Now;

                            var eventData = new PaySuccessEventData();
                            eventData.ListNo = input.ListNo;
                            eventData.PayTypeId = payTypeId;
                            eventData.Attach = netPayInput.Attach;
                            await _eventBus.TriggerAsync(eventData);
                        }
                        else if (input.PaySuccess)
                        {
                            var refundMoneyApply = new RefundMoneyApply();
                            refundMoneyApply.RefundListNo = await _dynamicCodeService.GenerateListNoAsync(ListNoType.门票网上订票);
                            refundMoneyApply.PayListNo = input.ListNo;
                            refundMoneyApply.RefundMoney = input.TotalFee;
                            refundMoneyApply.Reason = message;
                            await ApplyRefundAsync(refundMoneyApply);
                        }

                        await uow.CompleteAsync();
                    }

                    return payService.GenerateNotifyResponse(true, input: netPayInput);
                }
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);

                return payService.GenerateNotifyResponse(false, ex.Message, netPayInput);
            }
        }

        private bool ValidatePayNotify(NotifyInput input, NetPayOrder netPayOrder, out string message)
        {
            if (netPayOrder == null)
            {
                message = $"订单{input.ListNo}不存在";

                return false;
            }
            if (netPayOrder.OrderStatusId == NetPayOrderStatus.已关闭)
            {
                message = $"订单{input.ListNo}已取消";

                return false;
            }
            if (!input.PaySuccess)
            {
                message = "支付失败";

                return false;
            }
            if (netPayOrder.TotalFee != input.TotalFee)
            {
                message = $"支付金额不正确，应付{netPayOrder.TotalFee}，实付{input.TotalFee}";

                return false;
            }

            message = "成功";
            return true;
        }

        public async Task ApplyRefundAsync(RefundMoneyApply refundMoneyApply)
        {
            var id = await _refundMoneyApplyRepository.InsertAndGetIdAsync(refundMoneyApply);

            await _backgroundJobAppService.EnqueueAsync<RefundMoneyJob>(id.ToString());
        }

        public async Task RefundAsync(long id)
        {
            var refundMoneyApply = await _refundMoneyApplyRepository.FirstOrDefaultAsync(id);

            var netPayOrder = await _netPayOrderRepository.FirstOrDefaultAsync(o => o.ListNo == refundMoneyApply.PayListNo);
            if (!netPayOrder.PayTypeId.HasValue)
            {
                netPayOrder.PayTypeId = await GetPayTypeIdAsync(netPayOrder.ListNo);
            }

            try
            {
                var payService = _netPayServiceFactory.GetPayService(netPayOrder.PayTypeId.Value);

                if (refundMoneyApply.ApplySuccess)
                {
                    var queryInput = new QueryRefundInput();
                    queryInput.ListNo = netPayOrder.ListNo;
                    queryInput.RefundListNo = refundMoneyApply.RefundListNo;
                    queryInput.TransactionId = netPayOrder.TransactionId;
                    queryInput.RefundId = refundMoneyApply.RefundId;
                    queryInput.SubPayTypeId = netPayOrder.SubPayTypeId;
                    queryInput.TradeType = netPayOrder.OnlinePayTradeType.Value;
                    queryInput.RefundFee = refundMoneyApply.RefundMoney;
                    queryInput.PayTime = netPayOrder.Ctime;

                    var result = await payService.QueryRefundAsync(queryInput);

                    using (var uow = _unitOfWorkManager.Begin(TransactionScopeOption.RequiresNew))
                    {
                        refundMoneyApply = await _refundMoneyApplyRepository.FirstOrDefaultAsync(id);
                        if (result.Success)
                        {
                            refundMoneyApply.Status = RefundApplyStatus.退款成功;
                            refundMoneyApply.RefundRecvAccount = result.RefundRecvAccount;
                            refundMoneyApply.RefundSuccessTime = result.RefundTime;
                            if (!result.RefundId.IsNullOrEmpty())
                            {
                                refundMoneyApply.RefundId = result.RefundId;
                            }
                        }
                        else
                        {
                            refundMoneyApply.Status = result.ShouldRetry ? RefundApplyStatus.退款中 : RefundApplyStatus.退款失败;
                        }
                        refundMoneyApply.ResultMessage = result.ErrorMessage;
                        refundMoneyApply.HandleTime = DateTime.Now;

                        await uow.CompleteAsync();
                    }

                    if (refundMoneyApply.Status == RefundApplyStatus.退款中)
                    {
                        throw new RetryJobException(result.ErrorMessage);
                    }
                }
                else
                {
                    var refundInput = new RefundInput();
                    refundInput.ListNo = netPayOrder.ListNo;
                    refundInput.RefundListNo = refundMoneyApply.RefundListNo;
                    refundInput.TransactionId = netPayOrder.TransactionId;
                    refundInput.SubPayTypeId = netPayOrder.SubPayTypeId;
                    refundInput.TradeType = netPayOrder.OnlinePayTradeType.Value;
                    refundInput.TotalFee = netPayOrder.TotalFee;
                    refundInput.RefundFee = refundMoneyApply.RefundMoney;
                    refundInput.PayTime = netPayOrder.Ctime;

                    var result = await payService.RefundAsync(refundInput);

                    using (var uow = _unitOfWorkManager.Begin(TransactionScopeOption.RequiresNew))
                    {
                        refundMoneyApply = await _refundMoneyApplyRepository.FirstOrDefaultAsync(id);
                        if (result.Success)
                        {
                            refundMoneyApply.Status = RefundApplyStatus.退款中;
                            refundMoneyApply.ApplySuccess = true;
                            refundMoneyApply.ApplySuccessTime = DateTime.Now;
                            refundMoneyApply.RefundId = result.RefundId;
                        }
                        else
                        {
                            refundMoneyApply.Status = result.ShouldRetry ? RefundApplyStatus.退款中 : RefundApplyStatus.退款失败;
                        }
                        refundMoneyApply.ResultMessage = result.ErrorMessage;
                        refundMoneyApply.HandleTime = DateTime.Now;

                        await uow.CompleteAsync();
                    }

                    if (refundMoneyApply.Status == RefundApplyStatus.退款中)
                    {
                        throw new RetryJobException(result.ErrorMessage);
                    }
                }
            }
            catch (UnSupportedPayTypeException)
            {
                await RefundFailAsync(netPayOrder, refundMoneyApply);
            }
        }

        private async Task<int> GetPayTypeIdAsync(string listNo)
        {
            var payTypeId = await _tradeRepository.GetAll()
                .Where(t => t.ListNo == listNo)
                .Select(t => t.PayTypeId)
                .FirstOrDefaultAsync();

            return payTypeId.Value;
        }

        private async Task RefundFailAsync(NetPayOrder netPayOrder, RefundMoneyApply refundMoneyApply)
        {
            var trade = await _tradeRepository.GetAll()
                .Where(t => t.ListNo == refundMoneyApply.RefundListNo)
                .Select(t => new { t.SalePointId, t.CashierId, t.CashPcid })
                .FirstOrDefaultAsync();

            var employeeNo = await _staffRepository.GetAll()
                .Where(s => s.Id == trade.CashierId)
                .Select(s => s.EmployeeNo)
                .FirstOrDefaultAsync();

            var refundFail = new NetPayRefundFail();
            refundFail.NetPayTypeId = netPayOrder.NetPayTypeId;
            refundFail.NetPayTypeName = netPayOrder.NetPayTypeName;
            refundFail.ListNo = netPayOrder.ListNo;
            refundFail.RefundListNo = refundMoneyApply.RefundListNo;
            refundFail.TotalFee = netPayOrder.TotalFee.ToString();
            refundFail.RefundFee = refundMoneyApply.RefundMoney.ToString();
            refundFail.StatusId = 2;
            refundFail.StatusName = "退款失败";
            refundFail.SalePointId = trade.SalePointId;
            refundFail.CashPcId = trade.CashPcid;
            refundFail.EmployeeNo = employeeNo;
            refundFail.Memo = "WebApi不支持该付款方式";

            await _netPayRefundFailRepository.InsertAsync(refundFail);
        }
    }
}
