using Egoal.Logging;
using Egoal.Models;
using Egoal.Mvc.Extensions;
using Egoal.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Egoal.Mvc.Validation
{
    public class ValidationResultFilter : IResultFilter
    {
        private readonly LogHelper _logHelper;
        private readonly IErrorInfoBuilder _errorInfoBuilder;

        public ValidationResultFilter(
            LogHelper logHelper,
            IErrorInfoBuilder errorInfoBuilder)
        {
            _logHelper = logHelper;
            _errorInfoBuilder = errorInfoBuilder;
        }

        public void OnResultExecuting(ResultExecutingContext context)
        {
            if (!context.ActionDescriptor.IsControllerAction())
            {
                return;
            }

            if (context.ModelState.IsValid)
            {
                return;
            }

            var validationErrors = new List<ValidationResult>();
            foreach (var state in context.ModelState)
            {
                foreach (var error in state.Value.Errors)
                {
                    validationErrors.Add(new ValidationResult(error.ErrorMessage, new[] { state.Key }));
                }
            }
            var exception = new Runtime.Validation.ValidationException("数据验证失败", validationErrors);

            var wrapResultAttribute =
                    ReflectionHelper.GetSingleAttributeOfMemberOrDeclaringTypeOrDefault(
                        context.ActionDescriptor.GetMethodInfo(),
                        new WrapResultAttribute()
                    );

            if (wrapResultAttribute.LogError)
            {
                _logHelper.LogException(exception);
            }

            if (wrapResultAttribute.WrapOnError)
            {
                context.Result = new BadRequestObjectResult(new AjaxResponse(_errorInfoBuilder.BuildForException(exception)));
            }
        }

        public void OnResultExecuted(ResultExecutedContext context)
        {

        }
    }
}
