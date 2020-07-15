using System;

namespace Egoal.Models
{
    public interface IErrorInfoBuilder
    {
        ErrorInfo BuildForException(Exception exception);
        void AddExceptionConverter(IExceptionToErrorInfoConverter converter);
    }
}
