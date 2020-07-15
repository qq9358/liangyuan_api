using System;

namespace Egoal.Domain.Entities.Auditing
{
    public interface IHasModificationTime
    {
        DateTime? MTime { get; set; }
    }
}
