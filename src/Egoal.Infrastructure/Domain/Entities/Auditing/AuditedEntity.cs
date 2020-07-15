using System;

namespace Egoal.Domain.Entities.Auditing
{
    [Serializable]
    public abstract class AuditedEntity<TPrimaryKey> : CreationAuditedEntity<TPrimaryKey>, IAudited
    {
        public int? MID { get; set; }
        public DateTime? MTime { get; set; }
    }

    [Serializable]
    public abstract class AuditedEntity : AuditedEntity<int>, IEntity
    {

    }
}
