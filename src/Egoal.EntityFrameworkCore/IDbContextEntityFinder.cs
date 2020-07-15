using Egoal.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Egoal
{
    public interface IDbContextEntityFinder
    {
        IEnumerable<EntityTypeInfo> GetEntityTypeInfos(Type dbContextType);
    }
}
