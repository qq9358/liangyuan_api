using Egoal.Application.Services.Dto;
using System;

namespace Egoal.Orders.Dto
{
    public class GetExplainerOrdersInput : PagedInputDto
    {
        public DateTime StartCTime { get; set; }
        public DateTime EndCTime { get; set; }
        public int? ExplainerId { get; set; }
        public int? TimeslotId { get; set; }
        public Guid? CustomerId { get; set; }
    }
}
