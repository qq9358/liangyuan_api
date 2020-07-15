using JetBrains.Annotations;
using System;
using System.Collections.Generic;

namespace Egoal.Runtime.Caching
{
    public interface ICacheManager : IDisposable
    {
        IReadOnlyList<ICache> GetAllCaches();
        [NotNull] ICache GetCache([NotNull] string name);
    }
}
