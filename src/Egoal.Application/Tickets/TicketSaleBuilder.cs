using Egoal.Dependency;
using Egoal.Extensions;
using Egoal.Invoice;
using Egoal.Members;
using Egoal.Messages.Dto;
using Egoal.Runtime.Session;
using Egoal.Tickets.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Egoal.Tickets
{
    public class TicketSaleBuilder : IScopedDependency
    {
        private readonly ISession _session;
        private readonly ITicketSaleDomainService _ticketSaleDomainService;
        private readonly IMemberAppService _memberAppService;

        public TicketSaleBuilder(
            ISession session,
            ITicketSaleDomainService ticketSaleDomainService,
            IMemberAppService memberAppService)
        {
            _session = session;
            _ticketSaleDomainService = ticketSaleDomainService;
            _memberAppService = memberAppService;
        }

        public async Task<InvoiceRequest> BuildInvoiceRequestAsync(List<TicketSale> ticketSales, InvoiceInput input)
        {
            InvoiceRequest invoiceRequest = new InvoiceRequest();
            invoiceRequest.FPQQLSH = input.ListNo;
            invoiceRequest.GMF_NSRSBH = input.TaxAccount;
            invoiceRequest.GMF_YHZH = input.BankAccount;
            invoiceRequest.GMF_SJH = input.Mobile;
            invoiceRequest.GMF_DZYX = input.Email;
            invoiceRequest.SKR = "微信平台";

            var memberId = _session.MemberId ?? ticketSales.Where(t => t.MemberId.HasValue).Select(t => t.MemberId).FirstOrDefault();
            invoiceRequest.WX_OPENID = await _memberAppService.GetOpenId(memberId);

            if (input.GMFType == InvoiceGMFType.个人)
            {
                invoiceRequest.GMF_MC = input.Name;
                invoiceRequest.GMF_DZDH = input.Mobile;
            }
            else
            {
                invoiceRequest.GMF_MC = input.InvoiceTitle;
                invoiceRequest.GMF_DZDH = $"{input.Address},{input.Telephone}".Trim(',');
            }

            foreach (var ticketSale in ticketSales)
            {
                if (!ticketSale.InvoiceNo.IsNullOrEmpty() || (ticketSale.ReaPrice.HasValue && ticketSale.ReaPrice.Value <= 0)) continue;

                InvoiceItem invoiceItem = invoiceRequest.Items.FirstOrDefault(i => i.XMMC == ticketSale.TicketTypeName && i.RealPrice == ticketSale.ReaPrice);
                if (invoiceItem == null)
                {
                    invoiceItem = new InvoiceItem();
                    invoiceItem.XMMC = ticketSale.TicketTypeName;
                    invoiceItem.DW = "张";
                    invoiceItem.XMSL = await _ticketSaleDomainService.GetRealNumAsync(ticketSale);
                    invoiceItem.RealPrice = ticketSale.ReaPrice.Value;

                    invoiceRequest.Items.Add(invoiceItem);
                }
                else
                {
                    invoiceItem.XMSL += await _ticketSaleDomainService.GetRealNumAsync(ticketSale);
                }
            }

            return invoiceRequest;
        }

        public InvoiceInfo BuildInvoice(InvoiceInput input, InvoiceRequest request, InvoiceResponse response)
        {
            var invoice = new InvoiceInfo();
            invoice.ListNo = input.ListNo;
            invoice.Type = InvoiceType.电子发票;
            invoice.GMFType = input.GMFType;
            invoice.Status = InvoiceStatus.正常;
            invoice.JE = request.HJJE;
            invoice.SE = request.HJSE;
            invoice.FPDM = response.FP_DM;
            invoice.FPHM = response.FP_HM;
            invoice.KPR = request.KPR;
            invoice.GMF_MC = request.GMF_MC;
            invoice.GMF_NSRSBH = request.GMF_NSRSBH;
            invoice.GMF_DZDH = request.GMF_DZDH;
            invoice.GMF_YHZH = request.GMF_YHZH;
            if (string.IsNullOrEmpty(request.GMF_SJH))
            {
                invoice.GMF_Email = request.GMF_DZYX;
            }
            else
            {
                invoice.GMF_Email = request.GMF_SJH;
            }
            invoice.GMF_Email = request.GMF_DZYX;
            invoice.Channel = InvoiceChannel.微信;
            invoice.CreateTime = response.KPRQ.HasValue ? response.KPRQ.Value.ToDateTimeString() : DateTime.Now.ToDateTimeString();

            return invoice;
        }

        public SendInvoiceMessageInput BuildInvoiceMessage(InvoiceRequest request, InvoiceInfo invoice)
        {
            return new SendInvoiceMessageInput
            {
                ListNo = invoice.ListNo,
                Email = invoice.GMF_Email,
                InvoiceDate = invoice.CreateTime.Substring(0, 10),
                SellerName = request.XSF_MC,
                TotalMoney = invoice.JE,
                InvoiceCode = invoice.FPDM,
                InvoiceNo = invoice.FPHM
            };
        }
    }
}
