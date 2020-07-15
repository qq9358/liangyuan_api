using Egoal.BackgroundJobs;
using Egoal.Common;
using Egoal.Dependency;
using Egoal.Domain.Repositories;
using Egoal.Events.Bus.Entities;
using Egoal.Events.Bus.Handlers;
using Egoal.Extensions;
using Egoal.TicketTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Egoal.Tickets
{
    public class TicketSaleCreatingEventHandler : IAsyncEventHandler<EntityCreatingEventData<TicketSale>>, IScopedDependency
    {
        private readonly IRepository<TicketTypeGroundSharing> _ticketTypeGroundSharingRepository;
        private readonly IRepository<TicketSaleGroundSharing, long> _ticketSaleGroundSharingRepository;
        private readonly IRepository<TmDate> _tmDateRepository;
        private readonly IBackgroundJobService _backgroundJobAppService;

        public TicketSaleCreatingEventHandler(
            IRepository<TicketTypeGroundSharing> ticketTypeGroundSharingRepository,
            IRepository<TicketSaleGroundSharing, long> ticketSaleGroundSharingRepository,
            IRepository<TmDate> tmDateRepository,
            IBackgroundJobService backgroundJobAppService)
        {
            _ticketTypeGroundSharingRepository = ticketTypeGroundSharingRepository;
            _ticketSaleGroundSharingRepository = ticketSaleGroundSharingRepository;
            _tmDateRepository = tmDateRepository;
            _backgroundJobAppService = backgroundJobAppService;
        }

        public async Task HandleEventAsync(EntityCreatingEventData<TicketSale> eventData)
        {
            await UpdateTicketSaleDayStatAsync(eventData.Entity);
            await HandleGroundSharingAsync(eventData.Entity);
        }

        private async Task UpdateTicketSaleDayStatAsync(TicketSale ticketSale)
        {
            var dayStat = new TicketSaleDayStat();
            dayStat.TicketNum = ticketSale.TicketNum ?? 0;
            dayStat.PersonNum = ticketSale.PersonNum ?? 0;
            dayStat.TicMoney = ticketSale.ReaMoney ?? 0;
            dayStat.TicketTypeId = ticketSale.TicketTypeId ?? 0;
            dayStat.CashierId = ticketSale.CashierId ?? 0;
            dayStat.CashPcid = ticketSale.CashPcid ?? 0;
            dayStat.Cdate = ticketSale.Cdate;
            dayStat.Ctp = ticketSale.Ctp;

            await _backgroundJobAppService.EnqueueAsync<UpdateTicketSaleDayStatJob>(dayStat.ToJson());
        }

        private async Task HandleGroundSharingAsync(TicketSale ticketSale)
        {
            List<TicketTypeGroundSharing> groundSharings = null;

            if (ticketSale.TicketStatusId == TicketStatus.已退)
            {
                var saleGroundSharings = await _ticketSaleGroundSharingRepository.GetAllListAsync(s => s.TicketId == ticketSale.ReturnTicketId);
                if (!saleGroundSharings.IsNullOrEmpty())
                {
                    groundSharings = saleGroundSharings.Select(s => new TicketTypeGroundSharing
                    {
                        GroundId = s.GroundId.Value,
                        SharingRate = s.SharingRate.Value
                    }).ToList();
                }
            }
            else
            {
                var travelDate = ticketSale.Stime.To<DateTime>().ToDateString();
                var dateTypeId = await _tmDateRepository.GetAll().Where(d => d.Date == travelDate).Select(d => d.DateTypeId).FirstOrDefaultAsync();
                groundSharings = await _ticketTypeGroundSharingRepository.GetAllListAsync(g => g.TicketTypeId == ticketSale.TicketTypeId && g.DateTypeId == dateTypeId);
            }

            if (groundSharings.IsNullOrEmpty())
            {
                return;
            }

            if (ticketSale.TicketSaleGroundSharings == null)
            {
                ticketSale.TicketSaleGroundSharings = new List<TicketSaleGroundSharing>();
            }
            foreach (var groundSharing in groundSharings)
            {
                var ticketSaleGroundSharing = new TicketSaleGroundSharing();
                ticketSaleGroundSharing.GroundId = groundSharing.GroundId;
                ticketSaleGroundSharing.SharingRate = groundSharing.SharingRate;
                ticketSaleGroundSharing.SharingMoney = Math.Round(ticketSale.ReaMoney.Value * groundSharing.SharingRate / 100, 2);

                ticketSale.TicketSaleGroundSharings.Add(ticketSaleGroundSharing);
            }

            var totalSharingMoney = ticketSale.TicketSaleGroundSharings.Sum(t => t.SharingMoney);
            if (totalSharingMoney != ticketSale.ReaMoney)
            {
                var ticketSaleGroundSharing = ticketSale.TicketSaleGroundSharings.First();
                ticketSaleGroundSharing.SharingMoney += ticketSale.ReaMoney - totalSharingMoney;
            }
        }
    }
}
