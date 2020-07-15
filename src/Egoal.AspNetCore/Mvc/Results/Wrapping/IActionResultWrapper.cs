using Microsoft.AspNetCore.Mvc.Filters;

namespace Egoal.Mvc.Results.Wrapping
{
    public interface IActionResultWrapper
    {
        void Wrap(ResultExecutingContext actionResult);
    }
}
