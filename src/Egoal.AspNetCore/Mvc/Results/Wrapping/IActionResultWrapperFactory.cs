using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Egoal.Mvc.Results.Wrapping
{
    public interface IActionResultWrapperFactory
    {
        IActionResultWrapper CreateFor([NotNull] ResultExecutingContext actionResult);
    }
}
