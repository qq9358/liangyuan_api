using System;

namespace Egoal.Models
{
    public interface IExceptionToErrorInfoConverter
    {
        IExceptionToErrorInfoConverter Next { set; }
        ErrorInfo Convert(Exception exception);
    }
}
