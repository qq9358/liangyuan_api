using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Egoal.Orders.Dto
{
    public class OrderForConsumeListDto
    {
        public OrderForConsumeListDto()
        {
            AgeRanges = new List<OrderAgeRangeDto>();
        }

        public string ListNo { get; set; }
        public string TravelDate { get; set; }
        public string CustomerName { get; set; }
        public int TotalNum { get; set; }
        [JsonIgnore]
        public int? KeYuanTypeId { get; set; }
        public string KeYuanTypeName { get; set; }
        [JsonIgnore]
        public int? KeYuanAreaId { get; set; }
        public string AreaName { get; set; }
        public string LicensePlateNumber { get; set; }
        public string Memo { get; set; }
        public bool HasCheckIn { get; set; }
        public DateTime? CheckInTime { get; set; }
        public bool HasCheckOut { get; set; }
        public DateTime? CheckOutTime { get; set; }

        public List<OrderAgeRangeDto> AgeRanges { get; set; }
    }
}
