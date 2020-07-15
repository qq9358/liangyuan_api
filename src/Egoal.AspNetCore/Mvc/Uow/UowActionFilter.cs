using Egoal.Domain.Uow;
using Egoal.Mvc.Extensions;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Egoal.Mvc.Uow
{
    public class UowActionFilter : IAsyncActionFilter
    {
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public UowActionFilter(IUnitOfWorkManager unitOfWorkManager)
        {
            _unitOfWorkManager = unitOfWorkManager;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.ActionDescriptor.IsControllerAction())
            {
                await next();
                return;
            }

            var unitOfWorkAttr = GetUnitOfWorkAttributeOrNull(context.ActionDescriptor.GetMethodInfo());

            if (unitOfWorkAttr == null || unitOfWorkAttr.IsDisabled)
            {
                await next();
                return;
            }

            using (var uow = _unitOfWorkManager.Begin(unitOfWorkAttr.CreateOptions()))
            {
                var result = await next();
                if (result.Exception == null || result.ExceptionHandled)
                {
                    await uow.CompleteAsync();
                }
            }
        }

        private UnitOfWorkAttribute GetUnitOfWorkAttributeOrNull(MethodInfo methodInfo)
        {
            var attrs = methodInfo.GetCustomAttributes(true).OfType<UnitOfWorkAttribute>().ToArray();
            if (attrs.Length > 0)
            {
                return attrs[0];
            }

            attrs = methodInfo.DeclaringType.GetTypeInfo().GetCustomAttributes(true).OfType<UnitOfWorkAttribute>().ToArray();
            if (attrs.Length > 0)
            {
                return attrs[0];
            }

            return null;
        }
    }
}
