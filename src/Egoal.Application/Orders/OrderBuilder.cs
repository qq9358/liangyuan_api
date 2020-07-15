using Egoal.Caches;
using Egoal.Cryptography;
using Egoal.Dependency;
using Egoal.DynamicCodes;
using Egoal.Extensions;
using Egoal.Localization;
using Egoal.Members;
using Egoal.Orders.Dto;
using Egoal.Payment;
using Egoal.Runtime.Session;
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
    public class OrderBuilder : IScopedDependency
    {
        private readonly ISession _session;
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly OrderOptions _orderOptions;
        private readonly IDynamicCodeService _dynamicCodeService;
        private readonly INameCacheService _nameCacheService;

        private readonly ITicketTypeRepository _ticketTypeRepository;
        private readonly ITicketTypeDomainService _ticketTypeDomainService;

        public OrderBuilder(
            ISession session,
            IStringLocalizer<SharedResource> localizer,
            IOptions<OrderOptions> orderOptions,
            IDynamicCodeService dynamicCodeService,
            INameCacheService nameCacheService,
            ITicketTypeRepository ticketTypeRepository,
            ITicketTypeDomainService ticketTypeDomainService)
        {
            _session = session;
            _localizer = localizer;
            _orderOptions = orderOptions.Value;
            _dynamicCodeService = dynamicCodeService;
            _nameCacheService = nameCacheService;

            _ticketTypeRepository = ticketTypeRepository;
            _ticketTypeDomainService = ticketTypeDomainService;
        }

        public async Task<Order> BuildOrderAsync(CreateOrderInput input, SaleChannel saleChannel, OrderType orderType)
        {
            var order = new Order();
            order.Id = await _dynamicCodeService.GenerateListNoAsync(saleChannel == SaleChannel.Local ? ListNoType.门票 : ListNoType.门票网上订票);
            order.OrderTypeId = orderType;
            order.OrderTypeName = orderType.ToString();
            order.Etime = input.TravelDate.ToDateString();
            order.ChangCiId = input.ChangCiId;
            order.PaymentMethod = saleChannel == SaleChannel.Local ? PaymentMethod.现场支付 : PaymentMethod.在线支付;
            order.MemberId = _session.MemberId;
            order.MemberName = _nameCacheService.GetMemberName(order.MemberId);
            //order.GuiderId = _session.GuiderId;
            //order.GuiderName = _nameCacheService.GetMemberName(order.GuiderId);
            order.Mobile = order.JidiaoMobile = input.ContactMobile;
            order.CertNo = input.ContactCertNo;
            order.CashierId = input.CashierId;
            order.CashPcId = input.CashPcid;
            order.SalePointId = input.SalePointId;
            order.ParkId = input.ParkId;
            order.ParkName = _nameCacheService.GetParkName(order.ParkId);

            foreach (var item in input.Items)
            {
                var ticketType = await _ticketTypeRepository
                    .GetAll()
                    .AsNoTracking()
                    .FirstOrDefaultAsync(t => t.Id == item.TicketTypeId);

                var orderDetail = ToOrderDetail(order);
                orderDetail.SetTicketType(ticketType);
                orderDetail.TotalNum = item.Quantity;
                orderDetail.SurplusNum = item.Quantity;
                var price = await _ticketTypeDomainService.GetPriceAsync(ticketType, input.TravelDate, saleChannel);
                if (price == null)
                {
                    throw new UserFriendlyException(_localizer.GetString("PriceNotSet", ticketType.GetDisplayName()));
                }
                orderDetail.SetTicMoney(orderDetail.TotalNum, price.Value);
                orderDetail.SetReaMoney(orderDetail.TotalNum, price.Value);

                orderDetail.OrderGroundChangCis = BuildGroundChangCi(item);

                orderDetail.OrderTourists = BuildOrderTourist(item);

                order.OrderDetails.Add(orderDetail);
            }

            var contactName = order.OrderDetails.FirstOrDefault(o => !o.OrderTourists.IsNullOrEmpty())?.OrderTourists.First().Name;
            order.YdrName = order.JidiaoName = contactName;

            order.StatFlag = order.OrderDetails.Any(o => o.StatFlag == true);
            order.Sum();

            return order;
        }

        public async Task<Order> BuildGroupOrderAsync(CreateGroupOrderInput input)
        {
            var order = new Order();
            order.Id = await _dynamicCodeService.GenerateListNoAsync(ListNoType.门票网上订票);
            order.OrderTypeId = input.OrderType;
            order.OrderTypeName = input.OrderType.ToString();
            order.Etime = input.TravelDate.ToDateString();
            order.EndTime = input.TravelDate.AddDays(2);
            order.ChangCiId = input.ChangCiId;
            order.TotalNum = order.SurplusNum = input.Quantity;
            order.TicketMoney = order.TotalMoney = 0M;
            order.PaymentMethod = PaymentMethod.现场支付;
            order.MemberId = _session.MemberId;
            order.MemberName = _nameCacheService.GetMemberName(order.MemberId);
            order.GuiderId = _session.GuiderId;
            order.GuiderName = _nameCacheService.GetMemberName(order.GuiderId);
            order.CustomerName = input.CompanyName;
            order.JidiaoName = order.YdrName = input.ContactName;
            order.JidiaoMobile = order.Mobile = input.Mobile;
            order.CashierId = _session.StaffId;
            order.CashPcId = _session.PcId;
            order.SalePointId = _session.SalePointId;
            order.ParkId = _session.ParkId;
            order.ParkName = _nameCacheService.GetParkName(order.ParkId);

            return order;
        }

        private OrderDetail ToOrderDetail(Order order)
        {
            var orderDetail = new OrderDetail();
            orderDetail.OrderTypeId = order.OrderTypeId;
            orderDetail.TicketStime = order.Etime;
            orderDetail.MemberId = order.MemberId;
            orderDetail.MemberName = order.MemberName;
            orderDetail.CustomerId = order.CustomerId;
            orderDetail.CustomerName = order.CustomerName;
            orderDetail.GuiderId = order.GuiderId;
            orderDetail.GuiderName = order.GuiderName;
            orderDetail.Cdate = order.Cdate;
            orderDetail.Cweek = order.Cweek;
            orderDetail.Cmonth = order.Cmonth;
            orderDetail.Cquarter = order.Cquarter;
            orderDetail.Cyear = order.Cyear;
            orderDetail.Ctp = order.Ctp;
            orderDetail.CID = order.CID;
            orderDetail.CTime = order.CTime;
            orderDetail.ParkId = order.ParkId;
            orderDetail.ParkName = order.ParkName;

            return orderDetail;
        }

        private List<OrderGroundChangCi> BuildGroundChangCi(OrderItemDto item)
        {
            if (item.GroundChangCis.IsNullOrEmpty()) return null;

            var orderGroundChangCis = new List<OrderGroundChangCi>();

            foreach (var groundChangCi in item.GroundChangCis)
            {
                var orderGroundChangCi = new OrderGroundChangCi();
                orderGroundChangCi.GroundId = groundChangCi.GroundId;
                orderGroundChangCi.ChangCiId = groundChangCi.ChangCiId;
                orderGroundChangCi.Quantity = item.Quantity;

                orderGroundChangCis.Add(orderGroundChangCi);
            }

            return orderGroundChangCis;
        }

        private List<OrderTourist> BuildOrderTourist(OrderItemDto item)
        {
            if (item.Tourists.IsNullOrEmpty()) return null;

            var orderTourists = new List<OrderTourist>();

            foreach (var tourist in item.Tourists)
            {
                var orderTourist = new OrderTourist();
                orderTourist.Name = tourist.Name;
                orderTourist.CertType = tourist.CertType ?? DefaultCertType.二代身份证;
                orderTourist.CertNo = tourist.CertNo.ToUpper();
                if (orderTourist.CertType == DefaultCertType.二代身份证 && _orderOptions.EncodeIDCardNo == "1")
                {
                    orderTourist.CertNo = DES3Helper.Encrypt(orderTourist.CertNo);
                }

                orderTourists.Add(orderTourist);
            }

            return orderTourists;
        }

        public OrderSimpleListDto ToSimpleListDto(Order order)
        {
            var orderDto = new OrderSimpleListDto();
            orderDto.ListNo = order.Id;
            orderDto.TravelDate = order.Etime;
            orderDto.TotalNum = order.TotalNum;
            orderDto.TotalMoney = order.TotalMoney;
            orderDto.IsFree = order.IsFree();
            orderDto.OrderStatusName = GetOrderStatusName(order);

            return orderDto;
        }

        public string GetOrderStatusName(Order order)
        {
            if (order.ShouldPay())
            {
                return _localizer["Paying"];
            }

            if (order.RefundStatus == RefundStatus.退款中)
            {
                return _localizer["Refunding"];
            }

            if(order.RefundStatus == RefundStatus.退款失败)
            {
                return _localizer.GetString("RefundFail","");
            }

            if (order.RefundStatus == RefundStatus.已退款)
            {
                return _localizer["Refunded"];
            }

            if (order.OrderStatusId == OrderStatus.Audited)
            {
                if (order.PaymentMethod == PaymentMethod.现场支付)
                {
                    return _localizer["Paying"];
                }

                return order.EndTime >= DateTime.Now ? _localizer["Using"] : _localizer["Expired"];
            }

            return _localizer[order.OrderStatusId.ToString()];
        }

        /// <summary>
        /// 订单查询返回值
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public OrderQueryDto ToQueryDto(Order order)
        {
            OrderQueryDto orderQueryDto = new OrderQueryDto();
            orderQueryDto.CTime = order.CTime.ToDateTimeString();
            orderQueryDto.TravelDate = order.Etime;
            orderQueryDto.TotalMoney = order.TotalMoney;
            orderQueryDto.ChangCiName = _nameCacheService.GetChangCiName(order.ChangCiId);
            orderQueryDto.Mobile = order.Mobile;
            orderQueryDto.TotalNum = order.TotalNum;
            orderQueryDto.OrderStatusName = GetOrderStatusName(order);
            return orderQueryDto;
        }
    }
}
