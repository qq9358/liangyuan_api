using System;
using System.ComponentModel.DataAnnotations;

namespace Egoal.Annotations
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class MinimumLengthAttribute : MinLengthAttribute
    {
        public MinimumLengthAttribute(int length)
            : base(length)
        {
            ErrorMessage = "{0}长度不能小于{1}";
        }
    }
}
