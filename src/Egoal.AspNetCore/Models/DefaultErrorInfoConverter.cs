using Egoal.Authorization;
using Egoal.Domain.Entities;
using Egoal.Extensions;
using Egoal.Runtime.Validation;
using Egoal.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Egoal.Models
{
    internal class DefaultErrorInfoConverter : IExceptionToErrorInfoConverter
    {
        public IExceptionToErrorInfoConverter Next { set; private get; }

        public DefaultErrorInfoConverter()
        {

        }

        public ErrorInfo Convert(Exception exception)
        {
            var errorInfo = CreateErrorInfoWithoutCode(exception);

            if (exception is IHasErrorCode)
            {
                errorInfo.Code = (exception as IHasErrorCode).Code;
            }

            return errorInfo;
        }

        private ErrorInfo CreateErrorInfoWithoutCode(Exception exception)
        {
            if (exception is AggregateException && exception.InnerException != null)
            {
                var aggException = exception as AggregateException;
                if (aggException.InnerException is UserFriendlyException ||
                    aggException.InnerException is ValidationException)
                {
                    exception = aggException.InnerException;
                }
            }

            if (exception is UserFriendlyException)
            {
                var userFriendlyException = exception as UserFriendlyException;
                return new ErrorInfo(userFriendlyException.Message, userFriendlyException.Details);
            }

            if (exception is ValidationException)
            {
                return new ErrorInfo("数据验证失败")
                {
                    ValidationErrors = GetValidationErrorInfos(exception as ValidationException),
                    Details = GetValidationErrorNarrative(exception as ValidationException)
                };
            }

            if (exception is EntityNotFoundException)
            {
                var entityNotFoundException = exception as EntityNotFoundException;

                if (entityNotFoundException.EntityType != null)
                {
                    return new ErrorInfo($"不存在Id={entityNotFoundException.Id}的实体{entityNotFoundException.EntityType.Name}");
                }

                return new ErrorInfo(entityNotFoundException.Message);
            }

            if (exception is AuthorizationException)
            {
                var authorizationException = exception as AuthorizationException;
                return new ErrorInfo(authorizationException.Message);
            }

            if (exception is WebException)
            {
                return new ErrorInfo("网络访问失败，请稍后重试");
            }

            return new ErrorInfo("操作失败，请稍后重试");
        }

        private ValidationErrorInfo[] GetValidationErrorInfos(ValidationException validationException)
        {
            var validationErrorInfos = new List<ValidationErrorInfo>();

            foreach (var validationResult in validationException.ValidationErrors)
            {
                var validationError = new ValidationErrorInfo(validationResult.ErrorMessage);

                if (validationResult.MemberNames != null && validationResult.MemberNames.Any())
                {
                    validationError.Members = validationResult.MemberNames.Select(m => m.ToCamelCase()).ToArray();
                }

                validationErrorInfos.Add(validationError);
            }

            return validationErrorInfos.ToArray();
        }

        private string GetValidationErrorNarrative(ValidationException validationException)
        {
            var detailBuilder = new StringBuilder();

            foreach (var validationResult in validationException.ValidationErrors)
            {
                detailBuilder.Append(validationResult.ErrorMessage);
                detailBuilder.AppendLine();
            }

            return detailBuilder.ToString();
        }
    }
}
