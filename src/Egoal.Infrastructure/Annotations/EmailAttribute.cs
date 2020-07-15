using System;
using System.ComponentModel.DataAnnotations;

namespace Egoal.Annotations
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class EmailAttribute : RegularExpressionAttribute
    {
        public const string PATTERN = "^[a-zA-Z0-9_-]+@[a-zA-Z0-9_-]+(\\.[a-zA-Z0-9_-]+)+$";

        public EmailAttribute()
            : base(PATTERN)
        {
            ErrorMessage = "{0}格式不正确";
        }
    }
}
