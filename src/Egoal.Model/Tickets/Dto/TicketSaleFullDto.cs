using Egoal.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Egoal.Tickets.Dto
{
    public class TicketSaleFullDto
    {
        public TicketSaleFullDto()
        {
            Grounds = new List<TicketGroundDto>();
            Seats = new List<TicketSaleSeatDto>();
        }

        public string TicketCode { get; set; }
        public string ListNo { get; set; }
        public string TicketStatusName { get; set; }
        public string TicketTypeName { get; set; }
        public decimal ReaPrice { get; set; }
        public decimal RealMoney { get; set; }
        public string PayTypeName { get; set; }
        public int Quantity { get; set; }
        public int SurplusQuantity { get; set; }
        public int TotalNum { get; set; }
        public string Stime { get; set; }
        public string Etime { get; set; }
        public string CustomerName { get; set; }
        public string CashierName { get; set; }
        public string SalePointName { get; set; }

        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime? Ctime { get; set; }

        public List<TicketGroundDto> Grounds { get; set; }

        public List<TicketSaleSeatDto> Seats { get; set; }
    }
}
