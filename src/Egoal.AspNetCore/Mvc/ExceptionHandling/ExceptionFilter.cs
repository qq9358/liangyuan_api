using Egoal.Authorization;
using Egoal.Domain.Entities;
using Egoal.Events.Bus;
using Egoal.Events.Bus.Exceptions;
using Egoal.Extensions;
using Egoal.Logging;
using Egoal.Models;
using Egoal.Mvc.Extensions;
using Egoal.Mvc.Results;
using Egoal.Reflection;
using Egoal.Runtime.Validation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Egoal.Mvc.ExceptionHandling
{
    public class ExceptionFilter : IExceptionFilter
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly IEventBus _eventBus;
        private readonly IErrorInfoBuilder _errorInfoBuilder;

        public ExceptionFilter(
            ILoggerFactory loggerFactory,
            IEventBus eventBus,
            IErrorInfoBuilder errorInfoBuilder)
        {
            _loggerFactory = loggerFactory;
            _eventBus = eventBus;
            _errorInfoBuilder = errorInfoBuilder;
        }

        public void OnException(ExceptionContext context)
        {
            if (!context.ActionDescriptor.IsControllerAction())
            {
                return;
            }

            var wrapResultAttribute =
                ReflectionHelper.GetSingleAttributeOfMemberOrDeclaringTypeOrDefault(
                    context.ActionDescriptor.GetMethodInfo(),
                    new WrapResultAttribute()
                );

            if (wrapResultAttribute.LogError)
            {
                var declaringType = context.Exception.GetDeclaringType() ?? 
                    context.ActionDescriptor.AsControllerActionDescriptor().ControllerTypeInfo;
                ILogger logger = _loggerFactory.CreateLogger(declaringType);
                logger.LogException(context.Exception);
            }

            if (wrapResultAttribute.WrapOnError)
            {
                HandleAndWrapException(context);
            }
        }

        private void HandleAndWrapException(ExceptionContext context)
        {
            if (!ActionResultHelper.IsObjectResult(context.ActionDescriptor.GetMethodInfo().ReturnType))
            {
                return;
            }

            context.HttpContext.Response.StatusCode = GetStatusCode(context);

            context.Result = new ObjectResult(
                new AjaxResponse(
                    _errorInfoBuilder.BuildForException(context.Exception),
                    context.Exception is AuthorizationException
                )
            );

            _eventBus.Trigger(this, new HandledExceptionData(context.Exception));

            context.Exception = null; //Handled!
        }

        protected virtual int GetStatusCode(ExceptionContext context)
        {
            if (context.Exception is AuthorizationException)
            {
                return context.HttpContext.User.Identity.IsAuthenticated
                    ? (int)HttpStatusCode.Forbidden
                    : (int)HttpStatusCode.Unauthorized;
            }

            if (context.Exception is ValidationException)
            {
                return (int)HttpStatusCode.BadRequest;
            }

            if (context.Exception is EntityNotFoundException)
            {
                return (int)HttpStatusCode.NotFound;
            }

            return (int)HttpStatusCode.InternalServerError;
        }
    }
}
