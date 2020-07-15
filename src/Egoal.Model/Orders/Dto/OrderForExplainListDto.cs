using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Egoal.Orders.Dto
{
    public class OrderForExplainListDto
    {
        public OrderForExplainListDto()
        {
            AgeRanges = new List<OrderAgeRangeDto>();
        }

        public string ListNo { get; set; }
        public string TravelDate { get; set; }
        public string CustomerName { get; set; }
        public int TotalNum { get; set; }
        public string JidiaoName { get; set; }
        [JsonIgnore]
        public int? KeYuanTypeId { get; set; }
        public string KeYuanTypeName { get; set; }
        [JsonIgnore]
        public int? KeYuanAreaId { get; set; }
        public string AreaName { get; set; }
        public int? ExplainerId { get; set; }
        public string ExplainerName { get; set; }
        public int ExplainerTimeId { get; set; }
        public string TimeslotName { get; set; }
        public string Memo { get; set; }
        public DateTime? BeginTime { get; set; }
        public DateTime? CompleteTime { get; set; }
        public bool Editable { get; set; }
        public bool HasCheckIn { get; set; }

        public List<OrderAgeRangeDto> AgeRanges { get; set; }
    }
}
