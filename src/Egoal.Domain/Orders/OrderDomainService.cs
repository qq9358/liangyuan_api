using Egoal.Common;
using Egoal.Domain.Repositories;
using Egoal.Domain.Services;
using Egoal.Events.Bus;
using Egoal.Events.Bus.Entities;
using Egoal.Extensions;
using Egoal.Localization;
using Egoal.Scenics;
using Egoal.Tickets;
using Egoal.TicketTypes;
using Egoal.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;
using Egoal.Members;

namespace Egoal.Orders
{
    public class OrderDomainService : DomainService, IOrderDomainService
    {
        private readonly IEventBus _eventBus;
        private readonly OrderOptions _orderOptions;
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IOrderStatRepository _orderStatRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IRepository<OrderDetail, long> _orderDetailRepository;
        private readonly IGroundDateChangCiSaleNumRepository _changCiSaleNumRepository;
        private readonly IChangCiRepository _changCiRepository;
        private readonly ITicketSaleDomainService _ticketSaleDomainService;
        private readonly IRepository<Member, Guid> _memberRepository;

        public OrderDomainService(
            IEventBus eventBus,
            IOptions<OrderOptions> orderOptions,
            IStringLocalizer<SharedResource> localizer,
            IOrderStatRepository orderStatRepository,
            IOrderRepository orderRepository,
            IRepository<OrderDetail, long> orderDetailRepository,
            IGroundDateChangCiSaleNumRepository changCiSaleNumRepository,
            IChangCiRepository changCiRepository,
            ITicketSaleDomainService ticketSaleDomainService,
            IRepository<Member, Guid> memberRepository)
        {
            _eventBus = eventBus;
            _orderOptions = orderOptions.Value;
            _localizer = localizer;
            _orderStatRepository = orderStatRepository;
            _orderRepository = orderRepository;
            _orderDetailRepository = orderDetailRepository;
            _changCiSaleNumRepository = changCiSaleNumRepository;
            _changCiRepository = changCiRepository;
            _ticketSaleDomainService = ticketSaleDomainService;
            _memberRepository = memberRepository;
        }

        public async Task CreateAsync(Order order)
        {
            ValidateTravelDate(order);

            await ValidateTouristAsync(order);

            await ValidateMemberBookAsync(order);

            await ValidateOrderPlanAsync(order);

            order.Audit();

            await BookChangCiAsync(order.Etime, order.ChangCiId.Value, order.TotalNum);

            await UpdateOrderStatAsync(order.Etime, order.TotalNum);

            await _orderRepository.InsertAsync(order);
        }

        private void ValidateTravelDate(Order order)
        {
            var now = DateTime.Now;
            var travelDate = order.Etime.To<DateTime>();

            if (travelDate == now.Date && now > $"{order.Etime} {_orderOptions.WeiXinSaleTime}:00".To<DateTime>())
            {
                throw new UserFriendlyException(_localizer.GetString("TodayBookTimeExpired", _orderOptions.WeiXinSaleTime));
            }

            if (travelDate < now.Date || travelDate > now.Date.AddDays(_orderOptions.WeChatBookDay - 1))
            {
                throw new UserFriendlyException(_localizer.GetString("TravelDateCannotBook", order.Etime));
            }
        }

        private async Task ValidateTouristAsync(Order order)
        {
            foreach (var orderDetail in order.OrderDetails)
            {
                if (orderDetail.OrderTourists.IsNullOrEmpty())
                {
                    if (orderDetail.TicketType.NeedCertFlag == true)
                    {
                        throw new UserFriendlyException(_localizer["TouristEmpty"]);
                    }

                    continue;
                }

                if (orderDetail.TicketType.NeedCertFlag == true && orderDetail.OrderTourists.Count() != orderDetail.TotalNum)
                {
                    throw new UserFriendlyException(_localizer.GetString("TouristQuantity", orderDetail.TotalNum));
                }

                await _ticketSaleDomainService.ValidateCertNoAsync(orderDetail.OrderTourists.Select(t => t.CertNo), order.Etime.To<DateTime>());
            }
        }

        private async Task ValidateMemberBookAsync(Order order)
        {
            var today = DateTime.Now.ToDateString();

            if (order.GuiderId.HasValue)
            {
                var guider = await _memberRepository.GetAll().AsNoTracking().FirstOrDefaultAsync(m => m.Id == order.GuiderId.Value);
                if (guider.IsBlacklisted())
                {
                    throw new UserFriendlyException(_localizer["InBlacklist"]);
                }

                if (_orderOptions.GroupMaxBuyQuantityPerOrder > 0)
                {
                    var bookNum = (await _orderRepository.GetAll().Where(o => o.GuiderId == order.GuiderId && o.Etime == order.Etime).Select(o => o.TotalNum - o.ReturnNum).ToListAsync()).Sum();
                    if (bookNum + order.TotalNum > _orderOptions.GroupMaxBuyQuantityPerOrder)
                    {
                        throw new UserFriendlyException(_localizer.GetString("QuantityAvailabelBeyondTravelDate", _orderOptions.GroupMaxBuyQuantityPerOrder));
                    }
                }
            }
            else if (order.MemberId.HasValue && _orderOptions.WeChatMaxBuyQuantityPerDay > 0)
            {
                var bookNum = (await _orderRepository.GetAll().Where(o => o.MemberId == order.MemberId && o.Etime == today && o.GuiderId == null).Select(o => o.TotalNum - o.ReturnNum).ToListAsync()).Sum();
                if (bookNum + order.TotalNum > _orderOptions.WeChatMaxBuyQuantityPerDay)
                {
                    throw new UserFriendlyException(_localizer["WeChatNumberTodayBuyTooMuch"]);
                }
            }
        }

        private async Task ValidateOrderPlanAsync(Order order)
        {
            if (_orderOptions.WeChatStock <= 0) return;

            var quantity = await _orderStatRepository.GetOrderQuantityAsync(order.Etime) + order.TotalNum;
            if (quantity > _orderOptions.WeChatStock)
            {
                throw new UserFriendlyException(_localizer["WeChatStockInsufficient"]);
            }
        }

        public async Task ChangeChangCiAsync(Order order, int changCiId)
        {
            await BookChangCiAsync(order.Etime, order.ChangCiId.Value, order.TotalNum - order.ReturnNum, true);

            order.ChangCiId = changCiId;

            await BookChangCiAsync(order.Etime, order.ChangCiId.Value, order.TotalNum - order.ReturnNum);
        }

        public async Task ChangeQuantityAsync(Order order, int quantity)
        {
            if (await _orderDetailRepository.AnyAsync(o => o.ListNo == order.Id))
            {
                throw new UserFriendlyException("订单已取票不能变更人数");
            }

            await BookChangCiAsync(order.Etime, order.ChangCiId.Value, order.TotalNum, true);
            order.TotalNum = quantity - order.TotalNum;
            await ValidateMemberBookAsync(order);
            order.TotalNum = order.SurplusNum = quantity;

            await BookChangCiAsync(order.Etime, order.ChangCiId.Value, order.TotalNum);
        }

        public void ApplyInvoice(Order order)
        {
            if (order.InvoiceStatus != InvoiceStatus.未开票)
            {
                throw new UserFriendlyException($"订单{order.InvoiceStatus}");
            }

            order.InvoiceStatus = InvoiceStatus.开票中;
        }

        public async Task<bool> AllowCancelAsync(string listNo)
        {
            var order = await _orderRepository.FirstOrDefaultAsync(listNo);
            if (order.IsFree())
            {
                if (order.SurplusNum != order.TotalNum)
                {
                    return false;
                }
                if (order.Etime.To<DateTime>() < DateTime.Now.Date)
                {
                    return false;
                }
            }
            else
            {
                if (order.RefundStatus == RefundStatus.退款中 || order.RefundStatus == RefundStatus.已退款)
                {
                    return false;
                }

                if (order.SurplusNum <= 0)
                {
                    return false;
                }
            }

            if (order.InvoiceStatus != InvoiceStatus.未开票) return false;

            return true;
        }

        public bool AllowInvoice(Order order)
        {
            if (order.InvoiceStatus != InvoiceStatus.未开票) return false;

            if (order.PayFlag != true) return false;

            if (order.IsFree()) return false;

            if (order.OrderStatusId == OrderStatus.Canceled) return false;

            if (order.TotalNum == order.ReturnNum) return false;

            return true;
        }

        public bool AllowChangeChangCi(Order order)
        {
            if (order.OrderStatusId == OrderStatus.Completed || order.OrderStatusId == OrderStatus.Canceled) return false;

            if (order.Etime.To<DateTime>() < DateTime.Now.Date)
            {
                return false;
            }

            return true;
        }

        public bool AllowChangeQuantity(Order order)
        {
            if (order.OrderStatusId == OrderStatus.Collected || order.OrderStatusId == OrderStatus.Canceled) return false;

            if (order.Etime.To<DateTime>() < DateTime.Now.Date)
            {
                return false;
            }

            if (!order.GuiderId.HasValue) return false;

            if (!order.OrderDetails.IsNullOrEmpty()) return false;

            return true;
        }

        public async Task CancelAsync(Order order)
        {
            await UpdateOrderStatAsync(order.Etime, -order.SurplusNum);

            order.Cancel();

            await BookChangCiAsync(order.Etime, order.ChangCiId.Value, order.TotalNum, true);

            var eventData = new EntityDeletingEventData<Order>(order);
            await _eventBus.TriggerAsync(eventData);
        }

        private async Task UpdateOrderStatAsync(string travelDate, int quantity)
        {
            var orderStat = new OrderStat();
            orderStat.Cdate = travelDate;
            orderStat.OrderNum = quantity;
            orderStat.OrderPlanType = 0;

            var eventData = new OrderStatChangingEventData { OrderStat = orderStat };
            await _eventBus.TriggerAsync(eventData);
        }

        public async Task BeginExplainAsync(string listNo, int explainerId)
        {
            var order = await _orderRepository.GetAsync(listNo);

            if (order.ExplainerId.HasValue)
            {
                throw new UserFriendlyException("该订单已开始讲解");
            }

            order.ExplainerId = explainerId;
        }

        public async Task ConsumeAsync(string listNo)
        {
            var query = _orderRepository.GetAllIncluding(o => o.OrderDetails).Where(o => o.Id == listNo);
            var order = await _orderRepository.FirstOrDefaultAsync(query);
            if (order == null)
            {
                throw new UserFriendlyException($"订单核销失败，listNo:{listNo}不存在");
            }

            foreach (var orderDetail in order.OrderDetails)
            {
                order.Consume(orderDetail, orderDetail.TotalNum);
            }
        }

        public async Task ConsumeAsync(string listNo, long orderDetailId, int consumeNum)
        {
            var order = await _orderRepository.FirstOrDefaultAsync(listNo);
            if (order == null)
            {
                throw new UserFriendlyException($"订单核销失败，listNo:{listNo}不存在");
            }

            var orderDetail = await _orderDetailRepository.FirstOrDefaultAsync(orderDetailId);
            if (orderDetail == null)
            {
                throw new UserFriendlyException($"订单核销失败，orderDetailId:{orderDetailId}不存在");
            }

            order.Consume(orderDetail, consumeNum);
        }

        public async Task BookChangCiAsync(string travelDate, int changCiId, int quantity, bool cancel = false)
        {
            var changCiSale = new GroundDateChangCiSaleNum();
            changCiSale.Date = travelDate;
            changCiSale.GroundId = 0;
            changCiSale.ChangCiId = changCiId;
            changCiSale.SaleNum = cancel ? -quantity : quantity;

            var totalNum = await _changCiRepository.GetAll()
                .Where(c => c.Id == changCiId)
                .Select(c => c.ChangCiNum)
                .FirstOrDefaultAsync();
            if (!totalNum.HasValue)
            {
                throw new UserFriendlyException($"场次：{changCiId}不存在");
            }

            var success = await _changCiSaleNumRepository.SaleAsync(changCiSale, totalNum.Value);
            if (!success)
            {
                throw new UserFriendlyException("场次剩余数量不足");
            }
        }
    }
}
