using Egoal.Application.Services;
using Egoal.BackgroundJobs;
using Egoal.Caches;
using Egoal.Common;
using Egoal.Cryptography;
using Egoal.Domain.Repositories;
using Egoal.Domain.Uow;
using Egoal.DynamicCodes;
using Egoal.Extensions;
using Egoal.Members;
using Egoal.Orders.Dto;
using Egoal.Payment;
using Egoal.Payment.Dto;
using Egoal.Runtime.Session;
using Egoal.Scenics.Dto;
using Egoal.Settings;
using Egoal.Stadiums;
using Egoal.Threading.Lock;
using Egoal.Tickets;
using Egoal.Tickets.Dto;
using Egoal.TicketTypes;
using Egoal.Trades;
using Egoal.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace Egoal.Orders
{
    public class OrderAppService : ApplicationService, IOrderAppService
    {
        private readonly ISession _session;
        private readonly IHostingEnvironment _environment;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IDistributedLockFactory _lockFactory;
        private readonly INameCacheService _nameCacheService;
        private readonly ITicketSaleRepository _ticketSaleRepository;
        private readonly IDynamicCodeService _dynamicCodeService;
        private readonly ITicketSaleAppService _ticketSaleAppService;
        private readonly IOrderDomainService _orderDomainService;
        private readonly IMemberAppService _memberAppService;
        private readonly IOrderRepository _orderRepository;
        private readonly IPayAppService _payAppService;
        private readonly IRepository<OrderDetail, long> _orderDetailRepository;
        private readonly IRepository<RefundOrderApply, long> _refundOrderApplyRepository;
        private readonly IBackgroundJobService _backgroundJobAppService;
        private readonly ISeatStatusCacheRepository _seatStatusCacheRepository;
        private readonly IRepository<OrderGroundChangCi, long> _orderGroundChangCiRepository;
        private readonly IRepository<OrderTourist, long> _orderTouristRepository;
        private readonly IRepository<TicketType> _ticketTypeRepository;
        private readonly ICommonAppService _commonAppService;
        private readonly ITicketSaleQueryAppService _ticketSaleQueryAppService;
        private readonly ISettingAppService _settingAppService;
        private readonly OrderBuilder _orderBuilder;

        public OrderAppService(
            ISession session,
            IHostingEnvironment environment,
            IUnitOfWorkManager unitOfWorkManager,
            IDistributedLockFactory lockFactory,
            INameCacheService nameCacheService,
            ITicketSaleRepository ticketSaleRepository,
            IDynamicCodeService dynamicCodeService,
            ITicketSaleAppService ticketSaleAppService,
            IOrderDomainService orderDomainService,
            IMemberAppService memberAppService,
            IOrderRepository orderRepository,
            IPayAppService payAppService,
            IRepository<OrderDetail, long> orderDetailRepository,
            IRepository<RefundOrderApply, long> refundOrderApplyRepository,
            IBackgroundJobService backgroundJobAppService,
            ISeatStatusCacheRepository seatStatusCacheRepository,
            IRepository<OrderGroundChangCi, long> orderGroundChangCiRepository,
            IRepository<OrderTourist, long> orderTouristRepository,
            IRepository<TicketType> ticketTypeRepository,
            ICommonAppService commonAppService,
            ITicketSaleQueryAppService ticketSaleQueryAppService,
            ISettingAppService settingAppService,
            OrderBuilder orderBuilder)
        {
            _session = session;
            _environment = environment;
            _unitOfWorkManager = unitOfWorkManager;
            _lockFactory = lockFactory;
            _nameCacheService = nameCacheService;
            _ticketSaleRepository = ticketSaleRepository;
            _dynamicCodeService = dynamicCodeService;
            _ticketSaleAppService = ticketSaleAppService;
            _orderDomainService = orderDomainService;
            _memberAppService = memberAppService;
            _orderRepository = orderRepository;
            _payAppService = payAppService;
            _orderDetailRepository = orderDetailRepository;
            _refundOrderApplyRepository = refundOrderApplyRepository;
            _backgroundJobAppService = backgroundJobAppService;
            _seatStatusCacheRepository = seatStatusCacheRepository;
            _orderGroundChangCiRepository = orderGroundChangCiRepository;
            _orderTouristRepository = orderTouristRepository;
            _ticketTypeRepository = ticketTypeRepository;
            _commonAppService = commonAppService;
            _ticketSaleQueryAppService = ticketSaleQueryAppService;
            _settingAppService = settingAppService;
            _orderBuilder = orderBuilder;
        }

        public async Task<CreateOrderOutput> CreateOrderAsync(CreateOrderInput input, SaleChannel saleChannel, OrderType orderType)
        {
            var order = await _orderBuilder.BuildOrderAsync(input, saleChannel, orderType);

            await _orderDomainService.CreateAsync(order);

            var output = new CreateOrderOutput();
            foreach (OrderDetail detail in order.OrderDetails)
            {
                if (detail.TicketType.FirstActiveFlag == true)
                {
                    output.FirstActiveFlag = true;
                    break;
                }
            }
            if (order.IsFree())
            {
                await _unitOfWorkManager.Current.SaveChangesAsync();

                await PayOrderAsync(order, DefaultPayType.现金);

                output.ShouldPay = false;

                await SendBookSuccessAsync(order);
            }
            else
            {
                await PrePayAsync(order);
            }

            output.ListNo = order.Id;

            return output;
        }

        public async Task<string> CreateGroupOrderAsync(CreateGroupOrderInput input)
        {
            var order = await _orderBuilder.BuildGroupOrderAsync(input);

            await _orderDomainService.CreateAsync(order);

            await _unitOfWorkManager.Current.SaveChangesAsync();

            await SendBookSuccessAsync(order);

            return order.Id;
        }

        private async Task PrePayAsync(Order order)
        {
            var prePayInput = new PrePayInput();
            prePayInput.ListNo = order.Id;
            prePayInput.PayMoney = order.TotalMoney;
            prePayInput.ProductInfo = "门票";
            prePayInput.Attach = NetPayAttach.BuyTicket;
            prePayInput.OpenId = await _memberAppService.GetOpenId(order.MemberId);

            await _payAppService.PrePayAsync(prePayInput);
        }

        public async Task PayOrderAsync(string listNo, int payTypeId)
        {
            var order = await _orderRepository
                .GetAllIncluding(o => o.OrderDetails)
                .Where(o => o.Id == listNo)
                .FirstOrDefaultAsync();

            await PayOrderAsync(order, payTypeId);

            await SendBookSuccessAsync(order);
        }

        private async Task PayOrderAsync(Order order, int payTypeId)
        {
            if (order.PayFlag == true) return;

            order.Pay(payTypeId, DefaultPayType.GetName(payTypeId));

            await ChangeTicketAsync(order);
        }

        private async Task ChangeTicketAsync(Order order)
        {
            var saleTicketInput = await BuildSaleTicketInputAsync(order);
            var ticketSales = await _ticketSaleAppService.SaleAsync(saleTicketInput);

            order.EndTime = ticketSales.Max(t => t.Etime.To<DateTime>());
        }

        private async Task<SaleTicketInput> BuildSaleTicketInputAsync(Order order)
        {
            var saleTicketInput = order.MapToSaleTicketInput();
            saleTicketInput.TravelDate = order.Etime.To<DateTime>();
            saleTicketInput.TradeTypeTypeId = TradeTypeType.门票;
            saleTicketInput.TradeTypeId = order.OrderTypeId == OrderType.微信订票 ? DefaultTradeType.门票_微信 : DefaultTradeType.门票;
            saleTicketInput.TradeTypeName = DefaultTradeType.GetName(saleTicketInput.TradeTypeId.Value);
            saleTicketInput.CashierId = order.CashierId;
            saleTicketInput.CashierName = _nameCacheService.GetStaffName(saleTicketInput.CashierId);
            saleTicketInput.CashPcid = order.CashPcId;
            saleTicketInput.CashPcname = _nameCacheService.GetPcName(saleTicketInput.CashPcid);
            saleTicketInput.SalePointId = order.SalePointId;
            saleTicketInput.SalePointName = _nameCacheService.GetSalePointName(saleTicketInput.SalePointId);
            saleTicketInput.ParkId = order.ParkId;
            saleTicketInput.ParkName = _nameCacheService.GetParkName(saleTicketInput.ParkId);
            saleTicketInput.IsExchange = true;

            if (order.OrderDetails.Any(o => o.HasGroundSeat))
            {
                saleTicketInput.Seats = await _seatStatusCacheRepository.GetOrderSeatsAsync(order.Id);
            }

            foreach (var orderDetail in order.OrderDetails)
            {
                var saleTicketItem = new SaleTicketItem();
                saleTicketItem.TicketTypeId = orderDetail.TicketTypeId.Value;
                saleTicketItem.Quantity = orderDetail.TotalNum;
                saleTicketItem.TicPrice = orderDetail.TicPrice.Value;
                saleTicketItem.RealPrice = orderDetail.ReaPrice.Value;
                saleTicketItem.OrderDetailId = orderDetail.Id;

                saleTicketItem.HasGroundSeat = orderDetail.HasGroundSeat;
                if (orderDetail.HasGroundSeat || orderDetail.HasGroundChangCi)
                {
                    saleTicketItem.GroundChangCis = await _orderGroundChangCiRepository.GetAll()
                        .Where(o => o.OrderDetailId == orderDetail.Id)
                        .Select(o => new GroundChangCiDto
                        {
                            GroundId = o.GroundId,
                            ChangCiId = o.ChangCiId
                        })
                        .ToListAsync();
                }

                saleTicketItem.Tourists = await _orderTouristRepository.GetAll()
                    .AsNoTracking()
                    .Where(o => o.OrderDetailId == orderDetail.Id)
                    .Select(o => new TicketTourist
                    {
                        Name = o.Name,
                        Mobile = o.Mobile,
                        CertType = o.CertType,
                        CertNo = o.CertNo
                    })
                    .ToListAsync();

                saleTicketInput.Items.Add(saleTicketItem);
            }

            return saleTicketInput;
        }

        public async Task ChangeChangCiAsync(ChangeChangCiInput input)
        {
            var order = await _orderRepository.FirstOrDefaultAsync(input.ListNo);
            UserFriendlyCheck.NotNull(order, $"订单{input.ListNo}不存在");

            await _orderDomainService.ChangeChangCiAsync(order, input.ChangCiId);

            var endTime = await _ticketSaleAppService.ChangeChangCiAsync(input);
            if (endTime.HasValue)
            {
                order.EndTime = endTime;
            }
        }

        public async Task ChangeQuantityAsync(ChangeQuantityInput input)
        {
            var order = await _orderRepository.FirstOrDefaultAsync(input.ListNo);
            UserFriendlyCheck.NotNull(order, $"订单{input.ListNo}不存在");

            await _orderDomainService.ChangeQuantityAsync(order, input.Quantity);
        }

        public async Task CancelByUserAsync(CancelOrderInput input)
        {
            using (var locker = await _lockFactory.LockAsync(ListNo.GetLockKey(input.ListNo)))
            {
                if (!locker.IsAcquired)
                {
                    throw new UserFriendlyException($"订单：{input.ListNo}锁定失败");
                }

                using (var uow = _unitOfWorkManager.Begin(TransactionScopeOption.RequiresNew))
                {
                    await CancelOrderAsync(input);

                    await uow.CompleteAsync();
                }
            }
        }

        public async Task CancelOrderAsync(CancelOrderInput input)
        {
            var order = await _orderRepository.GetAllIncluding(o => o.OrderDetails).Where(o => o.Id == input.ListNo).FirstOrDefaultAsync();
            if (order == null)
            {
                throw new UserFriendlyException($"订单{input.ListNo}不存在");
            }

            if (order.OrderStatusId == OrderStatus.Canceled)
            {
                return;
            }

            if (order.OrderStatusId == OrderStatus.Completed)
            {
                throw new UserFriendlyException("订单已完成不能取消");
            }

            if (_session.MemberId.HasValue && order.MemberId != _session.MemberId)
            {
                throw new UserFriendlyException("只能取消自己的订单");
            }

            if (_session.GuiderId.HasValue && order.GuiderId != null && order.GuiderId != _session.GuiderId)
            {
                throw new UserFriendlyException("只能取消自己的订单");
            }

            if (order.HasPaid())
            {
                throw new UserFriendlyException("订单已付款，请申请退款");
            }

            var ticketSales = await _ticketSaleRepository.GetAll()
                .AsNoTracking()
                .Where(t => t.OrderListNo == input.ListNo)
                .ToListAsync();
            if (ticketSales.IsNullOrEmpty())
            {
                await _orderDomainService.CancelAsync(order);

                return;
            }

            foreach (var ticketSale in ticketSales)
            {
                ticketSale.ValidateCancelOrder();
            }

            var refundTicketInput = new RefundTicketInput();
            refundTicketInput.OriginalTradeId = ticketSales.FirstOrDefault().TradeId;
            refundTicketInput.RefundListNo = await _dynamicCodeService.GenerateListNoAsync(ListNoType.门票网上订票);
            refundTicketInput.PayListNo = order.Id;
            refundTicketInput.PayTypeId = order.PayTypeId.Value;
            refundTicketInput.RefundReason = "取消订单";
            refundTicketInput.CashierId = _session.StaffId;
            refundTicketInput.CashierName = _nameCacheService.GetStaffName(_session.StaffId);
            refundTicketInput.CashPcid = _session.PcId;
            refundTicketInput.CashPcname = _nameCacheService.GetPcName(_session.PcId);
            refundTicketInput.SalePointId = _session.SalePointId;
            refundTicketInput.SalePointName = _nameCacheService.GetSalePointName(_session.SalePointId);
            refundTicketInput.ParkId = _session.ParkId;
            refundTicketInput.ParkName = _nameCacheService.GetParkName(_session.ParkId);
            foreach (var ticketSale in ticketSales)
            {
                RefundTicketItem refundTicketItem = new RefundTicketItem();
                refundTicketItem.TicketId = ticketSale.Id;
                refundTicketItem.RefundQuantity = ticketSale.PersonNum.Value;
                refundTicketItem.SurplusQuantityAfterRefund = 0;

                refundTicketInput.Items.Add(refundTicketItem);
            }

            await _ticketSaleAppService.RefundAsync(refundTicketInput);
        }

        public async Task ApplyInvoiceAsync(InvoiceInput input)
        {
            var order = await _orderRepository.FirstOrDefaultAsync(input.ListNo);
            UserFriendlyCheck.NotNull(order, $"订单{input.ListNo}不存在");

            _orderDomainService.ApplyInvoice(order);

            await _backgroundJobAppService.EnqueueAsync<InvoiceJob>(input.ToJson());
        }

        public async Task ApplyRefundAsync(RefundOrderInput input)
        {
            decimal refundMoney = 0;

            foreach (var detail in input.Details)
            {
                var orderDetail = await _orderDetailRepository.GetAll().AsNoTracking().FirstOrDefaultAsync(o => o.Id == detail.Id);
                if (orderDetail.SurplusNum < detail.RefundQuantity)
                {
                    throw new UserFriendlyException("可退票数不足");
                }

                refundMoney += orderDetail.ReaPrice.Value * detail.RefundQuantity;
            }

            var order = await _orderRepository.FirstOrDefaultAsync(input.ListNo);
            order.RefundStatus = RefundStatus.退款中;

            var refundApply = new RefundOrderApply();
            refundApply.RefundListNo = await _dynamicCodeService.GenerateListNoAsync(ListNoType.门票网上订票);
            refundApply.ListNo = input.ListNo;
            refundApply.RefundQuantity = input.Details.Sum(d => d.RefundQuantity);
            refundApply.RefundMoney = refundMoney;
            refundApply.Details = input.Details.ToJson();
            refundApply.Reason = input.Reason;
            refundApply.CashierId = _session.StaffId;
            refundApply.CashPcid = _session.PcId;
            refundApply.SalePointId = _session.SalePointId;
            refundApply.ParkId = _session.ParkId;
            var id = await _refundOrderApplyRepository.InsertAndGetIdAsync(refundApply);

            var delay = _environment.IsDevelopment() ? 1 : RandomHelper.CreateRandomNumber(20, 300);
            await _backgroundJobAppService.EnqueueAsync<RefundOrderJob>(id.ToString(), delay: TimeSpan.FromSeconds(delay));
        }

        /// <summary>
        /// 下单成功后通知
        /// </summary>
        /// <param name="listNo"></param>
        /// <returns></returns>
        private async Task SendBookSuccessAsync(Order order)
        {
            if (order != null && order.OrderTypeId == OrderType.网上订票)
            {
                //成功后发送短信
                DateTime eDateTime = DateTime.Now;
                string eTime = order.Etime;
                if (DateTime.TryParse(order.Etime, out eDateTime))
                {
                    eTime = eDateTime.ToString("MM月dd日");
                }
                if (order.GuiderId == null)
                {
                    var tickets = await _ticketSaleQueryAppService.GetOrderTicketSalesAsync(order.Id);
                    int i = 1;
                    string qrCodeString = "";
                    bool firstActiveFlag = false;
                    string webSaleHostUrl = await _settingAppService.GetSysPara("WebSaleHostUrl");
                    foreach (OrderDetail detail in order.OrderDetails)
                    {
                        foreach (TicketSaleSimpleDto ticketSaleSimpleDto in tickets.Where(t => t.OrderDetailId == detail.Id))
                        {
                            qrCodeString += $"，二维码{i++}：{webSaleHostUrl}/Order/TicketCodeQR/{ticketSaleSimpleDto.TicketCode}";
                        }
                        TicketType ticketType = _ticketTypeRepository.GetAll().FirstOrDefault(t => t.Id == detail.TicketTypeId);
                        if (ticketType != null && ticketType.FirstActiveFlag == true)
                        {
                            firstActiveFlag = true;
                        }
                    }

                    await _commonAppService.SendPersonalBookSuccessAsync(order.Mobile, eTime, order.Id, _nameCacheService.GetChangCiTimeRange(order.ChangCiId), firstActiveFlag, qrCodeString);
                }
                else
                {
                    await _commonAppService.SendGroupBookSuccessAsync(order.Mobile, eTime, order.Id, _nameCacheService.GetChangCiTimeRange(order.ChangCiId), order.TotalNum);
                }
            }
        }
    }
}
