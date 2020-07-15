using System;
using System.Net;
using System.Runtime.ExceptionServices;

namespace Egoal.Extensions
{
    public static class ExceptionExtensions
    {
        public static void ReThrow(this Exception exception)
        {
            ExceptionDispatchInfo.Capture(exception).Throw();
        }

        public static bool IsWebException(this Exception exception)
        {
            var innerException = exception;

            while (innerException != null)
            {
                if (innerException is WebException) return true;

                innerException = innerException.InnerException;
            }

            return false;
        }

        public static Type GetDeclaringType(this Exception exception)
        {
            if (exception == null || exception.TargetSite == null) return null;

            return GetDeclaringType(exception.TargetSite.DeclaringType);
        }

        private static Type GetDeclaringType(Type declaringType)
        {
            if (declaringType == null) return null;

            if (declaringType.DeclaringType != null)
            {
                return GetDeclaringType(declaringType.DeclaringType);
            }

            return declaringType;
        }
    }
}