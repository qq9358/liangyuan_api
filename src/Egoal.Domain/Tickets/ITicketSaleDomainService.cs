using Egoal.Scenics.Dto;
using Egoal.Tickets.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Egoal.Tickets
{
    public interface ITicketSaleDomainService
    {
        Task<bool> ShouldInValidAsync(TicketSale ticketSale, int surplusQuantity, int refundQuantity);
        Task<int> GetConsumeNumAsync(TicketSale ticketSale);
        Task<int> GetRealNumAsync(TicketSale ticketSale);
        Task<int> GetRefundNumAsync(TicketSale ticketSale);
        Task<int> GetSurplusNumAsync(TicketSale ticketSale);
        Task<bool> IsUsableAsync(TicketSale ticketSale);
        Task<bool> AllowRefundAsync(TicketSale ticketSale);
        Task ValidateCertNoAsync(IEnumerable<string> certNos, DateTime travelDate);
        Task<bool> IsActiveAsync(TicketSale ticketSale);
        Task ActiveAsync(TicketSale ticketSale, IEnumerable<GroundChangCiDto> groundChangCis = null);
        Task ChangeChangCiAsync(TicketSale ticketSale, int changCiId);
        Task<TicketSale> RenewAsync(long ticketId);
        Task InValidAsync(TicketSale ticketSale);
        Task ConsumeAsync(TicketSale ticketSale, ConsumeTicketInput input);
        Task CheckOutAsync(TicketSale ticketSale, CheckOutTicketInput input);
        Task<DateTime?> GetLastCheckInTimeAsync(long id, bool isChecking = false);
        Task InvoiceAsync(List<TicketSale> ticketSales, InvoiceInfo invoice);
    }
}
