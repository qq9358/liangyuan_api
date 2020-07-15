using System;

namespace Egoal.Domain.Entities.Auditing
{
    public interface IHasCreationTime
    {
        DateTime CTime { get; set; }
    }
}
