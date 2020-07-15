using System;
using System.ComponentModel.DataAnnotations;

namespace Egoal.Annotations
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class MacAddressAttribute : RegularExpressionAttribute
    {
        public const string PATTERN = @"^([A-Fa-f0-9]{2}[-:]){5}[A-Fa-f0-9]{2}$";

        public MacAddressAttribute()
            : base(PATTERN)
        {
            ErrorMessage = "{0}格式不正确";
        }
    }
}
