using Newtonsoft.Json;

namespace Egoal.Orders.Dto
{
    public class ExplainerOrderListDto
    {
        public string ListNo { get; set; }
        public string TravelDate { get; set; }
        [JsonIgnore]
        public int? TimeslotId { get; set; }
        public string TimeslotName { get; set; }
        public int? ExplainerId { get; set; }
        public string ExplainerName { get; set; }
        public string CustomerName { get; set; }
        public int TotalNum { get; set; }
        public string BeginTime { get; set; }
        public string CompleteTime { get; set; }
        public bool AllowChange { get; set; }
    }
}
