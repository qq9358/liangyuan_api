using Egoal.Annotations;
using Egoal.Tickets.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Egoal.Orders.Dto
{
    public class CreateIndividualOrderInput
    {
        public Guid MemberId { get; set; }
        public DateTime TravelDate { get; set; }
        public IEnumerable<TouristDto> Tourists { get; set; }
        public Contact Contact { get; set; }
        public int? ParkId { get; set; }
    }

    public class Contact
    {
        [Display(Name = "手机号")]
        [MustFillIn]
        [MobileNumber]
        public string Mobile { get; set; }
    }
}
