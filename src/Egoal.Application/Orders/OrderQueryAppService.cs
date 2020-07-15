using Egoal.Application.Services;
using Egoal.Application.Services.Dto;
using Egoal.Caches;
using Egoal.Common;
using Egoal.Common.Dto;
using Egoal.Cryptography;
using Egoal.Domain.Repositories;
using Egoal.Excel;
using Egoal.Extensions;
using Egoal.Localization;
using Egoal.Members;
using Egoal.Orders.Dto;
using Egoal.Payment;
using Egoal.Runtime.Session;
using Egoal.Scenics;
using Egoal.Stadiums;
using Egoal.Tickets;
using Egoal.Tickets.Dto;
using Egoal.TicketTypes;
using Egoal.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Egoal.Orders
{
    public class OrderQueryAppService : ApplicationService, IOrderQueryAppService
    {
        private readonly OrderOptions _orderOptions;
        private readonly ScenicOptions _scenicOptions;

        private readonly ISession _session;
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IOrderRepository _orderRepository;
        private readonly IRepository<OrderPlan> _orderPlanRepository;
        private readonly IRepository<OrderAgeRange, long> _orderAgeRangeRepository;
        private readonly IRepository<OrderGroundChangCi, long> _orderGroundChangCiRepository;
        private readonly IRepository<RefundOrderApply, long> _refundOrderApplyRepository;
        private readonly IRepository<RefundMoneyApply, long> _refundMoneyApplyRepository;
        private readonly IRepository<TicketType> _ticketTypeRepository;
        private readonly IRepository<ChangCi> _changCiRepository;
        private readonly IRepository<InvoiceInfo> _invoiceRepository;
        private readonly ISeatStatusCacheRepository _seatStatusCacheRepository;
        private readonly IOrderDomainService _orderDomainService;
        private readonly IRepository<OrderTourist, long> _orderTouristRepository;
        private readonly OrderBuilder _orderBuilder;

        private readonly ITicketSaleQueryAppService _ticketSaleQueryAppService;
        private readonly INameCacheService _nameCacheService;
        private readonly IPayAppService _payAppService;
        private readonly ICommonAppService _commonAppService;

        public OrderQueryAppService(
            ISession session,
            IStringLocalizer<SharedResource> localizer,
            IOptions<OrderOptions> orderOptions,
            IOptions<ScenicOptions> scenicOptions,
            IOrderRepository orderRepository,
            IRepository<OrderPlan> orderPlanRepository,
            IRepository<OrderAgeRange, long> orderAgeRangeRepository,
            IRepository<OrderGroundChangCi, long> orderGroundChangCiRepository,
            IRepository<RefundOrderApply, long> refundOrderApplyRepository,
            IRepository<RefundMoneyApply, long> refundMoneyApplyRepository,
            IRepository<TicketType> ticketTypeRepository,
            IRepository<ChangCi> changCiRepository,
            IRepository<InvoiceInfo> invoiceRepository,
            ISeatStatusCacheRepository seatStatusCacheRepository,
            IOrderDomainService orderDomainService,
            IRepository<OrderTourist, long> orderTouristRepository,
            OrderBuilder orderBuilder,
            ITicketSaleQueryAppService ticketSaleQueryAppService,
            INameCacheService nameCacheService,
            IPayAppService payAppService,
            ICommonAppService commonService)
        {
            _session = session;
            _localizer = localizer;
            _orderOptions = orderOptions.Value;
            _scenicOptions = scenicOptions.Value;

            _orderRepository = orderRepository;
            _orderPlanRepository = orderPlanRepository;
            _orderAgeRangeRepository = orderAgeRangeRepository;
            _orderGroundChangCiRepository = orderGroundChangCiRepository;
            _refundOrderApplyRepository = refundOrderApplyRepository;
            _refundMoneyApplyRepository = refundMoneyApplyRepository;
            _seatStatusCacheRepository = seatStatusCacheRepository;
            _ticketTypeRepository = ticketTypeRepository;
            _changCiRepository = changCiRepository;
            _invoiceRepository = invoiceRepository;

            _orderDomainService = orderDomainService;
            _orderTouristRepository = orderTouristRepository;
            _orderBuilder = orderBuilder;
            _ticketSaleQueryAppService = ticketSaleQueryAppService;
            _nameCacheService = nameCacheService;
            _payAppService = payAppService;
            _commonAppService = commonService;
        }

        public async Task<PagedResultDto<OrderSimpleListDto>> GetMemberOrdersForMobileAsync(GetMemberOrdersForMobileInput input)
        {
            var now = DateTime.Now;
            var query = _orderRepository.GetAll();
            if (_session.GuiderId.HasValue)
            {
                if (_session.MemberId.HasValue)
                {
                    query = query.Where(o => (o.MemberId == _session.MemberId.Value && o.GuiderId == null) || o.GuiderId == _session.GuiderId.Value);
                }
                else
                {
                    query = query.Where(o => o.GuiderId == _session.GuiderId.Value);
                }
            }
            else if (_session.MemberId.HasValue)
            {
                query = query.Where(o => o.MemberId == _session.MemberId.Value && o.GuiderId == null);
            }
            if (input.IsUsable == true)
            {
                query = query.Where(o => o.PayFlag == true && o.OrderStatusId == OrderStatus.Audited && o.EndTime >= now && o.SurplusNum > 0);
            }
            if (input.IsNotPaid == true)
            {
                query = query.Where(o => o.PayFlag == false && o.OrderStatusId == OrderStatus.Audited);
            }

            var count = await _orderRepository.CountAsync(query);

            query = query.OrderByDescending(o => o.CTime).PageBy(input);

            var orders = await _orderRepository.ToListAsync(query);

            var orderDtos = new List<OrderSimpleListDto>();
            foreach (var order in orders)
            {
                var orderDto = _orderBuilder.ToSimpleListDto(order);
                orderDtos.Add(orderDto);
            }

            return new PagedResultDto<OrderSimpleListDto>(count, orderDtos);
        }

        /// <summary>
        /// 网页端查询订单列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<OrderInfoDto>> GetMemberOrdersForQueryAsync(GetMemberOrdersForQueryInput input)
        {
            var now = DateTime.Now;
            var query = _orderRepository.GetAll();
            if (_session.GuiderId.HasValue)
            {
                if (_session.MemberId.HasValue)
                {
                    query = query.Where(o => (o.MemberId == _session.MemberId.Value && o.GuiderId == null) || o.GuiderId == _session.GuiderId.Value);
                }
                else
                {
                    query = query.Where(o => o.GuiderId == _session.GuiderId.Value);
                }
            }
            else if (_session.MemberId.HasValue)
            {
                query = query.Where(o => o.MemberId == _session.MemberId.Value && o.GuiderId == null);
            }
            query = query.WhereIf(input.CTimeFrom != null, o => o.CTime >= Convert.ToDateTime(Convert.ToDateTime(input.CTimeFrom).ToDateString())).
                WhereIf(input.CTimeTo != null, o => o.CTime < Convert.ToDateTime(Convert.ToDateTime(input.CTimeTo).AddDays(1).ToDateString())).
                WhereIf(!string.IsNullOrEmpty(input.ListNo), o => o.Id.Contains(input.ListNo)).
                WhereIf(input.EtimeFrom != null, o => Convert.ToDateTime(o.Etime) >= Convert.ToDateTime(Convert.ToDateTime(input.EtimeFrom).ToDateString())).
                WhereIf(input.EtimeTo != null, o => Convert.ToDateTime(o.Etime) < Convert.ToDateTime(Convert.ToDateTime(input.EtimeTo).AddDays(1).ToDateString())).
                WhereIf(!string.IsNullOrEmpty(input.OrderStatusName), o => _orderBuilder.GetOrderStatusName(o) == input.OrderStatusName);

            var count = await _orderRepository.CountAsync(query);

            query = query.OrderByDescending(o => o.CTime).PageBy(input);

            var orders = await _orderRepository.ToListAsync(query);
            List<OrderInfoDto> orderInfoDtos = new List<OrderInfoDto>();
            foreach (var order in orders)
            {
                GetOrderInfoInput getOrderInfoInput = new GetOrderInfoInput();
                getOrderInfoInput.ListNo = order.Id;
                OrderInfoDto orderInfoDto = new OrderInfoDto();
                orderInfoDto = await GetOrderQueryInfoAsync(getOrderInfoInput);
                orderInfoDtos.Add(orderInfoDto);
            }

            return new PagedResultDto<OrderInfoDto>(count, orderInfoDtos);
        }

        public async Task<PagedResultDto<OrderQueryDto>> GetMemberOrdersForQueryAsync2(GetMemberOrdersForQueryInput input)
        {
            var now = DateTime.Now;
            var query = _orderRepository.GetAll();
            if (_session.GuiderId.HasValue)
            {
                if (_session.MemberId.HasValue)
                {
                    query = query.Where(o => (o.MemberId == _session.MemberId.Value && o.GuiderId == null) || o.GuiderId == _session.GuiderId.Value);
                }
                else
                {
                    query = query.Where(o => o.GuiderId == _session.GuiderId.Value);
                }
            }
            else if (_session.MemberId.HasValue)
            {
                query = query.Where(o => o.MemberId == _session.MemberId.Value && o.GuiderId == null);
            }
            query = query.WhereIf(input.CTimeFrom != null, o => o.CTime >= input.CTimeFrom).
                WhereIf(input.CTimeTo != null, o => o.CTime <= input.CTimeTo).
                WhereIf(!string.IsNullOrEmpty(input.ListNo), o => o.Id.Contains(input.ListNo)).
                WhereIf(input.EtimeFrom != null, o => Convert.ToDateTime(o.Etime) >= input.EtimeFrom).
                WhereIf(input.EtimeTo != null, o => Convert.ToDateTime(o.Etime) >= input.EtimeTo).
                WhereIf(!string.IsNullOrEmpty(input.OrderStatusName), o => _orderBuilder.GetOrderStatusName(o) == input.OrderStatusName);

            var count = await _orderRepository.CountAsync(query);

            query = query.OrderByDescending(o => o.CTime).PageBy(input);

            var orders = await _orderRepository.ToListAsync(query);

            List<OrderQueryDto> orderQueryDtos = new List<OrderQueryDto>();
            foreach (var order in orders)
            {
                OrderQueryDto orderQueryDto = new OrderQueryDto();
                orderQueryDto = _orderBuilder.ToQueryDto(order);
            }

            return new PagedResultDto<OrderQueryDto>(count, orderQueryDtos);
        }

        public async Task<OrderSimpleListDto> GetMemberOrderForMobileAsync(string listNo)
        {
            var order = await _orderRepository.GetAll().AsNoTracking().FirstOrDefaultAsync(o => o.Id == listNo);

            return _orderBuilder.ToSimpleListDto(order);
        }

        public async Task<OrderInfoDto> GetLastOrderFullInfoAsync(GetLastOrderInput input)
        {
            var queryInput = new GetOrderInfoInput();
            queryInput.StartCTime = DateTime.Now.Date;
            queryInput.EndCTime = DateTime.Now;
            queryInput.CashierId = input.CashierId;
            queryInput.CashPcid = input.CashPcid;
            queryInput.SalePointId = input.SalePointId;
            queryInput.ParkId = input.ParkId;

            return await GetOrderFullInfoAsync(queryInput);
        }

        public async Task<OrderInfoDto> GetOrderFullInfoAsync(GetOrderInfoInput input)
        {
            var order = await _orderRepository.GetAllIncluding(o => o.OrderDetails)
                .WhereIf(!input.ListNo.IsNullOrEmpty(), o => o.Id == input.ListNo)
                .WhereIf(input.StartCTime.HasValue, o => o.CTime >= input.StartCTime)
                .WhereIf(input.EndCTime.HasValue, o => o.CTime <= input.EndCTime)
                .WhereIf(input.CashierId.HasValue, o => o.CashierId == input.CashierId)
                .WhereIf(input.CashPcid.HasValue, o => o.CashPcId == input.CashPcid)
                .WhereIf(input.SalePointId.HasValue, o => o.SalePointId == input.SalePointId)
                .WhereIf(input.ParkId.HasValue, o => o.ParkId == input.ParkId)
                .OrderByDescending(o => o.CTime)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            

            if (order == null)
            {
                throw new UserFriendlyException(_localizer["NoData"]);
            }

            var orderInfo = new OrderInfoDto();
            orderInfo.ListNo = order.Id;
            orderInfo.GuideId = order.GuiderId;
            orderInfo.OrderStatusName = _orderBuilder.GetOrderStatusName(order);
            orderInfo.TravelDate = order.Etime;
            orderInfo.TotalMoney = order.TotalMoney;
            orderInfo.PayFlag = order.PayFlag;
            orderInfo.PayTypeName = _nameCacheService.GetPayTypeName(order.PayTypeId);
            orderInfo.CTime = order.CTime.ToDateTimeString();
            orderInfo.LicensePlateNumber = order.LicensePlateNumber;
            orderInfo.JidiaoName = order.JidiaoName;
            orderInfo.Memo = order.Memo;
            orderInfo.KeYuanTypeName = _nameCacheService.GetKeYuanTypeName(order.KeYuanTypeId);
            orderInfo.AreaName = _nameCacheService.GetAreaName(order.KeYuanAreaId);
            orderInfo.CashierName = _nameCacheService.GetStaffName(order.CashierId);
            orderInfo.PersonNum = order.TotalNum;
            orderInfo.CompanyName = order.CustomerName;
            ChangCi changCi = await _changCiRepository.GetAll().Where(c => c.Id == order.ChangCiId).FirstAsync();
            orderInfo.ChangCiName = $"{changCi.Stime}-{changCi.Etime}";
            orderInfo.InvoiceStatus = order.InvoiceStatus;
            orderInfo.AllowInvoice = _orderDomainService.AllowInvoice(order);
            orderInfo.AllowChangeChangCi = _orderDomainService.AllowChangeChangCi(order);
            orderInfo.AllowChangeQuantity = _orderDomainService.AllowChangeQuantity(order);
            orderInfo.Mobile = order.Mobile;

            orderInfo.ShouldPay = order.ShouldPay();
            if (orderInfo.ShouldPay)
            {
                var payOrder = await _payAppService.GetNetPayOrderAsync(order.Id);
                if (payOrder.ExpireSeconds <= 0)
                {
                    orderInfo.ShouldPay = false;
                }
                else
                {
                    orderInfo.ExpireSeconds = payOrder.ExpireSeconds;
                }
            }

            var tickets = await _ticketSaleQueryAppService.GetOrderTicketSalesAsync(order.Id);
            foreach (TicketSaleSimpleDto ticketSaleSimpleDto in tickets)
            {
                if (ticketSaleSimpleDto.ShowTicketCode == TicketKind.二代证.ToString() && order.GuiderId != null && ticketSaleSimpleDto.TicketCode.Length > 24)
                {
                    string certNo = DES3Helper.Decrypt(ticketSaleSimpleDto.TicketCode);
                    ticketSaleSimpleDto.ShowTicketCode = $"{certNo.Substring(0, 6)}********{certNo.Substring(14, 4)}";
                }
                else
                {
                    ticketSaleSimpleDto.ShowTicketCode = ticketSaleSimpleDto.TicketCode;
                }
            }

            orderInfo.AllowCancel = await _orderDomainService.AllowCancelAsync(order.Id);
            if (orderInfo.AllowCancel && !tickets.IsNullOrEmpty())
            {
                orderInfo.AllowCancel = tickets.Any(t => t.AllowRefund);
            }

            List<TicketSaleSeatDto> seats = null;
            if (order.OrderDetails.Any(o => o.HasGroundSeat))
            {
                if (orderInfo.ShouldPay)
                {
                    seats = await _seatStatusCacheRepository.GetOrderSeatsAsync(order.Id);
                }
                else
                {
                    seats = await _ticketSaleQueryAppService.GetTicketSeatsAsync(new GetTicketSeatsInput { ListNo = order.Id });
                }
            }

            foreach (var orderDetail in order.OrderDetails)
            {
                var orderDetailDto = new OrderDetailDto();
                orderDetailDto.Id = orderDetail.Id;
                orderDetailDto.TicketTypeId = orderDetail.TicketTypeId;
                orderDetailDto.TotalNum = orderDetail.TotalNum;
                orderDetailDto.SurplusNum = orderDetail.SurplusNum;
                orderDetailDto.ReaPrice = orderDetail.ReaPrice.Value;
                orderDetailDto.Tickets = tickets.Where(t => t.OrderDetailId == orderDetail.Id).ToList();
                TicketType ticketType = _ticketTypeRepository.GetAll().FirstOrDefault(t => t.Id == orderDetail.TicketTypeId);
                if(ticketType != null)
                {
                    orderDetailDto.TicketTypeName = ticketType.GetDisplayName();
                    if (ticketType.FirstActiveFlag == true)
                    {
                        orderDetailDto.FirstActiveFlag = true;
                    }
                }
                if (!orderDetailDto.Tickets.IsNullOrEmpty())
                {
                    orderDetailDto.SurplusNum = orderDetailDto.Tickets.Sum(t => t.SurplusQuantity);
                    orderDetailDto.UsableQuantity = orderDetailDto.Tickets.Where(t => t.IsUsable).Sum(t => t.SurplusQuantity);
                    orderDetailDto.ETime = orderDetailDto.Tickets.Max(t => t.Etime.To<DateTime>()).ToDateString();
                }
                orderDetailDto.WxShowQrCode = ticketType.WxShowQrCode;
                orderDetailDto.UsageMethod = ticketType.UsageMethod;
                orderInfo.Details.Add(orderDetailDto);

                if (orderDetail.HasGroundChangCi || orderDetail.HasGroundSeat)
                {
                    orderDetailDto.GroundChangCis = new List<OrderGroundChangCiDto>();

                    var orderGroundChangCis = await _orderGroundChangCiRepository.GetAllListAsync(g => g.OrderDetailId == orderDetail.Id);
                    foreach (var orderGroundChangCi in orderGroundChangCis)
                    {
                        var groundChangCiDto = new OrderGroundChangCiDto();
                        groundChangCiDto.GroundName = _nameCacheService.GetGroundName(orderGroundChangCi.GroundId);
                        groundChangCiDto.ChangCiName = _nameCacheService.GetChangCiName(orderGroundChangCi.ChangCiId);
                        orderDetailDto.GroundChangCis.Add(groundChangCiDto);

                        if (orderDetail.HasGroundSeat)
                        {
                            groundChangCiDto.Seats = seats
                                .Where(s => s.GroundId == orderGroundChangCi.GroundId && s.ChangCiId == orderGroundChangCi.ChangCiId)
                                .Select(s => new OrderSeatDto
                                {
                                    SeatName = _nameCacheService.GetSeatName(s.SeatId)
                                })
                                .ToList();
                        }
                    }
                }

                var tourists = await _orderTouristRepository.GetAll()
                    .Where(o => o.OrderDetailId == orderDetail.Id)
                    .Select(o => new TouristDto
                    {
                        Name = o.Name,
                        CertType = o.CertType,
                        CertNo = o.CertNo
                    })
                    .ToListAsync();
                foreach (var tourist in tourists)
                {
                    if (tourist.CertType == DefaultCertType.二代身份证)
                    {
                        if (_orderOptions.EncodeIDCardNo == "1" && tourist.CertNo.Length > 18)
                        {
                            tourist.CertNo = DES3Helper.Decrypt(tourist.CertNo);
                        }
                        tourist.CertNo = $"{tourist.CertNo.Substring(0, 6)}********{tourist.CertNo.Substring(14, 4)}";
                    }
                    else
                    {
                        tourist.CertNo = $"{tourist.CertNo.Substring(0, tourist.CertNo.Length - 2)}**";
                    }
                    tourist.CertTypeName = _nameCacheService.GetCertTypeName(tourist.CertType);
                }
                orderInfo.Tourists.AddRange(tourists);
            }

            orderInfo.RefundStatusName = await GetRefundStatusNameAsync(order);

            orderInfo.Contact = new
            {
                ContactName = order.YdrName,
                ContactMobile = order.Mobile,
                ContactCertNo = order.CertNo
            };

            return orderInfo;
        }

        /// <summary>
        /// 获取订单查询信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OrderInfoDto> GetOrderQueryInfoAsync(GetOrderInfoInput input)
        {
            var order = await _orderRepository.GetAllIncluding(o => o.OrderDetails)
                .WhereIf(!input.ListNo.IsNullOrEmpty(), o => o.Id == input.ListNo)
                .WhereIf(input.StartCTime.HasValue, o => o.CTime >= input.StartCTime)
                .WhereIf(input.EndCTime.HasValue, o => o.CTime <= input.EndCTime)
                .WhereIf(input.CashierId.HasValue, o => o.CashierId == input.CashierId)
                .WhereIf(input.CashPcid.HasValue, o => o.CashPcId == input.CashPcid)
                .WhereIf(input.SalePointId.HasValue, o => o.SalePointId == input.SalePointId)
                .WhereIf(input.ParkId.HasValue, o => o.ParkId == input.ParkId)
                .OrderByDescending(o => o.CTime)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (order == null)
            {
                throw new UserFriendlyException(_localizer["NoData"]);
            }

            var orderInfo = new OrderInfoDto();
            orderInfo.ListNo = order.Id;
            orderInfo.OrderStatusName = _orderBuilder.GetOrderStatusName(order);
            orderInfo.TravelDate = order.Etime;
            orderInfo.TotalMoney = order.TotalMoney;
            orderInfo.PayFlag = order.PayFlag;
            orderInfo.CTime = order.CTime.ToDateTimeString();
            orderInfo.PersonNum = order.TotalNum;
            ChangCi changCi = await _changCiRepository.GetAll().Where(c => c.Id == order.ChangCiId).FirstAsync();
            orderInfo.ChangCiName = $"{changCi.Stime}-{changCi.Etime}";
            orderInfo.InvoiceStatus = order.InvoiceStatus;
            orderInfo.AllowInvoice = _orderDomainService.AllowInvoice(order);
            orderInfo.AllowChangeChangCi = _orderDomainService.AllowChangeChangCi(order);
            orderInfo.AllowChangeQuantity = _orderDomainService.AllowChangeQuantity(order);
            orderInfo.Mobile = order.Mobile;

            orderInfo.ShouldPay = order.ShouldPay();
            if (orderInfo.ShouldPay)
            {
                var payOrder = await _payAppService.GetNetPayOrderAsync(order.Id);
                if (payOrder.ExpireSeconds <= 0)
                {
                    orderInfo.ShouldPay = false;
                }
                else
                {
                    orderInfo.ExpireSeconds = payOrder.ExpireSeconds;
                }
            }

            var tickets = await _ticketSaleQueryAppService.GetOrderTicketSalesAsync(order.Id);
            orderInfo.TicketsLength = tickets.Count;

            orderInfo.AllowCancel = await _orderDomainService.AllowCancelAsync(order.Id);
            if (orderInfo.AllowCancel && !tickets.IsNullOrEmpty())
            {
                orderInfo.AllowCancel = tickets.Any(t => t.AllowRefund);
            }

            foreach (var orderDetail in order.OrderDetails)
            {
                var orderDetailDto = new OrderDetailDto();
                orderInfo.Details.Add(orderDetailDto);
            }

            return orderInfo;
        }

        private async Task<string> GetRefundStatusNameAsync(Order order)
        {
            if (!order.RefundStatus.HasValue) return null;

            var refundOrderApplys = await _refundOrderApplyRepository.GetAllListAsync(r => r.ListNo == order.Id);
            if (refundOrderApplys.Count == 1)
            {
                var refundOrderApply = refundOrderApplys.First();
                var payType = _nameCacheService.GetPayType(order.PayTypeId);
                var statusName = await GetRefundApplyStatusName(refundOrderApply, payType);
                if (statusName == _localizer["FreeTicket"])
                {
                    return null;
                }
                return _localizer.GetString("RefundProgress", statusName);
            }
            else if (refundOrderApplys.Count > 1)
            {
                if (order.RefundStatus == RefundStatus.退款失败)
                {
                    var totalNum = refundOrderApplys.Where(r => r.Status == RefundApplyStatus.退款失败).Sum(r => r.RefundQuantity);

                    return _localizer.GetString("RefundFailStatusName", totalNum);
                }
                else if (order.RefundStatus == RefundStatus.退款中)
                {
                    var totalNum = refundOrderApplys.Where(r => r.Status == RefundApplyStatus.退款中).Sum(r => r.RefundQuantity);

                    return _localizer.GetString("RefundingStatusName", totalNum);
                }
                else
                {
                    var totalNum = refundOrderApplys.Where(r => r.Status == RefundApplyStatus.退款成功).Sum(r => r.RefundQuantity);

                    return _localizer.GetString("RefundedStatusName", totalNum);
                }
            }

            return null;
        }

        public async Task<GroupOrderDto> GetGroupOrderAsync(string listNo)
        {
            var order = await _orderRepository.GetAll()
                .Where(o => o.Id == listNo)
                .Select(o => new GroupOrderDto
                {
                    TravelDate = o.Etime,
                    ChangCiId = o.ChangCiId,
                    TotalNum = o.TotalNum,
                    TotalMoney = o.TotalMoney
                })
                .FirstOrDefaultAsync();

            if (order != null)
            {
                order.ChangCiName = _nameCacheService.GetChangCiTimeRange(order.ChangCiId);
            }

            return order;
        }

        public async Task<List<RefundApplyDto>> GetRefundApplysWithStatusDetailAsync(string listNo)
        {
            var refundOrderApplyDtos = new List<RefundApplyDto>();

            int lastRefundDays = 3;

            var payTypeId = await _orderRepository.GetAll().Where(o => o.Id == listNo).Select(o => o.PayTypeId).FirstOrDefaultAsync();
            var payType = _nameCacheService.GetPayType(payTypeId);
            var supplier = _localizer[payType.Supplier];

            var refundOrderApplys = await _refundOrderApplyRepository.GetAllListAsync(r => r.ListNo == listNo);
            foreach (var refundOrderApply in refundOrderApplys)
            {
                var lastRefundDate = refundOrderApply.Ctime.AddDays(lastRefundDays).ToDateString();

                var refundOrderApplyDto = new RefundApplyDto();
                refundOrderApplyDto.RefundListNo = refundOrderApply.RefundListNo;
                refundOrderApplyDto.RefundTimeDescription = _localizer.GetString("RefundTimeDescription", lastRefundDate);
                refundOrderApplyDto.RefundQuantity = refundOrderApply.RefundQuantity;
                refundOrderApplyDto.RefundMoney = refundOrderApply.RefundMoney;
                refundOrderApplyDto.RefundRecvAccount = _localizer["PayAccount"];
                refundOrderApplyDto.RefundStatusName = await GetRefundApplyStatusName(refundOrderApply, payType);

                refundOrderApplyDto.StatusDetails.Add(new RefundApplyStatusDetail
                {
                    Title = _localizer["ApplyRefund"],
                    Details = new List<string>
                    {
                        refundOrderApply.Ctime.ToDateTimeString()
                    }
                });

                if (refundOrderApply.Status == RefundApplyStatus.退款中)
                {
                    refundOrderApplyDto.StatusDetails.Add(new RefundApplyStatusDetail
                    {
                        Title = _localizer["AuditingName"],
                        Details = new List<string>
                        {
                            _localizer.GetString("AuditingRefundNote", refundOrderApply.Ctime.AddMinutes(15).ToDateTimeString())
                        }
                    });
                }
                else if (refundOrderApply.Status == RefundApplyStatus.退款成功)
                {
                    refundOrderApplyDto.StatusDetails.Add(new RefundApplyStatusDetail
                    {
                        Title = _localizer["AuditedName"],
                        Details = new List<string>
                        {
                            refundOrderApply.HandleTime.Value.ToDateTimeString(),
                            _localizer.GetString("AuditedRefundNote", refundOrderApply.RefundMoney, supplier)
                        }
                    });

                    var refundMoneyApply = await _refundMoneyApplyRepository.FirstOrDefaultAsync(r => r.RefundListNo == refundOrderApply.RefundListNo);
                    if (refundMoneyApply != null)
                    {
                        if (refundMoneyApply.Status == RefundApplyStatus.退款中)
                        {
                            if (refundMoneyApply.ApplySuccess)
                            {
                                refundOrderApplyDto.StatusDetails.Add(new RefundApplyStatusDetail
                                {
                                    Title = _localizer.GetString("ProcessingRefund", supplier),
                                    Details = new List<string>
                                {
                                    refundMoneyApply.ApplySuccessTime.Value.ToDateTimeString(),
                                    _localizer.GetString("ProcessingRefundNote", supplier, refundMoneyApply.RefundId, supplier, payType.ServicePhone)
                                }
                                });
                            }
                        }
                        else if (refundMoneyApply.Status == RefundApplyStatus.退款成功)
                        {
                            refundOrderApplyDto.StatusDetails.Add(new RefundApplyStatusDetail
                            {
                                Title = _localizer.GetString("AcceptRefund", supplier),
                                Details = new List<string>
                            {
                                refundMoneyApply.ApplySuccessTime.Value.ToDateTimeString(),
                                _localizer.GetString("AcceptRefundNote", supplier)
                            }
                            });

                            var lastRefundTime = refundMoneyApply.ApplySuccessTime.Value.AddDays(lastRefundDays);
                            if (lastRefundTime > DateTime.Now)
                            {
                                refundOrderApplyDto.StatusDetails.Add(new RefundApplyStatusDetail
                                {
                                    Title = _localizer.GetString("RefundRecording", supplier),
                                    Details = new List<string>
                            {
                                refundMoneyApply.RefundSuccessTime.Value.ToDateTimeString(),
                                _localizer.GetString("RefundRecordingNote", supplier, refundMoneyApply.RefundMoney, _localizer["PayAccount"], lastRefundDate, refundMoneyApply.RefundId, supplier, payType.ServicePhone)
                            }
                                });
                            }
                            else
                            {
                                refundOrderApplyDto.RefundTimeDescription = _localizer["RefundReceived"];
                                refundOrderApplyDto.StatusDetails.Add(new RefundApplyStatusDetail
                                {
                                    Title = _localizer["RefundRecorded"],
                                    Details = new List<string>
                                {
                                    lastRefundTime.ToDateTimeString(),
                                    _localizer.GetString("RefundRecordedNote", supplier, lastRefundDate, refundMoneyApply.RefundMoney, _localizer["PayAccount"], refundMoneyApply.RefundId, supplier, payType.ServicePhone)
                                }
                                });
                            }
                        }
                        else
                        {
                            refundOrderApplyDto.StatusDetails.Add(new RefundApplyStatusDetail
                            {
                                Title = _localizer.GetString("RefundFail", supplier),
                                Details = new List<string>
                            {
                                refundMoneyApply.HandleTime.Value.ToDateTimeString(),
                                refundMoneyApply.ResultMessage
                            }
                            });
                        }
                    }
                }
                else
                {
                    refundOrderApplyDto.StatusDetails.Add(new RefundApplyStatusDetail
                    {
                        Title = _localizer["RefundAuditFail"],
                        Details = new List<string>
                        {
                            refundOrderApply.HandleTime.Value.ToDateTimeString(),
                            refundOrderApply.ResultMessage
                        }
                    });
                }

                refundOrderApplyDtos.Add(refundOrderApplyDto);
            }

            return refundOrderApplyDtos;
        }

        public async Task<string> GetRefundApplyStatusName(RefundOrderApply refundOrderApply, PayType payType)
        {
            var supplier = _localizer[payType.Supplier];

            if (refundOrderApply.Status == RefundApplyStatus.退款中)
            {
                return _localizer["AuditingName"];
            }

            if (refundOrderApply.Status == RefundApplyStatus.退款成功)
            {
                var refundMoneyApply = await _refundMoneyApplyRepository.FirstOrDefaultAsync(r => r.RefundListNo == refundOrderApply.RefundListNo);
                if (refundMoneyApply == null)
                {
                    return _localizer["FreeTicket"];
                }
                if (refundMoneyApply.Status == RefundApplyStatus.退款中)
                {
                    if (refundMoneyApply.ApplySuccess)
                    {
                        return _localizer.GetString("RefundRecord", supplier);
                    }
                    else
                    {
                        return _localizer["AuditedName"];
                    }
                }

                if (refundMoneyApply.Status == RefundApplyStatus.退款成功)
                {
                    if (refundMoneyApply.ApplySuccessTime.Value.AddDays(3) > DateTime.Now)
                    {
                        return _localizer.GetString("RefundRecording", supplier);
                    }

                    return _localizer["RefundRecorded"];
                }

                return _localizer.GetString("RefundFail", supplier);
            }

            return _localizer["RefundAuditFail"];
        }

        public async Task<OrderOptionsDto> GetOrderOptionsAsync(string travelDate)
        {
            var optionsDto = new OrderOptionsDto();

            var today = DateTime.Now.Date;
            var closeTime = $"{today.ToDateString()} {_orderOptions.WeiXinSaleTime}:00".To<DateTime>();
            optionsDto.Dates.Add(new
            {
                Date = today.ToDateString(),
                Text = _localizer[today.DayOfWeek.ToString()].Value,
                Disable = DateTime.Now > closeTime,
                DateType = await _orderRepository.GetTMDateTypeName(today.ToDateString())
            }); ;

            int weChatBookDay = _orderOptions.WeChatBookDay < 0 ? OrderOptions.MaxWeChatBookDay - 1 : _orderOptions.WeChatBookDay - 1;
            for (int i = 0; i < weChatBookDay; i++)
            {
                var date = today.AddDays(i + 1);
                string dateType = await _orderRepository.GetTMDateTypeName(date.ToDateString());
                if (!string.IsNullOrEmpty(dateType))
                {
                    optionsDto.Dates.Add(new
                    {
                        Date = date.ToDateString(),
                        Text = _localizer[date.DayOfWeek.ToString()].Value,
                        Disable = false,
                        DateType = dateType
                    });
                }
            }

            optionsDto.GroupMaxBuyQuantityPerOrder = _orderOptions.GroupMaxBuyQuantityPerOrder;
            int orderSum = (await _orderRepository.GetAll().Where(o => o.MemberId == _session.MemberId && o.Etime == today.ToDateString() && o.GuiderId == null).Select(o => o.TotalNum - o.ReturnNum).ToListAsync()).Sum();
            if (_session.MemberId == null)
            {
                orderSum = 0;
            }
            optionsDto.WeChatMaxBuyQuantityPerDay = _orderOptions.WeChatMaxBuyQuantityPerDay - orderSum;

            return await Task.FromResult(optionsDto);
        }

        /// <summary>
        /// 获取购票日期附带票状态
        /// </summary>
        /// <returns></returns>
        public async Task<OrderOptionsTicketDto> GetOrderOptionsTicketAsync(string travelDate)
        {
            OrderOptionsTicketDto orderOptionsTicketDto = new OrderOptionsTicketDto();
            OrderOptionsDto orderOptionsDto = await GetOrderOptionsAsync(travelDate);
            foreach (dynamic date in orderOptionsDto.Dates)
            {
                List<DateChangCiSaleDto> dateChangCiSaleDtos = await _commonAppService.GetChangCiForSaleAsync(date.Date);
                string canBookStatus = "full";
                DateChangCiSaleDto dateChangCiSaleDto = dateChangCiSaleDtos.Find(d => d.SurplusNum > 0);
                if (dateChangCiSaleDto != null)
                {
                    canBookStatus = "haveTicket";
                }
                orderOptionsTicketDto.DateTickets.Add(new
                {
                    Date = date.Date,
                    Text = date.Text,
                    Disable = date.Disable,
                    DateType = date.DateType,
                    CanBookStatus = canBookStatus
                });
            }
            return await Task.FromResult(orderOptionsTicketDto).ConfigureAwait(false);
        }

        public async Task<List<OrderForExplainListDto>> GetOrdersForExplainAsync(GetOrdersForExplainInput input)
        {
            var orders = await _orderRepository.GetOrdersForExplainAsync(DateTime.Now.ToDateString(), input.CustomerName);
            foreach (var order in orders)
            {
                order.AgeRanges.AddRange(await GetOrderAgeRanges(order.ListNo));

                if (order.KeYuanTypeId.HasValue)
                {
                    order.KeYuanTypeName = _nameCacheService.GetKeYuanTypeName(order.KeYuanTypeId.Value);
                }
                if (order.KeYuanAreaId.HasValue)
                {
                    order.AreaName = _nameCacheService.GetAreaName(order.KeYuanAreaId.Value);
                }
                if (order.ExplainerId.HasValue)
                {
                    order.ExplainerName = _nameCacheService.GetStaffName(order.ExplainerId.Value);
                }
                order.TimeslotName = _nameCacheService.GetExplainerTimeslotName(order.ExplainerTimeId);
                if (order.CompleteTime.HasValue)
                {
                    order.Editable = false;
                }
                else if (order.ExplainerId.HasValue && order.ExplainerId.Value != _session.StaffId.Value)
                {
                    order.Editable = false;
                }
                else
                {
                    order.Editable = true;
                }
                var checkInTime = await _orderRepository.GetOrderCheckInTimeAsync(order.ListNo);
                order.HasCheckIn = checkInTime.HasValue;
            }

            return orders;
        }

        public async Task<List<OrderForConsumeListDto>> GetGroupOrdersForConsumeAsync(GetGroupOrdersForConsumeInput input)
        {
            var travelDate = DateTime.Now.ToDateString();

            var query = _orderRepository.GetAll()
                .Where(o => o.Etime == travelDate && o.CustomerId != null)
                .WhereIf(!input.QueryText.IsNullOrEmpty(), o => o.LicensePlateNumber.Contains(input.QueryText) || o.CustomerName.Contains(input.QueryText));

            var resultQuery = query.Select(o => new OrderForConsumeListDto
            {
                ListNo = o.Id,
                TravelDate = o.Etime,
                CustomerName = o.CustomerName,
                TotalNum = o.TotalNum,
                KeYuanTypeId = o.KeYuanTypeId,
                KeYuanAreaId = o.KeYuanAreaId,
                LicensePlateNumber = o.LicensePlateNumber,
                Memo = o.Memo
            });

            var orders = await _orderRepository.ToListAsync(resultQuery);
            foreach (var order in orders)
            {
                order.AgeRanges.AddRange(await GetOrderAgeRanges(order.ListNo));

                if (order.KeYuanTypeId.HasValue)
                {
                    order.KeYuanTypeName = _nameCacheService.GetKeYuanTypeName(order.KeYuanTypeId.Value);
                }
                if (order.KeYuanAreaId.HasValue)
                {
                    order.AreaName = _nameCacheService.GetAreaName(order.KeYuanAreaId.Value);
                }

                order.CheckInTime = await _orderRepository.GetOrderCheckInTimeAsync(order.ListNo);
                order.HasCheckIn = order.CheckInTime.HasValue;
                order.CheckOutTime = await _orderRepository.GetOrderCheckOutTimeAsync(order.ListNo);
                order.HasCheckOut = order.CheckOutTime.HasValue;
            }

            return orders.OrderBy(o => o.CheckOutTime).ThenBy(o => o.CheckInTime).ToList();
        }

        private async Task<List<OrderAgeRangeDto>> GetOrderAgeRanges(string listNo)
        {
            var ageRangeDtos = new List<OrderAgeRangeDto>();

            var ageRanges = await _orderAgeRangeRepository.GetAllListAsync(a => a.ListNo == listNo);
            if (!ageRanges.IsNullOrEmpty())
            {
                foreach (var ageRange in ageRanges)
                {
                    ageRangeDtos.Add(new OrderAgeRangeDto
                    {
                        AgeRangeName = _nameCacheService.GetAgeRangeName(ageRange.AgeRangeId),
                        PersonNum = ageRange.PersonNum
                    });
                }
            }

            return ageRangeDtos;
        }

        public async Task<byte[]> GetOrdersToExcelAsync(GetOrdersInput input)
        {
            input.ShouldPage = false;

            var result = await GetOrdersAsync(input);

            return await ExcelHelper.ExportToExcelAsync(result.Items, "订单查询", string.Empty);
        }

        public async Task<PagedResultDto<OrderListDto>> GetOrdersAsync(GetOrdersInput input)
        {
            var result = await _orderRepository.GetOrdersAsync(input);
            foreach (var order in result.Items)
            {
                order.OrderTypeName = order.OrderTypeId.ToString();
                order.OrderStatusName = order.OrderStatusId.ToString();
                order.ConsumeStatusName = order.ConsumeStatus?.ToString();
                order.RefundStatusName = order.RefundStatus?.ToString();
                order.ExplainerName = _nameCacheService.GetStaffName(order.ExplainerId);
                order.ExplainerTimeslotName = _nameCacheService.GetExplainerTimeslotName(order.ExplainerTimeId);
                if (order.YdrName.IsNullOrEmpty() && !order.JidiaoName.IsNullOrEmpty())
                {
                    order.YdrName = order.JidiaoName;
                }
                if (order.Mobile.IsNullOrEmpty() && !order.JidiaoMobile.IsNullOrEmpty())
                {
                    order.Mobile = order.JidiaoMobile;
                }
                if (order.TotalMoney == 0)
                {
                    order.AllowCancel = await _orderDomainService.AllowCancelAsync(order.ListNo);
                }
                if (input.NeedCheckTime)
                {
                    order.CheckInTime = await _orderRepository.GetOrderCheckInTimeAsync(order.ListNo);
                    order.CheckOutTime = await _orderRepository.GetOrderCheckOutTimeAsync(order.ListNo);
                }
            }

            return result;
        }

        /// <summary>
        /// 讲解预约查询
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<ExplainerOrderListDto>> GetExplainerOrdersAsync(GetExplainerOrdersInput input)
        {
            var result = await _orderRepository.GetExplainerOrdersAsync(input);
            foreach (var order in result.Items)
            {
                order.TimeslotName = order.TimeslotId.HasValue ? _nameCacheService.GetExplainerTimeslotName(order.TimeslotId.Value) : string.Empty;
                order.ExplainerName = order.ExplainerId.HasValue ? _nameCacheService.GetStaffName(order.ExplainerId.Value) : string.Empty;
                if (order.TravelDate.To<DateTime>() == DateTime.Now.Date && !order.BeginTime.IsNullOrEmpty() && order.CompleteTime.IsNullOrEmpty())
                {
                    order.AllowChange = true;
                }
            }

            return result;
        }

        public async Task<DynamicColumnResultDto> StatOrderByCustomerAsync(StatOrderByCustomerInput input)
        {
            var data = await _orderRepository.StatOrderByCustomerAsync(input);

            return new DynamicColumnResultDto(data);
        }

        /// <summary>
        /// 获取订单状态下拉列表
        /// </summary>
        /// <returns></returns>
        public List<ComboboxItemDto<int>> GetOrderStatusComboboxItemDtos()
        {
            var items = typeof(OrderStatus).ToComboboxItems();
            foreach (var item in items)
            {
                item.DisplayText = _localizer[item.DisplayText];
            }
            return items;
        }

        /// <summary>
        /// 获取完整订单状态下拉列表
        /// </summary>
        /// <returns></returns>
        public List<ComboboxItemDto<int>> GetCompleteOrderStatusComboboxItemDtos()
        {

            List<ComboboxItemDto<int>> items = GetOrderStatusComboboxItemDtos();
            items.Add(new ComboboxItemDto<int> { Value = 20, DisplayText = _localizer["Paying"] });
            items.Add(new ComboboxItemDto<int> { Value = 21, DisplayText = _localizer["Refunding"] });
            items.Add(new ComboboxItemDto<int> { Value = 22, DisplayText = _localizer["Refunded"] });
            items.Add(new ComboboxItemDto<int> { Value = 23, DisplayText = _localizer["Using"] });
            items.Add(new ComboboxItemDto<int> { Value = 24, DisplayText = _localizer.GetString("RefundFail", "") });
            return items;
        }


        public async Task<bool> GetIfInvoiceAsync(string listNo)
        {
            bool ifInvoice = false;
            InvoiceInfo invoiceInfo = await _invoiceRepository.GetAll().Where(t => t.ListNo == listNo).FirstOrDefaultAsync();
            if (invoiceInfo != null)
            {
                ifInvoice = true;
            }
            return ifInvoice;
        }
    }
}
