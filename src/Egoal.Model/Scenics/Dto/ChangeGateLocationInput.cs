using Egoal.Application.Services.Dto;

namespace Egoal.Scenics.Dto
{
    public class ChangeGateLocationInput : EntityDto
    {
        public int GroundId { get; set; }
        public int GateGroupId { get; set; }
    }
}
