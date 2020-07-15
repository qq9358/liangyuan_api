using Egoal.Annotations;
using System;
using System.ComponentModel.DataAnnotations;

namespace Egoal.Orders.Dto
{
    public class CreateGroupOrderInput
    {
        public OrderType OrderType { get; set; }

        [Display(Name = "使用日期")]
        [MustFillIn]
        public DateTime TravelDate { get; set; }

        [Display(Name = "入场时段")]
        [MustFillIn]
        public int ChangCiId { get; set; }

        [Display(Name = "单位名称")]
        [MustFillIn]
        public string CompanyName { get; set; }

        [Display(Name = "游客人数")]
        [MustFillIn]
        public int Quantity { get; set; }

        [Display(Name = "姓名")]
        [MustFillIn]
        [MaximumLength(10)]
        public string ContactName { get; set; }

        [Display(Name = "手机号")]
        [MustFillIn]
        [MobileNumber]
        public string Mobile { get; set; }
    }
}
