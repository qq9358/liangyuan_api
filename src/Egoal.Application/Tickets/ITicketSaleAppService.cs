using Egoal.Orders.Dto;
using Egoal.Tickets.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Egoal.Tickets
{
    public interface ITicketSaleAppService
    {
        Task<List<TicketSale>> SaleAsync(SaleTicketInput saleTicketInput);
        Task<DateTime?> ChangeChangCiAsync(ChangeChangCiInput input);
        Task InvoiceAsync(InvoiceInput input);
        Task RefundAsync(RefundInput input, RefundChannel refundChannel);
        Task RefundAsync(RefundTicketInput input);
        Task<CheckTicketOutput> CheckTicketAsync(CheckTicketInput input);
        Task RePrintAsync(PrintTicketInput input);
        Task PrintAsync(PrintTicketInput input, bool isReprint = false);
    }
}
