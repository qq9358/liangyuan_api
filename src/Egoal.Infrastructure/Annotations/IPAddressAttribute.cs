using System;
using System.ComponentModel.DataAnnotations;

namespace Egoal.Annotations
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class IPAddressAttribute : RegularExpressionAttribute
    {
        public const string PATTERN = @"^(1\d{2}|2[0-4]\d|25[0-5]|[1-9]\d|[1-9])(\.(1\d{2}|2[0-4]\d|25[0-5]|[1-9]\d|\d)){3}$";

        public IPAddressAttribute()
            : base(PATTERN)
        {
            ErrorMessage = "{0}格式不正确";
        }
    }
}
