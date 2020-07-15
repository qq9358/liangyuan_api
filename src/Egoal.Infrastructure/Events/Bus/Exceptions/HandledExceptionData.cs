using System;

namespace Egoal.Events.Bus.Exceptions
{
    public class HandledExceptionData : ExceptionData
    {
        public HandledExceptionData(Exception ex)
            : base(ex)
        {

        }
    }
}
