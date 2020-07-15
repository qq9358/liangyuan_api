using Egoal.Models;
using Egoal.Mvc.Extensions;
using Egoal.Mvc.Results.Wrapping;
using Egoal.Reflection;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Egoal.Mvc.Results
{
    public class ResultFilter : IResultFilter
    {
        private readonly IActionResultWrapperFactory _actionResultWrapperFactory;

        public ResultFilter(IActionResultWrapperFactory actionResultWrapperFactory)
        {
            _actionResultWrapperFactory = actionResultWrapperFactory;
        }

        public virtual void OnResultExecuting(ResultExecutingContext context)
        {
            if (!context.ActionDescriptor.IsControllerAction())
            {
                return;
            }

            var methodInfo = context.ActionDescriptor.GetMethodInfo();
            var wrapResultAttribute =
                ReflectionHelper.GetSingleAttributeOfMemberOrDeclaringTypeOrDefault(
                    methodInfo,
                    new WrapResultAttribute()
                );

            if (!wrapResultAttribute.WrapOnSuccess)
            {
                return;
            }

            _actionResultWrapperFactory.CreateFor(context).Wrap(context);
        }

        public virtual void OnResultExecuted(ResultExecutedContext context)
        {
            //no action
        }
    }
}
