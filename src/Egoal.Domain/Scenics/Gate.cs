using Egoal.Domain.Entities;
using System;

namespace Egoal.Scenics
{
    public class Gate : Entity
    {
        public string Name { get; set; }
        public string SortCode { get; set; }
        public bool? UseFlag { get; set; }
        public int? GateGroupId { get; set; }
        public GateType? GateTypeId { get; set; }
        public int? PdaTypeId { get; set; }
        public int? MachineId { get; set; }
        public bool? InOutFlag { get; set; }
        public bool? AutoCheckFlag { get; set; } = true;
        public string DeviceId { get; set; }
        public bool IsBothWay { get; set; }
        public bool RecycleCard { get; set; }
        public int? GroundId { get; set; }
        public int? Pcid { get; set; }
        public string SpPort { get; set; }
        public bool? TcpFlag { get; set; }
        public string TcpIp { get; set; }
        public int? TcpPort { get; set; }
        public string TcpMask { get; set; }
        public string TcpGateway { get; set; }
        public string TcpMac { get; set; }
        public string IdentifyCode { get; set; }
        public bool? EnableNetPayFlag { get; set; }
        public int? VerifyFaceMode { get; set; }
        public int? VerifyFingerMode { get; set; }
        public int? Status { get; set; }
        public DateTime? LastStatusUpdateTime { get; set; }
    }
}
