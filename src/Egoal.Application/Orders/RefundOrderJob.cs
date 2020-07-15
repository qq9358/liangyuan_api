using Egoal.BackgroundJobs;
using Egoal.Caches;
using Egoal.Dependency;
using Egoal.Domain.Repositories;
using Egoal.Domain.Uow;
using Egoal.Extensions;
using Egoal.Orders.Dto;
using Egoal.Payment;
using Egoal.Tickets;
using Egoal.Tickets.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Egoal.Orders
{
    public class RefundOrderJob : IBackgroundJob, IScopedDependency
    {
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IRepository<RefundOrderApply, long> _refundOrderApplyRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly ITicketSaleRepository _ticketSaleRepository;
        private readonly ITicketSaleAppService _ticketSaleAppService;
        private readonly ITicketSaleDomainService _ticketSaleDomainService;
        private readonly INameCacheService _nameCacheService;

        public RefundOrderJob(
            IUnitOfWorkManager unitOfWorkManager,
            IRepository<RefundOrderApply, long> refundOrderApplyRepository,
            IOrderRepository orderRepository,
            ITicketSaleRepository ticketSaleRepository,
            ITicketSaleAppService ticketSaleAppService,
            ITicketSaleDomainService ticketSaleDomainService,
            INameCacheService nameCacheService)
        {
            _unitOfWorkManager = unitOfWorkManager;
            _refundOrderApplyRepository = refundOrderApplyRepository;
            _orderRepository = orderRepository;
            _ticketSaleRepository = ticketSaleRepository;
            _ticketSaleAppService = ticketSaleAppService;
            _ticketSaleDomainService = ticketSaleDomainService;
            _nameCacheService = nameCacheService;
        }

        public async Task ExecuteAsync(string args, CancellationToken stoppingToken)
        {
            using (var uow = _unitOfWorkManager.Begin())
            {
                var id = Convert.ToInt64(args);
                var refundOrderApply = await _refundOrderApplyRepository.FirstOrDefaultAsync(id);

                var order = await _orderRepository.FirstOrDefaultAsync(refundOrderApply.ListNo);

                try
                {
                    var refundDetails = refundOrderApply.Details.JsonToObject<List<RefundOrderDetailDto>>();

                    var refundTicketInput = new RefundTicketInput();
                    refundTicketInput.RefundListNo = refundOrderApply.RefundListNo;
                    refundTicketInput.PayListNo = refundOrderApply.ListNo;
                    refundTicketInput.PayTypeId = order.PayTypeId.Value;
                    refundTicketInput.RefundReason = refundOrderApply.Reason;
                    refundTicketInput.CashierId = refundOrderApply.CashierId;
                    refundTicketInput.CashierName = _nameCacheService.GetStaffName(refundOrderApply.CashierId);
                    refundTicketInput.CashPcid = refundOrderApply.CashPcid;
                    refundTicketInput.CashPcname = _nameCacheService.GetPcName(refundOrderApply.CashPcid);
                    refundTicketInput.SalePointId = refundOrderApply.SalePointId;
                    refundTicketInput.SalePointName = _nameCacheService.GetSalePointName(refundOrderApply.SalePointId);
                    refundTicketInput.ParkId = refundOrderApply.ParkId;
                    refundTicketInput.ParkName = _nameCacheService.GetParkName(refundOrderApply.ParkId);
                    foreach (var refundDetail in refundDetails)
                    {
                        int refundQuantity = refundDetail.RefundQuantity;

                        var ticketSales = await _ticketSaleRepository.GetAll()
                            .AsNoTracking()
                            .Where(t => t.OrderListNo == refundOrderApply.ListNo && t.OrderDetailId == refundDetail.Id && t.TicketStatusId != TicketStatus.已退)
                            .ToListAsync();
                        foreach (var ticketSale in ticketSales)
                        {
                            if (!await _ticketSaleDomainService.AllowRefundAsync(ticketSale))
                            {
                                continue;
                            }

                            var surplusNum = await _ticketSaleDomainService.GetSurplusNumAsync(ticketSale);
                            if (surplusNum <= 0)
                            {
                                continue;
                            }

                            var selfRefundQuantity = Math.Min(surplusNum, refundQuantity);

                            RefundTicketItem refundTicketItem = new RefundTicketItem();
                            refundTicketItem.TicketId = ticketSale.Id;
                            refundTicketItem.RefundQuantity = selfRefundQuantity;
                            refundTicketItem.SurplusQuantityAfterRefund = surplusNum - selfRefundQuantity;
                            refundTicketInput.Items.Add(refundTicketItem);

                            refundQuantity -= selfRefundQuantity;
                            if (refundQuantity <= 0)
                            {
                                break;
                            }
                        }

                        if (refundQuantity > 0)
                        {
                            throw new TmsException($"{ticketSales.FirstOrDefault()?.TicketTypeName}可退票数不足");
                        }

                        refundTicketInput.OriginalTradeId = ticketSales.FirstOrDefault().TradeId;
                    }

                    await _ticketSaleAppService.RefundAsync(refundTicketInput);

                    refundOrderApply.Status = RefundApplyStatus.退款成功;
                    refundOrderApply.ResultMessage = refundOrderApply.Status.ToString();
                }
                catch (TmsException ex)
                {
                    refundOrderApply.Status = RefundApplyStatus.退款失败;
                    refundOrderApply.ResultMessage = ex.Message;

                    order.RefundStatus = RefundStatus.退款失败;
                }

                refundOrderApply.HandleTime = DateTime.Now;

                await uow.CompleteAsync();
            }
        }
    }
}
