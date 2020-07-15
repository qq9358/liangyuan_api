using Egoal.Application.Services.Dto;
using Egoal.Domain.Repositories;
using Egoal.Tickets.Dto;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Egoal.Tickets
{
    public interface ITicketSaleRepository : IRepository<TicketSale, long>
    {
        Task InValidAsync(TicketSale ticketSale);
        Task RefundAsync(TicketSale ticketSale, int quantity);
        Task<int> GetFingerprintQuantityAsync(long ticketId);
        Task<DateTime> GetFacePhotoBindTimeAsync(long ticketId);
        Task<decimal> GetOrderRealMoneyAsync(string listNo);
        Task<PagedResultDto<TicketSaleListDto>> QueryTicketSalesAsync(QueryTicketSaleInput input);
        Task<DataTable> StatAsync(StatTicketSaleInput input);
        Task<DataTable> StatCashierSaleAsync(StatCashierSaleInput input);
        Task<DataTable> StatByTradeSourceAsync(StatTicketSaleByTradeSourceInput input);
        Task<DataTable> StatByTicketTypeClassAsync(StatTicketSaleByTicketTypeClassInput input);
        Task<DataTable> StatByPayTypeAsync(StatTicketSaleByPayTypeInput input, IEnumerable<ComboboxItemDto<int>> payTypes);
        Task<DataTable> StatBySalePointAsync(StatTicketSaleBySalePointInput input);
        Task<DataTable> StatGroundSharingAsync(StatGroundSharingInput input);
        Task<DataTable> StatJbAsync(StatJbInput input);
    }
}
