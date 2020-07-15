using System;

namespace Egoal.Domain.Entities.Auditing
{
    [Serializable]
    public abstract class CreationAuditedEntity<TPrimaryKey> : Entity<TPrimaryKey>, ICreationAudited
    {
        public int? CID { get; set; }
        public DateTime CTime { get; set; }

    }

    [Serializable]
    public abstract class CreationAuditedEntity : CreationAuditedEntity<int>, IEntity
    {

    }
}
