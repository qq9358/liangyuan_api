using Egoal.Annotations;
using Egoal.Scenics.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Egoal.Orders.Dto
{
    public class CreateOrderInput
    {
        public DateTime TravelDate { get; set; }

        [Display(Name = "入场时段")]
        [MustFillIn]
        public int ChangCiId { get; set; }

        public string ContactName { get; set; }

        [Display(Name = "手机号码")]
        //[MobileNumber]
        public string ContactMobile { get; set; }

        [Display(Name = "证件号码")]
        [IdentityCardNo]
        public string ContactCertNo { get; set; }
        public int? CashierId { get; set; }
        public int? CashPcid { get; set; }
        public int? SalePointId { get; set; }
        public int? ParkId { get; set; }

        public List<OrderItemDto> Items { get; set; }
    }

    public class OrderItemDto
    {
        public int TicketTypeId { get; set; }
        public int Quantity { get; set; }
        public List<GroundChangCiDto> GroundChangCis { get; set; }
        public List<OrderTouristDto> Tourists { get; set; }
    }

    public class OrderTouristDto
    {
        [Display(Name = "姓名")]
        [MustFillIn]
        public string Name { get; set; }

        public int? CertType { get; set; }

        [Display(Name = "证件号码")]
        [MustFillIn]
        public string CertNo { get; set; }
    }
}
