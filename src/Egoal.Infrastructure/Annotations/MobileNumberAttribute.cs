using System;
using System.ComponentModel.DataAnnotations;

namespace Egoal.Annotations
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class MobileNumberAttribute : RegularExpressionAttribute
    {
        public const string PATTERN = @"^1\d{10}$";

        public MobileNumberAttribute()
            : base(PATTERN)
        {
            ErrorMessage = "{0}格式不正确";
        }
    }
}
