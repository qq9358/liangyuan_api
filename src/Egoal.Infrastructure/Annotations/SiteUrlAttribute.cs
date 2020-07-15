using System;
using System.ComponentModel.DataAnnotations;

namespace Egoal.Annotations
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class SiteUrlAttribute : RegularExpressionAttribute
    {
        public const string PATTERN = @"(https?|ftp|file)://[-A-Za-z0-9+&@#/%?=~_|!:,.;]+[-A-Za-z0-9+&@#/%=~_|]";

        public SiteUrlAttribute()
            : base(PATTERN)
        {
            ErrorMessage = "{0}格式不正确";
        }
    }
}
