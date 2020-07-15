using System;

namespace Egoal.TicketTypes.Dto
{
    public class GetByCertNoInput
    {
        public string CertNo { get; set; }
        public DateTime TravelDate { get; set; }
        public SaleChannel SaleChannel { get; set; }
    }
}
