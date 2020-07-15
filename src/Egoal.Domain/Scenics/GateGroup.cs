using Egoal.Domain.Entities;

namespace Egoal.Scenics
{
    public class GateGroup : Entity
    {
        public string Name { get; set; }
        public string SortCode { get; set; }
        public bool? UseFlag { get; set; }
        public int? GroundId { get; set; }
        public int Pcid { get; set; }
        public string SpPort { get; set; }
        public bool? TcpFlag { get; set; }
        public string TcpIp { get; set; }
        public int? TcpPort { get; set; }
        public string TcpMask { get; set; }
        public string TcpGateway { get; set; }
        public string TcpMac { get; set; }
    }
}
