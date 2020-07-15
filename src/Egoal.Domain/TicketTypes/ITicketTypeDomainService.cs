using Egoal.Tickets;
using Egoal.TicketTypes.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Egoal.TicketTypes
{
    public interface ITicketTypeDomainService
    {
        Task<bool> HasSpecifiedCheckGroundAsync(int ticketTypeId);
        Task<bool> HasGrantedToGroundAsync(int ticketTypeId, int groundId);
        Task<bool> HasGrantedToStaffAsync(int ticketTypeId, int staffId);
        Task<bool> HasGrantedToSalePointAsync(int ticketTypeId, int salePointId);
        Task<bool> HasGrantedToGateGroupAsync(int ticketTypeId, int gateGroupId);
        Task<bool> HasGrantedToParkAsync(int ticketTypeId, int parkId);
        Task<bool> HasGroundSharingAsync(int ticketTypeId);
        Task<decimal?> GetPriceAsync(TicketType ticketType, DateTime date, SaleChannel saleChannel);
        Task<TicketTypeDateTypePrice> GetPriceAsync(int ticketTypeId, string date);
        Task<List<TicketTypeDailyPriceDto>> GetPriceAsync(int ticketTypeId, string startDate, string endDate);
        Task<bool> AutoChooseByAgeAsync(TicketType ticketType);
        Task ReduceStockAsync(TicketSaleStock saleStock);
        Task IncreaseStockAsync(TicketSaleStock saleStock);
    }
}
